using CryptoService;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTestCryptoService
    {
        [TestMethod]
        public void TestMethodCryptography()
        {
            string password = "Test@12345";
            string expected = "5c428d8875d2948607f3e3fe134d71b4";
            ICryptography Crypto = new Cryptography();
            Assert.AreEqual(expected, Crypto.HashMD5(password));

        }
    }
}
