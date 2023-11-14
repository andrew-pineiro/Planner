using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planner.Data
{
    public class FunctionLibrary
    {
        private readonly string filePath =
            ConfigurationManager.AppSettings["dataFile"] ?? throw new Exception("Unable to find dataFile in App.Config");
        public DataTable LoadTableData()
        {
            DataTable table = new DataTable();
            if (!File.Exists(filePath))
            {
                throw new Exception("Data File does not exist");
            }
            using (StreamReader reader = new StreamReader(filePath))
            {
                var headers = reader.ReadLine()!.Split(',');
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
                    table.Rows.Add(row);
                }
            }

            return table;
        }
        public void AddDataToCsv(TaskModel task)
        {
            if (!File.Exists(filePath))
            {
                throw new Exception("Data File does not exist");
            }

            var lines = File.ReadAllLines(filePath);
            var newLines = new List<string>();
            var output = $"{task.Task},{task.DueDate},{task.TaskDescription}";

            foreach (var line in lines)
            {
                newLines.Add(line);
                if (line.Contains(task.Task)) 
                {
                    //TODO - implement error label notification
                    throw new Exception("duplicate task subject not allowed");
                }
            }

            newLines.Add(output);
            File.WriteAllLines(filePath, newLines);

        }
        public void DeleteDataFromCsv(TaskModel task)
        {
            if (!File.Exists(filePath))
            {
                throw new Exception("Data File does not exist");
            }
            var newLines = new List<string>();
            var lines = File.ReadAllLines(filePath);
            foreach(var line in lines)
            {
                if(!line.Contains(task.Task))
                {
                    newLines.Add(line);
                }
            }
            File.WriteAllLines(filePath, newLines);
        }
        public void MarkDataComplete(int rowIndex) 
        {
        
        }

    }
}
