using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
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

            var connectionStrings = FileHelper.LoadItemsLines(_fileDialog.FileName);

            foreach (var connectionString in connectionStrings)
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    LoadExcptionalSteps(connection.Database); 
                    
                    _executionSteps = new List<BaseStep>(_executionSteps.OrderBy(step => step.StepOrder).ThenBy(step => step.StepOrder));

                    foreach (var step in _executionSteps)
                        step.Execute(connection, txtNewDatabaseName.Text);
                }
            }

            PartitionDatabase(txtNewDatabaseName.Text, connectionStrings);
        }

        private void PartitionDatabase(string newDatabaseName, IEnumerable<string> connectionStrings)
        {
            var governrateIds = GetGovernerateIds(connectionStrings);
        }

        private static List<string> GetGovernerateIds(IEnumerable<string> connectionStrings)
        {
            var governrateIds = new List<string>();

            foreach (var connectionString in connectionStrings)
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    if (connection.State == ConnectionState.Closed) connection.Open();

                    var retriveGovernratedIdCommand = connection.CreateCommand();

                    string commandText = "SELECT [PARAM_VALUE] FROM [{0}].[dbo].[AUDIT_SETTINGS] where [PARAM_NAME] = 'SITE_ID'";

                    commandText = string.Format(commandText, connection.Database);

                    retriveGovernratedIdCommand.CommandText = commandText;

                    object governrateId = retriveGovernratedIdCommand.ExecuteScalar();

                    if (governrateId == null)
                        throw new Exception(string.Format("{0} doesn't have valid governerate Id", connection.Database));

                    governrateIds.Add(governrateId.ToString());
                }
            }

            return governrateIds;
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
            string currentDirectory = Directory.GetCurrentDirectory();

            string databaseDirectory = string.Format("{0}\\Scripts\\{1}", currentDirectory, databaseName);

            if (!Directory.Exists(databaseDirectory)) return;

            string tablesScriptFilePath = string.Format("{0}\\{1}", databaseDirectory, "Tables.txt");

            if (File.Exists(tablesScriptFilePath))
            {
                Stream tablesFileStream =  File.OpenRead(tablesScriptFilePath);
                _executionSteps.Add(new CreateTables(true, 2, StepType.Second, tablesFileStream ));
            }

            string primaryKeysScriptFilePath = string.Format("{0}\\{1}", databaseDirectory, "PrimaryKeys.txt");

            if (File.Exists(primaryKeysScriptFilePath))
            {
                Stream primarykeysFileStream = File.OpenRead(primaryKeysScriptFilePath);
                _executionSteps.Add(new CreatePrimaryKeys(true, 3, StepType.Second, primarykeysFileStream));
            }
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
            _executionSteps.Add(new CreateDestinationDB(runOnce: true, stepOrder: 1, type: StepType.First, sourceStream: null));
            _executionSteps.Add(new CreateTables(runOnce: false, stepOrder: 2, type: StepType.First, sourceStream: null));
            _executionSteps.Add(new CreatePrimaryKeys(runOnce: true, stepOrder: 3, type: StepType.First, sourceStream: null));
            _executionSteps.Add(new CreateRelationShips(runOnce: true, stepOrder: 4, type: StepType.First, sourceStream: null));
        }

        private void LblSelectedFileNameLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (string.IsNullOrEmpty(lblSelectedFileName.Text)) return;

            Process.Start(_fileDialog.FileName);
        }
    }
}
