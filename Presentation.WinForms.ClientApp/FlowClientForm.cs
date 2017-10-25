// ReSharper disable ArgumentsStyleNamedExpression

namespace Presentation.WinForms.ClientApp
{
    using System;
    using System.Diagnostics;
    using System.Drawing;
    using System.Net;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using FlowProtocol.Interfaces.CommonConventions;
    using FlowProtocol.Interfaces.Workers.Clients;
    using Infrastructure;
    using libZPlay;
    using Properties;
    using static FlowProtocol.Interfaces.CommonConventions.Conventions;
    using Timer = System.Timers.Timer;

    public partial class FlowClientForm : Form
    {
        private IFlowClientWorker _flowClientWorker;
        private Timer _timer;
        public string ClientType { get; set; }

        #region CONSTRUCTORS

        static FlowClientForm()
        {
            IoC.RegisterAll();
        }

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
                _flowClientWorker = IoC.Resolve<IFlowTcpClientWorker>();

                serverPortTextBox.Text = TcpServerListeningPort.ToString();
                Text = @"Chat Client [ TCP is used as underlying transport protocol ]";
                return;
            }

            if (ClientType.Equals(Conventions.ClientType.Udp))
            {
                _flowClientWorker = IoC.Resolve<IFlowUdpClientWorker>();

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

            activateOfflineModeButton.FlatStyle = FlatStyle.Flat;
            activateOnlineModeButton.FlatStyle = FlatStyle.Flat;

            activateOfflineModeButton.FlatAppearance.BorderColor = Color.DeepSkyBlue;
            activateOnlineModeButton.FlatAppearance.BorderColor = Color.Gray;

            activateOfflineModeButton.FlatAppearance.BorderSize = 1;
            activateOnlineModeButton.FlatAppearance.BorderSize = 1;

            translateButton.FlatStyle = FlatStyle.Flat;
            translateButton.FlatAppearance.BorderColor = Color.MediumSeaGreen;
            translateButton.BackColor = Color.PaleGreen;

            sendMessageButton.FlatStyle = FlatStyle.Flat;
            sendMessageButton.FlatAppearance.BorderSize = 1;
            sendMessageButton.FlatAppearance.BorderColor = Color.MediumSeaGreen;
            sendMessageButton.BackColor = Color.PaleGreen;

            registerButton.FlatStyle = FlatStyle.Flat;
            registerButton.FlatAppearance.BorderColor = Color.MediumSeaGreen;
            registerButton.BackColor = Color.PaleGreen;

            authButton.FlatStyle = FlatStyle.Flat;
            authButton.FlatAppearance.BorderColor = Color.MediumSeaGreen;
            authButton.BackColor = Color.PaleGreen;

            connectToServerButton.FlatStyle = FlatStyle.Flat;
            connectToServerButton.FlatAppearance.BorderColor = Color.MediumSeaGreen;
            connectToServerButton.BackColor = Color.PaleGreen;

            registerPassTextBox.PasswordChar = '*';
            authPassTextBox.PasswordChar = '*';
        }

        private void RegisterEventHandlers()
        {
            authButton.Click += (sender, args) =>
            {
                Task.Run(() =>
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
                        MessageBox.Show(string.IsNullOrWhiteSpace(exception.Message)
                            ? "Something is wrong: check your server connection"
                            : exception.Message);
                    }
                });
            };

            connectToServerButton.Click += (sender, args) =>
            {
                Task.Run(() =>
                {
                    try
                    {
                        bool connected = _flowClientWorker.Connect(
                            IPAddress.Parse(serverIpAddressTextBox.Text.Trim()),
                            int.Parse(serverPortTextBox.Text.Trim()));

                        MessageBox.Show($@"Connected: {connected}");
                    }
                    catch (Exception exception)
                    {
                        Debug.WriteLine(exception);
                        MessageBox.Show(string.IsNullOrWhiteSpace(exception.Message)
                            ? "Something is wrong: check your server connection"
                            : exception.Message);
                    }
                });
            };

            registerButton.Click += (sender, args) =>
            {
                Task.Run(() =>
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
                        MessageBox.Show(string.IsNullOrWhiteSpace(exception.Message)
                            ? "Something is wrong: check your server connection"
                            : exception.Message);
                    }
                });
            };

            translateButton.Click += (sender, args) =>
            {
                Task.Run(() =>
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

                        if (translateOutputRichTextBox.InvokeRequired)
                        {
                            translateOutputRichTextBox.Invoke((MethodInvoker) (() =>
                            {
                                translateOutputRichTextBox.Text = translatedText;
                            }));
                        }
                        else
                        {
                            translateOutputRichTextBox.Text = translatedText;
                        }
                    }
                    catch (Exception exception)
                    {
                        Debug.WriteLine(exception);
                        MessageBox.Show(string.IsNullOrWhiteSpace(exception.Message)
                            ? "Something is wrong: check your server connection"
                            : exception.Message);
                    }
                });
            };

            sendMessageButton.Click += (sender, args) =>
            {
                Task.Run(() =>
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
                            MessageBox.Show(string.IsNullOrWhiteSpace(result.ResponseMessage)
                                ? "Recipient not found"
                                : result.ResponseMessage);
                        }
                    }
                    catch (Exception exception)
                    {
                        Debug.WriteLine(exception);
                        MessageBox.Show(string.IsNullOrWhiteSpace(exception.Message)
                            ? "Something is wrong: check your server connection"
                            : exception.Message);
                    }
                });
            };

            activateOnlineModeButton.Click += (sender, args) =>
            {
                Task.Run(() =>
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

                    ((Button) sender).FlatAppearance.BorderColor = Color.DeepSkyBlue;
                    activateOfflineModeButton.FlatAppearance.BorderColor = Color.Gray;
                });
            };

            activateOfflineModeButton.Click += (sender, args) =>
            {
                Task.Run(() =>
                {
                    if (_timer != null)
                    {
                        _timer.Stop();
                        _timer = null;
                    }

                    ((Button) sender).FlatAppearance.BorderColor = Color.DeepSkyBlue;
                    activateOnlineModeButton.FlatAppearance.BorderColor = Color.Gray;
                });
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
                    if (incomingMessagesRichTextBox.InvokeRequired)
                    {
                        incomingMessagesRichTextBox.Invoke((MethodInvoker) (() =>
                        {
                            incomingMessagesRichTextBox.AppendText(
                                Environment.NewLine + new string('-', 168) +
                                Environment.NewLine +
                                $"From {result.SenderName} [{result.SenderId}] : {result.MessageBody}");
                        }));
                    }
                    else
                    {
                        incomingMessagesRichTextBox.AppendText(
                            Environment.NewLine + new string('-', 168) +
                            Environment.NewLine +
                            $"From {result.SenderName} [{result.SenderId}] : {result.MessageBody}");
                    }

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
                        // I don't care if soundplayer is going crazy
                    }
                }
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception);
                MessageBox.Show(string.IsNullOrWhiteSpace(exception.Message)
                    ? "Something is wrong: check your server connection"
                    : exception.Message);
            }
        }
    }
}