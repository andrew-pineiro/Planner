using Planner.Data;
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
        private void addButton_Click(object sender, EventArgs e)
        {
            TaskModel task = new TaskModel()
            {
                //TODO - implement error label notification
                Task = taskTextBox.Text ?? throw new Exception("Task is required"),
                DueDate = dueDatePicker.Value,
                TaskDescription = taskDescriptionTextBox.Text ?? string.Empty
            };

            _lib.AddDataToCsv(task);
            loadDataTable();
        }

        private void completeButton_Click(object sender, EventArgs e)
        {
            int index = taskGridView.CurrentRow.Index;
            if (index < 0)
            {
                throw new Exception("no row selected");
            }
            _lib.MarkDataComplete(index);
            loadDataTable();
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            foreach(DataGridViewRow row in taskGridView.SelectedRows)
            {
                TaskModel task = new TaskModel()
                {
                    //TODO - implement error label notification
                    Task = row.Cells[0].Value.ToString() ?? throw new Exception("error"),
                };

                _lib.DeleteDataFromCsv(task);
            }

            loadDataTable();
        }

        private void loadButton_Click(object sender, EventArgs e)
        {
            //TODO - implement load functionality
        }
    }
}