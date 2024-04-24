using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.CustomModels
{
    public class Records
    {
        public int employeeid { get; set; }
        public string? firstname { get; set; }
        public string? lastname { get; set; }
        public string? email { get; set; }
        public string? age { get; set; }
        public int? departmentid { get; set; }
        public string? departmentname { get; set; }
        public string? education { get; set; }
        public string? company { get; set; }
        public int? experience { get; set; }
        public string? package { get; set; }
    }
}
