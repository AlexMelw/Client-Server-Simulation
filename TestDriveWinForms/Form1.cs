namespace TestDriveWinForms
{
    using System;
    using System.Diagnostics;
    using System.Windows.Forms;
    using TranslatorService;

    public partial class Form1 : Form
    {
        #region CONSTRUCTORS

        public Form1()
        {
            InitializeComponent();
            RegisterEvents();
        }

        #endregion

        private void RegisterEvents()
        {
            translateButton.Click += (sender, args) =>
            {
                try
                {
                    var translatorClient = new LanguageServiceClient();
                    translatedTextRichTextBox.Text = translatorClient.Translate(
                        appId: "6CE9C85A41571C050C379F60DA173D286384E0F2",
                        text: originalTextRichTextBox.Text,
                        @from: "",
                        to: "en");
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                    throw;
                }
            };
        }
    }
}