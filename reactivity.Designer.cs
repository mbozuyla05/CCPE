namespace CCPE
{
    partial class reactivity
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(reactivity));
            this.lResult = new System.Windows.Forms.Label();
            this.tbLumo = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbHomo = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbEv = new System.Windows.Forms.RadioButton();
            this.rbAu = new System.Windows.Forms.RadioButton();
            this.rtbSolution = new System.Windows.Forms.RichTextBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            this.SuspendLayout();
            // 
            // lResult
            // 
            this.lResult.AutoSize = true;
            this.lResult.Font = new System.Drawing.Font("Cambria", 10.8F, System.Drawing.FontStyle.Bold);
            this.lResult.Location = new System.Drawing.Point(203, 15);
            this.lResult.Name = "lResult";
            this.lResult.Size = new System.Drawing.Size(127, 21);
            this.lResult.TabIndex = 5;
            this.lResult.Text = "LUMO Energy:";
            // 
            // tbLumo
            // 
            this.tbLumo.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.tbLumo.Font = new System.Drawing.Font("Cambria", 10.8F, System.Drawing.FontStyle.Bold);
            this.tbLumo.Location = new System.Drawing.Point(343, 12);
            this.tbLumo.Name = "tbLumo";
            this.tbLumo.Size = new System.Drawing.Size(111, 29);
            this.tbLumo.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Cambria", 10.8F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(203, 69);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(131, 21);
            this.label1.TabIndex = 5;
            this.label1.Text = "HOMO Energy:";
            // 
            // tbHomo
            // 
            this.tbHomo.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.tbHomo.Font = new System.Drawing.Font("Cambria", 10.8F, System.Drawing.FontStyle.Bold);
            this.tbHomo.Location = new System.Drawing.Point(343, 65);
            this.tbHomo.Name = "tbHomo";
            this.tbHomo.Size = new System.Drawing.Size(111, 29);
            this.tbHomo.TabIndex = 6;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbEv);
            this.groupBox1.Controls.Add(this.rbAu);
            this.groupBox1.Font = new System.Drawing.Font("Cambria", 10.8F, System.Drawing.FontStyle.Bold);
            this.groupBox1.Location = new System.Drawing.Point(478, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(125, 82);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            // 
            // rbEv
            // 
            this.rbEv.AutoSize = true;
            this.rbEv.Location = new System.Drawing.Point(11, 55);
            this.rbEv.Name = "rbEv";
            this.rbEv.Size = new System.Drawing.Size(52, 25);
            this.rbEv.TabIndex = 0;
            this.rbEv.Text = "eV";
            this.rbEv.UseVisualStyleBackColor = true;
            // 
            // rbAu
            // 
            this.rbAu.AutoSize = true;
            this.rbAu.Checked = true;
            this.rbAu.Location = new System.Drawing.Point(11, 22);
            this.rbAu.Name = "rbAu";
            this.rbAu.Size = new System.Drawing.Size(56, 25);
            this.rbAu.TabIndex = 0;
            this.rbAu.TabStop = true;
            this.rbAu.Text = "a.u";
            this.rbAu.UseVisualStyleBackColor = true;
            // 
            // rtbSolution
            // 
            this.rtbSolution.Location = new System.Drawing.Point(12, 114);
            this.rtbSolution.Name = "rtbSolution";
            this.rtbSolution.Size = new System.Drawing.Size(797, 479);
            this.rtbSolution.TabIndex = 9;
            this.rtbSolution.Text = "";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBox1.Image = global::CCPE.Properties.Resources.excel8;
            this.pictureBox1.Location = new System.Drawing.Point(116, 27);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(65, 62);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 11;
            this.pictureBox1.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox1, "Save");
            this.pictureBox1.Click += new System.EventHandler(this.btSave_Click);
            this.pictureBox1.MouseEnter += new System.EventHandler(this.pictureBox1_MouseEnter);
            this.pictureBox1.MouseLeave += new System.EventHandler(this.pictureBox1_MouseLeave);
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBox2.Image = global::CCPE.Properties.Resources._2rightarrow;
            this.pictureBox2.Location = new System.Drawing.Point(628, 27);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(68, 62);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 11;
            this.pictureBox2.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox2, "Generate");
            this.pictureBox2.Click += new System.EventHandler(this.btGenerate_Click);
            this.pictureBox2.MouseEnter += new System.EventHandler(this.pictureBox1_MouseEnter);
            this.pictureBox2.MouseLeave += new System.EventHandler(this.pictureBox1_MouseLeave);
            // 
            // pictureBox3
            // 
            this.pictureBox3.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox3.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBox3.Image = global::CCPE.Properties.Resources.exit;
            this.pictureBox3.Location = new System.Drawing.Point(729, 27);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(65, 62);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox3.TabIndex = 11;
            this.pictureBox3.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox3, "Exit");
            this.pictureBox3.Click += new System.EventHandler(this.btExit_Click);
            this.pictureBox3.MouseEnter += new System.EventHandler(this.pictureBox1_MouseEnter);
            this.pictureBox3.MouseLeave += new System.EventHandler(this.pictureBox1_MouseLeave);
            // 
            // pictureBox4
            // 
            this.pictureBox4.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox4.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBox4.Image = global::CCPE.Properties.Resources.Back;
            this.pictureBox4.Location = new System.Drawing.Point(16, 27);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(69, 62);
            this.pictureBox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox4.TabIndex = 11;
            this.pictureBox4.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox4, "Home");
            this.pictureBox4.Click += new System.EventHandler(this.btHome_Click);
            this.pictureBox4.MouseEnter += new System.EventHandler(this.pictureBox1_MouseEnter);
            this.pictureBox4.MouseLeave += new System.EventHandler(this.pictureBox1_MouseLeave);
            // 
            // reactivity
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(818, 607);
            this.Controls.Add(this.pictureBox4);
            this.Controls.Add(this.pictureBox3);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.rtbSolution);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.tbHomo);
            this.Controls.Add(this.tbLumo);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lResult);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "reactivity";
            this.Text = "Reactivity Descriptors";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.reactivity_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lResult;
        private System.Windows.Forms.TextBox tbLumo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbHomo;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbEv;
        private System.Windows.Forms.RadioButton rbAu;
        private System.Windows.Forms.RichTextBox rtbSolution;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.PictureBox pictureBox4;
    }
}