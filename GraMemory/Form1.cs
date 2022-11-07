using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GraMemory
{
    public partial class MemoryForm : Form
    {
        //obiekt przechowujący ustawienia gry
        private GameSettings _settings;
        public MemoryForm()
        {
            InitializeComponent();
            _settings = new GameSettings();
            UstawKontrolki();
            GenerujKarty();
        }

        private void UstawKontrolki()
        {
            //obliczamy ile miejsca zajmą karty i poszerzamy panelKart o te wartość
            panelKart.Width = _settings.Bok * _settings.Kolumny;
            panelKart.Height = _settings.Bok * _settings.Wiersze;

            //ustawiamy rozmiar formularza aby pomieścił on panelKart (plus lekki margines)
            Width = panelKart.Width + 40;
            Height = panelKart.Height + 100;

            //ustawiamy teksty na labelach za pomocą zmiennych w klasie GameSettings
            lblStartInfo.Text = $"Początek gry za {_settings.CzasPodgladu}";
            lblPunktyWartosc.Text = _settings.AktualnePunkty.ToString();
            lblCzasWartosc.Text = _settings.CzasGry.ToString();

            //zmieniamy widoczność labelki do odliczania początku gry
            lblStartInfo.Visible = true;
        }

        private void GenerujKarty()
        {
            // pobieramy ścieżki do wszystkich plików z katalogu memory do tablicy string
            string[] memories = Directory.GetFiles(_settings.FolderObrazki);

            //ustalamy w jaki sposób będą się liczyć punkty
            _settings.MaxPunkty = memories.Length;

            List<MemoryCard> buttons = new List<MemoryCard>();


            foreach(string img in memories)
            {
                //zmienna pomocnicza do wygenerowania GuId
                Guid id = Guid.NewGuid();

                //tworzymy kartę
                MemoryCard b1 = new MemoryCard(id, _settings.PlikLogo, img);

                //dodajemy pierwszą kartę do listy
                buttons.Add(b1);

                //tworzymy duplikat karty
                MemoryCard b2 = new MemoryCard(id, _settings.PlikLogo, img);

                //dodajemy duplikat do listy
                buttons.Add(b2);
            }

            //tworzymy generator liczb pseudolosowych
            //wykorzystamy go do losowego rozmieszczenia kart na panelu
            Random random = new Random();

            panelKart.Controls.Clear();

            

        }



    }
}
