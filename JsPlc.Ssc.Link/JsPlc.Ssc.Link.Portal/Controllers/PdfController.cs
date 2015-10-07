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

namespace JsPlc.Ssc.Link.Portal.Controllers
{
    public class PdfController : Controller
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
            //var AppSettings = ConfigurationManager.AppSettings;

            //var MeetingTemplateFileName = AppSettings["MeetingTemplateFileName"]??@"\PdfTemplates\MeetingTemplate.pdf";
            //MeetingTemplateFileName = Path.Combine(AppBasePath, MeetingTemplateFileName);
            //MeetingTemplateFileName = HttpContext.Server.MapPath(MeetingTemplateFileName);

            //MemoryStream newFileStream = new MemoryStream();
            //MakeMeetingPdf(newFileStream, GetPdfTemplateFileName(), MeetingId);

            MeetingView MeetingData = _LinkService.Value.GetMeeting(MeetingId);

            PdfMeetingTemplate maker = new PdfMeetingTemplate(MeetingData, GetPdfTemplateFileName());

            String MimeTypeStr = MimeMapping.GetMimeMapping("*.pdf");
            return File(maker.MakePdf().GetBuffer(), MimeTypeStr, "Meeting.pdf");
        }

        // GET: Pdf
        public ActionResult MakeFromJson(MeetingView MeetingData)
        {
            AppBasePath = Request.ApplicationPath;

            PdfMeetingTemplate maker = new PdfMeetingTemplate(MeetingData, GetPdfTemplateFileName());

            String MimeTypeStr = MimeMapping.GetMimeMapping("*.pdf");
            return File(maker.MakePdf().GetBuffer(), MimeTypeStr, "Meeting.pdf");
        }

        private string GetPdfTemplateFileName()
        {
            var MeetingTemplateFileName = AppSettings["MeetingTemplateFileName"] ?? @"\PdfTemplates\MeetingTemplate.pdf";
            MeetingTemplateFileName = Path.Combine(AppBasePath, MeetingTemplateFileName);
            MeetingTemplateFileName = HttpContext.Server.MapPath(MeetingTemplateFileName);

            return MeetingTemplateFileName;
        }

        public MemoryStream MakeMeetingPdf(MemoryStream newFileStream, string fileNameExisting, int MeetingId)
        {
            using (var existingFileStream = new FileStream(fileNameExisting, FileMode.Open, FileAccess.Read))
            {
                // Open existing PDF template
                var pdfReader = new PdfReader(existingFileStream);

                // PdfStamper, which will create new pdf
                var stamper = new PdfStamper(pdfReader, newFileStream);

                MeetingView MeetingData = _LinkService.Value.GetMeeting(MeetingId);

                SetPdfFormFields(stamper, MeetingData);

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

        }//MakeMeetingPdf

        private static void SetPdfFormFields(PdfStamper stamper, MeetingView MeetingData)
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
                        break;
                    case "ColleagueName":
                        value = MeetingData.ColleagueName;
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

        }//SetPdfFormFields

        #region PDF Factory
        interface ILinkPdfTemplate
        {
            MemoryStream MakePdf();
        }

        public class PdfMeetingTemplate : ILinkPdfTemplate
        {
            MeetingView Data { get; set; }
            string TemplateFileName {get;set;}

            public PdfMeetingTemplate(MeetingView MeetingData, string _TemplateFileName)
            {
                Data = MeetingData;
                TemplateFileName = _TemplateFileName;
            }

            public MemoryStream MakePdf()
            {
                MemoryStream newFileStream = new MemoryStream();

                using (var existingFileStream = new FileStream(TemplateFileName, FileMode.Open, FileAccess.Read))
                {
                    // Open existing PDF template
                    var pdfReader = new PdfReader(existingFileStream);

                    // PdfStamper, which will create new pdf
                    var stamper = new PdfStamper(pdfReader, newFileStream);

                    SetPdfFormFields(stamper);

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

            private void SetPdfFormFields(PdfStamper stamper)
            {
                var form = stamper.AcroFields;
                var fieldKeys = form.Fields.Keys;

                string value = String.Empty;

                foreach (string fieldKey in fieldKeys)
                {
                    switch (fieldKey)
                    {
                        case "Date":
                            value = Data.MeetingDate.ToShortDateString();
                            break;
                        case "ManagerName":
                            value = Data.ManagerName;
                            break;
                        case "ColleagueName":
                            value = Data.ColleagueName;
                            break;
                        case "ColleagueComments1":
                            value = Data.Questions.ToArray<QuestionView>()[0].ColleagueComment;
                            break;
                        case "ColleagueComments2":
                            value = Data.Questions.ToArray<QuestionView>()[1].ColleagueComment;
                            break;
                        case "ColleagueComments3":
                            value = Data.Questions.ToArray<QuestionView>()[2].ColleagueComment;
                            break;
                        case "ColleagueComments4":
                            value = Data.Questions.ToArray<QuestionView>()[3].ColleagueComment;
                            break;
                        case "ColleagueComments5":
                            value = Data.Questions.ToArray<QuestionView>()[4].ColleagueComment;
                            break;
                        case "ManagerComments1":
                            value = Data.Questions.ToArray<QuestionView>()[0].ManagerComment;
                            break;
                        case "ManagerComments2":
                            value = Data.Questions.ToArray<QuestionView>()[1].ManagerComment;
                            break;
                        case "ManagerComments3":
                            value = Data.Questions.ToArray<QuestionView>()[2].ManagerComment;
                            break;
                        case "ManagerComments4":
                            value = Data.Questions.ToArray<QuestionView>()[3].ManagerComment;
                            break;
                        case "ManagerComments5":
                            value = Data.Questions.ToArray<QuestionView>()[4].ManagerComment;
                            break;
                    };

                    form.SetField(fieldKey, value);
                }
            }
        }

        #endregion

    }
}


