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
        private void TaskGridView_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            if (!DateTime.TryParse(taskGridView.Rows[e.RowIndex].Cells[1].Value.ToString(), out DateTime dueDate))
            {
                return;
            }
            if(dueDate.CompareTo(DateTime.Now) < 0) {
                //TODO(#10): clean up the color scheme (its ugly)
                taskGridView.Rows[e.RowIndex].Cells[1].Style.BackColor = Color.Crimson;
                
            }
        }
        private void LoadDataTable()
        {
            try
            {
                taskGridView.DataSource = FunctionLibrary.LoadTableData();
                taskGridView.Columns[2].Visible = false;
                taskGridView.RowPrePaint 
                    += new DataGridViewRowPrePaintEventHandler(
                        TaskGridView_RowPrePaint!);
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
            taskTextBox.ReadOnly = false;
            taskTextBox.Text = "";
            taskDescriptionTextBox.Text = "";
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
