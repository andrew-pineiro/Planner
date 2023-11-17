using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planner.Data.Models
{
    public class ReturnModel
    {
        public enum Code
        {
            OK,
            ERROR,
            CRITICAL
        };
        public Code ReturnCode { get; set; }
        public string? Message { get; set; }
    }
}
