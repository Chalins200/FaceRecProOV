using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace MultiFaceRec
{
    public partial class splashscreen : Form
    {
        
        public splashscreen()            
        {
            InitializeComponent();
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            progressBar1.Increment(1);
            porciento.Text = progressBar1.Value.ToString();
            if (progressBar1.Value == 100)
            {
                Menu menu = new Menu();
                menu.Show();
                timer1.Stop();
                this.Hide();
            }

           
        }
    }
}
