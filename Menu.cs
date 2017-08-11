using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MultiFaceRec
{
    public partial class Menu : Form
    {
        public Menu()
        {
            InitializeComponent();
        }
        private Reconocimiento form = null;
        
        private Reconocimiento FormInstance
        {
            get
            {
                if (form == null)
                {
                    form = new Reconocimiento();
                    form.Disposed += new EventHandler(form_Disposed);
                }

                return form;
            }
        }

        private FrmPrincipal frmp = null;
        void form_Disposed(object sender, EventArgs e)
        {
            form = null;
        }
        private FrmPrincipal FrmInstance
        {
            get
            {
                if (frmp == null)
                {
                    frmp = new FrmPrincipal();
                    frmp.Disposed += new EventHandler(frm_Disposed);
                }

                return frmp;
            }
        }

        void frm_Disposed(object sender, EventArgs e)
        {
            frmp = null;
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Reconocimiento frm = this.FormInstance;
            frm.Show();

            // si el Formulario estaba abierto seguramente este en segundo plano
            // con esta linea hacemos que pase adelante
            frm.BringToFront();
            this.Hide();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            FrmPrincipal frmp = this.FrmInstance;
            frmp.Show();

            // si el Formulario estaba abierto seguramente este en segundo plano
            // con esta linea hacemos que pase adelante
            frmp.BringToFront();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("¿Desea Salir?", "Salir", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes)
            {
                Application.Exit();
            }
        }
    }
}
