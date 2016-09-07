using System;
using AGS.Types;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace AGS.Plugin.AndroidBuilder
{
    public partial class AndroidBuilderPane : EditorContentPanel
    {
        private static AndroidBuilderPane _instance;
        private static List<string> errors = new List<string>();

        public static AndroidBuilderPane Instance
        {
            get { return _instance == null ? _instance = new AndroidBuilderPane() : _instance; }
        }

        private AndroidBuilderPane()
        {
            InitializeComponent();
            btnShowKeyStorePassword.Tag = txtKeyStorePassword;
            btnShowKeyStoreAliasPassword.Tag = txtKeyStoreAliasPassword;
            // KeyValuePair isn't totally appropriate here, but it's effectively the same as a Tuple/Pair for our needs
            btnKeyStorePath.Tag = new KeyValuePair<OpenFileDialog, TextBox>(dlgSelectKeyStore, txtKeyStorePath);
            btnLauncherIconZip.Tag = new KeyValuePair<OpenFileDialog, TextBox>(dlgSelectLauncherIconZip, txtLauncherIconZip);
            // TODO: tooltip
        }

        private static DialogResult ShowWarning(MessageBoxButtons buttons)
        {
            if (errors.Count == 0)
                return DialogResult.OK;
            StringBuilder message = new StringBuilder("Oops! Looks like there ")
                .Append(errors.Count == 1 ? "was an error " : "were some errors ")
                .Append("with the values you entered! Please correct the following before building for Android: ");
            foreach (string error in errors)
            {
                message.Append(Environment.NewLine).Append(Environment.NewLine)
                    .Append("* ").Append(error);
            }
            return MessageBox.Show(message.ToString(), "Uh-oh!", buttons, MessageBoxIcon.Warning);
        }

        internal static IList<string> Errors
        {
            get { return errors; }
        }

        internal void CheckForErrors()
        {
            errors.Clear();
            if (string.IsNullOrEmpty(txtPackageName.Text))
            {
                errors.Add("You forgot to enter a package name!");
            }
            else if (!Regex.IsMatch(txtPackageName.Text, @"([\p{L}_$][\p{L}\p{N}_$]*\.)*[\p{L}_$][\p{L}\p{N}_$]*"))
            {
                errors.Add("There's some invalid characters in your package name. Please make sure to use a valid Java package name.");
            }
            if (string.IsNullOrEmpty(txtVersionName.Text))
            {
                errors.Add("You forgot to enter a version name!");
            }
            if (!File.Exists(txtKeyStorePath.Text))
            {
                errors.Add("The selected key store file doesn't exist. Please make sure to include the path to a Java key store file (required for signing your APK).");
            }
            if (string.IsNullOrEmpty(txtKeyStoreAlias.Text))
            {
                errors.Add("You forgot to enter the alias for your key store!");
            }
            if (string.IsNullOrEmpty(txtAppName.Text))
            {
                errors.Add("You forgot to enter your app's name!");
            }
            if ((string.IsNullOrEmpty(txtLauncherIconZip.Text)) || (!File.Exists(txtLauncherIconZip.Text)))
            {
                errors.Add("The specified launcher icon zip doesn't exist! Make sure you've entered a valid filename!");
            }
        }

        internal void OnMetadataLoaded()
        {
            txtPackageName.Text = AndroidMetadata.PackageName;
            nmbVersionCode.Value = AndroidMetadata.VersionCode;
            txtVersionName.Text = AndroidMetadata.VersionName;
            txtKeyStorePath.Text = AndroidMetadata.KeyStorePath;
            txtKeyStorePassword.Text = AndroidMetadata.KeyStorePassword;
            txtKeyStoreAlias.Text = AndroidMetadata.KeyStoreAlias;
            txtKeyStoreAliasPassword.Text = AndroidMetadata.KeyStoreAliasPassword;
            txtAppName.Text = AndroidMetadata.AppName;
            nmbObbVersion.Value = AndroidMetadata.ObbVersion;
            txtRsaPublicKey.Text = AndroidMetadata.RsaPublicKey == "@null" ? "" : AndroidMetadata.RsaPublicKey;
            txtPrivateSalt.Text = AndroidMetadata.PrivateSalt.ToCsv();
            txtLauncherIconZip.Text = AndroidMetadata.LauncherIconsZip;
        }

        internal void SaveMetadata()
        {
            AndroidMetadata.PackageName = txtPackageName.Text;
            AndroidMetadata.VersionCode = (int)nmbVersionCode.Value;
            AndroidMetadata.VersionName = txtVersionName.Text;
            AndroidMetadata.KeyStorePath = txtKeyStorePath.Text;
            AndroidMetadata.KeyStorePassword = txtKeyStorePassword.Text;
            AndroidMetadata.KeyStoreAlias = txtKeyStoreAlias.Text;
            AndroidMetadata.KeyStoreAliasPassword = txtKeyStoreAliasPassword.Text;
            AndroidMetadata.AppName = txtAppName.Text;
            AndroidMetadata.ObbVersion = (int)nmbObbVersion.Value;
            AndroidMetadata.RsaPublicKey = txtRsaPublicKey.Text;
            AndroidMetadata.PrivateSalt = XmlCsvSByteArray.CreateFromCsv(txtPrivateSalt.Text
                .Replace("\r", "").Replace("\n", ""));
            AndroidMetadata.LauncherIconsZip = txtLauncherIconZip.Text;
        }

        protected override void OnPanelClosing(bool canCancel, ref bool cancelClose)
        {
            if (canCancel)
            {
                MessageBoxButtons warnButtons = canCancel ? MessageBoxButtons.OKCancel : MessageBoxButtons.OK;
                CheckForErrors();
                cancelClose = (ShowWarning(warnButtons) == DialogResult.Cancel);
                if (cancelClose)
                    return;
            }
            SaveMetadata();
        }

        private void AndroidBuilderPane_VisibleChanged(object sender, EventArgs e)
        {
        }

        private void ShowPassword_MouseDown(object sender, MouseEventArgs e)
        {
            ((TextBox)((Control)sender).Tag).UseSystemPasswordChar = false;
        }

        private void ShowPassword_MouseUp(object sender, MouseEventArgs e)
        {
            ((TextBox)((Control)sender).Tag).UseSystemPasswordChar = true;
        }

        private void btnRandomizePrivateSalt_Click(object sender, EventArgs e)
        {
            byte[] randomBytes = new byte[16];
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetNonZeroBytes(randomBytes);
            sbyte[] sbytes = new sbyte[randomBytes.Length];
            for (int i = 0; i < randomBytes.Length; ++i)
            {
                sbytes[i] = (sbyte)randomBytes[i];
            }
            AndroidMetadata.PrivateSalt = XmlCsvSByteArray.CreateFromBytes(sbytes);
            txtPrivateSalt.Text = AndroidMetadata.PrivateSalt.ToCsv();
        }

        private void SelectPath_Click(object sender, EventArgs e)
        {
            KeyValuePair<OpenFileDialog, TextBox> tag = (KeyValuePair<OpenFileDialog, TextBox>)((Button)sender).Tag;
            OpenFileDialog dialog = tag.Key;
            TextBox textBox = tag.Value;
            if ((!string.IsNullOrEmpty(textBox.Text)) &&
                File.Exists(textBox.Text))
            {
                dialog.InitialDirectory = Path.GetDirectoryName(textBox.Text);
                dialog.FileName = textBox.Text;
            }
            if (dialog.ShowDialog() == DialogResult.OK)
                textBox.Text = dialog.FileName;
        }
    }
}
