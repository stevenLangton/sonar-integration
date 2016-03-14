using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Data.Entity;
using System.Runtime.Remoting.Lifetime;
using System.Transactions;
using System.Web;
using Elmah;
using JsPlc.Ssc.Link.Interfaces.Services;
using JsPlc.Ssc.Link.Models.Entities;
using JsPlc.Ssc.Link.Repository;

namespace JsPlc.Ssc.Link.Service.Services
{
    public class ColleaguePdpService : IColleaguePdpService
    {
        private readonly RepositoryContext _db;
        private readonly IConfigurationDataService _configurationDataService;

        // Could inject a PdpVersionSelector into this service based on Strategy pattern... 
        public ColleaguePdpService()
        {
            _db = new RepositoryContext();
            _configurationDataService = new ConfigurationDataService();
        }

        public ColleaguePdpService(RepositoryContext context, IConfigurationDataService configurationDataService)
        {
            _db = context;
            _configurationDataService = configurationDataService;
        }

        private PeriodType? GetPdpPeriodicity()
        {
            object parsedVal = Enum.Parse(typeof(PeriodType),
                _configurationDataService.GetConfigSettingValue("PdpDefinedPeriodicity")); // potential values Quarter, Year
            if (!(parsedVal is PeriodType))
            {
                return null;
            }
            return (PeriodType) parsedVal;
        }

        //create pdp if one does not already exist.
        //no manager id as kept in pdp as only one record.
        public ColleaguePdp GetPdp(string colleagueId)
        {
            var periodType = GetPdpPeriodicity();

            if (!_db.ColleaguePdps.Any(e => e.ColleagueId == colleagueId)) return BuildNewColleaguePdp(colleagueId, periodType);

            var pdp = _db.ColleaguePdps.First(e => e.ColleagueId == colleagueId);
            return pdp;
        }

        public ColleaguePdp UpdatePdp(ColleaguePdp colleaguePdp)
        {
            using (var scope = new TransactionScope())
            {
                _db.Entry(colleaguePdp).State = EntityState.Modified;
                _db.SaveChanges();

                UpdatePdpSectionInstances(colleaguePdp.PdpSectionInstances);

                var pdp = _db.ColleaguePdps.First(e => e.ColleagueId == colleaguePdp.ColleagueId);
                scope.Complete();
                return pdp;
            }
        }

        private void UpdatePdpSectionInstances(IEnumerable<ColleaguePdpSectionInstance> instances)
        {
            foreach (var instance in instances)
            {
                _db.Entry(instance).State = EntityState.Modified;
                UpdatePdpAnswers(instance.ColleaguePdpAnswers);
            }
            _db.SaveChanges();
        }

        private void UpdatePdpAnswers(IEnumerable<ColleaguePdpAnswer> answers)
        {
            foreach (var answer in answers)
            {
                _db.Entry(answer).State = EntityState.Modified;
            }
            _db.SaveChanges();
        }

        // Builds a new ColleaguePdp (in memory) based on the currently applicable PdpVersion
        private ColleaguePdp BuildNewColleaguePdp(string colleagueId, PeriodType? periodType)
        {
            var applicablePdpVersion = GetPdPVersion(periodType);

            var colleaguePdp = new ColleaguePdp
            {
                Id = 0, ColleagueId = colleagueId, Created = DateTime.Now, 
                PdpVersion = applicablePdpVersion, Period = GetCurrentlyApplicablePdpPeriod(periodType),
                Shared = false,
                PdpSectionInstances = new List<ColleaguePdpSectionInstance>()
            };

            applicablePdpVersion.Sections.ForEach(x => colleaguePdp.PdpSectionInstances.Add(new ColleaguePdpSectionInstance
            {
                PdpSection = x, InstanceNumber = 1, ColleaguePdp = colleaguePdp, Created = DateTime.Now
            }));
            return colleaguePdp;
        }

        // TODO - kept for later use 
        private bool InsertPdp(ColleaguePdp colleaguePdp)
        {
            try
            {
                using (var scope = new TransactionScope())
                {
                    // add pdp to DB
                    var newPdp = _db.ColleaguePdps.Add(colleaguePdp);
                    _db.SaveChanges();

                    // add section instances to DB
                    foreach (var instance in colleaguePdp.PdpSectionInstances)
                    {
                        _db.ColleaguePdpSectionInstances.Add(instance);
                        _db.SaveChanges();
                        // add section instance answers to DB
                        foreach (var secAnswer in instance.ColleaguePdpAnswers)
                        {
                            _db.ColleaguePdpAnswers.Add(secAnswer);
                            _db.SaveChanges();
                        }
                    }
                    scope.Complete();
                    return true;
                }
            }
            catch (Exception ex)
            {
                ErrorLog.GetDefault(HttpContext.Current).Log(new Error(ex));
                return false;
            }
        }

        // Given a period (date or FY), build a PdpVersion object..
        private PdpVersion GetPdPVersion(PeriodType? periodType = PeriodType.Year, DateTime? dateSelected=null, Period selectedPeriod = null)
        {
            var pdpVersion = (dateSelected != null)
                ? GetPdpVersionByDate(dateSelected)
                : GetPdpVersionByPeriod(selectedPeriod, periodType);

            if (pdpVersion != null)
            {
                var pdpSections = from sq in _db.PdpSectionQuestions
                    join s in pdpVersion.Sections on sq.PdpSection.Id equals s.Id
                    select (new PdpSection
                    {
                        Id = s.Id,
                        PdpVersion = s.PdpVersion,
                        PresentationOrder = s.PresentationOrder,
                        Section = s.Section,
                        Questions = s.Questions
                    });
                pdpVersion.Sections = pdpSections.ToList();
            }

            return new PdpVersion();
        }

        private PdpVersion GetPdpVersionByDate(DateTime? dateSelected=null)
        {
            if (dateSelected == null) dateSelected = DateTime.Now;

            var pdpVersion = _db.PdpVersions.Include(x => x.Sections).FirstOrDefault(x => DbFunctions.DiffSeconds(x.ValidFrom, dateSelected) <= 0 &&
                                                DbFunctions.DiffSeconds(dateSelected, x.ValidTo) <= 0);
            return pdpVersion;
        }

        private PdpVersion GetPdpVersionByPeriod(Period selectedPeriod = null, PeriodType? periodType=PeriodType.Year)
        {
            Period currentlyApplicablePdpVersion = selectedPeriod ?? GetCurrentlyApplicablePdpPeriod(periodType);
            var applicablePdpVersion = _db.PdpVersions.FirstOrDefault(
                x => DbFunctions.DiffSeconds(x.ValidFrom, currentlyApplicablePdpVersion.Start) <= 0 &&
                     DbFunctions.DiffSeconds(x.ValidTo, currentlyApplicablePdpVersion.End) >= 0);

            return applicablePdpVersion;
        }

        // As per configured PeriodType for Pdp (Year = 5), get the period valid within the date
        private Period GetCurrentlyApplicablePdpPeriod(PeriodType? periodType)
        {
            return _db.Periods.FirstOrDefault(x => x.PeriodType.Equals(periodType) &&
                                            DbFunctions.DiffSeconds(x.Start, DateTime.Now) <= 0 &&
                                            DbFunctions.DiffSeconds(x.End, DateTime.Now) >= 0);
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}