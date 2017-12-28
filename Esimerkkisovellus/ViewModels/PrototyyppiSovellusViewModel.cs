using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.Mvvm;
using Esimerkkisovellus.Models;
using Esimerkkisovellus.Services;
using MySql.Data.MySqlClient;
using Esimerkkisovellus.Helpers;
using Application = System.Windows.Application;
using IMessageBoxService = Esimerkkisovellus.Services.IMessageBoxService;

namespace Esimerkkisovellus.ViewModels
{
    public class PrototyyppiSovellusViewModel : ViewModelBase
    {
        private ITuoteRekisteri tuoteRekisteri;
        private readonly IMessageBoxService messageBoxService;
        private List<Tuote> tallennettuTuoteLista;
        public virtual ObservableCollection<Tuote> Tuotteet { get; set; }
        public virtual Tuote SelectedTuote { get; set; }
        public virtual string TuoteNimi { get; set; }
        public virtual string TuoteHinta { get; set; }

        public PrototyyppiSovellusViewModel():this(new TuoteRekisteri(), new MessageBoxService())
        {
        }

        public PrototyyppiSovellusViewModel(ITuoteRekisteri tuoteRekisteri, IMessageBoxService messageBoxService)
        {
            this.tuoteRekisteri = tuoteRekisteri;
            this.messageBoxService = messageBoxService;
            InitializeData();
        }

        private void InitializeData()
        {
            if (tuoteRekisteri.PaivitaTuotteetTietokannasta())
            {
                Tuotteet = new ObservableCollection<Tuote>(tuoteRekisteri.GetTuoteLista());
                tallennettuTuoteLista = Tuotteet.Select(x => x.GetCopy()).ToList();
            }
            else
            {
                messageBoxService.Show("Ei yhteyttä tietokantaan. Ohjelma suljetaan.");
                if (Application.Current != null)
                {
                    Application.Current.Shutdown();
                }
            }
        }

        public bool CanLisaaTuote()
        {
            return !string.IsNullOrWhiteSpace(TuoteNimi) && !string.IsNullOrWhiteSpace(TuoteHinta);
        }

        public void LisaaTuote()
        {
            if (HintaHelper.OnkoMahdollinenHinta(TuoteHinta))
            {
                var hinta = HintaHelper.MuunnaHinta(TuoteHinta);
                Tuotteet.Add(new Tuote(TuoteNimi, hinta));
            }
            else
            {
                messageBoxService.Show("Tuotetta ei lisätty. Tuotteen hinta ei ole kelvollinen.");
            }
        }

        public bool CanPoistaTuote()
        {
            return SelectedTuote != null;
        }

        public void PoistaTuote()
        {
            Tuotteet.Remove(SelectedTuote);
        }

        public bool CanTallenna()
        {
            var nykyinenTuoteLista = Tuotteet.ToList();

            return
                nykyinenTuoteLista.Except(tallennettuTuoteLista)
                    .Union(tallennettuTuoteLista.Except(nykyinenTuoteLista))
                    .Any();
        }

        public void Tallenna()
        {
            if (tuoteRekisteri.Tallenna(Tuotteet.ToList()))
            {
                Tuotteet = new ObservableCollection<Tuote>(tuoteRekisteri.GetTuoteLista());
                tallennettuTuoteLista = tuoteRekisteri.GetTuoteLista().Select(x => x.GetCopy()).ToList();
                messageBoxService.Show("Tuotteet tallennettu tietokantaan.");
            }
            else
            {
                messageBoxService.Show("Tuotteiden tallennus tietokantaan ei onnistunut.");
            }
        }
    }
}
