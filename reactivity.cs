using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CCPE
{
    public partial class reactivity : Form
    {
        public reactivity()
        {
            {
                InitializeComponent();
            }
        }

        private void btHome_Click(object sender, EventArgs e)
        {
            Form1 form = new Form1();
            form.Show();
            this.Hide();
        }

        private void btSave_Click(object sender, EventArgs e)
        {
            SaveToExcel();
        }

        private void SaveToExcel()
        {
            // EPPlus kütüphanesi ile yeni bir Excel paketi oluştur
            using (var excelPackage = new ExcelPackage())
            {
                // Excel dosyasında yeni bir çalışma sayfası oluştur
                var worksheet = excelPackage.Workbook.Worksheets.Add("Sheet1");

                // RichTextBox içeriğini al
                string richTextBoxContent = rtbSolution.Text;

                // Her satırı ayrıştır ve Excel hücrelerine yaz
                string[] lines = richTextBoxContent.Split('\n');
                for (int i = 0; i < lines.Length; i++)
                {
                    string[] cells = lines[i].Split('\t');
                    for (int j = 0; j < cells.Length; j++)
                    {
                        worksheet.Cells[i + 1, j + 1].Value = cells[j];
                    }
                }

                // Excel dosyasını kaydetmek için bir diyalog kutusu göster
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.Filter = "Excel Files|*.xlsx";
                saveFileDialog1.Title = "Save to excel file";
                saveFileDialog1.ShowDialog();

                // Kullanıcı dosya seçtiyse Excel dosyasını kaydet
                if (saveFileDialog1.FileName != "")
                {
                    FileInfo excelFile = new FileInfo(saveFileDialog1.FileName);
                    excelPackage.SaveAs(excelFile);
                    MessageBox.Show("The Excel file has been saved successfully.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void btGenerate_Click(object sender, EventArgs e)
        {
            if (tbHomo.Text != "" && tbLumo.Text != "")
            {
                Double lumo = Convert.ToDouble(tbLumo.Text), homo = Convert.ToDouble(tbHomo.Text);
                if (rbAu.Checked)
                {
                    lumo = Convert.ToDouble(tbLumo.Text) * 27.2114;
                    homo = Convert.ToDouble(tbHomo.Text) * 27.2114;
                }

                double AG = lumo - homo;
                double I = -1 * homo;
                double A = -1 * lumo;
                double n = (I - A) / 2;
                double S = 1 / (2 * n);
                double M = (-1) * (I + A) / 2;
                double X = -1 * M;
                double w = (M * M) / (2 * n);

                rtbSolution.Text = "";

                rtbSolution.AppendText("Parameters (eV) \t Values\n");
                rtbSolution.AppendText("LUMO Energy \t" + Math.Round(lumo, 2).ToString() + "\n");
                rtbSolution.AppendText("HOMO Energy \t" + Math.Round(homo, 2).ToString() + "\n");
                rtbSolution.AppendText("Energy Gap (ΔG) \t" + Math.Round(AG, 2).ToString() + "\n");
                rtbSolution.AppendText("Ionization Potantial (I) \t" + Math.Round(I, 2).ToString() + "\n");
                rtbSolution.AppendText("Electron Affinity (A) \t" + Math.Round(A, 2).ToString() + "\n");
                rtbSolution.AppendText("Chemical Hardness (η) \t" + Math.Round(n, 2).ToString() + "\n");
                rtbSolution.AppendText("Chemical Softness (S) \t" + Math.Round(S, 2).ToString() + "\n");
                rtbSolution.AppendText("Electronegativity (χ) \t" + Math.Round(X, 2).ToString() + "\n");
                rtbSolution.AppendText("Chemical Potential (μ) \t" + Math.Round(M, 2).ToString() + "\n");
                rtbSolution.AppendText("Electrophilicity Index (ω) \t" + Math.Round(w, 2).ToString() + "\n");
            }
            else
            {
                MessageBox.Show("Please enter LUMO and HOMO energy parameters","Warning",MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }
        }

        private void btExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void reactivity_FormClosing(object sender, FormClosingEventArgs e)
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


        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            // Fare PictureBox üzerine geldiğinde arka plan rengini değiştir
            PictureBox pictureBox = sender as PictureBox;
            if (pictureBox != null)
            {
                pictureBox.BackColor = Color.Gray; // İstediğiniz rengi buraya yazabilirsiniz
            }
        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            // Fare PictureBox üzerinden ayrıldığında arka plan rengini eski haline döndür
            PictureBox pictureBox = sender as PictureBox;
            if (pictureBox != null)
            {
                pictureBox.BackColor = Color.Transparent;
            }
        }
    }
}
