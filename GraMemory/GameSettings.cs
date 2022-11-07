using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//klasa przechowująca wszystkie ustawienia gry
namespace GraMemory
{
    public class GameSettings
    {
        //ustawienia gry
        public int CzasGry;
        public int CzasPodgladu;
        public int MaxPunkty;
        public int Wiersze;
        public int Kolumny;
        public int Bok;
        public int AktualnePunkty;

        public string PlikLogo = $@"{AppDomain.CurrentDomain.BaseDirectory}\obrazki\logo.jpg";
        public string FolderObrazki = $@"{AppDomain.CurrentDomain.BaseDirectory}\obrazki\memory";

        public GameSettings()
        {
            UstawStartowe();
        }

        //metoda ustawia startowe parametry gry
        public void UstawStartowe()
        {
            CzasPodgladu = 5;
            CzasGry = 60;
            MaxPunkty = 0;

            //6x4 = 24 (12 grafik po 2 karty)
            Wiersze = 4;
            Kolumny = 6;
            Bok = 150;
            AktualnePunkty = 0;
        }
    }
}
