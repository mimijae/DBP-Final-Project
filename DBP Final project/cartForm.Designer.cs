
namespace DBP_Final_project
{
    partial class cartForm
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.purchaseBtn = new FontAwesome.Sharp.IconButton();
            this.fPanelShowCart = new System.Windows.Forms.FlowLayoutPanel();
            this.dataSet11 = new DBP_Final_project.DataSet1();
            this.itemTableAdapter1 = new DBP_Final_project.DataSet1TableAdapters.ITEMTableAdapter();
            this.cartTableAdapter1 = new DBP_Final_project.DataSet1TableAdapters.CARTTableAdapter();
            this.tableAdapterManager1 = new DBP_Final_project.DataSet1TableAdapters.TableAdapterManager();
            this.purchaseTableAdapter1 = new DBP_Final_project.DataSet1TableAdapters.PURCHASETableAdapter();
            this.purchaseditemTableAdapter1 = new DBP_Final_project.DataSet1TableAdapters.PURCHASEDITEMTableAdapter();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet11)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 74.70167F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.29833F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 167F));
            this.tableLayoutPanel1.Controls.Add(this.purchaseBtn, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 587);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1135, 100);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // purchaseBtn
            // 
            this.purchaseBtn.BackColor = System.Drawing.Color.YellowGreen;
            this.purchaseBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.purchaseBtn.Font = new System.Drawing.Font("배달의민족 도현", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.purchaseBtn.IconChar = FontAwesome.Sharp.IconChar.None;
            this.purchaseBtn.IconColor = System.Drawing.Color.Black;
            this.purchaseBtn.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.purchaseBtn.Location = new System.Drawing.Point(726, 3);
            this.purchaseBtn.Name = "purchaseBtn";
            this.purchaseBtn.Size = new System.Drawing.Size(238, 94);
            this.purchaseBtn.TabIndex = 0;
            this.purchaseBtn.Text = "구매하기";
            this.purchaseBtn.UseVisualStyleBackColor = false;
            this.purchaseBtn.Click += new System.EventHandler(this.purchaseBtn_Click);
            // 
            // fPanelShowCart
            // 
            this.fPanelShowCart.AutoScroll = true;
            this.fPanelShowCart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fPanelShowCart.Location = new System.Drawing.Point(0, 0);
            this.fPanelShowCart.Name = "fPanelShowCart";
            this.fPanelShowCart.Padding = new System.Windows.Forms.Padding(70, 25, 70, 25);
            this.fPanelShowCart.Size = new System.Drawing.Size(1135, 587);
            this.fPanelShowCart.TabIndex = 1;
            // 
            // dataSet11
            // 
            this.dataSet11.DataSetName = "DataSet1";
            this.dataSet11.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // itemTableAdapter1
            // 
            this.itemTableAdapter1.ClearBeforeFill = true;
            // 
            // cartTableAdapter1
            // 
            this.cartTableAdapter1.ClearBeforeFill = true;
            // 
            // tableAdapterManager1
            // 
            this.tableAdapterManager1.ADMINTableAdapter = null;
            this.tableAdapterManager1.BackupDataSetBeforeUpdate = false;
            this.tableAdapterManager1.CARTTableAdapter = this.cartTableAdapter1;
            this.tableAdapterManager1.CUSTOMERTableAdapter = null;
            this.tableAdapterManager1.ITEMTableAdapter = this.itemTableAdapter1;
            this.tableAdapterManager1.PURCHASEDITEMTableAdapter = null;
            this.tableAdapterManager1.PURCHASETableAdapter = null;
            this.tableAdapterManager1.REFUNDTableAdapter = null;
            this.tableAdapterManager1.REVIEWTableAdapter = null;
            this.tableAdapterManager1.SELLERTableAdapter = null;
            this.tableAdapterManager1.UpdateOrder = DBP_Final_project.DataSet1TableAdapters.TableAdapterManager.UpdateOrderOption.InsertUpdateDelete;
            // 
            // purchaseTableAdapter1
            // 
            this.purchaseTableAdapter1.ClearBeforeFill = true;
            // 
            // purchaseditemTableAdapter1
            // 
            this.purchaseditemTableAdapter1.ClearBeforeFill = true;
            // 
            // cartForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1135, 687);
            this.Controls.Add(this.fPanelShowCart);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "cartForm";
            this.Text = "cartForm";
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataSet11)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private FontAwesome.Sharp.IconButton purchaseBtn;
        private System.Windows.Forms.FlowLayoutPanel fPanelShowCart;
        private DataSet1 dataSet11;
        private DataSet1TableAdapters.ITEMTableAdapter itemTableAdapter1;
        private DataSet1TableAdapters.CARTTableAdapter cartTableAdapter1;
        private DataSet1TableAdapters.TableAdapterManager tableAdapterManager1;
        private DataSet1TableAdapters.PURCHASETableAdapter purchaseTableAdapter1;
        private DataSet1TableAdapters.PURCHASEDITEMTableAdapter purchaseditemTableAdapter1;
    }
}