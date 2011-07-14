using CoApp.Toolkit.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;


namespace TestProject1
{
    
    
    /// <summary>
    ///This is a test class for FilesystemExtensionsTest and is intended
    ///to contain all FilesystemExtensionsTest Unit Tests
    ///</summary>
    [TestClass()]
    public class FilesystemExtensionsTest
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
        ///A test for FindFilesSmarter
        ///</summary>
        ///
        /*
        [TestMethod()]
        public void FindFilesSmarterTest()
        {
            IEnumerable<string> pathMasks = new List<String>(){"./*"}; // TODO: Initialize to an appropriate value
            IEnumerable<string> expected = null; // TODO: Initialize to an appropriate value
            IEnumerable<string> actual;
            actual = FilesystemExtensions.FindFilesSmarter(pathMasks);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }*/


        [TestMethod()]
        public void FindFilesSmarter1Test()
        {
            var basic = "Dir\\Dir\\final.txt";

            IEnumerable<string> actual;
            actual = basic.FindFilesSmarterComplex();

            Assert.IsTrue(actual.Count() == 1, basic + " does not have one item");
            Assert.IsTrue(actual.First().EndsWith(basic));

            var basicReverseSlash = "Dir/Dir/final.txt";

            
            actual = basicReverseSlash.FindFilesSmarterComplex();

            Assert.IsTrue(actual.Count() == 1, basicReverseSlash + " does not have one item");
            Assert.IsTrue(actual.First().EndsWith(basic));

            var simpleWC = "*\\Dir\\final.txt";

            
            actual = simpleWC.FindFilesSmarterComplex();

            Assert.IsTrue(actual.Count() == 1, simpleWC + "does not have one item");
            Assert.IsTrue(actual.First().EndsWith("Dir\\Dir\\final.txt"));


            var recWC = "**\\final.txt";

            
            actual = recWC.FindFilesSmarterComplex();

            Assert.IsTrue(actual.Count() == 2, recWC + " does not have 2 items");
            Assert.IsTrue(actual.All((item) => item.EndsWith("final.txt")));



            var localDir2 = "Dir2\\*.txt";

            
            actual = localDir2.FindFilesSmarterComplex();
            Assert.IsTrue(actual.Count() == 2, localDir2 + " does not have 2 items");
            
            Assert.IsTrue(actual.Any((i) => i.EndsWith("Dir2\\final.txt")), "Dir2\\final.txt not found");
            Assert.IsTrue(actual.Any((i) => i.EndsWith("Dir2\\tests.txt")), "Dir2\\tests.txt not found");

            //var final = "*\\Dir\\final.txt";


            var allTxtInDirs = "**\\Dir*\\*.txt";


            

            actual = allTxtInDirs.FindFilesSmarterComplex();

            Assert.IsTrue(actual.Count() == 5, allTxtInDirs + " does not 5 have items");



        }
        
        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void FindFilesSmarter1Safety1Test()
        {
            @"something\a**".FindFilesSmarterComplex();
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void FindFilesSmarter1Safety2Test()
        {
            @"something\**a\".FindFilesSmarterComplex();
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void FindFilesSmarter1Safety3Test()
        {
            @"something\**\**\".FindFilesSmarterComplex();
        }
    }
}
