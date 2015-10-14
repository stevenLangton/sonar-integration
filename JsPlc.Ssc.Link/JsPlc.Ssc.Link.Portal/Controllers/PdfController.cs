using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.IO;
using iTextSharp.text.pdf;
using System.Configuration;
using System.Web.Configuration;
using JsPlc.Ssc.Link.Models;
using System.Collections.Specialized;
using JsPlc.Ssc.Link.Portal.Controllers.Base;
using JsPlc.Ssc.Link.Portal.Helpers.Api;

namespace JsPlc.Ssc.Link.Portal.Controllers
{
    public class PdfController : LinkBaseController
    {
        private Lazy<LinkServiceFacade> _LinkService;
        private string AppBasePath { get; set; }
        private NameValueCollection AppSettings { get; set; }

        public PdfController()
        {
            _LinkService = new Lazy<LinkServiceFacade>();

            AppSettings = ConfigurationManager.AppSettings;
        }

        // GET: Pdf
        public ActionResult MakeFromDb(int MeetingId)
        {
            AppBasePath = Request.ApplicationPath;

            MeetingView MeetingData = _LinkService.Value.GetMeeting(MeetingId);

            PdfMeetingTemplate template = new PdfMeetingTemplate(MeetingData, GetPdfTemplateFileName());
            PdfMaker maker = new PdfMaker(template);

            String MimeTypeStr = MimeMapping.GetMimeMapping("*.pdf");
            return File(maker.MakePdf().GetBuffer(), MimeTypeStr, "Meeting.pdf");
        }

        // GET: Pdf
        [HttpPost]
        public ActionResult MakeFromJson(MeetingView MeetingData)
        {
            AppBasePath = Request.ApplicationPath;

            PdfMeetingTemplate template = new PdfMeetingTemplate(MeetingData, GetPdfTemplateFileName());
            PdfMaker maker = new PdfMaker(template);

            MemoryStream ms = maker.MakePdf();
            string fName = "Meeting.pdf";

            Session[fName] = ms;

            return Json(new { success = true, fName }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult DownloadPdf(string fName)
        {
            var ms = Session[fName] as MemoryStream;
            if(ms == null)
                return new EmptyResult();
            Session[fName] = null;

            String MimeTypeStr = MimeMapping.GetMimeMapping("*.pdf");

            return File(ms.GetBuffer(), MimeTypeStr, fName);
        }

        private string GetPdfTemplateFileName()
        {
            var MeetingTemplateFileName = AppSettings["MeetingTemplateFileName"] ?? @"\PdfTemplates\MeetingTemplate.pdf";
            MeetingTemplateFileName = Path.Combine(AppBasePath, MeetingTemplateFileName);
            MeetingTemplateFileName = HttpContext.Server.MapPath(MeetingTemplateFileName);

            return MeetingTemplateFileName;
        }

        #region PDF Factory

        public interface ILinkPdfTemplate
        {
            string TemplateFileName { get; set; }
            void SetPdfFormFields(PdfStamper stamper);
        }

        public class PdfMeetingTemplate : ILinkPdfTemplate
        {
            public string TemplateFileName { get; set; }
            MeetingView MeetingData { get; set; }

            public PdfMeetingTemplate(MeetingView _MeetingData, string _TemplateFileName)
            {
                TemplateFileName = _TemplateFileName;
                MeetingData = _MeetingData;
            }

            public void SetPdfFormFields(PdfStamper stamper)
            {
                var form = stamper.AcroFields;
                var fieldKeys = form.Fields.Keys;

                string value = String.Empty;

                foreach (string fieldKey in fieldKeys)
                {
                    switch (fieldKey)
                    {
                        case "Date":
                            value = MeetingData.MeetingDate.ToShortDateString();
                            break;
                        case "ManagerName":
                            value = MeetingData.ManagerName;
                            if (!String.IsNullOrEmpty(MeetingData.ManagerId))
                                value = value + " (" + MeetingData.ManagerId + ")";
                            break;
                        case "ColleagueName":
                            value = MeetingData.ColleagueName;
                            if (!String.IsNullOrEmpty(MeetingData.ColleagueId))
                                value = value + " (" + MeetingData.ColleagueId + ")";
                            break;
                        case "ColleagueComments1":
                            value = MeetingData.Questions.ToArray<QuestionView>()[0].ColleagueComment;
                            break;
                        case "ColleagueComments2":
                            value = MeetingData.Questions.ToArray<QuestionView>()[1].ColleagueComment;
                            break;
                        case "ColleagueComments3":
                            value = MeetingData.Questions.ToArray<QuestionView>()[2].ColleagueComment;
                            break;
                        case "ColleagueComments4":
                            value = MeetingData.Questions.ToArray<QuestionView>()[3].ColleagueComment;
                            break;
                        case "ColleagueComments5":
                            value = MeetingData.Questions.ToArray<QuestionView>()[4].ColleagueComment;
                            break;
                        case "ManagerComments1":
                            value = MeetingData.Questions.ToArray<QuestionView>()[0].ManagerComment;
                            break;
                        case "ManagerComments2":
                            value = MeetingData.Questions.ToArray<QuestionView>()[1].ManagerComment;
                            break;
                        case "ManagerComments3":
                            value = MeetingData.Questions.ToArray<QuestionView>()[2].ManagerComment;
                            break;
                        case "ManagerComments4":
                            value = MeetingData.Questions.ToArray<QuestionView>()[3].ManagerComment;
                            break;
                        case "ManagerComments5":
                            value = MeetingData.Questions.ToArray<QuestionView>()[4].ManagerComment;
                            break;
                    };

                    form.SetField(fieldKey, value);
                }
            }

        }//PdfMeetingTemplate

        public class PdfMaker
        {
            ILinkPdfTemplate Template;

            public PdfMaker(ILinkPdfTemplate _template)
            {
                Template = _template;
            }

            public MemoryStream MakePdf()
            {
                MemoryStream newFileStream = new MemoryStream();

                using (var templateFileStream = new FileStream(Template.TemplateFileName, FileMode.Open, FileAccess.Read))
                {
                    // Open existing PDF template
                    var pdfReader = new PdfReader(templateFileStream);

                    // PdfStamper, which will create new pdf
                    var stamper = new PdfStamper(pdfReader, newFileStream);

                    Template.SetPdfFormFields(stamper);

                    // "Flatten" the form so it wont be editable/usable anymore
                    stamper.FormFlattening = true;

                    // You can also specify fields to be flattened, which
                    // leaves the rest of the form still be editable/usable
                    //
                    //stamper.PartialFormFlattening("field1");

                    stamper.Close();
                    pdfReader.Close();
                }

                return newFileStream;
            }
        }
        #endregion
    }
}