using System;
using Esimerkkisovellus.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EsimerkkisovellusUnitTests.Models
{
    [TestClass]
    public class TuoteUnitTests
    {
        [TestMethod]
        public void GetCopy_TestOnnistunutKopiointi()
        {
            var tuote = new Tuote
            {
                Id = 1,
                Nimi = "Testituote",
                Hinta = 19.90
            };

            var tuoteKopio = tuote.GetCopy();
            Assert.AreEqual(tuote.Id, tuoteKopio.Id);
            Assert.AreEqual(tuote.Nimi, tuoteKopio.Nimi);
            Assert.AreEqual(tuote.Hinta, tuoteKopio.Hinta);
        }
    }
}
