using CoApp.Autopackage;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
namespace TestProject1
{
    
    
    /// <summary>
    ///This is a test class for AutopackageMainTest and is intended
    ///to contain all AutopackageMainTest Unit Tests
    ///</summary>
    [TestClass()]
    public class AutopackageMainTest
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
        ///A test for ExpandWildcards
        ///</summary>
        [TestMethod()]
        [DeploymentItem("autopackage.exe")]
        public void ExpandWildcardsTest()
        {
            AutopackageMain_Accessor target = new AutopackageMain_Accessor(); // TODO: Initialize to an appropriate value
            IEnumerable<Tup> includes = new List<Tup>() { new Tup() { L="Foo", R="Dir/**/*.txt"}}; // TODO: Initialize to an appropriate value
            IEnumerable<string> excludes = null; // TODO: Initialize to an appropriate value
            IEnumerable<Tup> expected = null; // TODO: Initialize to an appropriate value
            IEnumerable<Tup> actual;
            actual = AutopackageMain_Accessor.ExpandWildcards(includes, excludes);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
