using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
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

        /// <summary>
        /// Returns a Pdp for a given ColleagueId for a selected Period..
        /// If no SelectedPeriod, use currentDate to select an applicable Period
        /// </summary>
        /// <param name="colleagueId"></param>
        /// <param name="selectedPeriodId"></param>
        /// <param name="now">To test with a simulated date</param>
        /// <returns></returns>
        public ColleaguePdp GetPdp(string colleagueId, int? selectedPeriodId = null, DateTime? now=null)
        {
            ColleaguePdp retval = null;

            if (!now.HasValue) now = DateTime.Now;

            if (selectedPeriodId.HasValue)
            {
                retval =
                    _db.ColleaguePdps.FirstOrDefault(
                        e => e.ColleagueId == colleagueId && e.Period.Id == selectedPeriodId);
            }

            if (retval != null) return retval;

            var periodType = GetPdpPeriodicity(); // Quarter or Year Parsed from Config key
            retval = BuildNewColleaguePdp(colleagueId, periodType, now);
            return retval;
        }

        public ColleaguePdp GetPdp(int pdpId)
        {
            var pdp = _db.ColleaguePdps.FirstOrDefault(e => e.Id == pdpId);
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

        internal void UpdatePdpSectionInstances(IEnumerable<ColleaguePdpSectionInstance> instances)
        {
            foreach (var instance in instances)
            {
                _db.Entry(instance).State = EntityState.Modified;
                UpdatePdpAnswers(instance.ColleaguePdpAnswers);
            }
            _db.SaveChanges();
        }

        internal void UpdatePdpAnswers(IEnumerable<ColleaguePdpAnswer> answers)
        {
            foreach (var answer in answers)
            {
                _db.Entry(answer).State = EntityState.Modified;
            }
            _db.SaveChanges();
        }

        // Builds a new ColleaguePdp (in memory) based on the currently applicable PdpVersion
        internal ColleaguePdp BuildNewColleaguePdp(string colleagueId, PeriodType? periodType, DateTime? now=null)
        {
            if (!now.HasValue) now = DateTime.Now;

            var applicablePdpVersion = (periodType.HasValue) ? GetPdPVersion(periodType, null, GetCurrentlyApplicablePdpPeriod(periodType, now))
                : GetPdPVersion(null, now);

            var colleaguePdp = new ColleaguePdp
            {
                Id = 0, ColleagueId = colleagueId, Created = now.Value, 
                PdpVersion = applicablePdpVersion, Period = GetCurrentlyApplicablePdpPeriod(periodType, now),
                Shared = false,
                PdpSectionInstances = new List<ColleaguePdpSectionInstance>()
            };

            applicablePdpVersion.Sections.ForEach(x => colleaguePdp.PdpSectionInstances.Add(new ColleaguePdpSectionInstance
            {
                PdpSection = x, InstanceNumber = 1, ColleaguePdp = colleaguePdp, Created = now.Value
            }));
            return colleaguePdp;
        }

        // TODO - kept for later use 
        internal bool InsertPdp(ColleaguePdp colleaguePdp)
        {
            try
            {
                using (var scope = new TransactionScope())
                {
                    // add pdp to DB
                    var newPdp =_db.ColleaguePdps.Add(colleaguePdp);
                    _db.SaveChanges();

                    // add section instances to DB
                    foreach (var instance in colleaguePdp.PdpSectionInstances)
                    {
                        instance.ColleaguePdp = newPdp;
                        _db.ColleaguePdpSectionInstances.Add(instance);
                        _db.SaveChanges();
                        // add section instance answers to DB
                        foreach (var secAnswer in instance.ColleaguePdpAnswers)
                        {
                            secAnswer.ColleaguePdpSectionInstance = instance;
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

        internal PeriodType? GetPdpPeriodicity()
        {
            PeriodType parsedVal;

            // potential values Quarter, Year, Null

            if (!Enum.TryParse(_configurationDataService.GetConfigSettingValue("PdpDefinedPeriodicity"), out parsedVal))
            {
                return null;
            }
            return parsedVal;
        }

        // Given a period (date or FY), build a PdpVersion object..
        internal PdpVersion GetPdPVersion(PeriodType? periodType = PeriodType.Year, DateTime? dateSelected = null, Period selectedPeriod = null)
        {
            var pdpVersion = (dateSelected != null)
                ? GetPdpVersionByDate(dateSelected)
                : GetPdpVersionByPeriod(selectedPeriod, periodType);

            return pdpVersion;
        }

        internal PdpVersion GetPdpVersionByDate(DateTime? dateSelected = null)
        {
            if (dateSelected == null) dateSelected = DateTime.Now;

            var pdpVersion = _db.PdpVersions.Include(x => x.Sections).FirstOrDefault(x => 
                                                DbFunctions.DiffHours(x.ValidFrom, dateSelected) >= 0 &&
                                                DbFunctions.DiffHours(dateSelected, x.ValidTo) >= 0);

            return pdpVersion;
        }

        internal PdpVersion GetPdpVersionByPeriod(Period selectedPeriod = null, PeriodType? periodType = PeriodType.Year)
        {
            var currentlyApplicablePdpVersion = selectedPeriod ?? GetCurrentlyApplicablePdpPeriod(periodType);
            var applicablePdpVersion = _db.PdpVersions.FirstOrDefault(
                x => DbFunctions.DiffHours(x.ValidFrom, currentlyApplicablePdpVersion.Start) >= 0 &&
                     DbFunctions.DiffHours(currentlyApplicablePdpVersion.End, x.ValidTo) >= 0);

            return applicablePdpVersion;
        }

        /// <summary>
        /// As per configured PeriodType for Pdp (Year = 5), get the period valid within the date
        /// May return null on purpose if ColleaguePdps are not tied to a Period..
        /// </summary>
        /// <param name="periodType"></param>
        /// <param name="now"></param>
        /// <returns></returns>
        internal Period GetCurrentlyApplicablePdpPeriod(PeriodType? periodType=PeriodType.Year, DateTime? now = null)
        {
            if (now == null) now = DateTime.Now;
           
            return _db.Periods.FirstOrDefault(x => x.PeriodType == periodType.Value &&
                                            DbFunctions.DiffHours(x.Start, now.Value) >= 0 &&
                                            DbFunctions.DiffHours(now.Value, x.End) >= 0);
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}