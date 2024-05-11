using Planner.Data;
using Planner.Data.Models;
using static Planner.Data.Models.ReturnModel;

namespace Planner.UI
{
    public partial class Planner : Form
    {
        public Planner()
        {
            InitializeComponent();
            LoadDataTable();
        }

        private void LoadDataTable()
        {
            try
            {
                //TODO(#5): Add cell coloring for tasks that are past due
                taskGridView.DataSource = FunctionLibrary.LoadTableData();
                taskGridView.Columns[2].Visible = false;
                taskGridView.Refresh();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }
        private void UpdateErrorMessage(string message)
        {
            errorLabel.Text = message;
        }
        private void RemoveErrorMessage()
        {
            errorLabel.Text = string.Empty;
        }
        private void ResetUI()
        {
            //TODO(#6): Cleanup ReadOnly code to get rid of ResetTextBoxes()()
            taskTextBox.ReadOnly = false;
            dueDatePicker.Value = DateTime.Now.Date;
            RemoveErrorMessage();
            LoadDataTable();
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            TaskModel task = new()
            {
                Task = taskTextBox.Text ?? string.Empty,
                DueDate = dueDatePicker.Value,
                TaskDescription = taskDescriptionTextBox.Text != "Description"
                                    ? taskDescriptionTextBox.Text : string.Empty
            };

            var returnVal = FunctionLibrary.SaveDataToCsv(task, taskTextBox.ReadOnly);

            if (returnVal.ReturnCode == Code.ERROR)
            {
                UpdateErrorMessage(returnVal.Message!);
                return;
            }
            else if (returnVal.ReturnCode == Code.CRITICAL)
            {
                throw new Exception(returnVal.Message);
            }

            ResetUI();
        }

        private void CompleteButton_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in taskGridView.SelectedRows)
            {
                if (!DateTime.TryParse(row.Cells[1].Value.ToString(), out DateTime dueDate))
                {
                    return;
                }
                TaskModel task = new()
                {
                    Task = row.Cells[0].Value.ToString()!,
                    DueDate = dueDate,
                    TaskDescription = row.Cells[2].Value.ToString() ?? string.Empty,
                };


                var returnVal = FunctionLibrary.MarkDataComplete(task);
                if (returnVal.ReturnCode == Code.ERROR)
                {
                    UpdateErrorMessage(returnVal.Message!);
                    return;
                }
                else if (returnVal.ReturnCode == Code.CRITICAL)
                {
                    throw new Exception(returnVal.Message);
                }

            }
            ResetUI();
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in taskGridView.SelectedRows)
            {
                TaskModel task = new()
                {
                    Task = row.Cells[0].Value.ToString()!
                };

                var returnVal = FunctionLibrary.DeleteDataFromCsv(task);
                if (returnVal.ReturnCode == Code.ERROR)
                {
                    UpdateErrorMessage(returnVal.Message!);
                    return;
                }
                else if (returnVal.ReturnCode == Code.CRITICAL)
                {
                    throw new Exception(returnVal.Message);
                }
            }
            ResetUI();
        }

        private void TaskGridView_Click(object sender, EventArgs e)
        {
            if (taskGridView.SelectedRows.Count < 1)
            {
                return;
            }

            DataGridViewRow row = taskGridView.SelectedRows[0];

            taskTextBox.Text = row.Cells[0].Value.ToString() ?? string.Empty;
            taskTextBox.ReadOnly = true;

            if (!DateTime.TryParse(row.Cells[1].Value.ToString() ?? string.Empty, out DateTime dateVal))
            {
                return;
            }
            dueDatePicker.Value = dateVal.Date;

            taskDescriptionTextBox.Text = row.Cells[2].Value.ToString() ?? string.Empty;
            taskDescriptionTextBox.Focus();
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            ResetUI();
        }

        private void Planner_FormClosing(object sender, FormClosingEventArgs e)
        {
            FunctionLibrary.BackupCSVFile();
        }
    }
}
