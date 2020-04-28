using Microsoft.VisualStudio.TestTools.UnitTesting;
using static NetGameShared.Util.Math;

namespace Tests
{
    [TestClass]
    public class MathTests
    {
        [TestMethod]
        public void Modular_Positive_InsideModulus()
        {
            const int m = 5;
            Assert.AreEqual(0, Mod(0, m));
            Assert.AreEqual(1, Mod(1, m));
            Assert.AreEqual(2, Mod(2, m));
            Assert.AreEqual(3, Mod(3, m));
            Assert.AreEqual(4, Mod(4, m));
        }

        [TestMethod]
        public void Modular_Positive_OutsideModulus()
        {
            const int m = 5;
            Assert.AreEqual(0, Mod(5, m));
            Assert.AreEqual(1, Mod(6, m));
            Assert.AreEqual(2, Mod(7, m));
            Assert.AreEqual(3, Mod(8, m));
            Assert.AreEqual(4, Mod(9, m));
        }

        [TestMethod]
        public void Modular_Negative_InsideModulus()
        {
            const int m = 5;
            Assert.AreEqual(4, Mod(-1, m));
            Assert.AreEqual(3, Mod(-2, m));
            Assert.AreEqual(2, Mod(-3, m));
            Assert.AreEqual(1, Mod(-4, m));
            Assert.AreEqual(0, Mod(-5, m));
        }

        [TestMethod]
        public void Modular_Negative_OutsideModulus()
        {
            const int m = 5;
            Assert.AreEqual(4, Mod(-6, m));
            Assert.AreEqual(3, Mod(-7, m));
            Assert.AreEqual(2, Mod(-8, m));
            Assert.AreEqual(1, Mod(-9, m));
            Assert.AreEqual(0, Mod(-10, m));
        }
    }
}
