namespace Presentation.WinForms.ClientApp
{
    using System;
    using System.Net;
    using System.Windows.Forms;
    using Protocol.Implementation.Response;
    using Protocol.Implementation.Workers;
    using Protocol.Interfaces;
    using Protocol.Interfaces.CommonConventions;

    public partial class FlowClientForm : Form
    {
        private IClientWorker _clientWorker;

        public string ClientType { get; set; }

        #region CONSTRUCTORS

        public FlowClientForm()
        {
            InitializeComponent();
        }

        #endregion

        private void FlowClientForm_Load(object sender, EventArgs e)
        {
            InitializeFlowClientWorker();
            ConfigControlsProperties();
            RegisterEventHandlers();
        }

        private void InitializeFlowClientWorker()
        {
            if (ClientType.Equals(Conventions.DataTransmissionType.Tcp, StringComparison.OrdinalIgnoreCase))
            {
                //_clientWorker = IoC.Resolve<TcpClientWorker>();
                _clientWorker = new TcpClientWorker(new ResponseProcessor(new ResponseParser()));
                serverPortTextBox.Text = Conventions.TcpServerListeningPort.ToString();
            }

            if (ClientType.Equals(Conventions.DataTransmissionType.Udp, StringComparison.OrdinalIgnoreCase))
            {
                //_clientWorker = IoC.Resolve<UdpClientWorker>();s
                _clientWorker = new UdpClientWorker(new ResponseProcessor(new ResponseParser()));
                serverPortTextBox.Text = Conventions.UdpServerListeningPort.ToString();
            }
        }

        private void ConfigControlsProperties()
        {
            serverIpAddressTextBox.Text = Conventions.Localhost;
        }

        private void RegisterEventHandlers()
        {
            connectToServerButton.Click += (sender, args) =>
            {
                _clientWorker.Init(
                    ipAddress: IPAddress.Parse(serverIpAddressTextBox.Text.Trim()),
                    port: int.Parse(serverPortTextBox.Text.Trim()));
            };

            authButton.Click += (sender, args) =>
            {
                _clientWorker.Authenticate(
                    authLoginTextBox.Text,
                    authPassTextBox.Text);
            }
        }
    }
}