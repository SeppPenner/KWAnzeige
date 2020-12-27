namespace KWAnzeige
{
    partial class Main
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.ButtonMinimize = new System.Windows.Forms.Button();
            this.LabelCW = new System.Windows.Forms.Label();
            this.textBoxCW = new System.Windows.Forms.TextBox();
            this.comboBoxLanguage = new System.Windows.Forms.ComboBox();
            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanelMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonMinimize
            // 
            this.ButtonMinimize.Location = new System.Drawing.Point(190, 38);
            this.ButtonMinimize.Name = "ButtonMinimize";
            this.ButtonMinimize.Size = new System.Drawing.Size(166, 23);
            this.ButtonMinimize.TabIndex = 0;
            this.ButtonMinimize.Text = "Minimize";
            this.ButtonMinimize.UseVisualStyleBackColor = true;
            this.ButtonMinimize.Click += new System.EventHandler(this.ButtonMinimize_Click);
            // 
            // labelCW
            // 
            this.LabelCW.AutoSize = true;
            this.LabelCW.Location = new System.Drawing.Point(3, 0);
            this.LabelCW.Name = "LabelCW";
            this.LabelCW.Size = new System.Drawing.Size(68, 13);
            this.LabelCW.TabIndex = 1;
            this.LabelCW.Text = "Today\'s CW:";
            // 
            // textBoxCW
            // 
            this.textBoxCW.Location = new System.Drawing.Point(3, 38);
            this.textBoxCW.Name = "textBoxCW";
            this.textBoxCW.ReadOnly = true;
            this.textBoxCW.Size = new System.Drawing.Size(165, 20);
            this.textBoxCW.TabIndex = 4;
            // 
            // comboBoxLanguage
            // 
            this.comboBoxLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxLanguage.FormattingEnabled = true;
            this.comboBoxLanguage.Location = new System.Drawing.Point(190, 3);
            this.comboBoxLanguage.Name = "comboBoxLanguage";
            this.comboBoxLanguage.Size = new System.Drawing.Size(166, 21);
            this.comboBoxLanguage.TabIndex = 5;
            this.comboBoxLanguage.SelectedIndexChanged += new System.EventHandler(this.ComboBoxLanguageSelectedIndexChanged);
            // 
            // tableLayoutPanelMain
            // 
            this.tableLayoutPanelMain.ColumnCount = 2;
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelMain.Controls.Add(this.LabelCW, 0, 0);
            this.tableLayoutPanelMain.Controls.Add(this.ButtonMinimize, 1, 1);
            this.tableLayoutPanelMain.Controls.Add(this.comboBoxLanguage, 1, 0);
            this.tableLayoutPanelMain.Controls.Add(this.textBoxCW, 0, 1);
            this.tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelMain.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.tableLayoutPanelMain.RowCount = 2;
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.Size = new System.Drawing.Size(375, 69);
            this.tableLayoutPanelMain.TabIndex = 6;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(375, 69);
            this.Controls.Add(this.tableLayoutPanelMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Main";
            this.Text = "Today\'s CW";
            this.Load += new System.EventHandler(this.MainLoad);
            this.tableLayoutPanelMain.ResumeLayout(false);
            this.tableLayoutPanelMain.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button ButtonMinimize;
        private System.Windows.Forms.Label LabelCW;
        private System.Windows.Forms.TextBox textBoxCW;
        private System.Windows.Forms.ComboBox comboBoxLanguage;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
    }
}

