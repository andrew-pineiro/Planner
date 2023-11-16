using Planner.Data;
using Planner.Data.Models;
using System.Configuration;
using System.Data;

namespace Planner
{
    public partial class Planner : Form
    {
        private FunctionLibrary _lib;
        public Planner()
        {
            InitializeComponent();

            FunctionLibrary lib = new();
            _lib = lib;

            // Load Data Source From CSV
            loadDataTable();
        }

        private void taskTextBox_Enter(object sender, EventArgs e)
        {
            if (taskTextBox.Text == "Task Subject")
            {
                taskTextBox.Text = string.Empty;
                taskTextBox.ForeColor = Color.Black;
            }
        }
        private void taskTextBox_Leave(object sender, EventArgs e)
        {
            if (taskTextBox.Text == string.Empty)
            {
                taskTextBox.Text = "Task Subject";
                taskTextBox.ForeColor = Color.Gray;
            }
        }
        private void taskDescriptionTextBox_Enter(object sender, EventArgs e)
        {
            if (taskDescriptionTextBox.Text == "Description")
            {
                taskDescriptionTextBox.Text = string.Empty;
                taskDescriptionTextBox.ForeColor = Color.Black;
            }
        }

        private void taskDescriptionTextBox_Leave(object sender, EventArgs e)
        {
            if (taskDescriptionTextBox.Text == string.Empty)
            {
                taskDescriptionTextBox.Text = "Description";
                taskDescriptionTextBox.ForeColor = Color.Gray;
            }
        }
        private void loadDataTable()
        {
            taskGridView.DataSource = _lib.LoadTableData();
            taskGridView.Refresh();
        }
        private void updateErrorMessage(string message)
        {
            errorLabel.Text = message;
        }
        private void removeErrorMessage()
        {
            errorLabel.Text = string.Empty;
        }

        private void resetTextBoxes()
        {
            taskDescriptionTextBox.Text = "Description";
            taskDescriptionTextBox.ForeColor = Color.Gray;
            taskTextBox.Text = "Task Subject";
            taskTextBox.ForeColor = Color.Gray;
            taskTextBox.ReadOnly = false;
        }
        private void saveButton_Click(object sender, EventArgs e)
        {
            TaskModel task = new TaskModel()
            {
                Task = taskTextBox.Text ?? string.Empty,
                DueDate = dueDatePicker.Value,
                TaskDescription = taskDescriptionTextBox.Text ?? string.Empty
            };
            if (string.IsNullOrEmpty(task.Task) || task.Task == "Task Subject")
            {
                updateErrorMessage("Task name is required");
                return;
            }

            var returnVal = _lib.SaveDataToCsv(task, taskTextBox.ReadOnly);

            if (returnVal.Code == 1)
            {
                updateErrorMessage(returnVal.Message!);
                return;
            }
            else if (returnVal.Code == 2)
            {
                throw new Exception(returnVal.Message);
            }

            removeErrorMessage();
            resetTextBoxes();
            loadDataTable();
        }

        private void completeButton_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in taskGridView.SelectedRows)
            {
                DateTime.TryParse(row.Cells[1].Value.ToString(), out DateTime dueDate);
                TaskModel task = new TaskModel()
                {
                    Task = row.Cells[0].Value.ToString()!,
                    DueDate = dueDate,
                    TaskDescription = row.Cells[2].Value.ToString() ?? string.Empty,
                };


                var returnVal = _lib.MarkDataComplete(task);
                if (returnVal.Code == 1)
                {
                    updateErrorMessage(returnVal.Message!);
                    return;
                }
                else if (returnVal.Code == 2)
                {
                    throw new Exception(returnVal.Message);
                }

            }
            removeErrorMessage();
            loadDataTable();
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in taskGridView.SelectedRows)
            {
                TaskModel task = new TaskModel()
                {
                    Task = row.Cells[0].Value.ToString()!
                };

                var returnVal = _lib.DeleteDataFromCsv(task);
                if (returnVal.Code == 1)
                {
                    updateErrorMessage(returnVal.Message!);
                    return;
                }
                else if (returnVal.Code == 2)
                {
                    throw new Exception(returnVal.Message);
                }
            }
            removeErrorMessage();
            resetTextBoxes();
            loadDataTable();
        }

        private void loadButton_Click(object sender, EventArgs e)
        {
            if (taskGridView.SelectedRows.Count < 1)
            {
                return;
            }

            DataGridViewRow row = taskGridView.SelectedRows[0];

            taskTextBox.Text = row.Cells[0].Value.ToString() ?? string.Empty;
            taskTextBox.ForeColor = Color.Black;
            taskTextBox.ReadOnly = true;

            DateTime.TryParse(row.Cells[1].Value.ToString() ?? string.Empty, out DateTime dateVal);
            dueDatePicker.Value = dateVal;

            taskDescriptionTextBox.Text = row.Cells[2].Value.ToString() ?? string.Empty;
            taskDescriptionTextBox.ForeColor = Color.Black;
            taskDescriptionTextBox.Focus();
        }
    }
}