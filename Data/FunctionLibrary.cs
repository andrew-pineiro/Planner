using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Planner.Data.Models;
using static Planner.Data.Models.ReturnModel;

namespace Planner.Data
{
    public static class FunctionLibrary
    {
        private static string FilePath =>
            Environment.ExpandEnvironmentVariables(
                ConfigurationManager.AppSettings["dataFile"] 
                    ?? throw new Exception("Unable to find dataFile in App.Config")
                );
        public static DataTable LoadTableData()
        {
            DataTable table = new();

            string folderPath = FilePath[..FilePath.LastIndexOf('\\')];
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            // check if file exists
            if (!File.Exists(FilePath))
            {

                File.WriteAllLines(
                    FilePath, 
                    new string[] { "Task,Due Date,Task Description,Completed" }
                    );
            }

            // populate data table
            using (StreamReader reader = new(FilePath))
            {
                var headers = reader.ReadLine()!.Split(',');

                if (headers[0] != "Task")
                {
                    throw new Exception("Unexpected headers in CSV.");
                }

                foreach (var header in headers)
                {
                    table.Columns.Add(header);
                }
                while (!reader.EndOfStream)
                {
                    var rows = reader.ReadLine()!.Split(',');
                    DataRow row = table.NewRow();
                    for (int i = 0; i < headers.Length; i++)
                    {
                        
                        row[i] = rows[i].Replace("**",Environment.NewLine);
                    }
                    if (row["Completed"].ToString() == "0")
                    {
                        table.Rows.Add(row);
                    }

                }
                table.Columns.Remove("Completed");
                table.DefaultView.Sort = $"{headers[1]} asc";
            }

            return table;
        }
        public static ReturnModel SaveDataToCsv(TaskModel task, bool saveTask)
        {
            char[] invalidChars = { '@', '\\', '/', ',' };
            var newLines = new List<string>();

            // Read in all lines
            var lines = File.ReadAllLines(FilePath);

            // Check for blank task name
            if (string.IsNullOrEmpty(task.Task) || task.Task == "Task Subject")
            {
                return new ReturnModel() { 
                    ReturnCode = Code.ERROR, 
                    Message = "Task name is required" 
                };
            }

            // Replace commas with semi-colons due to data being comma seperated
            task.Task = task.Task.Replace(',', ';');

            // Check for new line characters and replace with ** if present
            // Also replaces commas with semi-colons due to data being comma seperated, if present.
            if (!string.IsNullOrEmpty(task.TaskDescription))
            {
                task.TaskDescription = task.TaskDescription.Replace(Environment.NewLine, "**").Replace(',',';');
            }

            // Check for any invalid characters present in task subject or description
            if ((!string.IsNullOrEmpty(task.TaskDescription) 
                    && task.TaskDescription.IndexOfAny(invalidChars) > -1) 
                        || task.Task.IndexOfAny(invalidChars) > -1)
            {
                return new ReturnModel() { 
                    ReturnCode = Code.ERROR, 
                    Message = "Invalid character present." 
                };
            }

            // Iterate through each lines in file
            foreach (var line in lines)
            {
                // Checks for duplicate task name, only if this is a new task being added
                if(line.StartsWith(task.Task) && !saveTask)
                {
                    return new ReturnModel() { 
                        ReturnCode = Code.ERROR, 
                        Message = "Duplicate task name" 
                    };
                }

                // Adds all other lines besides effected line to list
                if(!line.StartsWith(task.Task))
                {
                    newLines.Add(line);
                }
                
            }

            // Adds new/updated line to list and writes to file
            newLines.Add($"{task.Task.Replace(',',';')},{task.DueDate},{task.TaskDescription},0");
            File.WriteAllLines(FilePath, newLines);


            return new ReturnModel() { ReturnCode = Code.OK };
        }
        public static ReturnModel DeleteDataFromCsv(TaskModel task)
        {
            var newLines = new List<string>();
            var lines = File.ReadAllLines(FilePath);
            foreach (var line in lines)
            {
                if (!line.Contains(task.Task))
                {
                    newLines.Add(line);
                }
            }
            File.WriteAllLines(FilePath, newLines);
            return new ReturnModel() { ReturnCode = Code.OK };
        }
        public static ReturnModel MarkDataComplete(TaskModel task) 
        {
            var newLines = new List<string>();
            var lines = File.ReadAllLines(FilePath);

            if (!string.IsNullOrEmpty(task.TaskDescription))
            {
                task.TaskDescription = task.TaskDescription.Replace(Environment.NewLine, "**");
            }

            foreach (var line in lines)
            {
                if(!line.StartsWith(task.Task)) {
                    newLines.Add(line);
                }
            }

            newLines.Add($"{task.Task},{task.DueDate},{task.TaskDescription},1");
            File.WriteAllLines(FilePath, newLines);

            return new ReturnModel() { ReturnCode = Code.OK };
        }
        public static void BackupCSVFile()
        {
            string currDate = DateTime.Now.Date.ToString("yyyyMMdd");
            string backupDirectory = $"{FilePath[..FilePath.LastIndexOf('\\')]}\\backup";
            string newFileName = $"{backupDirectory}\\data_{currDate}.csv";

            var data = File.ReadAllLines(FilePath);

            if (!Directory.Exists(backupDirectory))
            {
                Directory.CreateDirectory(backupDirectory);
            }

            if (File.Exists(newFileName))
            {
                File.Delete(newFileName);
            }

            File.WriteAllLines(newFileName, data);
            CleanupCSVBackups(backupDirectory);
        }
        
        public static void CleanupCSVBackups(string backupDirectory)
        {
            foreach (var file in Directory.GetFiles(backupDirectory))
            {
                // Purge backup files older than 6 months
                if (File.GetCreationTime(file) < DateTime.Now.AddMonths(-6))
                {
                    File.Delete(file);
                }
            }
        }

    }
}
