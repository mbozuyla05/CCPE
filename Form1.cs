using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections;
using System.IO;
using OfficeOpenXml;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace CCPE
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btOptimize_Click(object sender, EventArgs e)
        {
            
            Form4 form= new Form4();
            form.type = "Optimization";
            form.leftText = "Optimized Parameters";
            form.rightText = "Numbered Atoms";
            form.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form4 form = new Form4();
            form.type = "Assignments";
            form.leftText = ".vdf Parameters";
            form.rightText = ".dd2 Parameters";
            form.Show();
            this.Hide();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Kapatma işlemi kullanıcı tarafından gerçekleştirilmişse
            if (e.CloseReason == CloseReason.UserClosing)
            {
                // Formun kapatılmasını engelle
                e.Cancel = true;
                // Uygulamayı tamamen kapat
                Application.Exit();
            }
        }

        private void Form1_HelpButtonClicked(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MessageBox.Show("This software was made by CCPE Team.\n", "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            reactivity form1=new reactivity();
            form1.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
