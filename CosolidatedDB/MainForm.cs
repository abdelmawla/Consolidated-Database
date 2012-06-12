using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using CosolidatedDB.Helpers;
using CosolidatedDB.Properties;
using CosolidatedDB.Steps;

namespace CosolidatedDB
{
    public partial class mainForm : Form
    {
        OpenFileDialog _fileDialog = new OpenFileDialog();
        List<BaseStep> _executionSteps = new List<BaseStep>();

        public mainForm()
        {
            InitializeComponent();
        }

        void FileDialogFileOk(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(_fileDialog.FileName)) return;

            lblSelectedFileName.Text = _fileDialog.SafeFileName;
        }

        private void BtnStartDataTransferClick(object sender, EventArgs e)
        {
            try
            {
                if (!IsFormValid()) return;

                if (!FileHelper.LoadItemsLines(_fileDialog.FileName).Any()) MessageBox.Show(Resources.EmptyConnectionStrings);

                loadingPic.Visible = true;

                Task.Factory.StartNew(ExcuteTasks).ContinueWith(TaskContinuation);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void ExcuteTasks()
        {
            CheckForIllegalCrossThreadCalls = false;

            btnStartDataTransfer.Enabled = false;

            foreach (var connectionString in FileHelper.LoadItemsLines(_fileDialog.FileName))
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    LoadExcptionalSteps(connection.Database); 
                    
                    _executionSteps = new List<BaseStep>(_executionSteps.OrderBy(step => step.StepOrder).ThenBy(step => step.StepOrder));

                    foreach (var step in _executionSteps)
                    {
                        step.Execute(connection, txtNewDatabaseName.Text);
                    }
                }
            }
        }

        private void TaskContinuation(Task continueTask)
        {
            loadingPic.Visible = false;
            btnStartDataTransfer.Enabled = true;


            if (continueTask.IsFaulted)
            {
                var exMessages = new StringBuilder();

                foreach (Exception ex in continueTask.Exception.InnerExceptions)
                {
                    exMessages.AppendLine(string.Format("Caught exception '{0}'", ex.Message));
                }

                MessageBox.Show(exMessages.ToString());

                return;
            }

            foreach (var step in _executionSteps)
            {
                if (step.Errors.Count == 0) continue;

                foreach (var error in step.Errors)
                {
                    Logger.Log(error);
                }
            }

            MessageBox.Show(Resources.OperationDoneSuccefully);
        }

        private void LoadExcptionalSteps(string databaseName)
        {

        }

        private bool IsFormValid()
        {
            if (string.IsNullOrEmpty(_fileDialog.FileName))
            {
                MessageBox.Show(Resources.NoFileSelected);
                return false;
            }

            if (string.IsNullOrEmpty(txtNewDatabaseName.Text))
            {
                MessageBox.Show(Resources.EmptyDataBaseName);
                return false;
            }

            return true;
        }

        private void BtnBrowseClick(object sender, EventArgs e)
        {
            _fileDialog.ShowDialog();
        }

        private void MainFormLoad(object sender, EventArgs e)
        {
            _fileDialog.FileOk += FileDialogFileOk;
            _fileDialog.Multiselect = false;
            loadingPic.Visible = false;

            LoadSteps();
        }

        private void LoadSteps()
        {
            _executionSteps.Add(new CreateTables(runOnce: false, stepOrder: 2));
            _executionSteps.Add(new CreateRelationShips(runOnce: true, stepOrder: 4));
            _executionSteps.Add(new CreateDestinationDB(runOnce: true, stepOrder: 1));
            _executionSteps.Add(new CreatePrimaryKeys(runOnce: true, stepOrder: 3));
            
        }

        private void LblSelectedFileNameLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (string.IsNullOrEmpty(lblSelectedFileName.Text)) return;

            Process.Start(_fileDialog.FileName);
        }
    }
}
