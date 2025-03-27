using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace goruntuisleme
{
    public partial class MainPage : Form
    {
        public MainPage()
        {
            InitializeComponent();
            new PanelHandler(this.mainPanel);

        }
    }
}
