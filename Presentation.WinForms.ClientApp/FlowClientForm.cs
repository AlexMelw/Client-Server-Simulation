using static FlowProtocol.Interfaces.CommonConventions.Conventions;

namespace Presentation.WinForms.ClientApp
{
    using System;
    using System.Diagnostics;
    using System.Drawing;
    using System.Net;
    using System.Windows.Forms;
    using FlowProtocol.Implementation.Response;
    using FlowProtocol.Implementation.Workers.Clients;
    using FlowProtocol.Interfaces.CommonConventions;
    using FlowProtocol.Interfaces.Workers;
    using libZPlay;
    using Properties;
    using Timer = System.Timers.Timer;

    public partial class FlowClientForm : Form
    {
        private IFlowClientWorker _flowClientWorker;
        private Timer _timer;
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
                Text = @"Chat Client [ TCP is used as underlying transport protocol ]";
                return;
            }

            if (ClientType.Equals(Conventions.ClientType.Udp))
            {
                //_flowClientWorker = IoC.Resolve<UdpClientWorker>();

                _flowClientWorker = new UdpClientWorker(new ResponseParser());
                serverPortTextBox.Text = UdpServerListeningPort.ToString();
                Text = @"Chat Client [ UDP is used as underlying transport protocol ]";
            }
        }

        private void ConfigControlsProperties()
        {
            Icon = Resources.Email;

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
            translateOutputRichTextBox.ReadOnly = true;
            translateOutputRichTextBox.BackColor = Color.White;

            incomingMessagesRichTextBox.ReadOnly = true;
            incomingMessagesRichTextBox.BackColor = Color.White;
            incomingMessagesRichTextBox.BorderStyle = BorderStyle.None;

            outgoingMessagesRichTextBox.BorderStyle = BorderStyle.None;
        }

        private void RegisterEventHandlers()
        {
            // TODO Run Asynchronously + use MethodInvoker delegate

            connectToServerButton.Click += (sender, args) =>
            {
                try
                {
                    bool connected = _flowClientWorker.Connect(
                        ipAddress: IPAddress.Parse(serverIpAddressTextBox.Text.Trim()),
                        port: int.Parse(serverPortTextBox.Text.Trim()));

                    MessageBox.Show($@"Connected: {connected}");
                }
                catch (Exception exception)
                {
                    Debug.WriteLine(exception);
                    MessageBox.Show($@"{exception.Message}");
                }
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
                if (string.IsNullOrWhiteSpace(translateInputRichTextBox.Text))
                {
                    return;
                }
                try
                {
                    string inputTextLang = fromLangComboBox.Text;
                    string outputTextLang = toLangComboBox.Text;

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
                if (string.IsNullOrWhiteSpace(outgoingMessagesRichTextBox.Text))
                {
                    return;
                }

                string recipient = recipientTextBox.Text.Trim();
                string messageBody = outgoingMessagesRichTextBox.Text;
                string sourceTextLanguage = outgoingLangComboBox.Text;

                try
                {
                    var result = _flowClientWorker.SendMessage(
                        recipient: recipient,
                        messageText: messageBody,
                        messageTextLang: sourceTextLanguage);

                    if (result.Success)
                    {
                        try
                        {
                            ZPlay player = new ZPlay();

                            if (player.OpenFile(@"Resources/Sent2.mp3", TStreamFormat.sfAutodetect))
                            {
                                player.SetMasterVolume(100, 100);
                                player.SetPlayerVolume(100, 100);

                                player.StartPlayback();
                            }
                        }
                        catch (Exception)
                        {
                            // I don't care if soundplayer is dgoing crazy
                        }
                        //MessageBox.Show($@"{result.ResponseMessage}");
                        outgoingMessagesRichTextBox.Clear();
                    }
                    else
                    {
                        MessageBox.Show($@"{result.ResponseMessage}");
                    }
                }
                catch (Exception exception)
                {
                    Debug.WriteLine(exception);
                    MessageBox.Show($@"{exception.Message}");
                }
            };

            activateOnlineModeButton.Click += (sender, args) =>
            {
                if (_timer != null)
                {
                    return;
                }
                _timer = new Timer
                {
                    AutoReset = true,
                    Interval = 3000
                };

                _timer.Elapsed += CheckMailbox;
                _timer.Start();
            };

            activateOfflineModeButton.Click += (sender, args) =>
            {
                if (_timer != null)
                {
                    _timer.Stop();
                    _timer = null;
                }
            };
        }

        private void CheckMailbox(object sender, EventArgs args)
        {
            string incomingMessagesTranslationMode = incomingLangComboBox.Text;

            try
            {
                var result = _flowClientWorker.GetMessage(incomingMessagesTranslationMode);

                if (result.Success)
                {
                    incomingMessagesRichTextBox.AppendText(
                        Environment.NewLine + new string('-', 168) +
                        Environment.NewLine +
                        $"From {result.SenderName} [{result.SenderId}] : {result.MessageBody}");

                    try
                    {
                        ZPlay player = new ZPlay();

                        if (player.OpenFile(@"Resources/OwOw.mp3", TStreamFormat.sfAutodetect))
                        {
                            player.SetMasterVolume(100, 100);
                            player.SetPlayerVolume(100, 100);

                            player.StartPlayback();
                        }
                    }
                    catch (Exception)
                    {
                        // I don't care if soundplayer is dgoing crazy
                    }
                }
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception);
                MessageBox.Show($@"{exception.Message}");
            }
        }
    }
}