using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentsMS
{
    public static class AppSettings
    {

        public static string DataBasePrefix { get; set; } = "chenli_";
        public static string TablePrefix { get; set; } = "chenl_";
        public static string PropertyPrefix { get; set; } = "cl_";
        public static string Suffix { get; set; } = "01";
        public static string SQLConnectString { get; set; } = "uid=sa;pwd=XXXXXXX;Database=chenliMIS01;Server=XXXXXX";
    
    }
}
