using OfficeOpenXml;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CCPE
{
    public partial class Form4 : Form
    {
        public string leftText;
        public string rightText;
        public string type;
        public Form4()
        {
            InitializeComponent();

        }

        private void btCalculate_Click(object sender, EventArgs e)
        {
            if (type == "Assignments")
            {
                Assignment();

            }
            else if (type == "Optimization")
            {
                Optimization();
            }
        }

        private void Assignment()
        {
            string hedef = "Calculated vibrational frequency with PED assignments of the title compound\n";
            hedef += "Intensity\tUnscaled\tScaled\tAssignments\n";
            double scaled = 0.01;
            if (double.TryParse(toolStripTextBox2.Text, out double result))
                scaled = result/100;
            int hatalısatır = 0;
            try
            {
                SortedList kaynak_vdf = new SortedList();
                SortedList kaynak_dd2 = new SortedList();

                kaynak_vdf = Kumele(rtbSourceLeft.Text);
                kaynak_dd2 = Kumele(rtbSourceRight.Text);

                for (int i = 0; i < kaynak_vdf.Count; i++)
                {
                    hatalısatır = i + 1;
                    string[] kaynakSatir = ((string[])kaynak_vdf.GetByIndex(i));
                    hedef += kaynakSatir[0] + "\t" + kaynakSatir[1] + "\t"+(Convert.ToDouble(kaynakSatir[1])*scaled).ToString()+"\t";

                    ArrayList key = new ArrayList();
                    ArrayList value = new ArrayList();

                    for (int j = 2; j < kaynakSatir.Length; j = j + 2)
                    {

                        string gec = "";

                        string[] ReferansSatir = ((string[])kaynak_dd2.GetByIndex(Convert.ToInt32(kaynakSatir[j].Substring(1)) - 1));

                        if (ReferansSatir[3] == "STRE")
                            gec = "ϑ " + ReferansSatir[6];
                        else if (ReferansSatir[3] == "BEND")
                            gec = "β " + ReferansSatir[7];
                        else if (ReferansSatir[3] == "TORS")
                            gec = "τ " + ReferansSatir[8];
                        else if (ReferansSatir[3] == "OUT")
                            gec = "ω " + ReferansSatir[8];
                        if (key.Contains(gec))
                            value[key.IndexOf(gec)] = Math.Abs(Convert.ToInt32(kaynakSatir[j + 1])) + Math.Abs(Convert.ToInt32(value[key.IndexOf(gec)]));
                        else
                        {
                            key.Add(gec);
                            value.Add(Math.Abs(Convert.ToInt32(kaynakSatir[j + 1])));
                        }
                    }

                    for (int k = 0; k < key.Count; k++)
                    {
                        if (k < key.Count - 1)
                            hedef += key[k] + "(" + value[k] + ") + ";
                        else
                            hedef += key[k] + "(" + value[k] + ")";
                    }
                    hedef += "\n";
                }

                hedef += "\n\n\n ϑ - streching, β - bending, τ - torsion, ω - out plane bending.\n";
                rtbResult.Text = hedef;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Check line " + hatalısatır.ToString() + ". in .vdf file.\n\nMessage:" + ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private SortedList Kumele(string text)
        {
            // Metni satır bazında ayır
            string[] lines = text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);

            // SortedList oluştur
            SortedList sortedList = new SortedList();

            // Metni SortedList'e aktar
            for (int i = 0; i < lines.Length; i++)
            {
                string[] words = lines[i].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                sortedList.Add(i, words);
            }

            return sortedList;
        }


        private void tsbSave_Click(object sender, EventArgs e)
        {
            SaveToExcel();
        }

        private void tsbOpenMainUp_Click(object sender, EventArgs e)
        {
            //rtbSourceLeft.Clear();
            //rtbResult.Clear();
            //if (type == "Optimization")
            //{
            //    rtbSourceRight.Clear();
            //}
            // Dosya seçme iletişim kutusunu göster
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (type == "Optimization")
                openFileDialog.Filter = "Text Files|*.log";
            else
                openFileDialog.Filter = "Text Files|*.vdf";
            //rtbSourceLeft.Clear();
            //rtbResult.Clear();
            if (type == "Optimization")
            {
                rtbSourceRight.Clear();
            }
            openFileDialog.Title = "Select a file for " + leftText;

            // Kullanıcı bir dosya seçtiyse
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // Dosyayı oku ve içeriğini RichTextBox'a aktar
                    string text = openFileDialog.FileName;
                    if (type == "Assignments")
                    {

                        rtbSourceLeft.Text = AssLeft(text);

                    }
                    else if (type == "Optimization")
                    {
                        rtbSourceLeft.Text = OpLeft(text);
                        rtbSourceRight.Text = OpRight(text);
                    }
                    rtbResult.Clear();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while uploading the file. " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private string OpRight(string filePath)
        {

            List<string> parametreSatirlari = new List<string>();
            string[] lines = File.ReadAllLines(filePath);
            bool kayitBasladi = false;
            int startLine = -1;

            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Contains("Mulliken atomic charges:"))
                {
                    startLine = i + 2; // Başlangıç satırı (2 satır sonrası)
                }

                if (i >= startLine && startLine != -1)
                {
                    string trimmedLine = lines[i].Trim();

                    if (trimmedLine.StartsWith("Sum of Mulliken atomic charges"))
                    {
                        break; // Durdurma koşulu
                    }
                    string[] strings = trimmedLine.Split(' ');
                    string deger = "";
                    int j = 0;
                    foreach(string stringg in strings)
                    {

                        if (stringg.Trim() != "")
                        {
                            deger += stringg + " ";
                            j++;
                        }
                        if (j == 2)
                            break;
                    }
                    kayitBasladi = true;
                    parametreSatirlari.Add(deger);
                }
            }

            // Çıktıyı ekrana yazdır
            string text = "";
            foreach (var satir in parametreSatirlari)
            {
                text += satir + "\n";
            }
            return text;
        }

        private string OpLeft(string filePath)
        {
            List<string> parametreSatirlari = new List<string>();
            string[] lines = File.ReadAllLines(filePath);
            bool kayitBasladi = false;
            int startLine = -1;

            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Contains("!   Optimized Parameters   !"))
                {
                    startLine = i + 5; // Başlangıç satırını belirle
                }

                if (i >= startLine && startLine != -1)
                {
                    string trimmedLine = lines[i].Trim();

                    if (trimmedLine.StartsWith("!"))
                    {
                        kayitBasladi = true;
                        parametreSatirlari.Add(lines[i]);
                    }
                    else if (kayitBasladi) // ! işareti bittiyse durdur
                    {
                        break;
                    }
                }
            }
            string text = "";
            foreach (var satir in parametreSatirlari)
            {
                text += satir + "\n";
            }
            return text;
        }

        private string AssRight(string filePath)
        {
            List<string> parametreSatirlari = new List<string>();
            string[] lines = File.ReadAllLines(filePath);
            bool sectionStarted = false; // "Average max." ile başlayan bölümün başladığını takip eder

            foreach (string line in lines)
            {
                string trimmedLine = line.Trim();

                // Bölüm başlangıcını tespit et
                if (!sectionStarted)
                {
                    if (trimmedLine.StartsWith("Average max."))
                    {
                        sectionStarted = true;
                    }
                    continue;
                }

                // Bölüm sonu: "***" ile başlayan satır
                if (trimmedLine.StartsWith("***"))
                {
                    break;
                }

                // "s" ile başlayan satırları al
                if (trimmedLine.StartsWith("s"))
                {
                    parametreSatirlari.Add(line);
                }
            }

            string text = "";
            foreach (var satir in parametreSatirlari)
            {
                text += satir + "\n";
            }
            return text;
        }

        private string AssLeft(string filePath)
        {
            List<string> parametreSatirlari = new List<string>();
            string[] lines = File.ReadAllLines(filePath);
            bool kayitBasladi = false;
            int startIndex = -1;

            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Contains("diagonality factor"))
                {
                    startIndex = i + 2; // 2 satır sonrası başlangıç
                }

                if (i >= startIndex && startIndex != -1)
                {
                    // "with" veya "Frequencies/PED" içeren satırla karşılaşınca dur
                    if (lines[i].Contains("with") || lines[i].Contains("Frequencies/PED"))
                    {
                        break;
                    }

                    // "* 322.16" gibi "*" ile başlayan sayıları kaldır
                    string temizSatir = Regex.Replace(lines[i], @"\*\s*\d+(\.\d+)?", "").Trim();


                    parametreSatirlari.Add(temizSatir);
                }
            }
            string text = "";
            foreach (var satir in parametreSatirlari)
            {
                text += satir + "\n";
            }
            return text;
        }

        private void SaveToExcel()
        {
            // EPPlus kütüphanesi ile yeni bir Excel paketi oluştur
            using (var excelPackage = new ExcelPackage())
            {
                // Excel dosyasında yeni bir çalışma sayfası oluştur
                var worksheet = excelPackage.Workbook.Worksheets.Add("Sheet1");

                // RichTextBox içeriğini al
                string richTextBoxContent = rtbResult.Text;

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

        private void Optimization()
        {
            string hedef = " ";

            int hatalısatır = 0;
            try
            {
                SortedList kaynak_ust = new SortedList();
                SortedList kaynak_alt = new SortedList();

                kaynak_ust = Kumele(rtbSourceLeft.Text);
                kaynak_alt = Kumele(rtbSourceRight.Text);

                //Atomların etiketleriini ayarla
                ArrayList AtomSira = new ArrayList();
                for (int i = 0; i < kaynak_alt.Count; i++)
                {
                    string[] kaynakSatir = ((string[])kaynak_alt.GetByIndex(i));

                    AtomSira.Add(kaynakSatir[1] + kaynakSatir[0]);
                }
                int flag = 0;
                for (int i = 0; i < kaynak_ust.Count; i++)
                {
                    string[] kaynakSatir = ((string[])kaynak_ust.GetByIndex(i));

                    string[] bag = kaynakSatir[2].Substring(2, kaynakSatir[2].Length - 3).Split(',');
                    string bagN = "";

                    if (bag.Length > flag && flag == 0)
                    {
                        hedef += "Bond Lenght (Å)\t\n";
                        flag = bag.Length;
                    }
                    else if (bag.Length > flag && flag == 2)
                    {
                        hedef += "\nBond Angle (˚)\t\n";
                        flag = bag.Length;
                    }
                    else if (bag.Length > flag && flag == 3)
                    {
                        hedef += "\nDihedral Angle (˚)\t\n";
                        flag = bag.Length;
                    }
                    for (int j = 0; j < bag.Length; j++)
                    {
                        bagN += AtomSira[Convert.ToInt32(bag[j]) - 1].ToString() + "-";

                    }
                    if (!string.IsNullOrEmpty(bagN))
                    {
                        char[] metinDizi = bagN.ToCharArray();
                        metinDizi[metinDizi.Length - 1] = ' ';
                        bagN = new string(metinDizi);
                    }
                    double doubleDeger;

                    if (double.TryParse(kaynakSatir[3].Replace(".", ","), out doubleDeger))
                    {
                        doubleDeger = Math.Round(doubleDeger, 2);
                    }
                    else
                    {
                        MessageBox.Show("Check line " + hatalısatır.ToString() + ". in optimized parameters file.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    }


                    hedef += bagN + "\t" + doubleDeger.ToString() + "\n";
                }
                hedef += "\n\n\n\n";
                rtbResult.Text = hedef;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Check line " + hatalısatır.ToString() + ". in optimized parameters file.\n\nMessage:" + ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            sourceLeftToolStripMenuItem.Text = leftText;
            sourceRightToolStripMenuItem.Text = rightText;
            lSourceLeft.Text = leftText;
            lSourceRight.Text = rightText;
            toolStripButton1.Text = "&Open " + leftText + " File";
            openToolStripButton.Text = "&Open " + rightText + " File";
            if (type == "Optimization")
            {
                openToolStripButton.Visible = false;
                toolStripTextBox2.Visible = false;
                toolStripLabel1.Visible = false;
            }
            //rtbSourceLeft.Text = "! R1    R(1,2)                  1.4042         -DE/DX =    0.0                 !\r\n ! R2    R(1,6)                  1.4035         -DE/DX =    0.0                 !\r\n ! R3    R(1,12)                 1.4922         -DE/DX =    0.0                 !\r\n ! R4    R(2,3)                  1.3851         -DE/DX =    0.0                 !\r\n ! R5    R(2,7)                  1.0835         -DE/DX =    0.0                 !\r\n ! R6    R(3,4)                  1.3928         -DE/DX =    0.0                 !\r\n ! R7    R(3,11)                 1.3449         -DE/DX =    0.0                 !\r\n ! R8    R(4,5)                  1.39           -DE/DX =    0.0                 !\r\n ! R9    R(4,8)                  1.0837         -DE/DX =    0.0                 !\r\n ! R10   R(5,6)                  1.3893         -DE/DX =    0.0                 !\r\n ! R11   R(5,10)                 1.345          -DE/DX =    0.0                 !\r\n ! R12   R(6,9)                  1.0817         -DE/DX =    0.0                 !\r\n ! R13   R(9,15)                 2.3981         -DE/DX =    0.0                 !\r\n ! R14   R(12,13)                1.2219         -DE/DX =    0.0                 !\r\n ! R15   R(12,14)                1.5413         -DE/DX =    0.0                 !\r\n ! R16   R(13,23)                2.3985         -DE/DX =    0.0                 !\r\n ! R17   R(14,15)                1.2219         -DE/DX =    0.0                 !\r\n ! R18   R(14,19)                1.4922         -DE/DX =    0.0                 !\r\n ! R19   R(16,17)                1.39           -DE/DX =    0.0                 !\r\n ! R20   R(16,21)                1.3928         -DE/DX =    0.0                 !\r\n ! R21   R(16,22)                1.0837         -DE/DX =    0.0                 !\r\n ! R22   R(17,18)                1.3893         -DE/DX =    0.0                 !\r\n ! R23   R(17,26)                1.345          -DE/DX =    0.0                 !\r\n ! R24   R(18,19)                1.4035         -DE/DX =    0.0                 !\r\n ! R25   R(18,23)                1.0817         -DE/DX =    0.0                 !\r\n ! R26   R(19,20)                1.4042         -DE/DX =    0.0                 !\r\n ! R27   R(20,21)                1.3851         -DE/DX =    0.0                 !\r\n ! R28   R(20,24)                1.0835         -DE/DX =    0.0                 !\r\n ! R29   R(21,25)                1.3449         -DE/DX =    0.0                 !\r\n ! A1    A(2,1,6)              120.4219         -DE/DX =    0.0                 !\r\n ! A2    A(2,1,12)             116.7644         -DE/DX =    0.0                 !\r\n ! A3    A(6,1,12)             122.7803         -DE/DX =    0.0                 !\r\n ! A4    A(1,2,3)              118.7696         -DE/DX =    0.0                 !\r\n ! A5    A(1,2,7)              120.2601         -DE/DX =    0.0                 !\r\n ! A6    A(3,2,7)              120.9702         -DE/DX =    0.0                 !\r\n ! A7    A(2,3,4)              122.3695         -DE/DX =    0.0                 !\r\n ! A8    A(2,3,11)             119.2907         -DE/DX =    0.0                 !\r\n ! A9    A(4,3,11)             118.3395         -DE/DX =    0.0                 !\r\n ! A10   A(3,4,5)              117.4075         -DE/DX =    0.0                 !\r\n ! A11   A(3,4,8)              121.302          -DE/DX =    0.0                 !\r\n ! A12   A(5,4,8)              121.2905         -DE/DX =    0.0                 !\r\n ! A13   A(4,5,6)              122.6512         -DE/DX =    0.0                 !\r\n ! A14   A(4,5,10)             118.4652         -DE/DX =    0.0                 !\r\n ! A15   A(6,5,10)             118.8835         -DE/DX =    0.0                 !\r\n ! A16   A(1,6,5)              118.38           -DE/DX =    0.0                 !\r\n ! A17   A(1,6,9)              122.1215         -DE/DX =    0.0                 !\r\n ! A18   A(5,6,9)              119.4826         -DE/DX =    0.0                 !\r\n ! A19   A(1,12,13)            122.2044         -DE/DX =    0.0                 !\r\n ! A20   A(1,12,14)            119.659          -DE/DX =    0.0                 !\r\n ! A21   A(13,12,14)           118.0556         -DE/DX =    0.0                 !\r\n ! A22   A(12,14,15)           118.0569         -DE/DX =    0.0                 !\r\n ! A23   A(12,14,19)           119.6586         -DE/DX =    0.0                 !\r\n ! A24   A(15,14,19)           122.2037         -DE/DX =    0.0                 !\r\n ! A25   A(17,16,21)           117.4069         -DE/DX =    0.0                 !\r\n ! A26   A(17,16,22)           121.2907         -DE/DX =    0.0                 !\r\n ! A27   A(21,16,22)           121.3023         -DE/DX =    0.0                 !\r\n ! A28   A(16,17,18)           122.6518         -DE/DX =    0.0                 !\r\n ! A29   A(16,17,26)           118.4654         -DE/DX =    0.0                 !\r\n ! A30   A(18,17,26)           118.8827         -DE/DX =    0.0                 !\r\n ! A31   A(17,18,19)           118.3801         -DE/DX =    0.0                 !\r\n ! A32   A(17,18,23)           119.479          -DE/DX =    0.0                 !\r\n ! A33   A(19,18,23)           122.125          -DE/DX =    0.0                 !\r\n ! A34   A(14,19,18)           122.7823         -DE/DX =    0.0                 !\r\n ! A35   A(14,19,20)           116.7631         -DE/DX =    0.0                 !\r\n ! A36   A(18,19,20)           120.421          -DE/DX =    0.0                 !\r\n ! A37   A(19,20,21)           118.7705         -DE/DX =    0.0                 !\r\n ! A38   A(19,20,24)           120.2597         -DE/DX =    0.0                 !\r\n ! A39   A(21,20,24)           120.9698         -DE/DX =    0.0                 !\r\n ! A40   A(16,21,20)           122.3693         -DE/DX =    0.0                 !\r\n ! A41   A(16,21,25)           118.3395         -DE/DX =    0.0                 !\r\n ! A42   A(20,21,25)           119.2908         -DE/DX =    0.0                 !\r\n ! D1    D(6,1,2,3)             -0.2062         -DE/DX =    0.0                 !\r\n ! D2    D(6,1,2,7)            179.7443         -DE/DX =    0.0                 !\r\n ! D3    D(12,1,2,3)          -178.1628         -DE/DX =    0.0                 !\r\n ! D4    D(12,1,2,7)             1.7878         -DE/DX =    0.0                 !\r\n ! D5    D(2,1,6,5)              0.114          -DE/DX =    0.0                 !\r\n ! D6    D(2,1,6,9)            178.6533         -DE/DX =    0.0                 !\r\n ! D7    D(12,1,6,5)           177.9438         -DE/DX =    0.0                 !\r\n ! D8    D(12,1,6,9)            -3.5169         -DE/DX =    0.0                 !\r\n ! D9    D(2,1,12,13)           -1.3857         -DE/DX =    0.0                 !\r\n ! D10   D(2,1,12,14)         -178.0478         -DE/DX =    0.0                 !\r\n ! D11   D(6,1,12,13)         -179.2898         -DE/DX =    0.0                 !\r\n ! D12   D(6,1,12,14)            4.0481         -DE/DX =    0.0                 !\r\n ! D13   D(1,2,3,4)              0.2207         -DE/DX =    0.0                 !\r\n ! D14   D(1,2,3,11)          -180.003          -DE/DX =    0.0                 !\r\n ! D15   D(7,2,3,4)           -179.7294         -DE/DX =    0.0                 !\r\n ! D16   D(7,2,3,11)             0.0469         -DE/DX =    0.0                 !\r\n ! D17   D(2,3,4,5)             -0.1373         -DE/DX =    0.0                 !\r\n ! D18   D(2,3,4,8)            179.9138         -DE/DX =    0.0                 !\r\n ! D19   D(11,3,4,5)          -179.9157         -DE/DX =    0.0                 !\r\n ! D20   D(11,3,4,8)             0.1355         -DE/DX =    0.0                 !\r\n ! D21   D(3,4,5,6)              0.0398         -DE/DX =    0.0                 !\r\n ! D22   D(3,4,5,10)          -179.8779         -DE/DX =    0.0                 !\r\n ! D23   D(8,4,5,6)            179.9887         -DE/DX =    0.0                 !\r\n ! D24   D(8,4,5,10)             0.071          -DE/DX =    0.0                 !\r\n ! D25   D(4,5,6,1)             -0.0305         -DE/DX =    0.0                 !\r\n ! D26   D(4,5,6,9)           -178.6094         -DE/DX =    0.0                 !\r\n ! D27   D(10,5,6,1)           179.8869         -DE/DX =    0.0                 !\r\n ! D28   D(10,5,6,9)             1.308          -DE/DX =    0.0                 !\r\n ! D29   D(1,12,14,15)          48.502          -DE/DX =    0.0                 !\r\n ! D30   D(1,12,14,19)        -134.6955         -DE/DX =    0.0                 !\r\n ! D31   D(13,12,14,15)       -128.2977         -DE/DX =    0.0                 !\r\n ! D32   D(13,12,14,19)         48.5049         -DE/DX =    0.0                 !\r\n ! D33   D(12,14,19,18)          4.0686         -DE/DX =    0.0                 !\r\n ! D34   D(12,14,19,20)       -178.0353         -DE/DX =    0.0                 !\r\n ! D35   D(15,14,19,18)       -179.2663         -DE/DX =    0.0                 !\r\n ! D36   D(15,14,19,20)         -1.3702         -DE/DX =    0.0                 !\r\n ! D37   D(21,16,17,18)          0.0361         -DE/DX =    0.0                 !\r\n ! D38   D(21,16,17,26)       -179.8798         -DE/DX =    0.0                 !\r\n ! D39   D(22,16,17,18)        179.9875         -DE/DX =    0.0                 !\r\n ! D40   D(22,16,17,26)          0.0716         -DE/DX =    0.0                 !\r\n ! D41   D(17,16,21,20)         -0.135          -DE/DX =    0.0                 !\r\n ! D42   D(17,16,21,25)       -179.9137         -DE/DX =    0.0                 !\r\n ! D43   D(22,16,21,20)        179.9136         -DE/DX =    0.0                 !\r\n ! D44   D(22,16,21,25)          0.1349         -DE/DX =    0.0                 !\r\n ! D45   D(16,17,18,19)         -0.0305         -DE/DX =    0.0                 !\r\n ! D46   D(16,17,18,23)       -178.6113         -DE/DX =    0.0                 !\r\n ! D47   D(26,17,18,19)        179.8851         -DE/DX =    0.0                 !\r\n ! D48   D(26,17,18,23)          1.3044         -DE/DX =    0.0                 !\r\n ! D49   D(17,18,19,14)        177.9407         -DE/DX =    0.0                 !\r\n ! D50   D(17,18,19,20)          0.1192         -DE/DX =    0.0                 !\r\n ! D51   D(23,18,19,14)         -3.5182         -DE/DX =    0.0                 !\r\n ! D52   D(23,18,19,20)        178.6603         -DE/DX =    0.0                 !\r\n ! D53   D(14,19,20,21)       -178.1615         -DE/DX =    0.0                 !\r\n ! D54   D(14,19,20,24)          1.7903         -DE/DX =    0.0                 !\r\n ! D55   D(18,19,20,21)         -0.2127         -DE/DX =    0.0                 !\r\n ! D56   D(18,19,20,24)        179.7391         -DE/DX =    0.0                 !\r\n ! D57   D(19,20,21,16)          0.2234         -DE/DX =    0.0                 !\r\n ! D58   D(19,20,21,25)       -179.9999         -DE/DX =    0.0                 !\r\n ! D59   D(24,20,21,16)       -179.7281         -DE/DX =    0.0                 !\r\n ! D60   D(24,20,21,25)          0.0486         -DE/DX =    0.0                 !";
            //rtbSourceRight.Text = "     1  C    5.010106   0.393592   0.005144  -0.025035   0.003226   0.441140\r\n     2  C    0.393592   5.320777   0.431396  -0.084021  -0.035951  -0.091279\r\n     3  C    0.005144   0.431396   4.537805   0.404493   0.021349  -0.033555\r\n     4  C   -0.025035  -0.084021   0.404493   5.375541   0.401320  -0.085975\r\n     5  C    0.003226  -0.035951   0.021349   0.401320   4.563169   0.409556\r\n     6  C    0.441140  -0.091279  -0.033555  -0.085975   0.409556   5.379588\r\n     7  H   -0.041169   0.346844  -0.035515   0.006493  -0.000102   0.006233\r\n     8  H   -0.000415   0.006874  -0.036251   0.340921  -0.034850   0.006928\r\n     9  H   -0.027515   0.005867  -0.000119   0.006347  -0.035666   0.328919\r\n    10  F    0.003059   0.000039   0.002492  -0.040794   0.306602  -0.041327\r\n    11  F    0.003000  -0.040616   0.302705  -0.037798   0.002530   0.000059\r\n    12  C    0.306263  -0.025125   0.003721   0.000325   0.002916  -0.023949\r\n    13  O   -0.079032   0.001727   0.000846   0.000002  -0.000038   0.003463\r\n    14  C   -0.063151   0.009025  -0.000018  -0.000020  -0.000154  -0.010367\r\n    15  O   -0.010332  -0.000336  -0.000006  -0.000017   0.002225  -0.026219\r\n    16  C    0.000000   0.000000   0.000000   0.000000   0.000000   0.000000\r\n    17  C    0.000010   0.000000   0.000000   0.000000   0.000000   0.000000\r\n    18  C    0.000181   0.000001   0.000000   0.000000   0.000000  -0.000018\r\n    19  C    0.004252  -0.000116   0.000000   0.000000   0.000010   0.000181\r\n    20  C   -0.000116   0.000001   0.000000   0.000000   0.000000   0.000001\r\n    21  C    0.000000   0.000000   0.000000   0.000000   0.000000   0.000000\r\n    22  H    0.000000   0.000000   0.000000   0.000000   0.000000   0.000000\r\n    23  H    0.000544   0.000010   0.000000   0.000000   0.000000  -0.000015\r\n    24  H    0.000007   0.000000   0.000000   0.000000   0.000000   0.000000\r\n    25  F    0.000000   0.000000   0.000000   0.000000   0.000000   0.000000\r\n    26  F    0.000000   0.000000   0.000000   0.000000   0.000000   0.000000";
            //rtbSourceLeft.Text = "   0.58  3265.99   s1 33   s3 -24   s5 -18   s6 -22\r\n   6.87  3265.95   s1 23   s3 -17   s5 26   s6 32\r\n   1.48  3248.65   s1 14   s2 -35   s4 -33   s6 12\r\n  10.34  3248.55   s1 13   s2 -33   s4 36   s6 -12\r\n   0.00  3247.02   s2 10   s3 17   s4 20   s5 -33   s6 13\r\n   0.07  3247.01   s1 12   s2 19   s3 36   s5 16\r\n  71.03  1754.46   s8 87\r\n 263.17  1754.25   s7 93\r\n  26.23  1663.51   s11 -28   s15 -28\r\n   0.56  1662.99   s11 -27   s15 27\r\n 300.67  1653.98   s9 16   s14 17   s17 16\r\n  29.76  1653.84   s9 17   s14 -16   s18 14\r\n   0.12  1507.24   s11 10   s15 -11\r\n   9.32  1506.69   s11 -10   s15 -10\r\n   2.41  1491.45   s18 26\r\n 199.67  1490.63   s17 27\r\n  42.52  1386.53   s18 -14   s20 16   s21 21   s24 16\r\n  63.91  1378.67   s10 27   s12 -27   s17 -23\r\n  12.92  1375.62   s10 -33   s12 -33   s18 -11\r\n 435.91  1361.65   s20 -20   s24 20\r\n   1.96  1271.16   s21 -27   s31 15   s34 -13\r\n   2.06  1264.30   s28 -37   s32 12   s33 19\r\n   0.25  1262.88   s28 -29   s32 -18   s33 -17\r\n 180.57  1172.39   s19 -22   s23 -22   s30 17   s31 10   s33 10\r\n  12.72  1171.66   s19 23   s23 -24   s29 -11   s30 -13   s33 10\r\n 202.90  1157.54   s22 -25   s29 -15   s32 11\r\n   1.18  1095.26   s29 14   s31 11   s32 11   s40 11   s41 11\r\n  44.60  1033.25   s9 16   s14 16\r\n   5.91  1024.00   s9 -22   s14 22   s19 12   s23 -12\r\n   2.72  1021.55   s13 -24   s16 24   s27 24   s37 -24\r\n   0.55  1021.51   s13 21   s16 21   s27 22   s37 24\r\n  52.87  1011.15   s22 -20\r\n   1.47   912.04   s50 -15   s51 23   s55 26\r\n   9.63   906.77   s50 12   s51 -29   s53 -11   s54 -10   s55 19\r\n   0.65   888.11   s52 -33   s53 -20   s54 -11   s55 -12\r\n   1.71   886.48   s25 25\r\n   0.31   881.14   s52 35   s53 -15   s55 -24\r\n  12.66   860.42   s51 13   s54 -26\r\n  86.53   857.70   s50 -15   s54 -29\r\n   8.66   819.16   s50 12   s66 -48\r\n  16.24   741.36   s49 -16   s67 -30\r\n 240.89   726.62   s35 33   s45 -11\r\n   3.07   665.51   s50 -11   s57 14   s61 12   s62 27\r\n   0.65   663.76   s50 11   s57 15   s61 12   s62 -28\r\n   0.13   600.33   s68 -39   s71 41\r\n   0.04   600.18   s68 41   s71 39\r\n   0.58   578.16\r\n  17.59   557.82\r\n   1.29   540.81   s27 -12   s37 -10\r\n   0.44   529.23\r\n   0.01   524.40   s47 -14\r\n  10.37   512.79   s26 -16   s36 18   s38 -17   s39 18\r\n   9.87   512.02   s44 24   s47 18\r\n   1.26   511.71   s36 -13   s39 17   s44 -15\r\n   7.66   417.83   s22 19   s35 17\r\n   1.24   408.34   s25 11   s67 14\r\n   4.55   346.56   s34 -43   s67 -14\r\n   0.53   329.16   s43 -33   s48 -31\r\n   7.82   321.20   s43 -36   s48 34\r\n   6.10   263.12   s56 29   s59 17\r\n   0.12   255.93   s56 -37   s59 31\r\n  13.60   245.38   s35 17   s45 19   s59 10\r\n   0.56   235.80   s57 13   s58 -24   s60 15   s69 19   s70 -19\r\n   1.49   235.40   s57 11   s58 20   s60 14   s69 17   s70 17\r\n   0.02   198.74   s25 28   s42 -10\r\n   4.63   147.37   s49 25   s67 -26\r\n   0.61   137.84   s57 -13   s59 11   s60 10   s62 10   s72 26\r\n   1.86   136.95   s42 11   s49 17   s72 17\r\n   0.51   112.64   s42 -15   s46 -45   s63 -10\r\n   0.13    27.53   s46 -19   s63 31   s64 17   s65 -18\r\n   0.25    21.95   s63 10   s64 11   s65 57\r\n   0.11    20.03   s63 -35   s64 54\r\n";
            //rtbSourceRight.Text = "s 1     1.00   STRE    2    7   CH    1.083472\r\ns 2     1.00   STRE    4    8   CH    1.083688\r\ns 3     1.00   STRE    6    9   CH    1.081685\r\ns 4     1.00   STRE   16   22   CH    1.083687\r\ns 5     1.00   STRE   18   23   CH    1.081685\r\ns 6     1.00   STRE   20   24   CH    1.083472\r\ns 7     1.00   STRE   13   12   OC    1.221880\r\ns 8     1.00   STRE   15   14   OC    1.221877\r\ns 9     1.00   STRE    2    3   CC    1.385113\r\ns 10    1.00   STRE   20   21   CC    1.385112\r\ns 11    1.00   STRE    4    3   CC    1.392754\r\ns 12    1.00   STRE    5    4   CC    1.390012\r\ns 13    1.00   STRE    6    5   CC    1.389340\r\ns 14    1.00   STRE   16   17   CC    1.390010\r\ns 15    1.00   STRE   17   18   CC    1.389343\r\ns 16    1.00   STRE   21   16   CC    1.392754\r\ns 17    1.00   STRE    1    6   CC    1.403466\r\ns 18    1.00   STRE   18   19   CC    1.403462\r\ns 19    1.00   STRE   10    5   FC    1.345027\r\ns 20    1.00   STRE   11    3   FC    1.344893\r\ns 21    1.00   STRE   12    1   CC    1.492192\r\ns 22    1.00   STRE   19   14   CC    1.492202\r\ns 23    1.00   STRE   25   21   FC    1.344893\r\ns 24    1.00   STRE   26   17   FC    1.345029\r\ns 25    1.00   STRE   14   12   CC    1.541286\r\ns 26    1.00   BEND    2    3    4   CCC   122.37\r\ns 27    1.00   BEND   20   21   16   CCC   122.37\r\ns 28    1.00   BEND    7    2    3   HCC   120.97\r\ns 29    1.00   BEND    8    4    5   HCC   121.29\r\ns 30    1.00   BEND    9    6    5   HCC   119.48\r\ns 31    1.00   BEND   22   16   21   HCC   121.30\r\ns 32    1.00   BEND   23   18   19   HCC   122.12\r\ns 33    1.00   BEND   24   20   21   HCC   120.97\r\ns 34    1.00   BEND   13   12   14   OCC   118.06\r\ns 35    1.00   BEND   15   14   19   OCC   122.20\r\ns 36    1.00   BEND    5    4    3   CCC   117.41\r\ns 37    1.00   BEND    6    5    4   CCC   122.65\r\ns 38    1.00   BEND   16   17   18   CCC   122.65\r\ns 39    1.00   BEND   17   18   19   CCC   118.38\r\ns 40    1.00   BEND   21   16   17   CCC   117.41\r\ns 41    1.00   BEND    1    6    5   CCC   118.38\r\ns 42    1.00   BEND   18   19   14   CCC   122.78\r\ns 43    1.00   BEND   10    5    6   FCC   118.88\r\ns 44    1.00   BEND   11    3    4   FCC   118.34\r\ns 45    1.00   BEND   12    1    6   CCC   122.78\r\ns 46    1.00   BEND   19   14   12   CCC   119.66\r\ns 47    1.00   BEND   25   21   20   FCC   119.29\r\ns 48    1.00   BEND   26   17   18   FCC   118.88\r\ns 49    1.00   BEND   14   12    1   CCC   119.66\r\ns 50    1.00   TORS    7    2    3    4   HCCC  -180.27\r\ns 51    1.00   TORS    8    4    5    6   HCCC  -179.99\r\ns 52    1.00   TORS    9    6    5    4   HCCC  -181.39\r\ns 53    1.00   TORS   22   16   21   20   HCCC  -179.91\r\ns 54    1.00   TORS   23   18   19   14   HCCC     3.52\r\ns 55    1.00   TORS   24   20   21   16   HCCC  -180.27\r\ns 56    1.00   TORS    2    3    4    5   CCCC     0.14\r\ns 57    1.00   TORS   20   21   16   17   CCCC     0.13\r\ns 58    1.00   TORS    6    5    4    3   CCCC    -0.04\r\ns 59    1.00   TORS   16   17   18   19   CCCC     0.03\r\ns 60    1.00   TORS   17   18   19   14   CCCC  -177.94\r\ns 61    1.00   TORS   21   16   17   18   CCCC    -0.04\r\ns 62    1.00   TORS    1    6    5    4   CCCC     0.03\r\ns 63    1.00   TORS   18   19   14   12   CCCC    -4.07\r\ns 64    1.00   TORS   19   14   12    1   CCCC  -225.30\r\ns 65    1.00   TORS   14   12    1    2   CCCC  -181.95\r\ns 66    1.00   OUT    15   12   19   14   OCCC     2.82\r\ns 67    1.00   OUT    13    1   14   12   OCCC     2.82\r\ns 68    1.00   OUT    26   16   18   17   FCCC     0.07\r\ns 69    1.00   OUT    25   16   20   21   FCCC     0.19\r\ns 70    1.00   OUT    11    2    4    3   FCCC     0.20\r\ns 71    1.00   OUT    10    4    6    5   FCCC     0.07\r\ns 72    1.00   OUT    12    2    6    1   CCCC     1.82";
        }

        private void Form4_FormClosing(object sender, FormClosingEventArgs e)
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

        private void btAbout_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This software was made by CCPE Team.\n", "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            if (toolStripTextBox1.Text != "")
            {
                if (toolStripDropDownButton1.Text == leftText)
                {
                    findInLeftFile();
                }
                else if (toolStripDropDownButton1.Text == rightText)
                {
                    findInRightFile();
                }
                else if (toolStripDropDownButton1.Text == "Result")
                {
                    findInResultFile();
                }
            }
            else
            {
                MessageBox.Show("Please write the word to search textbox.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void findInResultFile()
        {
            string pat = toolStripTextBox1.Text;
            string text = rtbResult.Text;
            int baslangic = 0;
            bool flag = true;
            int i = 0;
            while (flag)
            {
                i++;
                baslangic = rtbResult.Find(pat, baslangic, RichTextBoxFinds.None);
                if (baslangic == -1)
                {
                    flag = false;
                    MessageBox.Show("Total " + (i - 1).ToString() + " words found.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                }
                rtbResult.Select(baslangic, pat.Length);
                baslangic++;
                rtbResult.SelectionBackColor = Color.Red;
                renkKontrol3 = false;
            }

        }
        private void findInLeftFile()
        {
            string pat = toolStripTextBox1.Text;
            string text = rtbSourceLeft.Text;
            int baslangic = 0;
            bool flag = true;
            int i = 0;
            while (flag)
            {
                i++;
                baslangic = rtbSourceLeft.Find(pat, baslangic, RichTextBoxFinds.None);
                if (baslangic == -1)
                {
                    flag = false;
                    MessageBox.Show("Total " + (i - 1).ToString() + " words found.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                }
                rtbSourceLeft.Select(baslangic, pat.Length);
                baslangic++;
                rtbSourceLeft.SelectionBackColor = Color.Red;
                renkKontrol = false;
            }

        }
        bool renkKontrol = true, renkKontrol2 = true, renkKontrol3 = true;
        private void rtbMainUp_Click(object sender, EventArgs e)
        {
            if (!renkKontrol)
            {
                rtbSourceLeft.SelectAll();
                rtbSourceLeft.SelectionBackColor = Color.White;
                renkKontrol = true;
                rtbSourceLeft.DeselectAll();
            }
            secim = "kaynak";
        }

        private void rtbMainDown_Click(object sender, EventArgs e)
        {
            if (!renkKontrol2)
            {
                rtbSourceRight.SelectAll();
                rtbSourceRight.SelectionBackColor = Color.White;
                renkKontrol2 = true;
                rtbSourceLeft.DeselectAll();
            }
            secim = "kaynak2";
        }
        private void findInRightFile()
        {
            string pat = toolStripTextBox1.Text;
            string text = rtbSourceRight.Text;
            int baslangic = 0;
            bool flag = true;
            int i = 0;
            while (flag)
            {
                i++;
                baslangic = rtbSourceRight.Find(pat, baslangic, RichTextBoxFinds.None);
                if (baslangic == -1)
                {
                    flag = false;
                    MessageBox.Show("Total " + (i - 1).ToString() + " words found in result file.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                }
                rtbSourceRight.Select(baslangic, pat.Length);
                baslangic++;
                rtbSourceRight.SelectionBackColor = Color.Red;
                renkKontrol2 = false;
            }
        }

        private void rtbResult_Click(object sender, EventArgs e)
        {
            if (!renkKontrol3)
            {
                rtbResult.SelectAll();
                rtbResult.SelectionBackColor = Color.White;
                renkKontrol3 = true;
                rtbResult.DeselectAll();
            }
            secim = "hedef";
        }

        private void btCleanAll_Click(object sender, EventArgs e)
        {
            rtbResult.Clear();
            rtbSourceLeft.Clear();
            rtbSourceRight.Clear();
        }
        private void tsbNewForm_Click(object sender, EventArgs e)
        {
            Form1 form = new Form1();
            form.Show();
            this.Hide();
        }

        private void tsbCut_Click(object sender, EventArgs e)
        {
            if (secim == "kaynak")
                rtbSourceLeft.Cut();
            else if (secim == "kaynak2")
                rtbSourceRight.Cut();
            else if (secim == "hedef")
                rtbResult.Cut();
        }
        string secim;
        private void rtbMainUp_SelectionChanged(object sender, EventArgs e)
        {
            secim = "kaynak";
        }

        private void rtbMainDown_SelectionChanged(object sender, EventArgs e)
        {
            secim = "kaynak2";
        }
        private void rtbResult_SelectionChanged(object sender, EventArgs e)
        {
            secim = "hedef";
        }

        private void tsbCopy_Click(object sender, EventArgs e)
        {
            if (secim == "kaynak")
                rtbSourceLeft.Copy();
            else if (secim == "kaynak2")
                rtbSourceRight.Copy();
            else if (secim == "hedef")
                rtbResult.Copy();
        }

        private void tsbPaste_Click(object sender, EventArgs e)
        {
            if (secim == "kaynak")
                rtbSourceLeft.Paste();
            else if (secim == "kaynak2")
                rtbSourceRight.Paste();
            else if (secim == "hedef")
                rtbResult.Paste();
        }

        private void tümünüToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rtbSourceLeft.SelectAll();
        }

        private void toolStripMenuItem7_Click(object sender, EventArgs e)
        {
            rtbSourceRight.SelectAll();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            rtbResult.SelectAll();
        }

        private void resultToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripDropDownButton1.Text = "Result";
        }

        private void openToolStripButton_Click(object sender, EventArgs e)
        {
            rtbSourceRight.Clear();
            // Dosya seçme iletişim kutusunu göster
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text Files|*.dd2";
            openFileDialog.Title = "Select a file for " + leftText;

            // Kullanıcı bir dosya seçtiyse
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // Dosyayı oku ve içeriğini RichTextBox'a aktar
                    string text = openFileDialog.FileName;
                    rtbSourceRight.Text = AssRight(text);
                    rtbResult.Clear();

                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while uploading the file. " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void toolStripTextBox2_Click(object sender, EventArgs e)
        {
            
        }

        private void sourceLeftToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripDropDownButton1.Text = sourceLeftToolStripMenuItem.Text;
        }

        private void sourceRightToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripDropDownButton1.Text = sourceRightToolStripMenuItem.Text;
        }




    }
}
