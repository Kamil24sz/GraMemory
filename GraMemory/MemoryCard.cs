using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GraMemory
{
    public class MemoryCard : Label
    {
        //Potrzebne aby porównać czy odkryte karty są takie same
        public Guid id;

        //obrazek gdy karta jest zakryta
        public Image Tyl;

        //obrazek gdy karta jest odkryta
        public Image Obrazek;

        public MemoryCard(Guid id, string tylPath, string obrazekPath)
        {
            this.id = id;
            Tyl = Image.FromFile(tylPath);
            Obrazek = Image.FromFile(obrazekPath);
        }

        public void Zakryj()
        {
            BackgroundImage = Tyl;
            Enabled = true;
        }

        public void Odkryj()
        {
            BackgroundImage = Obrazek;
            Enabled = false;
        }

    }
}
