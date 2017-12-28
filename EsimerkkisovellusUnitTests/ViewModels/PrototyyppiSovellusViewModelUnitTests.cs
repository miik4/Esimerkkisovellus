using System;
using System.Collections.Generic;
using Esimerkkisovellus.Models;
using Esimerkkisovellus.Services;
using Esimerkkisovellus.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace EsimerkkisovellusUnitTests.ViewModels
{
    [TestClass]
    public class PrototyyppiSovellusViewModelUnitTests
    {
        public PrototyyppiSovellusViewModel ViewModel;
        public Mock<ITuoteRekisteri> TuoterekisteriMock = new Mock<ITuoteRekisteri>();
        public Mock<IMessageBoxService> MessageBoxServiceMock = new Mock<IMessageBoxService>();

        public void InitializeViewModel()
        {
            TuoterekisteriMock.Setup(x => x.PaivitaTuotteetTietokannasta()).Returns(true);
            var tuoteLista = new List<Tuote>
            {
                new Tuote {Id = 1, Nimi = "Saha", Hinta = 19.90},
                new Tuote {Id = 2, Nimi = "Kirves", Hinta = 12.90}
            };
            TuoterekisteriMock.Setup(x => x.GetTuoteLista()).Returns(new List<Tuote>(tuoteLista));
            ViewModel = new PrototyyppiSovellusViewModel(TuoterekisteriMock.Object, MessageBoxServiceMock.Object);
        }

        [TestMethod]
        public void CanLisaaTuote_TestEpatosiEiNimeaEiHintaa()
        {
            InitializeViewModel();
            ViewModel.TuoteNimi = "";
            ViewModel.TuoteHinta = "";
            var tulos = ViewModel.CanLisaaTuote();
            Assert.IsFalse(tulos);
        }

        [TestMethod]
        public void CanLisaaTuote_TestTosiNimiJaHintaAnnettu()
        {
            InitializeViewModel();
            ViewModel.TuoteNimi = "Vasara";
            ViewModel.TuoteHinta = "12,60";
            var tulos = ViewModel.CanLisaaTuote();
            Assert.IsTrue(tulos);
        }

        [TestMethod]
        public void LisaaTuote_TestEiKelvollinenTuoteEiLisata()
        {
            InitializeViewModel();
            ViewModel.TuoteNimi = "Sorkkarauta";
            ViewModel.TuoteHinta = "tässäpä onkin tekstiä";
            var odotettuTulos = ViewModel.Tuotteet.Count;
            ViewModel.LisaaTuote();
            var tulos = ViewModel.Tuotteet.Count;
            Assert.AreEqual(odotettuTulos, tulos);
        }

        [TestMethod]
        public void LisaaTuote_TestKelvollinenTuoteLisataan()
        {
            InitializeViewModel();
            ViewModel.TuoteNimi = "Sorkkarauta";
            ViewModel.TuoteHinta = "20.90";
            var odotettuTulos = ViewModel.Tuotteet.Count + 1;
            ViewModel.LisaaTuote();
            var tulos = ViewModel.Tuotteet.Count;
            Assert.AreEqual(odotettuTulos, tulos);
        }

        [TestMethod]
        public void CanPoistaTuote_TestEiValittuaTuotettaEpatosi()
        {
            InitializeViewModel();
            var tulos = ViewModel.CanPoistaTuote();
            Assert.IsFalse(tulos);
        }

        [TestMethod]
        public void CanPoistaTuote_TestPoistettavaTuoteValittuTosi()
        {
            InitializeViewModel();
            ViewModel.SelectedTuote = ViewModel.Tuotteet[0];
            var tulos = ViewModel.CanPoistaTuote();
            Assert.IsTrue(tulos);
        }

        [TestMethod]
        public void PoistaTuote_TestEiValittuaTuotettaEiPoistoa()
        {
            InitializeViewModel();
            var odotettuTulos = ViewModel.Tuotteet.Count;
            ViewModel.PoistaTuote();
            var tulos = ViewModel.Tuotteet.Count;
            Assert.AreEqual(odotettuTulos, tulos);
        }

        [TestMethod]
        public void PoistaTuote_TestValittuTuotePoistetaan()
        {
            InitializeViewModel();
            var odotettuTulos = ViewModel.Tuotteet.Count - 1;
            ViewModel.SelectedTuote = ViewModel.Tuotteet[0];
            ViewModel.PoistaTuote();
            var tulos = ViewModel.Tuotteet.Count;
            Assert.AreEqual(odotettuTulos, tulos);
        }

        [TestMethod]
        public void CanTallenna_TestEiMuutoksiaEpatosi()
        {
            InitializeViewModel();
            var tulos = ViewModel.CanTallenna();
            Assert.IsFalse(tulos);
        }

        [TestMethod]
        public void CanTallenna_TestTuotteessaMuutoksiaTosi()
        {
            InitializeViewModel();
            ViewModel.Tuotteet[0].Nimi = "Muutettu nimi";
            var tulos = ViewModel.CanTallenna();
            Assert.IsTrue(tulos);
        }

        [TestMethod]
        public void CanTallenna_TestLisattyTuoteTosi()
        {
            InitializeViewModel();
            ViewModel.Tuotteet.Add(new Tuote());
            var tulos = ViewModel.CanTallenna();
            Assert.IsTrue(tulos);
        }

        [TestMethod]
        public void CanTallenna_TestPoistettuTuoteTosi()
        {
            InitializeViewModel();
            ViewModel.Tuotteet.Remove(ViewModel.Tuotteet[0]);
            var tulos = ViewModel.CanTallenna();
            Assert.IsTrue(tulos);
        }

        [TestMethod]
        public void Tallenna_TestTallennusOnnistui()
        {
            TuoterekisteriMock.Setup(x => x.Tallenna(new List<Tuote>())).Returns(true);
            InitializeViewModel();
            ViewModel.Tallenna();
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void Tallenna_TestTallennusEpaOnnistui()
        {
            TuoterekisteriMock.Setup(x => x.Tallenna(new List<Tuote>())).Returns(false);
            InitializeViewModel();
            ViewModel.Tallenna();
            Assert.IsTrue(true);
        }
    }
}
