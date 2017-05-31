namespace Presentation.WinForms.ClientApp
{
    using System;
    using System.Diagnostics;
    using System.Net;
    using System.Windows.Forms;
    using FlowProtocol.Implementation.Response;
    using FlowProtocol.Implementation.Workers.Clients;
    using FlowProtocol.Interfaces.CommonConventions;
    using FlowProtocol.Interfaces.Workers;
    using static FlowProtocol.Interfaces.CommonConventions.Conventions;

    public partial class FlowClientForm : Form
    {
        private IFlowClientWorker _flowClientWorker;
        public string ClientType { get; set; }

        #region CONSTRUCTORS

        public FlowClientForm()
        {
            InitializeComponent();
        }

        #endregion

        private void FlowClientForm_Load(object sender, EventArgs e)
        {
            ConfigControlsProperties();
            InitializeFlowClientWorker();
            RegisterEventHandlers();
        }

        private void InitializeFlowClientWorker()
        {
            if (ClientType.Equals(Conventions.ClientType.Tcp))
            {
                //_flowClientWorker = IoC.Resolve<TcpClientWorker>();
                _flowClientWorker = new TcpClientWorker(new ResponseParser());
                serverPortTextBox.Text = TcpServerListeningPort.ToString();
                return;
            }

            if (ClientType.Equals(Conventions.ClientType.Udp))
            {
                //_flowClientWorker = IoC.Resolve<UdpClientWorker>();
                _flowClientWorker = new UdpClientWorker(new ResponseParser());
                serverPortTextBox.Text = UdpServerListeningPort.ToString();
            }
        }

        private void ConfigControlsProperties()
        {
            serverIpAddressTextBox.Text = Localhost;

            fromLangComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            fromLangComboBox.SelectedIndex = 3;

            toLangComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            toLangComboBox.SelectedIndex = 0;

            incomingLangComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            incomingLangComboBox.SelectedIndex = 0;

            outgoingLangComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            outgoingLangComboBox.SelectedIndex = 3;

            translateInputRichTextBox.BorderStyle = BorderStyle.None;
            translateOutputRichTextBox.BorderStyle = BorderStyle.None;

            incomingMessagesRichTextBox.BorderStyle = BorderStyle.None;
            outgoingMessagesRichTextBox.BorderStyle = BorderStyle.None;
        }

        private void RegisterEventHandlers()
        {
            // TODO Run Asynchronously + use MethodInvoker delegate

            connectToServerButton.Click += (sender, args) =>
            {
                bool connected = _flowClientWorker.Connect(
                    ipAddress: IPAddress.Parse(serverIpAddressTextBox.Text.Trim()),
                    port: int.Parse(serverPortTextBox.Text.Trim()));

                MessageBox.Show($@"Connected: {connected}");
            };

            authButton.Click += (sender, args) =>
            {
                try
                {
                    bool authenticated = _flowClientWorker.Authenticate(
                        login: authLoginTextBox.Text,
                        password: authPassTextBox.Text);

                    MessageBox.Show($@"Authenticated: {authenticated}");
                }
                catch (Exception exception)
                {
                    Debug.WriteLine(exception);
                    MessageBox.Show($@"{exception.Message}");
                }
            };

            registerButton.Click += (sender, args) =>
            {
                try
                {
                    bool registered = _flowClientWorker.Register(
                        login: registerLoginTextBox.Text,
                        password: registerPassTextBox.Text,
                        name: registerNameTextBox.Text);

                    MessageBox.Show(registered
                        ? @"Registered successfully"
                        : @"Registration failed");
                }
                catch (Exception exception)
                {
                    Debug.WriteLine(exception);
                    MessageBox.Show($@"{exception.Message}");
                }
            };

            translateButton.Click += (sender, args) =>
            {
                try
                {
                    string inputTextLang = (string) fromLangComboBox.Text;
                    string outputTextLang = (string) toLangComboBox.Text;

                    string translatedText = _flowClientWorker.Translate(
                        sourceText: translateInputRichTextBox.Text,
                        sourceTextLang: inputTextLang,
                        targetTextLanguage: outputTextLang);

                    translateOutputRichTextBox.Text = translatedText;
                }
                catch (Exception exception)
                {
                    Debug.WriteLine(exception);
                    MessageBox.Show($@"{exception.Message}");
                }
            };

            SendMessageButton.Click += (sender, args) =>
            {
                try
                {
                    _flowClientWorker.
                }
                catch (Exception exception)
                {
                    Debug.WriteLine(exception);
                    MessageBox.Show($@"{exception.Message}");
                }
            };
        }
    }
}