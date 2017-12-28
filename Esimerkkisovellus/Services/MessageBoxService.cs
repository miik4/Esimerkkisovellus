using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Esimerkkisovellus.Services
{
    public interface IMessageBoxService
    {
        void Show(string message);
    }

    public class MessageBoxService : IMessageBoxService
    {
        public void Show(string message)
        {
            MessageBox.Show(message);
        }
    }
}
