using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Esimerkkisovellus.Models
{
    public class PerusTietokantaOlio
    {
        protected MySqlConnection yhteys = null;
        protected MySqlCommand komento = null;
        protected MySqlDataReader tulos = null;

        public MySqlDataReader GetTulos()
        {
            return this.tulos;
        }

        public bool AvaaYhteys(string ktunnus, string sala)
        {
            bool ok = true;
            try
            {
                string yhteysjono = @"server=localhost;userid=" + ktunnus + ";password=" + sala + ";database=esimerkkikanta";
                this.yhteys = new MySqlConnection(yhteysjono);
                this.yhteys.Open();
            }
            catch (MySqlException e1)
            {
                ok = false;
            }
            return ok;
        }

        public bool SuljeYhteys()
        {
            bool ok = true;

            try
            {
                if (this.yhteys != null)
                {
                    this.yhteys.Close();
                }
            }
            catch (MySqlException e1)
            {
                ok = false;
            }
            return ok;
        }
    }
}
