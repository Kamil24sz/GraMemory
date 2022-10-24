using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
        }
    }
}
