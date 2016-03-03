using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JsPlc.Ssc.Link.ImportRoutine;
using System.Transactions;
using FizzWare.NBuilder;
using Moq;

using System.Collections.Generic;
using System.Data;

namespace JsPlc.Ssc.Link.Import.Tests
{
  

    [TestClass]
    public class ImportTests
    {

        [TestMethod]
        [Ignore]
        public void TestFilestreamRead()
        {
            IImportFactory xxx = new ImportFactory();

            List<ColleagueDto> CTL = new List<ColleagueDto>();

            CTL = xxx.ImportColleagueDataFromFile();

            Assert.AreNotEqual(CTL.Count, 0);
        }


        //integration test, not unit tested as simple sql bulk update operation
        //[TestMethod]
        //public void TestSQLBulkImport()
        //{
        //    using (TransactionScope scope = new TransactionScope())
        //    {
        //        IImportFactory xxx = new ImportFactory();

        //        List<ColleagueDto> CTL = new List<ColleagueDto>();

        //        CTL = Builder<ColleagueDto>.CreateListOfSize(10).Build() as List<ColleagueDto>;

        //        bool done = xxx.LoadDataIntoSqlServer(CTL);

        //        Assert.IsTrue(done);
        //    }

        //}


    }
}
