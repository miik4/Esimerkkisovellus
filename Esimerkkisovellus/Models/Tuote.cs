using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Esimerkkisovellus.Models
{
    public class Tuote : INotifyPropertyChanged
    {

        private int id;
        private string nimi;
        private double? hinta;

        public int Id
        {
            get { return id; }
            set
            {
                id = value; 
                OnPropertyChanged();
            }
        }

        public string Nimi
        {
            get { return nimi; }
            set
            {
                nimi = value; 
                OnPropertyChanged();
            }
        }

        public double? Hinta
        {
            get { return hinta; }
            set
            {
                hinta = value;
                OnPropertyChanged();
            }
        }

        public Tuote()
        {
        }

        public Tuote(int id, string nimi, double? hinta)
        {
            Id = id;
            Nimi = nimi;
            Hinta = hinta;
        }

        public Tuote(string nimi, double? hinta)
        {
            Nimi = nimi;
            Hinta = hinta;
        }

        public Tuote GetCopy()
        {
            return new Tuote(Id ,Nimi, Hinta);
        }

        protected bool Equals(Tuote other)
        {
            return string.Equals(nimi, other.nimi) && hinta.Equals(other.hinta);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Tuote) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((nimi != null ? nimi.GetHashCode() : 0)*397) ^ hinta.GetHashCode();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
