using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planner.Data
{
    public class TaskModel
    {
        public required string Task { get; set; }
        public DateTime DueDate { get; set; }
        public string? TaskDescription  { get; set; }
    }
}
