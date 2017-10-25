namespace Presentation.WinForms.ClientApp
{
    using System;
    using System.Windows.Forms;
    using Properties;

    public partial class IntroForm : Form
    {
        #region CONSTRUCTORS

        public IntroForm() => InitializeComponent();

        #endregion

        private void OnLoadIntroForm(object sender, EventArgs e)
        {
            ConfigControlsProperties();
            RegisterEventHendlers();
        }

        private void ConfigControlsProperties()
        {
            Text = @"Client initializer";
            Icon = Resources.Email;

            pickupProtocolComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            pickupProtocolComboBox.SelectedIndex = 0;
        }

        private void RegisterEventHendlers()
        {
            runClientButton.Click += (sender, args) =>
            {
                Hide();

                string selectedValue = pickupProtocolComboBox.Text;

                FlowClientForm flowClientForm = new FlowClientForm
                {
                    ClientType = selectedValue
                };

                flowClientForm.Closed += (o, eventArgs) => Close();

                flowClientForm.Show();
            };
        }
    }
}