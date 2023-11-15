using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planner.Data.Models
{
    public class ReturnModel
    {
        // Code Breakdown
        // 0 - OK
        // 1 - Error
        // 2 - Critical

        public required int Code { get; set; }
        public string? Message { get; set; }
    }
}
