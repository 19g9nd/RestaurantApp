using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurauntApp.Services.Classes
{
    public class LogEntry
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Url { get; set; }
        public string Method { get; set; }
        public int StatusCode { get; set; }
        public string RequestBody { get; set; }
        public string ResponseBody { get; set; }
        public DateTime Timestamp { get; set; }
    }
}