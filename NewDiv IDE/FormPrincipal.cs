using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace NewDiv_IDE
{
    public partial class FormPrincipal : Form
    {
        public FormPrincipal()
        {
            InitializeComponent();
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void cargarProgramaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                System.IO.FileInfo fi = new System.IO.FileInfo(ofd.FileName);

                // Creamos una nueva pestaña
                tabControlFiles.TabPages.Add(fi.FullName, fi.Name);

                // Creamos un textbox
                RichTextBox rtb = new RichTextBox();
                rtb.Dock = DockStyle.Fill;
                tabControlFiles.TabPages[fi.FullName].Controls.Add(rtb);
                
                // Leemos el fichero
                System.IO.FileStream fs = fi.OpenRead();
                byte[] array = new byte[fs.Length];
                fs.Read(array, 0, (int)fs.Length);

                // Volcamos el array al textbox
                //rtb.Text = BitConverter.ToString(array);
                rtb.Text = System.Text.ASCIIEncoding.ASCII.GetString(array);
            }
        }

        private void compilarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tabControlFiles.SelectedTab == null)
            {
                MessageBox.Show("No hay ningún programa abierto");
            }

            // Enviamos el archivo actual
            NewDiv_Compiler.Compiler comp = new NewDiv_Compiler.Compiler();

            //try
            //{
                String result = comp.Compile(tabControlFiles.SelectedTab.Controls[0].Text);

                // La compilacion ha ido bien
                // Creamos una nueva pestaña
                tabControlFiles.TabPages.Add("Result", "Result");

                // Creamos un textbox
                RichTextBox rtb = new RichTextBox();
                rtb.Dock = DockStyle.Fill;
                tabControlFiles.TabPages["Result"].Controls.Add(rtb);

                rtb.Text = result;
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}

        }
    }
}