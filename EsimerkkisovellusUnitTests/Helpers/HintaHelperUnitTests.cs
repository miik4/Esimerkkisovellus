using System;
using Esimerkkisovellus.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EsimerkkisovellusUnitTests.Helpers
{
    [TestClass]
    public class HintaHelperUnitTests
    {
        [TestMethod]
        public void OnkoMahdollinenHinta_TestEiKelvollinenHinta()
        {
            const string negatiivinenHinta = "-5";
            const string nollaHinta = "0";
            const string tyhjaHinta = "";
            const string sisaltaTekstiaHinta = "5asd32";
            const string sisaltaaMerkkejaHinta = "4$*123";
            const string hintaKahdellaDesimaaliErottimella = "0,32,1";
            const string hintaSisaltaaOpraattoreita1 = "3+2-1*2/1";
            const string hintaSisaltaaSulkuja = "(23)";

            Assert.IsFalse(HintaHelper.OnkoMahdollinenHinta(negatiivinenHinta));
            Assert.IsFalse(HintaHelper.OnkoMahdollinenHinta(nollaHinta));
            Assert.IsFalse(HintaHelper.OnkoMahdollinenHinta(tyhjaHinta));
            Assert.IsFalse(HintaHelper.OnkoMahdollinenHinta(sisaltaTekstiaHinta));
            Assert.IsFalse(HintaHelper.OnkoMahdollinenHinta(sisaltaaMerkkejaHinta));
            Assert.IsFalse(HintaHelper.OnkoMahdollinenHinta(hintaKahdellaDesimaaliErottimella));
            Assert.IsFalse(HintaHelper.OnkoMahdollinenHinta(hintaSisaltaaOpraattoreita1));
            Assert.IsFalse(HintaHelper.OnkoMahdollinenHinta(hintaSisaltaaSulkuja));
        }

        [TestMethod]
        public void OnkoMahdollinenHinta_TestKelvollinenHinta()
        {
            const string hinta = "123";
            const string hintaDesimaalilla1 = "0,13";
            const string hintaDesimaalilla2 = "0.13";

            Assert.IsTrue(HintaHelper.OnkoMahdollinenHinta(hinta));
            Assert.IsTrue(HintaHelper.OnkoMahdollinenHinta(hintaDesimaalilla1));
            Assert.IsTrue(HintaHelper.OnkoMahdollinenHinta(hintaDesimaalilla2));
        }

        [TestMethod]
        [ExpectedException (typeof(NullReferenceException))]
        public void MuunnaHinta_EiKelvollinenMerkkijono_TestTyhjäMerkkijono()
        {
            HintaHelper.MuunnaHinta(null);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void MuunnaHinta_EiKelvollinenMerkkijono_TestEiMahdollistaMuuttaa()
        {
            HintaHelper.MuunnaHinta("tekstiä");
        }

        [TestMethod]
        [ExpectedException(typeof(OverflowException))]
        public void MuunnaHinta_EiKelvollinenMerkkijono_TestYliVuoto()
        {
            HintaHelper.MuunnaHinta((Double.MaxValue).ToString());
        }

        [TestMethod]
        public void MuunnaHinta_TestKelvollinenHinta()
        {
            const string hintaInt = "234";
            const string hintaDesimaalilla1 = "2,3";
            const string hintaDesimaalilla2 = "2.4";

            Assert.AreEqual(234, HintaHelper.MuunnaHinta(hintaInt));
            Assert.AreEqual(2.3, HintaHelper.MuunnaHinta(hintaDesimaalilla1));
            Assert.AreEqual(2.4, HintaHelper.MuunnaHinta(hintaDesimaalilla2));
        }
    }
}
