using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections;
using System.IO;
using OfficeOpenXml;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace alp_app
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string hedef = "Calculated vibrational frequency with PED assignments of the title compound\n";
            hedef += "Intensity\tUnscaled\tAssignments\n";

            int hatalısatır = 0;
            try
            {
                SortedList kaynak_vdf = new SortedList();
                SortedList kaynak_dd2 = new SortedList();

                kaynak_vdf = Kumele(rtbKaynak.Text);
                kaynak_dd2 = Kumele(rtbKaynak2.Text);

                for (int i = 0; i < kaynak_vdf.Count; i++)
                {
                    hatalısatır = i + 1;
                    string[] kaynakSatir = ((string[])kaynak_vdf.GetByIndex(i));
                    hedef += kaynakSatir[0] + "\t" + kaynakSatir[1] + "\t";

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
                rtbHedef.Text = hedef;

            }
            catch (Exception ex)
            {
                MessageBox.Show(".vdf dosyasında " + hatalısatır.ToString() + " numaralı satırı kontrol ediniz.\n\nMesaj:" + ex.Message, "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string BagHesapla(string[] referansSatir)
        {
            throw new NotImplementedException();
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

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("BU YAZILIM MEHMET BOZUYLA TARAFINDAN GERÇEKLEŞTİRİLMİŞTİR.\n\n\nbozuyla@gmail.com\nmbozuyla.pau.edu.tr", "HAKKINDA", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void tümünüToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rtbKaynak.SelectAll();
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            rtbKaynak.Cut();
        }

        private void kopyalaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rtbKaynak.Copy();
        }

        private void yapıştırToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rtbKaynak.Paste();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            rtbHedef.SelectAll();
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            rtbHedef.Cut();
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            rtbHedef.Copy();
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            rtbHedef.Paste();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            rtbKaynak.Text = "";
        }

        private void silToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rtbHedef.Text = "";
        }

        private void kaynakDosyadaBulToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string pat = rtbHedef.SelectedText;
            string text = rtbKaynak.Text;
            int baslangic = 0;
            bool flag = true;
            int i = 0;
            while (flag)
            {
                i++;
                baslangic = rtbKaynak.Find(pat, baslangic, RichTextBoxFinds.None);
                if (baslangic == -1)
                {
                    flag = false;
                    MessageBox.Show("Toplam " + (i - 1).ToString() + " adet kelime bulundu...", "BİLGİ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                }
                rtbKaynak.Select(baslangic, pat.Length);
                baslangic++;
                rtbKaynak.SelectionBackColor = Color.Red;
                renkKontrol = false;
            }

        }
        bool renkKontrol = true, renkKontrol2 = true;
        private void rtbKaynak_Click(object sender, EventArgs e)
        {
            if (!renkKontrol)
            {
                rtbKaynak.SelectAll();
                rtbKaynak.SelectionBackColor = Color.White;
                renkKontrol = true;
                rtbKaynak.DeselectAll();
            }
            secim = "kaynak";
        }

        private void rtbKaynak2_Click(object sender, EventArgs e)
        {
            if (!renkKontrol)
            {
                rtbKaynak2.SelectAll();
                rtbKaynak2.SelectionBackColor = Color.White;
                renkKontrol = true;
                rtbKaynak.DeselectAll();
            }
            secim = "kaynak2";
        }
        private void hedefDosyadaBulToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string pat = rtbKaynak.SelectedText;
            string text = rtbKaynak.Text;
            int baslangic = 0;
            bool flag = true;
            int i = 0;
            while (flag)
            {
                i++;
                baslangic = rtbHedef.Find(pat, baslangic, RichTextBoxFinds.None);
                if (baslangic == -1)
                {
                    flag = false;
                    MessageBox.Show("Toplam " + (i - 1).ToString() + " adet kelime bulundu...", "BİLGİ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                }
                rtbHedef.Select(baslangic, pat.Length);
                baslangic++;
                rtbHedef.SelectionBackColor = Color.Red;
                renkKontrol2 = false;
            }
        }

        private void rtbHedef_Click(object sender, EventArgs e)
        {
            if (!renkKontrol2)
            {
                rtbHedef.SelectAll();
                rtbHedef.SelectionBackColor = Color.White;
                renkKontrol2 = true;
                rtbHedef.DeselectAll();
            }
            secim = "hedef";
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            rtbHedef.Clear();
            rtbKaynak.Clear();
            rtbKaynak2.Clear();
        }

        private void newToolStripButton_Click(object sender, EventArgs e)
        {
            Form1 form = new Form1();
            form.Show();
        }

        private void cutToolStripButton_Click(object sender, EventArgs e)
        {
            if (secim == "kaynak")
                rtbKaynak.Cut();
            else if (secim == "hedef")
                rtbHedef.Cut();
        }
        string secim;
        private void rtbKaynak_SelectionChanged(object sender, EventArgs e)
        {
            secim = "kaynak";
        }

        private void rtbKaynak2_SelectionChanged(object sender, EventArgs e)
        {
            secim = "kaynak2";
        }
        private void rtbHedef_SelectionChanged(object sender, EventArgs e)
        {
            secim = "hedef";
        }

        private void copyToolStripButton_Click(object sender, EventArgs e)
        {
            if (secim == "kaynak")
                rtbKaynak.Copy();
            else if (secim == "hedef")
                rtbHedef.Copy();
        }

        private void pasteToolStripButton_Click(object sender, EventArgs e)
        {
            if (secim == "kaynak")
                rtbKaynak.Paste();
            else if (secim == "hedef")
                rtbHedef.Paste();
        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            rtbHedef.SaveFile("belge.txt");
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            rtbKaynak.Clear();
            // Dosya seçme iletişim kutusunu göster
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text Files|*.txt";
            openFileDialog.Title = "VDF Alanı İçin Bir Dosya Seçiniz";

            // Kullanıcı bir dosya seçtiyse
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // Dosyayı oku ve içeriğini RichTextBox'a aktar
                    string text = File.ReadAllText(openFileDialog.FileName);
                    rtbKaynak.Text = text;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Dosya yüklenirken hata oluştu: " + ex.Message);
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
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
                string richTextBoxContent = rtbHedef.Text;

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
                saveFileDialog1.Title = "Excel'e Kaydet";
                saveFileDialog1.ShowDialog();

                // Kullanıcı dosya seçtiyse Excel dosyasını kaydet
                if (saveFileDialog1.FileName != "")
                {
                    FileInfo excelFile = new FileInfo(saveFileDialog1.FileName);
                    excelPackage.SaveAs(excelFile);
                    MessageBox.Show("Excel dosyası başarıyla kaydedildi.");
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string hedef = " ";

            int hatalısatır = 0;
            try
            {
                SortedList kaynak_ust = new SortedList();
                SortedList kaynak_alt = new SortedList();

                kaynak_ust = Kumele(rtbKaynak.Text);
                kaynak_alt = Kumele(rtbKaynak2.Text);

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

                    if(bag.Length>flag && flag==0)
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

                    if (double.TryParse(kaynakSatir[3].Replace(".",","), out doubleDeger))
                    {
                        doubleDeger = Math.Round(doubleDeger, 2);
                    }
                    else
                    {
                        MessageBox.Show("Üst dosyada " + hatalısatır.ToString() + " numaralı satırı kontrol ediniz.\n\nMesaj: Ortalama hatalı formatta", "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }


                    hedef += bagN + "\t" + doubleDeger.ToString() + "\n";
                }
                hedef += "\n\n\n\n";
                rtbHedef.Text = hedef;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Üst dosyada " + hatalısatır.ToString() + " numaralı satırı kontrol ediniz.\n\nMesaj:" + ex.Message, "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //rtbKaynak.Text = "! R1    R(1,2)                  1.4042         -DE/DX =    0.0                 !\r\n ! R2    R(1,6)                  1.4035         -DE/DX =    0.0                 !\r\n ! R3    R(1,12)                 1.4922         -DE/DX =    0.0                 !\r\n ! R4    R(2,3)                  1.3851         -DE/DX =    0.0                 !\r\n ! R5    R(2,7)                  1.0835         -DE/DX =    0.0                 !\r\n ! R6    R(3,4)                  1.3928         -DE/DX =    0.0                 !\r\n ! R7    R(3,11)                 1.3449         -DE/DX =    0.0                 !\r\n ! R8    R(4,5)                  1.39           -DE/DX =    0.0                 !\r\n ! R9    R(4,8)                  1.0837         -DE/DX =    0.0                 !\r\n ! R10   R(5,6)                  1.3893         -DE/DX =    0.0                 !\r\n ! R11   R(5,10)                 1.345          -DE/DX =    0.0                 !\r\n ! R12   R(6,9)                  1.0817         -DE/DX =    0.0                 !\r\n ! R13   R(9,15)                 2.3981         -DE/DX =    0.0                 !\r\n ! R14   R(12,13)                1.2219         -DE/DX =    0.0                 !\r\n ! R15   R(12,14)                1.5413         -DE/DX =    0.0                 !\r\n ! R16   R(13,23)                2.3985         -DE/DX =    0.0                 !\r\n ! R17   R(14,15)                1.2219         -DE/DX =    0.0                 !\r\n ! R18   R(14,19)                1.4922         -DE/DX =    0.0                 !\r\n ! R19   R(16,17)                1.39           -DE/DX =    0.0                 !\r\n ! R20   R(16,21)                1.3928         -DE/DX =    0.0                 !\r\n ! R21   R(16,22)                1.0837         -DE/DX =    0.0                 !\r\n ! R22   R(17,18)                1.3893         -DE/DX =    0.0                 !\r\n ! R23   R(17,26)                1.345          -DE/DX =    0.0                 !\r\n ! R24   R(18,19)                1.4035         -DE/DX =    0.0                 !\r\n ! R25   R(18,23)                1.0817         -DE/DX =    0.0                 !\r\n ! R26   R(19,20)                1.4042         -DE/DX =    0.0                 !\r\n ! R27   R(20,21)                1.3851         -DE/DX =    0.0                 !\r\n ! R28   R(20,24)                1.0835         -DE/DX =    0.0                 !\r\n ! R29   R(21,25)                1.3449         -DE/DX =    0.0                 !\r\n ! A1    A(2,1,6)              120.4219         -DE/DX =    0.0                 !\r\n ! A2    A(2,1,12)             116.7644         -DE/DX =    0.0                 !\r\n ! A3    A(6,1,12)             122.7803         -DE/DX =    0.0                 !\r\n ! A4    A(1,2,3)              118.7696         -DE/DX =    0.0                 !\r\n ! A5    A(1,2,7)              120.2601         -DE/DX =    0.0                 !\r\n ! A6    A(3,2,7)              120.9702         -DE/DX =    0.0                 !\r\n ! A7    A(2,3,4)              122.3695         -DE/DX =    0.0                 !\r\n ! A8    A(2,3,11)             119.2907         -DE/DX =    0.0                 !\r\n ! A9    A(4,3,11)             118.3395         -DE/DX =    0.0                 !\r\n ! A10   A(3,4,5)              117.4075         -DE/DX =    0.0                 !\r\n ! A11   A(3,4,8)              121.302          -DE/DX =    0.0                 !\r\n ! A12   A(5,4,8)              121.2905         -DE/DX =    0.0                 !\r\n ! A13   A(4,5,6)              122.6512         -DE/DX =    0.0                 !\r\n ! A14   A(4,5,10)             118.4652         -DE/DX =    0.0                 !\r\n ! A15   A(6,5,10)             118.8835         -DE/DX =    0.0                 !\r\n ! A16   A(1,6,5)              118.38           -DE/DX =    0.0                 !\r\n ! A17   A(1,6,9)              122.1215         -DE/DX =    0.0                 !\r\n ! A18   A(5,6,9)              119.4826         -DE/DX =    0.0                 !\r\n ! A19   A(1,12,13)            122.2044         -DE/DX =    0.0                 !\r\n ! A20   A(1,12,14)            119.659          -DE/DX =    0.0                 !\r\n ! A21   A(13,12,14)           118.0556         -DE/DX =    0.0                 !\r\n ! A22   A(12,14,15)           118.0569         -DE/DX =    0.0                 !\r\n ! A23   A(12,14,19)           119.6586         -DE/DX =    0.0                 !\r\n ! A24   A(15,14,19)           122.2037         -DE/DX =    0.0                 !\r\n ! A25   A(17,16,21)           117.4069         -DE/DX =    0.0                 !\r\n ! A26   A(17,16,22)           121.2907         -DE/DX =    0.0                 !\r\n ! A27   A(21,16,22)           121.3023         -DE/DX =    0.0                 !\r\n ! A28   A(16,17,18)           122.6518         -DE/DX =    0.0                 !\r\n ! A29   A(16,17,26)           118.4654         -DE/DX =    0.0                 !\r\n ! A30   A(18,17,26)           118.8827         -DE/DX =    0.0                 !\r\n ! A31   A(17,18,19)           118.3801         -DE/DX =    0.0                 !\r\n ! A32   A(17,18,23)           119.479          -DE/DX =    0.0                 !\r\n ! A33   A(19,18,23)           122.125          -DE/DX =    0.0                 !\r\n ! A34   A(14,19,18)           122.7823         -DE/DX =    0.0                 !\r\n ! A35   A(14,19,20)           116.7631         -DE/DX =    0.0                 !\r\n ! A36   A(18,19,20)           120.421          -DE/DX =    0.0                 !\r\n ! A37   A(19,20,21)           118.7705         -DE/DX =    0.0                 !\r\n ! A38   A(19,20,24)           120.2597         -DE/DX =    0.0                 !\r\n ! A39   A(21,20,24)           120.9698         -DE/DX =    0.0                 !\r\n ! A40   A(16,21,20)           122.3693         -DE/DX =    0.0                 !\r\n ! A41   A(16,21,25)           118.3395         -DE/DX =    0.0                 !\r\n ! A42   A(20,21,25)           119.2908         -DE/DX =    0.0                 !\r\n ! D1    D(6,1,2,3)             -0.2062         -DE/DX =    0.0                 !\r\n ! D2    D(6,1,2,7)            179.7443         -DE/DX =    0.0                 !\r\n ! D3    D(12,1,2,3)          -178.1628         -DE/DX =    0.0                 !\r\n ! D4    D(12,1,2,7)             1.7878         -DE/DX =    0.0                 !\r\n ! D5    D(2,1,6,5)              0.114          -DE/DX =    0.0                 !\r\n ! D6    D(2,1,6,9)            178.6533         -DE/DX =    0.0                 !\r\n ! D7    D(12,1,6,5)           177.9438         -DE/DX =    0.0                 !\r\n ! D8    D(12,1,6,9)            -3.5169         -DE/DX =    0.0                 !\r\n ! D9    D(2,1,12,13)           -1.3857         -DE/DX =    0.0                 !\r\n ! D10   D(2,1,12,14)         -178.0478         -DE/DX =    0.0                 !\r\n ! D11   D(6,1,12,13)         -179.2898         -DE/DX =    0.0                 !\r\n ! D12   D(6,1,12,14)            4.0481         -DE/DX =    0.0                 !\r\n ! D13   D(1,2,3,4)              0.2207         -DE/DX =    0.0                 !\r\n ! D14   D(1,2,3,11)          -180.003          -DE/DX =    0.0                 !\r\n ! D15   D(7,2,3,4)           -179.7294         -DE/DX =    0.0                 !\r\n ! D16   D(7,2,3,11)             0.0469         -DE/DX =    0.0                 !\r\n ! D17   D(2,3,4,5)             -0.1373         -DE/DX =    0.0                 !\r\n ! D18   D(2,3,4,8)            179.9138         -DE/DX =    0.0                 !\r\n ! D19   D(11,3,4,5)          -179.9157         -DE/DX =    0.0                 !\r\n ! D20   D(11,3,4,8)             0.1355         -DE/DX =    0.0                 !\r\n ! D21   D(3,4,5,6)              0.0398         -DE/DX =    0.0                 !\r\n ! D22   D(3,4,5,10)          -179.8779         -DE/DX =    0.0                 !\r\n ! D23   D(8,4,5,6)            179.9887         -DE/DX =    0.0                 !\r\n ! D24   D(8,4,5,10)             0.071          -DE/DX =    0.0                 !\r\n ! D25   D(4,5,6,1)             -0.0305         -DE/DX =    0.0                 !\r\n ! D26   D(4,5,6,9)           -178.6094         -DE/DX =    0.0                 !\r\n ! D27   D(10,5,6,1)           179.8869         -DE/DX =    0.0                 !\r\n ! D28   D(10,5,6,9)             1.308          -DE/DX =    0.0                 !\r\n ! D29   D(1,12,14,15)          48.502          -DE/DX =    0.0                 !\r\n ! D30   D(1,12,14,19)        -134.6955         -DE/DX =    0.0                 !\r\n ! D31   D(13,12,14,15)       -128.2977         -DE/DX =    0.0                 !\r\n ! D32   D(13,12,14,19)         48.5049         -DE/DX =    0.0                 !\r\n ! D33   D(12,14,19,18)          4.0686         -DE/DX =    0.0                 !\r\n ! D34   D(12,14,19,20)       -178.0353         -DE/DX =    0.0                 !\r\n ! D35   D(15,14,19,18)       -179.2663         -DE/DX =    0.0                 !\r\n ! D36   D(15,14,19,20)         -1.3702         -DE/DX =    0.0                 !\r\n ! D37   D(21,16,17,18)          0.0361         -DE/DX =    0.0                 !\r\n ! D38   D(21,16,17,26)       -179.8798         -DE/DX =    0.0                 !\r\n ! D39   D(22,16,17,18)        179.9875         -DE/DX =    0.0                 !\r\n ! D40   D(22,16,17,26)          0.0716         -DE/DX =    0.0                 !\r\n ! D41   D(17,16,21,20)         -0.135          -DE/DX =    0.0                 !\r\n ! D42   D(17,16,21,25)       -179.9137         -DE/DX =    0.0                 !\r\n ! D43   D(22,16,21,20)        179.9136         -DE/DX =    0.0                 !\r\n ! D44   D(22,16,21,25)          0.1349         -DE/DX =    0.0                 !\r\n ! D45   D(16,17,18,19)         -0.0305         -DE/DX =    0.0                 !\r\n ! D46   D(16,17,18,23)       -178.6113         -DE/DX =    0.0                 !\r\n ! D47   D(26,17,18,19)        179.8851         -DE/DX =    0.0                 !\r\n ! D48   D(26,17,18,23)          1.3044         -DE/DX =    0.0                 !\r\n ! D49   D(17,18,19,14)        177.9407         -DE/DX =    0.0                 !\r\n ! D50   D(17,18,19,20)          0.1192         -DE/DX =    0.0                 !\r\n ! D51   D(23,18,19,14)         -3.5182         -DE/DX =    0.0                 !\r\n ! D52   D(23,18,19,20)        178.6603         -DE/DX =    0.0                 !\r\n ! D53   D(14,19,20,21)       -178.1615         -DE/DX =    0.0                 !\r\n ! D54   D(14,19,20,24)          1.7903         -DE/DX =    0.0                 !\r\n ! D55   D(18,19,20,21)         -0.2127         -DE/DX =    0.0                 !\r\n ! D56   D(18,19,20,24)        179.7391         -DE/DX =    0.0                 !\r\n ! D57   D(19,20,21,16)          0.2234         -DE/DX =    0.0                 !\r\n ! D58   D(19,20,21,25)       -179.9999         -DE/DX =    0.0                 !\r\n ! D59   D(24,20,21,16)       -179.7281         -DE/DX =    0.0                 !\r\n ! D60   D(24,20,21,25)          0.0486         -DE/DX =    0.0                 !";
            //rtbKaynak2.Text = "     1  C    5.010106   0.393592   0.005144  -0.025035   0.003226   0.441140\r\n     2  C    0.393592   5.320777   0.431396  -0.084021  -0.035951  -0.091279\r\n     3  C    0.005144   0.431396   4.537805   0.404493   0.021349  -0.033555\r\n     4  C   -0.025035  -0.084021   0.404493   5.375541   0.401320  -0.085975\r\n     5  C    0.003226  -0.035951   0.021349   0.401320   4.563169   0.409556\r\n     6  C    0.441140  -0.091279  -0.033555  -0.085975   0.409556   5.379588\r\n     7  H   -0.041169   0.346844  -0.035515   0.006493  -0.000102   0.006233\r\n     8  H   -0.000415   0.006874  -0.036251   0.340921  -0.034850   0.006928\r\n     9  H   -0.027515   0.005867  -0.000119   0.006347  -0.035666   0.328919\r\n    10  F    0.003059   0.000039   0.002492  -0.040794   0.306602  -0.041327\r\n    11  F    0.003000  -0.040616   0.302705  -0.037798   0.002530   0.000059\r\n    12  C    0.306263  -0.025125   0.003721   0.000325   0.002916  -0.023949\r\n    13  O   -0.079032   0.001727   0.000846   0.000002  -0.000038   0.003463\r\n    14  C   -0.063151   0.009025  -0.000018  -0.000020  -0.000154  -0.010367\r\n    15  O   -0.010332  -0.000336  -0.000006  -0.000017   0.002225  -0.026219\r\n    16  C    0.000000   0.000000   0.000000   0.000000   0.000000   0.000000\r\n    17  C    0.000010   0.000000   0.000000   0.000000   0.000000   0.000000\r\n    18  C    0.000181   0.000001   0.000000   0.000000   0.000000  -0.000018\r\n    19  C    0.004252  -0.000116   0.000000   0.000000   0.000010   0.000181\r\n    20  C   -0.000116   0.000001   0.000000   0.000000   0.000000   0.000001\r\n    21  C    0.000000   0.000000   0.000000   0.000000   0.000000   0.000000\r\n    22  H    0.000000   0.000000   0.000000   0.000000   0.000000   0.000000\r\n    23  H    0.000544   0.000010   0.000000   0.000000   0.000000  -0.000015\r\n    24  H    0.000007   0.000000   0.000000   0.000000   0.000000   0.000000\r\n    25  F    0.000000   0.000000   0.000000   0.000000   0.000000   0.000000\r\n    26  F    0.000000   0.000000   0.000000   0.000000   0.000000   0.000000";
        }

        private void openToolStripButton_Click(object sender, EventArgs e)
        {
            rtbKaynak2.Clear();
            // Dosya seçme iletişim kutusunu göster
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text Files|*.txt";
            openFileDialog.Title = "DD2 Alanı İçin Bir Dosya Seçiniz";

            // Kullanıcı bir dosya seçtiyse
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // Dosyayı oku ve içeriğini RichTextBox'a aktar
                    string text = File.ReadAllText(openFileDialog.FileName);
                    rtbKaynak2.Text = text;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Dosya yüklenirken hata oluştu: " + ex.Message);
                }
            }
        }
    }
}
