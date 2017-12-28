using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Esimerkkisovellus.Models
{
    public interface ITuoteRekisteri
    {
        bool PaivitaTuotteetTietokannasta();
        List<Tuote> GetTuoteLista();
        bool Tallenna(List<Tuote> tuotelista);
        MySqlDataReader GetTulos();
        bool AvaaYhteys(string ktunnus, string sala);
        bool SuljeYhteys();
    }

    public class TuoteRekisteri: PerusTietokantaOlio, ITuoteRekisteri
    {
        private List<Tuote> tuotteet = new List<Tuote>();

        public bool PaivitaTuotteetTietokannasta()
        {
            bool ok = true;
            try
            {
                if (AvaaYhteys("root", ""))
                {
                    tuotteet = new List<Tuote>();
                    const string lause = "select * from tuotteet order by id asc;";
                    this.komento = new MySqlCommand(lause, this.yhteys);
                    this.tulos = this.komento.ExecuteReader();
                    var mySqlDataReader = GetTulos();
                    while (mySqlDataReader.Read())
                    {
                        tuotteet.Add(new Tuote
                        {
                            Id = mySqlDataReader.GetInt32(mySqlDataReader.GetOrdinal("id")),
                            Nimi = mySqlDataReader.GetString(mySqlDataReader.GetOrdinal("nimi")),
                            Hinta = mySqlDataReader.GetDouble(mySqlDataReader.GetOrdinal("hinta"))
                        });
                    }
                }
                else
                {
                    ok = false;
                }
            }
            catch (MySqlException e)
            {
                ok = false;
            }
            finally
            {
                SuljeYhteys();
            }
            return ok;
        }

        public List<Tuote> GetTuoteLista()
        {
            PaivitaTuotteetTietokannasta();
            return tuotteet;
        }

        public bool Tallenna(List<Tuote> tuotelista)
        {
            var vanhaTuoteLista = GetTuoteLista().Select(x => x.GetCopy()).ToList();
            var uusiTuoteLista = tuotelista;

            foreach (var tuote in uusiTuoteLista)
            {
                if (vanhaTuoteLista.Any(x => x.Id == tuote.Id))
                {
                    UpdateTuote(tuote);
                }
                else
                {
                    LisaaTuote(tuote);
                }
            }

            foreach (var tuote in vanhaTuoteLista.Where(tuote => uusiTuoteLista.All(x => x.Id != tuote.Id)))
            {
                PoistaTuote(tuote);
            }
            
            return true;
        }

        private void LisaaTuote(Tuote tuote)
        {
            try
            {
                if (AvaaYhteys("root", ""))
                {
                    string lause = "insert into tuotteet (nimi, hinta) values ('"+tuote.Nimi+"', "+tuote.Hinta+");";
                    this.komento = new MySqlCommand(lause, this.yhteys);
                    this.komento.ExecuteNonQuery();
                }
            }
            catch (MySqlException e)
            {

            }
            finally
            {
                SuljeYhteys();
            }
        }

        private void PoistaTuote(Tuote tuote)
        {
            try
            {
                if (AvaaYhteys("root", ""))
                {
                    string lause = "delete from tuotteet where id="+tuote.Id+";";
                    this.komento = new MySqlCommand(lause, this.yhteys);
                    this.komento.ExecuteNonQuery();
                }
            }
            catch (MySqlException e)
            {

            }
            finally
            {
                SuljeYhteys();
            }
        }

        private void UpdateTuote(Tuote tuote)
        {
            try
            {
                if (AvaaYhteys("root", ""))
                {
                    string lause = "update tuotteet set nimi='" + tuote.Nimi + "', hinta=" + tuote.Hinta + " where id=" + tuote.Id + ";";
                    this.komento = new MySqlCommand(lause, this.yhteys);
                    this.komento.ExecuteNonQuery();
                }
            }
            catch (MySqlException e)
            {

            }
            finally
            {
                SuljeYhteys();
            }
        }

    }
}
