namespace DemoSAPB1ServiceLayer
{
    partial class MainView
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            BSearch = new Button();
            BAdd = new Button();
            LCardCode = new Label();
            LCardName = new Label();
            LCardType = new Label();
            TBCardCode = new TextBox();
            TBCardName = new TextBox();
            BAction = new Button();
            LResponse = new RichTextBox();
            CBCardType = new ComboBox();
            dataGridView1 = new DataGridView();
            ITEMCODE = new DataGridViewTextBoxColumn();
            ITEMNAME = new DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // BSearch
            // 
            BSearch.Location = new Point(12, 12);
            BSearch.Name = "BSearch";
            BSearch.Size = new Size(112, 34);
            BSearch.TabIndex = 0;
            BSearch.Text = "Search";
            BSearch.UseVisualStyleBackColor = true;
            BSearch.Click += BSearch_Click;
            // 
            // BAdd
            // 
            BAdd.Location = new Point(130, 12);
            BAdd.Name = "BAdd";
            BAdd.Size = new Size(112, 34);
            BAdd.TabIndex = 1;
            BAdd.Text = "Add";
            BAdd.UseVisualStyleBackColor = true;
            BAdd.Click += BAdd_Click;
            // 
            // LCardCode
            // 
            LCardCode.AutoSize = true;
            LCardCode.Location = new Point(130, 79);
            LCardCode.Name = "LCardCode";
            LCardCode.Size = new Size(91, 25);
            LCardCode.TabIndex = 2;
            LCardCode.Text = "CardCode";
            // 
            // LCardName
            // 
            LCardName.AutoSize = true;
            LCardName.Location = new Point(130, 139);
            LCardName.Name = "LCardName";
            LCardName.Size = new Size(96, 25);
            LCardName.TabIndex = 3;
            LCardName.Text = "CardName";
            // 
            // LCardType
            // 
            LCardType.AutoSize = true;
            LCardType.Location = new Point(130, 204);
            LCardType.Name = "LCardType";
            LCardType.Size = new Size(86, 25);
            LCardType.TabIndex = 4;
            LCardType.Text = "CardType";
            // 
            // TBCardCode
            // 
            TBCardCode.Location = new Point(227, 73);
            TBCardCode.Name = "TBCardCode";
            TBCardCode.Size = new Size(260, 31);
            TBCardCode.TabIndex = 5;
            // 
            // TBCardName
            // 
            TBCardName.Location = new Point(227, 136);
            TBCardName.Name = "TBCardName";
            TBCardName.Size = new Size(260, 31);
            TBCardName.TabIndex = 6;
            // 
            // BAction
            // 
            BAction.Location = new Point(840, 501);
            BAction.Name = "BAction";
            BAction.Size = new Size(112, 34);
            BAction.TabIndex = 8;
            BAction.Text = "Search";
            BAction.UseVisualStyleBackColor = true;
            BAction.Click += BAction_Click;
            // 
            // LResponse
            // 
            LResponse.Location = new Point(493, 12);
            LResponse.Name = "LResponse";
            LResponse.Size = new Size(459, 217);
            LResponse.TabIndex = 9;
            LResponse.Text = "";
            // 
            // CBCardType
            // 
            CBCardType.FormattingEnabled = true;
            CBCardType.Items.AddRange(new object[] { "cCustomer", "cVendor", "cContact" });
            CBCardType.Location = new Point(227, 196);
            CBCardType.Name = "CBCardType";
            CBCardType.Size = new Size(260, 33);
            CBCardType.TabIndex = 10;
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { ITEMCODE, ITEMNAME });
            dataGridView1.Location = new Point(12, 235);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 62;
            dataGridView1.Size = new Size(822, 300);
            dataGridView1.TabIndex = 11;
            // 
            // ITEMCODE
            // 
            ITEMCODE.HeaderText = "ITEM CODE";
            ITEMCODE.MinimumWidth = 8;
            ITEMCODE.Name = "ITEMCODE";
            ITEMCODE.Width = 300;
            // 
            // ITEMNAME
            // 
            ITEMNAME.HeaderText = "ITEM NAME";
            ITEMNAME.MinimumWidth = 8;
            ITEMNAME.Name = "ITEMNAME";
            ITEMNAME.Width = 300;
            // 
            // MainView
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(964, 547);
            Controls.Add(dataGridView1);
            Controls.Add(CBCardType);
            Controls.Add(LResponse);
            Controls.Add(BAction);
            Controls.Add(TBCardName);
            Controls.Add(TBCardCode);
            Controls.Add(LCardType);
            Controls.Add(LCardName);
            Controls.Add(LCardCode);
            Controls.Add(BAdd);
            Controls.Add(BSearch);
            Name = "MainView";
            Text = "DemoB1SLayer";
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button BSearch;
        private Button BAdd;
        private Label LCardCode;
        private Label LCardName;
        private Label LCardType;
        private TextBox TBCardCode;
        private TextBox TBCardName;
        private Button BAction;
        private RichTextBox LResponse;
        private ComboBox CBCardType;
        private DataGridView dataGridView1;
        private DataGridViewTextBoxColumn ITEMCODE;
        private DataGridViewTextBoxColumn ITEMNAME;
    }
}
