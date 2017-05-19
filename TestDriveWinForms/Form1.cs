using System;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;

namespace TestDriveWinForms
{
    public partial class Form1 : Form
    {
        private readonly BackgroundWorker _backgroundWorker;

        #region CONSTRUCTORS

        public Form1()
        {
            InitializeComponent();
            _backgroundWorker = new BackgroundWorker
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true
            };
        }

        #endregion

        private void startAsyncButton_Click(object sender, EventArgs e)
        {
            if (!_backgroundWorker.IsBusy)
            {
                // Start the asynchronous operation.
                _backgroundWorker.RunWorkerAsync();
            }
        }

        private void cancelAsyncButton_Click(object sender, EventArgs e)
        {
            if (_backgroundWorker.WorkerSupportsCancellation)
            {
                // Cancel the asynchronous operation.
                _backgroundWorker.CancelAsync();
            }
        }

        // This event handler is where the time-consuming work is done.
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = (BackgroundWorker) sender;

            for (int i = 1; i <= 10; i++)
            {
                if (worker.CancellationPending)
                {
                    e.Cancel = true;
                    break;
                }
                // Perform a time consuming operation and report progress.
                Thread.Sleep(500);
                worker.ReportProgress(i * 10);
            }
        }

        // This event handler updates the progress.
        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            resultLabel.Text = e.ProgressPercentage + "%";
        }

        // This event handler deals with the results of the background operation.
        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                resultLabel.Text = "Canceled!";
            }
            else if (e.Error != null)
            {
                resultLabel.Text = "Error: " + e.Error.Message;
            }
            else
            {
                resultLabel.Text = "Done!";
            }
        }
    }
}