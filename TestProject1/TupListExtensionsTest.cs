using CoApp.Autopackage;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace TestProject1
{
    
    
    /// <summary>
    ///This is a test class for TupListExtensionsTest and is intended
    ///to contain all TupListExtensionsTest Unit Tests
    ///</summary>
    [TestClass()]
    public class TupListExtensionsTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for BinaryFiles
        ///</summary>
       
        public void BinaryFilesTest()
        {
            IEnumerable<Tup> input = new List<Tup>() { new Tup() { R = ".exe" }, new Tup() { R = ".exe.home" }, new Tup() { R = ".exe/blah" }}; // TODO: Initialize to an appropriate value
            IEnumerable<Tup> expected = null; // TODO: Initialize to an appropriate value
            IEnumerable<Tup> actual;
            actual = TupListExtensions.BinaryFiles(input);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
