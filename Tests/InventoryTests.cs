using Microsoft.VisualStudio.TestTools.UnitTesting;
using Comps = NetGameShared.Ecs.Components;

namespace Tests
{
    [TestClass]
    public class InventoryTests
    {
        [TestMethod]
        public void Inventories_Empty_AreEqual()
        {
            var a = new Comps.Inventory();
            var b = new Comps.Inventory();
            Assert.AreEqual(a, b);
        }

        [TestMethod]
        public void Inventories_NonEmpty_AreEqual()
        {
            var a = new Comps.Inventory();
            a.data[Comps.Item.Kind.Bow].Count = 1;

            var b = new Comps.Inventory();
            b.data[Comps.Item.Kind.Bow].Count = 1;

            Assert.AreEqual(a, b);
        }

        [TestMethod]
        public void Inventories_OneEmpty_AreNotEqual()
        {
            var a = new Comps.Inventory();
            a.data[Comps.Item.Kind.Bow].Count = 1;

            var b = new Comps.Inventory();

            Assert.AreNotEqual(a, b);
        }

        [TestMethod]
        public void Inventories_FromClone_AreNotEqual()
        {
            var a = new Comps.Inventory();

            var b = (Comps.Inventory)a.Clone();
            b.data[Comps.Item.Kind.Bow].Count = 1;

            Assert.AreNotEqual(a, b);
        }
    }
}
