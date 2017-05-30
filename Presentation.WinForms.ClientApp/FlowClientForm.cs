namespace Presentation.WinForms.ClientApp
{
    using System;
    using System.Net;
    using System.Windows.Forms;
    using FlowProtocol.Implementation.Response;
    using FlowProtocol.Implementation.Workers.Clients;
    using FlowProtocol.Implementation.Workers.Servers;
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
                _flowClientWorker = new TcpClientWorker(new ResponseProcessor(new ResponseParser()));
                serverPortTextBox.Text = TcpServerListeningPort.ToString();
            }

            if (ClientType.Equals(Conventions.ClientType.Udp))
            {
                //_flowClientWorker = IoC.Resolve<UdpClientWorker>();
                _flowClientWorker = new UdpClientWorker(new ResponseProcessor(new ResponseParser()));
                serverPortTextBox.Text = UdpServerListeningPort.ToString();
            }
        }

        private void ConfigControlsProperties() => serverIpAddressTextBox.Text = Localhost;

        private void RegisterEventHandlers()
        {
            // TODO Run Asynchronously + use MethodInvoker delegate

            connectToServerButton.Click += (sender, args) =>
            {
                bool connected = _flowClientWorker.TryConnect(
                    ipAddress: IPAddress.Parse(serverIpAddressTextBox.Text.Trim()),
                    port: int.Parse(serverPortTextBox.Text.Trim()));

                MessageBox.Show($@"Connected: {connected}");
            };

            authButton.Click += (sender, args) =>
            {
                bool authenticated = _flowClientWorker.TryAuthenticate(
                    login: authLoginTextBox.Text,
                    password: authPassTextBox.Text);

                MessageBox.Show($@"Authenticated: {authenticated}");
            };

            registerButton.Click += (sender, args) =>
            {
                bool registered = _flowClientWorker.TryRegister(
                    login: registerLoginTextBox.Text,
                    password: registerPassTextBox.Text,
                    name: registerNameTextBox.Text);

                MessageBox.Show(registered
                    ? @"Registered successfully"
                    : @"Registration failed");
            };

            translateButton.Click += (sender, args) =>
            {
                string inputTextLang = (string) fromLangComboBox.Text;
                string outputTextLang = (string) toLangComboBox.Text;

                string translatedText = _flowClientWorker.Translate(
                    sourceText: translateInputRichTextBox.Text,
                    sourceTextLang: inputTextLang,
                    targetTextLanguage: outputTextLang);

                translateOutputRichTextBox.Text = translatedText;
            };
        }
    }
}