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
        public required Code ReturnCode { get; set; }
        public string? Message { get; set; }
    }
}
