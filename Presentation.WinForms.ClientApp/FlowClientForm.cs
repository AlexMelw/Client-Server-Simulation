namespace Presentation.WinForms.ClientApp
{
    using System;
    using System.Net;
    using System.Windows.Forms;
    using FlowProtocol.Interfaces.CommonConventions;

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
            InitializeFlowClientWorker();
            ConfigControlsProperties();
            RegisterEventHandlers();
        }

        private void InitializeFlowClientWorker()
        {
            if (ClientType.Equals(Conventions.TransportType.Tcp, StringComparison.OrdinalIgnoreCase))
            {
                //_flowClientWorker = IoC.Resolve<TcpClientWorker>();
                _flowClientWorker = new TcpServerWorker(new ResponseParser());
                serverPortTextBox.Text = Conventions.TcpServerListeningPort.ToString();
            }

            if (ClientType.Equals(Conventions.TransportType.Udp, StringComparison.OrdinalIgnoreCase))
            {
                //_flowClientWorker = IoC.Resolve<UdpClientWorker>();s
                _flowClientWorker = new UdpClientWorker(new ResponseParser());
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
                _flowClientWorker.Init(
                    ipAddress: IPAddress.Parse(serverIpAddressTextBox.Text.Trim()),
                    port: int.Parse(serverPortTextBox.Text.Trim()));
            };

            authButton.Click += (sender, args) =>
            {
                _flowClientWorker.Authenticate(
                    authLoginTextBox.Text,
                    authPassTextBox.Text);
            };
        }
    }
}