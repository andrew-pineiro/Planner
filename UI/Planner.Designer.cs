﻿namespace Planner.UI
{
    partial class Planner
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Planner));
            taskGridView = new DataGridView();
            completeButton = new Button();
            deleteButton = new Button();
            clearButton = new Button();
            saveButton = new Button();
            taskTextBox = new TextBox();
            taskDescriptionTextBox = new TextBox();
            dueDatePicker = new DateTimePicker();
            errorLabel = new Label();
            ((System.ComponentModel.ISupportInitialize)taskGridView).BeginInit();
            SuspendLayout();
            // 
            // taskGridView
            // 
            taskGridView.AllowUserToAddRows = false;
            taskGridView.AllowUserToDeleteRows = false;
            taskGridView.AllowUserToResizeRows = false;
            taskGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            taskGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            taskGridView.EditMode = DataGridViewEditMode.EditProgrammatically;
            taskGridView.Location = new Point(12, 12);
            taskGridView.Name = "taskGridView";
            taskGridView.ReadOnly = true;
            taskGridView.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            taskGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            taskGridView.ShowEditingIcon = false;
            taskGridView.Size = new Size(481, 246);
            taskGridView.TabIndex = 0;
            taskGridView.Click += TaskGridView_Click;
            // 
            // completeButton
            // 
            completeButton.Location = new Point(510, 21);
            completeButton.Name = "completeButton";
            completeButton.Size = new Size(278, 52);
            completeButton.TabIndex = 1;
            completeButton.Text = "Complete Task";
            completeButton.UseVisualStyleBackColor = true;
            completeButton.Click += CompleteButton_Click;
            // 
            // deleteButton
            // 
            deleteButton.Location = new Point(510, 206);
            deleteButton.Name = "deleteButton";
            deleteButton.Size = new Size(278, 23);
            deleteButton.TabIndex = 1;
            deleteButton.Text = "Delete";
            deleteButton.UseVisualStyleBackColor = true;
            deleteButton.Click += DeleteButton_Click;
            // 
            // clearButton
            // 
            clearButton.Location = new Point(510, 235);
            clearButton.Name = "clearButton";
            clearButton.Size = new Size(278, 23);
            clearButton.TabIndex = 1;
            clearButton.Text = "Clear";
            clearButton.UseVisualStyleBackColor = true;
            clearButton.Click += ClearButton_Click;
            // 
            // saveButton
            // 
            saveButton.Location = new Point(510, 79);
            saveButton.Name = "saveButton";
            saveButton.Size = new Size(278, 54);
            saveButton.TabIndex = 1;
            saveButton.Text = "Save";
            saveButton.UseVisualStyleBackColor = true;
            saveButton.Click += SaveButton_Click;
            // 
            // taskTextBox
            // 
            taskTextBox.ForeColor = Color.Black;
            taskTextBox.Location = new Point(12, 281);
            taskTextBox.Name = "taskTextBox";
            taskTextBox.PlaceholderText = "Task Subject";
            taskTextBox.Size = new Size(481, 23);
            taskTextBox.TabIndex = 2;
            // 
            // taskDescriptionTextBox
            // 
            taskDescriptionTextBox.ForeColor = Color.Black;
            taskDescriptionTextBox.Location = new Point(12, 310);
            taskDescriptionTextBox.Multiline = true;
            taskDescriptionTextBox.Name = "taskDescriptionTextBox";
            taskDescriptionTextBox.PlaceholderText = "Description";
            taskDescriptionTextBox.Size = new Size(481, 122);
            taskDescriptionTextBox.TabIndex = 3;
            // 
            // dueDatePicker
            // 
            dueDatePicker.CustomFormat = "MM/dd/yyyy";
            dueDatePicker.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            dueDatePicker.Format = DateTimePickerFormat.Custom;
            dueDatePicker.Location = new Point(510, 281);
            dueDatePicker.Name = "dueDatePicker";
            dueDatePicker.Size = new Size(278, 29);
            dueDatePicker.TabIndex = 5;
            dueDatePicker.Value = new DateTime(2024, 4, 16, 0, 0, 0, 0);
            // 
            // errorLabel
            // 
            errorLabel.AutoSize = true;
            errorLabel.ForeColor = Color.Red;
            errorLabel.Location = new Point(571, 417);
            errorLabel.Name = "errorLabel";
            errorLabel.Size = new Size(0, 15);
            errorLabel.TabIndex = 6;
            errorLabel.UseMnemonic = false;
            // 
            // Planner
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(errorLabel);
            Controls.Add(dueDatePicker);
            Controls.Add(taskDescriptionTextBox);
            Controls.Add(taskTextBox);
            Controls.Add(saveButton);
            Controls.Add(clearButton);
            Controls.Add(deleteButton);
            Controls.Add(completeButton);
            Controls.Add(taskGridView);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Planner";
            Text = "Planner";
            FormClosing += Planner_FormClosing;
            ((System.ComponentModel.ISupportInitialize)taskGridView).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView taskGridView;
        private Button completeButton;
        private Button deleteButton;
        private Button clearButton;
        private Button saveButton;
        private TextBox taskTextBox;
        private TextBox taskDescriptionTextBox;
        private DateTimePicker dueDatePicker;
        private Label errorLabel;
    }
}