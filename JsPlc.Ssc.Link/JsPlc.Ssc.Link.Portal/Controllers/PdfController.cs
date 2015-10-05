using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.IO;
using iTextSharp.text.pdf;
using System.Configuration;
using System.Web.Configuration;

namespace JsPlc.Ssc.Link.Portal.Controllers
{
    public class PdfController : Controller
    {
        private Lazy<LinkServiceFacade> _LinkService;

        public PdfController()
        {
            _LinkService = new Lazy<LinkServiceFacade>();
        }

        // GET: Pdf
        public ActionResult New()
        {
            //Test
            _LinkService.Value.GetAMeeting(1);

            //End test

            String MimeTypeStr = MimeMapping.GetMimeMapping("*.pdf");

            MemoryStream newFileStream = new MemoryStream();

            var AppSettings = ConfigurationManager.AppSettings;

            var MeetingTemplateFileName = AppSettings["MeetingTemplateFileName"]??@"\PdfTemplates\MeetingTemplate.pdf";

            var TemplateFileName = HttpContext.Server.MapPath(MeetingTemplateFileName);

            MakeMeetingPdf(newFileStream, TemplateFileName);

            return File(newFileStream.GetBuffer(), MimeTypeStr, "Meeting.pdf");
        }

        public static MemoryStream MakeMeetingPdf(MemoryStream newFileStream, string fileNameExisting)
        {
            using (var existingFileStream = new FileStream(fileNameExisting, FileMode.Open))
            {
                // Open existing PDF template
                var pdfReader = new PdfReader(existingFileStream);

                // PdfStamper, which will create new pdf
                var stamper = new PdfStamper(pdfReader, newFileStream);

                var form = stamper.AcroFields;
                var fieldKeys = form.Fields.Keys;

                foreach (string fieldKey in fieldKeys)
                {
                    form.SetField(fieldKey, "Diesel Scrum!State-of-the art colleagues appraisal system.");
                }

                // "Flatten" the form so it wont be editable/usable anymore
                stamper.FormFlattening = true;

                // You can also specify fields to be flattened, which
                // leaves the rest of the form still be editable/usable
                stamper.PartialFormFlattening("field1");

                stamper.Close();
                pdfReader.Close();
            }

            return newFileStream;
        }
    }
}


