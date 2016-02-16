using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JsPlc.Ssc.Link.Portal.Controllers;
using System.Web.Mvc;
using JsPlc.Ssc.Link.Models;
using System.IO;

namespace JsPlc.Ssc.Link.Portal.Tests.Controllers
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class PdfControllerTests
    {
        [Ignore]
        [TestMethod]
        [TestCategory("IntegrationTests")]
        public void CanGenerateMeetingPdf()
        {
            // Arrange
            //PdfController controller = new PdfController();
            //TestHelpers.AddHttpRequest(controller);
            //// Act
            //FileContentResult result = controller.New(10) as FileContentResult;

            //// Assert
            //Assert.IsInstanceOfType(result, typeof(FileContentResult));
            //Assert.AreEqual(result.FileDownloadName, "DataCleansingReport.csv");
            //Assert.AreEqual(result.ContentType, "text/csv");
        }


        [Ignore]
        [TestMethod]
        [TestCategory("IntegrationTests")]
        public void CanGenerateEmptyMeetingPdf()
        {
            // Arrange
            MeetingView meeting = new MeetingView();

            string path = AppDomain.CurrentDomain.BaseDirectory;
            string dirPath = Directory.GetParent(Directory.GetParent(path).FullName).FullName;
            string TemplateFileName = dirPath + @"\PdfTemplates\MeetingTemplate.pdf";

            PdfController.PdfMeetingTemplate template = new PdfController.PdfMeetingTemplate(meeting, TemplateFileName);
            PdfController.PdfMaker maker = new PdfController.PdfMaker(template);

            File.Delete("Meeting.pdf");
            FileStream result = new FileStream("Meeting.pdf", FileMode.CreateNew);
            maker.MakePdf();
            //maker.MakePdf().CopyTo(result);
            result.Close();
        }
    }
}
