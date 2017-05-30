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
            connectToServerButton.Click += (sender, args) =>
            {
                _flowClientWorker.TryConnect(
                    ipAddress: IPAddress.Parse(serverIpAddressTextBox.Text.Trim()),
                    port: int.Parse(serverPortTextBox.Text.Trim()));
            };

            authButton.Click += (sender, args) =>
            {
                _flowClientWorker.TryAuthenticate(
                    login: authLoginTextBox.Text,
                    password: authPassTextBox.Text);
            };

            registerButton.Click += (sender, args) =>
            {
                _flowClientWorker.TryRegister(
                    registerLoginTextBox.Text,
                    registerPassTextBox.Text,
                    registerNameTextBox.Text);
            };
        }
    }
}