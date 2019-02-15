namespace PalletBuilder
{
    partial class PalletBldForm
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
            this.txtTotalTags = new System.Windows.Forms.TextBox();
            this.dataSetTagEvents = new System.Data.DataSet();
            this.dtThreshEvents = new System.Data.DataTable();
            this.EpcColumn = new System.Data.DataColumn();
            this.dataColumn1 = new System.Data.DataColumn();
            this.dataColumn3 = new System.Data.DataColumn();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnInsert = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.DbTimer = new System.Windows.Forms.Timer(this.components);
            this.btnConfig = new System.Windows.Forms.Button();
            this.grpParams = new System.Windows.Forms.GroupBox();
            this.grpFacility = new System.Windows.Forms.GroupBox();
            this.grpDoor = new System.Windows.Forms.GroupBox();
            this.grpRun = new System.Windows.Forms.GroupBox();
            this.txtRun = new System.Windows.Forms.TextBox();
            this.grpResults = new System.Windows.Forms.GroupBox();
            this.grpTotalTags = new System.Windows.Forms.GroupBox();
            this.gridResults = new System.Windows.Forms.DataGridView();
            this.epcDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Threshold = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnExport = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ClearBtn = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.PalletCntTxt = new System.Windows.Forms.TextBox();
            this.gridPallet = new System.Windows.Forms.DataGridView();
            this.epcDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.palletIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataSetPalletTags = new System.Data.DataSet();
            this.dataTable1 = new System.Data.DataTable();
            this.dataColumn2 = new System.Data.DataColumn();
            this.dataColumn4 = new System.Data.DataColumn();
            this.MoveBtn = new System.Windows.Forms.Button();
            this.SelectBtn = new System.Windows.Forms.Button();
            this.txtFacility = new System.Windows.Forms.TextBox();
            this.txtZone = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataSetTagEvents)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtThreshEvents)).BeginInit();
            this.grpParams.SuspendLayout();
            this.grpFacility.SuspendLayout();
            this.grpDoor.SuspendLayout();
            this.grpRun.SuspendLayout();
            this.grpResults.SuspendLayout();
            this.grpTotalTags.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridResults)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridPallet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSetPalletTags)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataTable1)).BeginInit();
            this.SuspendLayout();
            // 
            // txtTotalTags
            // 
            this.txtTotalTags.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTotalTags.BackColor = System.Drawing.Color.White;
            this.txtTotalTags.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTotalTags.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.txtTotalTags.Location = new System.Drawing.Point(14, 24);
            this.txtTotalTags.Margin = new System.Windows.Forms.Padding(2);
            this.txtTotalTags.Name = "txtTotalTags";
            this.txtTotalTags.ReadOnly = true;
            this.txtTotalTags.Size = new System.Drawing.Size(155, 48);
            this.txtTotalTags.TabIndex = 1;
            this.txtTotalTags.Text = "0";
            this.txtTotalTags.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // dataSetTagEvents
            // 
            this.dataSetTagEvents.DataSetName = "TagEvents";
            this.dataSetTagEvents.Tables.AddRange(new System.Data.DataTable[] {
            this.dtThreshEvents});
            // 
            // dtThreshEvents
            // 
            this.dtThreshEvents.Columns.AddRange(new System.Data.DataColumn[] {
            this.EpcColumn,
            this.dataColumn1,
            this.dataColumn3});
            this.dtThreshEvents.TableName = "ThreshEvents";
            // 
            // EpcColumn
            // 
            this.EpcColumn.AllowDBNull = false;
            this.EpcColumn.ColumnName = "Epc";
            // 
            // dataColumn1
            // 
            this.dataColumn1.Caption = "Threshold";
            this.dataColumn1.ColumnName = "Threshold";
            // 
            // dataColumn3
            // 
            this.dataColumn3.Caption = "PalletId";
            this.dataColumn3.ColumnName = "PalletId";
            this.dataColumn3.DataType = typeof(long);
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.125F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExit.Location = new System.Drawing.Point(1167, 605);
            this.btnExit.Margin = new System.Windows.Forms.Padding(2);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(140, 50);
            this.btnExit.TabIndex = 14;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnInsert
            // 
            this.btnInsert.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnInsert.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.125F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnInsert.Location = new System.Drawing.Point(1000, 605);
            this.btnInsert.Margin = new System.Windows.Forms.Padding(2);
            this.btnInsert.Name = "btnInsert";
            this.btnInsert.Size = new System.Drawing.Size(163, 50);
            this.btnInsert.TabIndex = 13;
            this.btnInsert.Text = "DB Insert";
            this.btnInsert.UseVisualStyleBackColor = true;
            this.btnInsert.Click += new System.EventHandler(this.btnInsert_Click);
            // 
            // btnStop
            // 
            this.btnStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnStop.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.125F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStop.Location = new System.Drawing.Point(378, 605);
            this.btnStop.Margin = new System.Windows.Forms.Padding(2);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(140, 50);
            this.btnStop.TabIndex = 12;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnStart
            // 
            this.btnStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.125F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStart.Location = new System.Drawing.Point(211, 605);
            this.btnStart.Margin = new System.Windows.Forms.Padding(2);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(149, 50);
            this.btnStart.TabIndex = 8;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnConfig
            // 
            this.btnConfig.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnConfig.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.125F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConfig.Location = new System.Drawing.Point(30, 605);
            this.btnConfig.Margin = new System.Windows.Forms.Padding(2);
            this.btnConfig.Name = "btnConfig";
            this.btnConfig.Size = new System.Drawing.Size(164, 50);
            this.btnConfig.TabIndex = 16;
            this.btnConfig.Text = "Configure";
            this.btnConfig.UseVisualStyleBackColor = true;
            this.btnConfig.Click += new System.EventHandler(this.btnConfig_Click);
            // 
            // grpParams
            // 
            this.grpParams.Controls.Add(this.grpFacility);
            this.grpParams.Controls.Add(this.grpDoor);
            this.grpParams.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpParams.Location = new System.Drawing.Point(11, 11);
            this.grpParams.Margin = new System.Windows.Forms.Padding(2);
            this.grpParams.Name = "grpParams";
            this.grpParams.Padding = new System.Windows.Forms.Padding(2);
            this.grpParams.Size = new System.Drawing.Size(593, 110);
            this.grpParams.TabIndex = 9;
            this.grpParams.TabStop = false;
            this.grpParams.Text = "Staging Area Params";
            // 
            // grpFacility
            // 
            this.grpFacility.Controls.Add(this.txtFacility);
            this.grpFacility.Location = new System.Drawing.Point(4, 36);
            this.grpFacility.Margin = new System.Windows.Forms.Padding(2);
            this.grpFacility.Name = "grpFacility";
            this.grpFacility.Padding = new System.Windows.Forms.Padding(2);
            this.grpFacility.Size = new System.Drawing.Size(265, 70);
            this.grpFacility.TabIndex = 7;
            this.grpFacility.TabStop = false;
            this.grpFacility.Text = "Facility:";
            // 
            // grpDoor
            // 
            this.grpDoor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpDoor.Controls.Add(this.txtZone);
            this.grpDoor.Location = new System.Drawing.Point(273, 36);
            this.grpDoor.Margin = new System.Windows.Forms.Padding(2);
            this.grpDoor.Name = "grpDoor";
            this.grpDoor.Padding = new System.Windows.Forms.Padding(2);
            this.grpDoor.Size = new System.Drawing.Size(296, 70);
            this.grpDoor.TabIndex = 1;
            this.grpDoor.TabStop = false;
            this.grpDoor.Text = "Threshold / Zone Name:";
            // 
            // grpRun
            // 
            this.grpRun.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpRun.Controls.Add(this.txtRun);
            this.grpRun.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpRun.Location = new System.Drawing.Point(810, 23);
            this.grpRun.Margin = new System.Windows.Forms.Padding(2);
            this.grpRun.Name = "grpRun";
            this.grpRun.Padding = new System.Windows.Forms.Padding(2);
            this.grpRun.Size = new System.Drawing.Size(497, 83);
            this.grpRun.TabIndex = 10;
            this.grpRun.TabStop = false;
            this.grpRun.Text = "Pallet ID:";
            // 
            // txtRun
            // 
            this.txtRun.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtRun.BackColor = System.Drawing.Color.White;
            this.txtRun.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRun.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.txtRun.Location = new System.Drawing.Point(23, 27);
            this.txtRun.Margin = new System.Windows.Forms.Padding(2);
            this.txtRun.Name = "txtRun";
            this.txtRun.ReadOnly = true;
            this.txtRun.Size = new System.Drawing.Size(468, 48);
            this.txtRun.TabIndex = 0;
            this.txtRun.Text = "1";
            this.txtRun.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // grpResults
            // 
            this.grpResults.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.grpResults.Controls.Add(this.grpTotalTags);
            this.grpResults.Controls.Add(this.gridResults);
            this.grpResults.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpResults.Location = new System.Drawing.Point(11, 125);
            this.grpResults.Margin = new System.Windows.Forms.Padding(2);
            this.grpResults.Name = "grpResults";
            this.grpResults.Padding = new System.Windows.Forms.Padding(2);
            this.grpResults.Size = new System.Drawing.Size(531, 461);
            this.grpResults.TabIndex = 11;
            this.grpResults.TabStop = false;
            this.grpResults.Text = "Specified Threshold / Zone Reads:";
            // 
            // grpTotalTags
            // 
            this.grpTotalTags.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.grpTotalTags.Controls.Add(this.txtTotalTags);
            this.grpTotalTags.Location = new System.Drawing.Point(317, 377);
            this.grpTotalTags.Margin = new System.Windows.Forms.Padding(2);
            this.grpTotalTags.Name = "grpTotalTags";
            this.grpTotalTags.Padding = new System.Windows.Forms.Padding(2);
            this.grpTotalTags.Size = new System.Drawing.Size(190, 80);
            this.grpTotalTags.TabIndex = 1;
            this.grpTotalTags.TabStop = false;
            this.grpTotalTags.Text = "Tag Count:";
            // 
            // gridResults
            // 
            this.gridResults.AllowUserToAddRows = false;
            this.gridResults.AllowUserToDeleteRows = false;
            this.gridResults.AllowUserToResizeRows = false;
            this.gridResults.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridResults.AutoGenerateColumns = false;
            this.gridResults.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gridResults.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllHeaders;
            this.gridResults.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridResults.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.epcDataGridViewTextBoxColumn,
            this.Threshold});
            this.gridResults.DataBindings.Add(new System.Windows.Forms.Binding("Tag", this.dataSetTagEvents, "ThreshEvents.Epc", true));
            this.gridResults.DataMember = "ThreshEvents";
            this.gridResults.DataSource = this.dataSetTagEvents;
            this.gridResults.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.gridResults.Location = new System.Drawing.Point(4, 32);
            this.gridResults.Margin = new System.Windows.Forms.Padding(2);
            this.gridResults.Name = "gridResults";
            this.gridResults.ReadOnly = true;
            this.gridResults.RowTemplate.Height = 33;
            this.gridResults.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridResults.ShowCellErrors = false;
            this.gridResults.ShowCellToolTips = false;
            this.gridResults.ShowEditingIcon = false;
            this.gridResults.ShowRowErrors = false;
            this.gridResults.Size = new System.Drawing.Size(523, 341);
            this.gridResults.TabIndex = 0;
            // 
            // epcDataGridViewTextBoxColumn
            // 
            this.epcDataGridViewTextBoxColumn.DataPropertyName = "Epc";
            this.epcDataGridViewTextBoxColumn.HeaderText = "Epc";
            this.epcDataGridViewTextBoxColumn.Name = "epcDataGridViewTextBoxColumn";
            this.epcDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // Threshold
            // 
            this.Threshold.DataPropertyName = "Threshold";
            this.Threshold.HeaderText = "Threshold";
            this.Threshold.Name = "Threshold";
            this.Threshold.ReadOnly = true;
            // 
            // btnExport
            // 
            this.btnExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExport.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.125F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExport.Location = new System.Drawing.Point(804, 605);
            this.btnExport.Margin = new System.Windows.Forms.Padding(2);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(174, 50);
            this.btnExport.TabIndex = 15;
            this.btnExport.Text = "Export to CSV";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.ClearBtn);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.gridPallet);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(810, 125);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox1.Size = new System.Drawing.Size(497, 461);
            this.groupBox1.TabIndex = 17;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Pallet Assignment:";
            // 
            // ClearBtn
            // 
            this.ClearBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ClearBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ClearBtn.Location = new System.Drawing.Point(23, 377);
            this.ClearBtn.Name = "ClearBtn";
            this.ClearBtn.Size = new System.Drawing.Size(176, 38);
            this.ClearBtn.TabIndex = 8;
            this.ClearBtn.Text = "Clear";
            this.ClearBtn.UseVisualStyleBackColor = true;
            this.ClearBtn.Click += new System.EventHandler(this.ClearBtn_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.PalletCntTxt);
            this.groupBox2.Location = new System.Drawing.Point(234, 369);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox2.Size = new System.Drawing.Size(244, 80);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Pallet Tag Count:";
            // 
            // PalletCntTxt
            // 
            this.PalletCntTxt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PalletCntTxt.BackColor = System.Drawing.Color.White;
            this.PalletCntTxt.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PalletCntTxt.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.PalletCntTxt.Location = new System.Drawing.Point(14, 27);
            this.PalletCntTxt.Margin = new System.Windows.Forms.Padding(2);
            this.PalletCntTxt.Name = "PalletCntTxt";
            this.PalletCntTxt.ReadOnly = true;
            this.PalletCntTxt.Size = new System.Drawing.Size(213, 48);
            this.PalletCntTxt.TabIndex = 1;
            this.PalletCntTxt.Text = "0";
            this.PalletCntTxt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // gridPallet
            // 
            this.gridPallet.AllowUserToAddRows = false;
            this.gridPallet.AllowUserToDeleteRows = false;
            this.gridPallet.AllowUserToResizeRows = false;
            this.gridPallet.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridPallet.AutoGenerateColumns = false;
            this.gridPallet.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gridPallet.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllHeaders;
            this.gridPallet.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridPallet.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridPallet.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.epcDataGridViewTextBoxColumn1,
            this.palletIDDataGridViewTextBoxColumn});
            this.gridPallet.DataBindings.Add(new System.Windows.Forms.Binding("Tag", this.dataSetTagEvents, "ThreshEvents.Epc", true));
            this.gridPallet.DataMember = "PalletEvents";
            this.gridPallet.DataSource = this.dataSetPalletTags;
            this.gridPallet.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.gridPallet.Location = new System.Drawing.Point(23, 32);
            this.gridPallet.Margin = new System.Windows.Forms.Padding(2);
            this.gridPallet.MultiSelect = false;
            this.gridPallet.Name = "gridPallet";
            this.gridPallet.ReadOnly = true;
            this.gridPallet.RowTemplate.Height = 33;
            this.gridPallet.ShowCellErrors = false;
            this.gridPallet.ShowCellToolTips = false;
            this.gridPallet.ShowEditingIcon = false;
            this.gridPallet.ShowRowErrors = false;
            this.gridPallet.Size = new System.Drawing.Size(456, 321);
            this.gridPallet.TabIndex = 0;
            // 
            // epcDataGridViewTextBoxColumn1
            // 
            this.epcDataGridViewTextBoxColumn1.DataPropertyName = "Epc";
            this.epcDataGridViewTextBoxColumn1.HeaderText = "Epc";
            this.epcDataGridViewTextBoxColumn1.Name = "epcDataGridViewTextBoxColumn1";
            this.epcDataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // palletIDDataGridViewTextBoxColumn
            // 
            this.palletIDDataGridViewTextBoxColumn.DataPropertyName = "PalletID";
            this.palletIDDataGridViewTextBoxColumn.HeaderText = "PalletID";
            this.palletIDDataGridViewTextBoxColumn.Name = "palletIDDataGridViewTextBoxColumn";
            this.palletIDDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // dataSetPalletTags
            // 
            this.dataSetPalletTags.DataSetName = "PalletTags";
            this.dataSetPalletTags.Tables.AddRange(new System.Data.DataTable[] {
            this.dataTable1});
            // 
            // dataTable1
            // 
            this.dataTable1.Columns.AddRange(new System.Data.DataColumn[] {
            this.dataColumn2,
            this.dataColumn4});
            this.dataTable1.TableName = "PalletEvents";
            // 
            // dataColumn2
            // 
            this.dataColumn2.AllowDBNull = false;
            this.dataColumn2.ColumnName = "Epc";
            // 
            // dataColumn4
            // 
            this.dataColumn4.ColumnName = "PalletID";
            this.dataColumn4.DataType = typeof(long);
            // 
            // MoveBtn
            // 
            this.MoveBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MoveBtn.Location = new System.Drawing.Point(592, 354);
            this.MoveBtn.Name = "MoveBtn";
            this.MoveBtn.Size = new System.Drawing.Size(174, 47);
            this.MoveBtn.TabIndex = 18;
            this.MoveBtn.Text = "Move ==>>";
            this.MoveBtn.UseVisualStyleBackColor = true;
            this.MoveBtn.Click += new System.EventHandler(this.MoveBtn_Click);
            // 
            // SelectBtn
            // 
            this.SelectBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SelectBtn.Location = new System.Drawing.Point(592, 283);
            this.SelectBtn.Name = "SelectBtn";
            this.SelectBtn.Size = new System.Drawing.Size(174, 47);
            this.SelectBtn.TabIndex = 19;
            this.SelectBtn.Text = "<=Select All";
            this.SelectBtn.UseVisualStyleBackColor = true;
            this.SelectBtn.Click += new System.EventHandler(this.SelectBtn_Click);
            // 
            // txtFacility
            // 
            this.txtFacility.Location = new System.Drawing.Point(24, 26);
            this.txtFacility.Name = "txtFacility";
            this.txtFacility.Size = new System.Drawing.Size(224, 30);
            this.txtFacility.TabIndex = 0;
            // 
            // txtZone
            // 
            this.txtZone.Location = new System.Drawing.Point(10, 29);
            this.txtZone.Name = "txtZone";
            this.txtZone.Size = new System.Drawing.Size(270, 30);
            this.txtZone.TabIndex = 1;
            this.txtZone.Text = "any";
            // 
            // PalletBldForm
            // 
            this.ClientSize = new System.Drawing.Size(1355, 681);
            this.Controls.Add(this.SelectBtn);
            this.Controls.Add(this.MoveBtn);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnInsert);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.btnConfig);
            this.Controls.Add(this.grpParams);
            this.Controls.Add(this.grpRun);
            this.Controls.Add(this.grpResults);
            this.Controls.Add(this.btnExport);
            this.MinimumSize = new System.Drawing.Size(1377, 737);
            this.Name = "PalletBldForm";
            this.Text = "Pallet Builder";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PalletBldForm_FormClosing);
            this.Load += new System.EventHandler(this.PalletBldForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataSetTagEvents)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtThreshEvents)).EndInit();
            this.grpParams.ResumeLayout(false);
            this.grpFacility.ResumeLayout(false);
            this.grpFacility.PerformLayout();
            this.grpDoor.ResumeLayout(false);
            this.grpDoor.PerformLayout();
            this.grpRun.ResumeLayout(false);
            this.grpRun.PerformLayout();
            this.grpResults.ResumeLayout(false);
            this.grpTotalTags.ResumeLayout(false);
            this.grpTotalTags.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridResults)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridPallet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSetPalletTags)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataTable1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtTotalTags;
        private System.Data.DataSet dataSetTagEvents;
        private System.Data.DataTable dtThreshEvents;
        private System.Data.DataColumn EpcColumn;
        private System.Data.DataColumn dataColumn1;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnInsert;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Timer DbTimer;
        private System.Windows.Forms.Button btnConfig;
        private System.Windows.Forms.GroupBox grpParams;
        public System.Windows.Forms.GroupBox grpFacility;
        private System.Windows.Forms.GroupBox grpDoor;
        private System.Windows.Forms.GroupBox grpRun;
        private System.Windows.Forms.TextBox txtRun;
        private System.Windows.Forms.GroupBox grpResults;
        private System.Windows.Forms.GroupBox grpTotalTags;
        private System.Windows.Forms.DataGridView gridResults;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox PalletCntTxt;
        private System.Windows.Forms.DataGridView gridPallet;
        private System.Data.DataSet dataSetPalletTags;
        private System.Data.DataTable dataTable1;
        private System.Data.DataColumn dataColumn2;
        private System.Data.DataColumn dataColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn epcDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn palletIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.Button MoveBtn;
        private System.Windows.Forms.Button ClearBtn;
        private System.Windows.Forms.Button SelectBtn;
        private System.Windows.Forms.DataGridViewTextBoxColumn epcDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn Threshold;
        private System.Data.DataColumn dataColumn3;
        private System.Windows.Forms.TextBox txtFacility;
        private System.Windows.Forms.TextBox txtZone;
    }
}