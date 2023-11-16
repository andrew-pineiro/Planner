using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Planner.Data.Models;

namespace Planner.Data
{
    public class FunctionLibrary
    {
        string filePath =>
            Environment.ExpandEnvironmentVariables(ConfigurationManager.AppSettings["dataFile"] ?? throw new Exception("Unable to find dataFile in App.Config"));
        public DataTable LoadTableData()
        {
            DataTable table = new DataTable();

            string folderPath = filePath.Substring(0, filePath.LastIndexOf('\\'));
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            // check if file exists
            if (!File.Exists(filePath))
            {

                File.WriteAllLines(
                    filePath, 
                    new string[] { "Task,Due Date,Task Description,Completed" }
                    );
            }

            // populate data table
            using (StreamReader reader = new StreamReader(filePath))
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
                        row[i] = rows[i];
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
        public ReturnModel SaveDataToCsv(TaskModel task, bool saveTask)
        {

            var lines = File.ReadAllLines(filePath);
            var newLines = new List<string>();
            var output = $"{task.Task},{task.DueDate},{task.TaskDescription},0";

            foreach (var line in lines)
            {
                if(line.StartsWith(task.Task) && !saveTask)
                {
                    return new ReturnModel() { Code = 1, Message = "Duplicate task name" };
                }

                if(!line.StartsWith(task.Task))
                {
                    newLines.Add(line);
                }
                
            }
            newLines.Add(output);
            File.WriteAllLines(filePath, newLines);
            return new ReturnModel() { Code = 0 };
        }
        public ReturnModel DeleteDataFromCsv(TaskModel task)
        {
            var newLines = new List<string>();
            var lines = File.ReadAllLines(filePath);
            foreach (var line in lines)
            {
                if (!line.Contains(task.Task))
                {
                    newLines.Add(line);
                }
            }
            File.WriteAllLines(filePath, newLines);
            return new ReturnModel() { Code = 0 };
        }
        public ReturnModel MarkDataComplete(TaskModel task) 
        {
            var newLines = new List<string>();
            var lines = File.ReadAllLines(filePath);
            var output = $"{task.Task},{task.DueDate},{task.TaskDescription},1";

            foreach (var line in lines)
            {
                if(!line.StartsWith(task.Task)) {
                    newLines.Add(line);
                }
            }
            newLines.Add(output);
            File.WriteAllLines(filePath, newLines);
            return new ReturnModel() { Code = 0 };
        }

    }
}
