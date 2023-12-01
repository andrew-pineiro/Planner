using Planner.Data;
using Planner.Data.Models;
using System.Configuration;
using System.Data;
using static Planner.Data.Models.ReturnModel;

namespace Planner
{
    public partial class Planner : Form
    {
        public Planner()
        {
            InitializeComponent();

            // Load Data Source From CSV
            LoadDataTable();
        }

        private void TaskTextBox_Enter(object sender, EventArgs e)
        {
            if (taskTextBox.Text == "Task Subject")
            {
                taskTextBox.Text = string.Empty;
                taskTextBox.ForeColor = Color.Black;
            }
        }
        private void TaskTextBox_Leave(object sender, EventArgs e)
        {
            if (taskTextBox.Text == string.Empty)
            {
                taskTextBox.Text = "Task Subject";
                taskTextBox.ForeColor = Color.Gray;
            }
        }
        private void TaskDescriptionTextBox_Enter(object sender, EventArgs e)
        {
            if (taskDescriptionTextBox.Text == "Description")
            {
                taskDescriptionTextBox.Text = string.Empty;
                taskDescriptionTextBox.ForeColor = Color.Black;
            }
        }
        private void TaskDescriptionTextBox_Leave(object sender, EventArgs e)
        {
            if (taskDescriptionTextBox.Text == string.Empty)
            {
                taskDescriptionTextBox.Text = "Description";
                taskDescriptionTextBox.ForeColor = Color.Gray;
            }
        }
        private void LoadDataTable()
        {
            try
            {
                taskGridView.DataSource = FunctionLibrary.LoadTableData();
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
        private void ResetTextBoxes()
        {
            taskDescriptionTextBox.Text = "Description";
            taskDescriptionTextBox.ForeColor = Color.Gray;
            taskTextBox.Text = "Task Subject";
            taskTextBox.ForeColor = Color.Gray;
            taskTextBox.ReadOnly = false;
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

            RemoveErrorMessage();
            ResetTextBoxes();
            LoadDataTable();
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
            RemoveErrorMessage();
            LoadDataTable();
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
            RemoveErrorMessage();
            ResetTextBoxes();
            LoadDataTable();
        }

        private void TaskGridView_Click(object sender, EventArgs e)
        {
            if (taskGridView.SelectedRows.Count < 1)
            {
                return;
            }

            DataGridViewRow row = taskGridView.SelectedRows[0];

            taskTextBox.Text = row.Cells[0].Value.ToString() ?? string.Empty;
            taskTextBox.ForeColor = Color.Black;
            taskTextBox.ReadOnly = true;

            if (!DateTime.TryParse(row.Cells[1].Value.ToString() ?? string.Empty, out DateTime dateVal))
            {
                return;
            }
            dueDatePicker.Value = dateVal;

            taskDescriptionTextBox.Text = row.Cells[2].Value.ToString() ?? string.Empty;
            taskDescriptionTextBox.ForeColor = Color.Black;
            taskDescriptionTextBox.Focus();
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            ResetTextBoxes();
            dueDatePicker.Value = DateTime.Now;
        }

        private void Planner_FormClosing(object sender, FormClosingEventArgs e)
        {
            FunctionLibrary.BackupCSVFile();
        }
    }
}