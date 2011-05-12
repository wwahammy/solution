using CoApp.Toolkit.Crypto;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Security.Cryptography.X509Certificates;

namespace TestProject1
{
    
    
    /// <summary>
    ///This is a test class for PfxStoreLoaderTest and is intended
    ///to contain all PfxStoreLoaderTest Unit Tests
    ///</summary>
    [TestClass()]
    public class PfxStoreLoaderTest
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
        ///A test for Load
        ///</summary>
        [TestMethod()]
        public void LoadTest()
        {
            string file = @"C:\Users\Eric\Desktop\CoappTest.pfx"; // TODO: Initialize to an appropriate value
            string password = "password"; // TODO: Initialize to an appropriate value
            X509Store expected = null; // TODO: Initialize to an appropriate value
            X509Store actual;
            actual = PfxStoreLoader.Load(file, password);
            foreach (var cert in actual.Certificates)
            {
                Console.WriteLine(cert.FriendlyName);
                Console.WriteLine("Has private key:" + cert.HasPrivateKey);
                Console.WriteLine("Issuer:" + cert.IssuerName.Name);
                Console.WriteLine();
            }
            //Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
