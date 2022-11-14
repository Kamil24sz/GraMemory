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

        //zmienne pomocnicze na odkryte karty
        MemoryCard _pierwsza = null;
        MemoryCard _druga = null;
        public MemoryForm()
        {
            InitializeComponent();
            _settings = new GameSettings();
            UstawKontrolki();
            GenerujKarty();
            timerCzasPodlgladu.Start();
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

            //pętla w pętli - przechodzi przez każdą pozycję w panelu kart
            for(int x = 0; x < _settings.Kolumny; x++)
            {
                for(int y = 0; y < _settings.Wiersze; y++)
                {
                    //wybiera losową kartę (losowanie od 0 do 23)
                    int index = random.Next(0, buttons.Count);

                    //losowa karta
                    MemoryCard b = buttons[index];

                    //ustawiamy pozycję karty (punkt X Y)
                    b.Location = new Point(x * _settings.Bok, y * _settings.Bok);

                    //ustawiamy wymiary karty
                    b.Width = _settings.Bok;
                    b.Height = _settings.Bok;

                    //przypisanie funkcji BtnCLick to każdej karty
                    b.Click += BtnClicked;

                    //odkrywamy karty, aby wszystkie były widoczne na początku gry
                    b.Odkryj();

                    //dodajemy kartę do panelu kart
                    panelKart.Controls.Add(b);

                    //usuwamy kartę z listy aby nie została wylosowana kolejny raz
                    buttons.Remove(b);

                }
            }

        }

        //metoda wywołująca się po kliknięciu jednej z kart
        private void BtnClicked(object sender, EventArgs e)
        {
            //rzutujemy obiekt który został kliknięty na memory card
            MemoryCard btn = (MemoryCard)sender;

            //sprawdzamy czy zmienna pomocnicza jest wolna
            if(_pierwsza == null)
            {
                //przypisujemy kartę do zmiennej pomocniczej
                _pierwsza = btn;
                //odkrywamy kartę
                _pierwsza.Odkryj();
            }
            else
            {
                _druga = btn;
                _druga.Odkryj();

                //wyłączamy możliwość wybrania większej ilości kart
                panelKart.Enabled = false;

                if(_pierwsza.id == _druga.id)
                {
                    //aktualizujemy punkty
                    _settings.AktualnePunkty++;

                    lblPunktyWartosc.Text = _settings.AktualnePunkty.ToString();

                    _pierwsza = null;
                    _druga = null;

                    //włączamy panel kart z powrotem
                    panelKart.Enabled = true;
                }
                else
                {
                    //wywołuje motodę TimerZakrywacz
                    timerZakrywacz.Start();
                }

            }
        }

        private void timerZakrywacz_Tick(object sender, EventArgs e)
        {
            //zakrywa obie karty
            _pierwsza.Zakryj();
            _druga.Zakryj();

            //czyści zmienne pomocnicze
            _pierwsza = null;
            _druga = null;

            //aktywuje panel, który był wcześniej zamrożony
            panelKart.Enabled = true;

            //zatrzymuje timer
            timerZakrywacz.Stop();

        }

        private void timerCzasPodlgladu_Tick(object sender, EventArgs e)
        {
            //zmniejszamy czas poglądu o 1
            _settings.CzasPodgladu--;
            //aktualizujemy widok
            lblStartInfo.Text = $"Początek gry za {_settings.CzasPodgladu}";

            if (_settings.CzasPodgladu <= 0)
            {
                //ukrycie tekstu "począek gry za 0"
                lblStartInfo.Visible = false;

                //zakrycie wszystkich kart gdy skończy się odliczanie
                foreach (Control kontrolka in panelKart.Controls)
                {
                    MemoryCard card = (MemoryCard)kontrolka;
                    card.Zakryj();
                }
                //jeśli timer dopił do 0 mnależy go zatrzymać
                timerCzasPodlgladu.Stop();

                //uruchamiamy odliczanie w grze
                timerCzasGry.Start();
            }

        }

        private void timerCzasGry_Tick(object sender, EventArgs e)
        {
            //aktualizacja czasu gry
            _settings.CzasGry--;

            lblCzasWartosc.Text = _settings.CzasGry.ToString();

            if(_settings.CzasGry <= 0 || _settings.AktualnePunkty == _settings.MaxPunkty)
            {
                //zatrzymujemy timery
                timerCzasGry.Stop();
                timerZakrywacz.Stop();

                DialogResult yesno = MessageBox.Show($"Zdobyte punkty: {_settings.AktualnePunkty}. Czy chcesz zagrać ponownie?", 
                    "Koniec gry", MessageBoxButtons.YesNo);

                if(yesno == DialogResult.Yes)
                {
                    _settings.UstawStartowe();
                    GenerujKarty();
                    UstawKontrolki();

                    panelKart.Enabled = true;
                    _pierwsza = null;
                    _druga = null;
                    timerCzasPodlgladu.Start();
                }
                else
                {
                    Application.Exit();
                }

            }

        }
    }
}
