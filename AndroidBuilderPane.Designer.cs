namespace AGS.Plugin.AndroidBuilder
{
    partial class AndroidBuilderPane
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.nmbObbVersion = new System.Windows.Forms.NumericUpDown();
            this.btnLauncherIconZip = new System.Windows.Forms.Button();
            this.nmbVersionCode = new System.Windows.Forms.NumericUpDown();
            this.lblLauncherIconZip = new System.Windows.Forms.Label();
            this.txtLauncherIconZip = new System.Windows.Forms.TextBox();
            this.lblAppName = new System.Windows.Forms.Label();
            this.txtPackageName = new System.Windows.Forms.TextBox();
            this.lblObbVersion = new System.Windows.Forms.Label();
            this.txtVersionName = new System.Windows.Forms.TextBox();
            this.txtAppName = new System.Windows.Forms.TextBox();
            this.lblPackageName = new System.Windows.Forms.Label();
            this.lblVersionName = new System.Windows.Forms.Label();
            this.lblVersionCode = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnRandomizePrivateSalt = new System.Windows.Forms.Button();
            this.txtRsaPublicKey = new System.Windows.Forms.TextBox();
            this.lblPrivateSalt = new System.Windows.Forms.Label();
            this.txtPrivateSalt = new System.Windows.Forms.TextBox();
            this.lblRsaPublicKey = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnShowKeyStoreAliasPassword = new System.Windows.Forms.Button();
            this.btnShowKeyStorePassword = new System.Windows.Forms.Button();
            this.btnKeyStorePath = new System.Windows.Forms.Button();
            this.txtKeyStorePath = new System.Windows.Forms.TextBox();
            this.lblKeyStorePath = new System.Windows.Forms.Label();
            this.lblKeyStorePassword = new System.Windows.Forms.Label();
            this.lblKeyStoreAlias = new System.Windows.Forms.Label();
            this.txtKeyStorePassword = new System.Windows.Forms.TextBox();
            this.lblKeyStoreAliasPassword = new System.Windows.Forms.Label();
            this.txtKeyStoreAlias = new System.Windows.Forms.TextBox();
            this.txtKeyStoreAliasPassword = new System.Windows.Forms.TextBox();
            this.dlgSelectKeyStore = new System.Windows.Forms.OpenFileDialog();
            this.dlgSelectLauncherIconZip = new System.Windows.Forms.OpenFileDialog();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nmbObbVersion)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmbVersionCode)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.nmbObbVersion);
            this.groupBox3.Controls.Add(this.btnLauncherIconZip);
            this.groupBox3.Controls.Add(this.nmbVersionCode);
            this.groupBox3.Controls.Add(this.lblLauncherIconZip);
            this.groupBox3.Controls.Add(this.txtLauncherIconZip);
            this.groupBox3.Controls.Add(this.lblAppName);
            this.groupBox3.Controls.Add(this.txtPackageName);
            this.groupBox3.Controls.Add(this.lblObbVersion);
            this.groupBox3.Controls.Add(this.txtVersionName);
            this.groupBox3.Controls.Add(this.txtAppName);
            this.groupBox3.Controls.Add(this.lblPackageName);
            this.groupBox3.Controls.Add(this.lblVersionName);
            this.groupBox3.Controls.Add(this.lblVersionCode);
            this.groupBox3.Location = new System.Drawing.Point(3, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(442, 201);
            this.groupBox3.TabIndex = 49;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "App";
            // 
            // nmbObbVersion
            // 
            this.nmbObbVersion.Location = new System.Drawing.Point(164, 138);
            this.nmbObbVersion.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.nmbObbVersion.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nmbObbVersion.Name = "nmbObbVersion";
            this.nmbObbVersion.Size = new System.Drawing.Size(231, 20);
            this.nmbObbVersion.TabIndex = 4;
            this.nmbObbVersion.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // btnLauncherIconZip
            // 
            this.btnLauncherIconZip.Location = new System.Drawing.Point(401, 164);
            this.btnLauncherIconZip.Name = "btnLauncherIconZip";
            this.btnLauncherIconZip.Size = new System.Drawing.Size(29, 23);
            this.btnLauncherIconZip.TabIndex = 6;
            this.btnLauncherIconZip.Text = "...";
            this.btnLauncherIconZip.UseVisualStyleBackColor = true;
            this.btnLauncherIconZip.Click += new System.EventHandler(this.SelectPath_Click);
            // 
            // nmbVersionCode
            // 
            this.nmbVersionCode.Location = new System.Drawing.Point(164, 80);
            this.nmbVersionCode.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.nmbVersionCode.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nmbVersionCode.Name = "nmbVersionCode";
            this.nmbVersionCode.Size = new System.Drawing.Size(231, 20);
            this.nmbVersionCode.TabIndex = 2;
            this.nmbVersionCode.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lblLauncherIconZip
            // 
            this.lblLauncherIconZip.AutoSize = true;
            this.lblLauncherIconZip.Location = new System.Drawing.Point(61, 169);
            this.lblLauncherIconZip.Name = "lblLauncherIconZip";
            this.lblLauncherIconZip.Size = new System.Drawing.Size(97, 13);
            this.lblLauncherIconZip.TabIndex = 32;
            this.lblLauncherIconZip.Text = "Launcher Icon Zip:";
            // 
            // txtLauncherIconZip
            // 
            this.txtLauncherIconZip.Location = new System.Drawing.Point(164, 166);
            this.txtLauncherIconZip.Name = "txtLauncherIconZip";
            this.txtLauncherIconZip.Size = new System.Drawing.Size(231, 20);
            this.txtLauncherIconZip.TabIndex = 5;
            // 
            // lblAppName
            // 
            this.lblAppName.AutoSize = true;
            this.lblAppName.Location = new System.Drawing.Point(98, 24);
            this.lblAppName.Name = "lblAppName";
            this.lblAppName.Size = new System.Drawing.Size(60, 13);
            this.lblAppName.TabIndex = 29;
            this.lblAppName.Text = "App Name:";
            // 
            // txtPackageName
            // 
            this.txtPackageName.Location = new System.Drawing.Point(164, 50);
            this.txtPackageName.Name = "txtPackageName";
            this.txtPackageName.Size = new System.Drawing.Size(231, 20);
            this.txtPackageName.TabIndex = 1;
            // 
            // lblObbVersion
            // 
            this.lblObbVersion.AutoSize = true;
            this.lblObbVersion.Location = new System.Drawing.Point(6, 140);
            this.lblObbVersion.Name = "lblObbVersion";
            this.lblObbVersion.Size = new System.Drawing.Size(152, 13);
            this.lblObbVersion.TabIndex = 30;
            this.lblObbVersion.Text = "APK Expansion (OBB) Version:";
            // 
            // txtVersionName
            // 
            this.txtVersionName.Location = new System.Drawing.Point(164, 108);
            this.txtVersionName.Name = "txtVersionName";
            this.txtVersionName.Size = new System.Drawing.Size(231, 20);
            this.txtVersionName.TabIndex = 3;
            // 
            // txtAppName
            // 
            this.txtAppName.Location = new System.Drawing.Point(164, 21);
            this.txtAppName.Name = "txtAppName";
            this.txtAppName.Size = new System.Drawing.Size(231, 20);
            this.txtAppName.TabIndex = 0;
            // 
            // lblPackageName
            // 
            this.lblPackageName.AutoSize = true;
            this.lblPackageName.Location = new System.Drawing.Point(74, 53);
            this.lblPackageName.Name = "lblPackageName";
            this.lblPackageName.Size = new System.Drawing.Size(84, 13);
            this.lblPackageName.TabIndex = 28;
            this.lblPackageName.Text = "Package Name:";
            // 
            // lblVersionName
            // 
            this.lblVersionName.AutoSize = true;
            this.lblVersionName.Location = new System.Drawing.Point(82, 111);
            this.lblVersionName.Name = "lblVersionName";
            this.lblVersionName.Size = new System.Drawing.Size(76, 13);
            this.lblVersionName.TabIndex = 26;
            this.lblVersionName.Text = "Version Name:";
            // 
            // lblVersionCode
            // 
            this.lblVersionCode.AutoSize = true;
            this.lblVersionCode.Location = new System.Drawing.Point(85, 82);
            this.lblVersionCode.Name = "lblVersionCode";
            this.lblVersionCode.Size = new System.Drawing.Size(73, 13);
            this.lblVersionCode.TabIndex = 27;
            this.lblVersionCode.Text = "Version Code:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnRandomizePrivateSalt);
            this.groupBox2.Controls.Add(this.txtRsaPublicKey);
            this.groupBox2.Controls.Add(this.lblPrivateSalt);
            this.groupBox2.Controls.Add(this.txtPrivateSalt);
            this.groupBox2.Controls.Add(this.lblRsaPublicKey);
            this.groupBox2.Location = new System.Drawing.Point(3, 355);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(483, 139);
            this.groupBox2.TabIndex = 48;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Google Play";
            // 
            // btnRandomizePrivateSalt
            // 
            this.btnRandomizePrivateSalt.Location = new System.Drawing.Point(401, 72);
            this.btnRandomizePrivateSalt.Name = "btnRandomizePrivateSalt";
            this.btnRandomizePrivateSalt.Size = new System.Drawing.Size(70, 23);
            this.btnRandomizePrivateSalt.TabIndex = 14;
            this.btnRandomizePrivateSalt.Text = "Randomize";
            this.btnRandomizePrivateSalt.UseVisualStyleBackColor = true;
            this.btnRandomizePrivateSalt.Click += new System.EventHandler(this.btnRandomizePrivateSalt_Click);
            // 
            // txtRsaPublicKey
            // 
            this.txtRsaPublicKey.Location = new System.Drawing.Point(164, 16);
            this.txtRsaPublicKey.Multiline = true;
            this.txtRsaPublicKey.Name = "txtRsaPublicKey";
            this.txtRsaPublicKey.Size = new System.Drawing.Size(231, 50);
            this.txtRsaPublicKey.TabIndex = 12;
            // 
            // lblPrivateSalt
            // 
            this.lblPrivateSalt.AutoSize = true;
            this.lblPrivateSalt.Location = new System.Drawing.Point(94, 77);
            this.lblPrivateSalt.Name = "lblPrivateSalt";
            this.lblPrivateSalt.Size = new System.Drawing.Size(64, 13);
            this.lblPrivateSalt.TabIndex = 32;
            this.lblPrivateSalt.Text = "Private Salt:";
            // 
            // txtPrivateSalt
            // 
            this.txtPrivateSalt.Location = new System.Drawing.Point(164, 74);
            this.txtPrivateSalt.Multiline = true;
            this.txtPrivateSalt.Name = "txtPrivateSalt";
            this.txtPrivateSalt.Size = new System.Drawing.Size(231, 50);
            this.txtPrivateSalt.TabIndex = 13;
            // 
            // lblRsaPublicKey
            // 
            this.lblRsaPublicKey.AutoSize = true;
            this.lblRsaPublicKey.Location = new System.Drawing.Point(73, 19);
            this.lblRsaPublicKey.Name = "lblRsaPublicKey";
            this.lblRsaPublicKey.Size = new System.Drawing.Size(85, 13);
            this.lblRsaPublicKey.TabIndex = 31;
            this.lblRsaPublicKey.Text = "RSA Public Key:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnShowKeyStoreAliasPassword);
            this.groupBox1.Controls.Add(this.btnShowKeyStorePassword);
            this.groupBox1.Controls.Add(this.btnKeyStorePath);
            this.groupBox1.Controls.Add(this.txtKeyStorePath);
            this.groupBox1.Controls.Add(this.lblKeyStorePath);
            this.groupBox1.Controls.Add(this.lblKeyStorePassword);
            this.groupBox1.Controls.Add(this.lblKeyStoreAlias);
            this.groupBox1.Controls.Add(this.txtKeyStorePassword);
            this.groupBox1.Controls.Add(this.lblKeyStoreAliasPassword);
            this.groupBox1.Controls.Add(this.txtKeyStoreAlias);
            this.groupBox1.Controls.Add(this.txtKeyStoreAliasPassword);
            this.groupBox1.Location = new System.Drawing.Point(3, 210);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(457, 139);
            this.groupBox1.TabIndex = 47;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Signing";
            // 
            // btnShowKeyStoreAliasPassword
            // 
            this.btnShowKeyStoreAliasPassword.Location = new System.Drawing.Point(401, 101);
            this.btnShowKeyStoreAliasPassword.Name = "btnShowKeyStoreAliasPassword";
            this.btnShowKeyStoreAliasPassword.Size = new System.Drawing.Size(44, 23);
            this.btnShowKeyStoreAliasPassword.TabIndex = 28;
            this.btnShowKeyStoreAliasPassword.TabStop = false;
            this.btnShowKeyStoreAliasPassword.Text = "Show";
            this.btnShowKeyStoreAliasPassword.UseVisualStyleBackColor = true;
            this.btnShowKeyStoreAliasPassword.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ShowPassword_MouseDown);
            this.btnShowKeyStoreAliasPassword.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ShowPassword_MouseUp);
            // 
            // btnShowKeyStorePassword
            // 
            this.btnShowKeyStorePassword.Location = new System.Drawing.Point(401, 43);
            this.btnShowKeyStorePassword.Name = "btnShowKeyStorePassword";
            this.btnShowKeyStorePassword.Size = new System.Drawing.Size(44, 23);
            this.btnShowKeyStorePassword.TabIndex = 27;
            this.btnShowKeyStorePassword.TabStop = false;
            this.btnShowKeyStorePassword.Text = "Show";
            this.btnShowKeyStorePassword.UseVisualStyleBackColor = true;
            this.btnShowKeyStorePassword.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ShowPassword_MouseDown);
            this.btnShowKeyStorePassword.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ShowPassword_MouseUp);
            // 
            // btnKeyStorePath
            // 
            this.btnKeyStorePath.Location = new System.Drawing.Point(401, 14);
            this.btnKeyStorePath.Name = "btnKeyStorePath";
            this.btnKeyStorePath.Size = new System.Drawing.Size(29, 23);
            this.btnKeyStorePath.TabIndex = 8;
            this.btnKeyStorePath.Text = "...";
            this.btnKeyStorePath.UseVisualStyleBackColor = true;
            this.btnKeyStorePath.Click += new System.EventHandler(this.SelectPath_Click);
            // 
            // txtKeyStorePath
            // 
            this.txtKeyStorePath.Location = new System.Drawing.Point(164, 16);
            this.txtKeyStorePath.Name = "txtKeyStorePath";
            this.txtKeyStorePath.Size = new System.Drawing.Size(231, 20);
            this.txtKeyStorePath.TabIndex = 7;
            // 
            // lblKeyStorePath
            // 
            this.lblKeyStorePath.AutoSize = true;
            this.lblKeyStorePath.Location = new System.Drawing.Point(77, 19);
            this.lblKeyStorePath.Name = "lblKeyStorePath";
            this.lblKeyStorePath.Size = new System.Drawing.Size(81, 13);
            this.lblKeyStorePath.TabIndex = 25;
            this.lblKeyStorePath.Text = "Key Store Path:";
            // 
            // lblKeyStorePassword
            // 
            this.lblKeyStorePassword.AutoSize = true;
            this.lblKeyStorePassword.Location = new System.Drawing.Point(53, 48);
            this.lblKeyStorePassword.Name = "lblKeyStorePassword";
            this.lblKeyStorePassword.Size = new System.Drawing.Size(105, 13);
            this.lblKeyStorePassword.TabIndex = 24;
            this.lblKeyStorePassword.Text = "Key Store Password:";
            // 
            // lblKeyStoreAlias
            // 
            this.lblKeyStoreAlias.AutoSize = true;
            this.lblKeyStoreAlias.Location = new System.Drawing.Point(77, 77);
            this.lblKeyStoreAlias.Name = "lblKeyStoreAlias";
            this.lblKeyStoreAlias.Size = new System.Drawing.Size(81, 13);
            this.lblKeyStoreAlias.TabIndex = 23;
            this.lblKeyStoreAlias.Text = "Key Store Alias:";
            // 
            // txtKeyStorePassword
            // 
            this.txtKeyStorePassword.Location = new System.Drawing.Point(164, 45);
            this.txtKeyStorePassword.Name = "txtKeyStorePassword";
            this.txtKeyStorePassword.Size = new System.Drawing.Size(231, 20);
            this.txtKeyStorePassword.TabIndex = 9;
            this.txtKeyStorePassword.UseSystemPasswordChar = true;
            // 
            // lblKeyStoreAliasPassword
            // 
            this.lblKeyStoreAliasPassword.AutoSize = true;
            this.lblKeyStoreAliasPassword.Location = new System.Drawing.Point(28, 106);
            this.lblKeyStoreAliasPassword.Name = "lblKeyStoreAliasPassword";
            this.lblKeyStoreAliasPassword.Size = new System.Drawing.Size(130, 13);
            this.lblKeyStoreAliasPassword.TabIndex = 22;
            this.lblKeyStoreAliasPassword.Text = "Key Store Alias Password:";
            // 
            // txtKeyStoreAlias
            // 
            this.txtKeyStoreAlias.Location = new System.Drawing.Point(164, 74);
            this.txtKeyStoreAlias.Name = "txtKeyStoreAlias";
            this.txtKeyStoreAlias.Size = new System.Drawing.Size(231, 20);
            this.txtKeyStoreAlias.TabIndex = 10;
            // 
            // txtKeyStoreAliasPassword
            // 
            this.txtKeyStoreAliasPassword.Location = new System.Drawing.Point(164, 103);
            this.txtKeyStoreAliasPassword.Name = "txtKeyStoreAliasPassword";
            this.txtKeyStoreAliasPassword.Size = new System.Drawing.Size(231, 20);
            this.txtKeyStoreAliasPassword.TabIndex = 11;
            this.txtKeyStoreAliasPassword.UseSystemPasswordChar = true;
            // 
            // AndroidBuilderPane
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "AndroidBuilderPane";
            this.Size = new System.Drawing.Size(489, 497);
            this.VisibleChanged += new System.EventHandler(this.AndroidBuilderPane_VisibleChanged);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nmbObbVersion)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmbVersionCode)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.NumericUpDown nmbObbVersion;
        private System.Windows.Forms.Button btnLauncherIconZip;
        private System.Windows.Forms.NumericUpDown nmbVersionCode;
        private System.Windows.Forms.Label lblLauncherIconZip;
        private System.Windows.Forms.TextBox txtLauncherIconZip;
        private System.Windows.Forms.Label lblAppName;
        private System.Windows.Forms.TextBox txtPackageName;
        private System.Windows.Forms.Label lblObbVersion;
        private System.Windows.Forms.TextBox txtVersionName;
        private System.Windows.Forms.TextBox txtAppName;
        private System.Windows.Forms.Label lblPackageName;
        private System.Windows.Forms.Label lblVersionName;
        private System.Windows.Forms.Label lblVersionCode;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnRandomizePrivateSalt;
        private System.Windows.Forms.TextBox txtRsaPublicKey;
        private System.Windows.Forms.Label lblPrivateSalt;
        private System.Windows.Forms.TextBox txtPrivateSalt;
        private System.Windows.Forms.Label lblRsaPublicKey;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnShowKeyStoreAliasPassword;
        private System.Windows.Forms.Button btnShowKeyStorePassword;
        private System.Windows.Forms.Button btnKeyStorePath;
        private System.Windows.Forms.TextBox txtKeyStorePath;
        private System.Windows.Forms.Label lblKeyStorePath;
        private System.Windows.Forms.Label lblKeyStorePassword;
        private System.Windows.Forms.Label lblKeyStoreAlias;
        private System.Windows.Forms.TextBox txtKeyStorePassword;
        private System.Windows.Forms.Label lblKeyStoreAliasPassword;
        private System.Windows.Forms.TextBox txtKeyStoreAlias;
        private System.Windows.Forms.TextBox txtKeyStoreAliasPassword;
        private System.Windows.Forms.OpenFileDialog dlgSelectKeyStore;
        private System.Windows.Forms.OpenFileDialog dlgSelectLauncherIconZip;
    }
}
