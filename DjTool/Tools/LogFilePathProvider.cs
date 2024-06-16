using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DjTool.Tools
{
    internal class LogFilePathProvider
    {
        public static string GetLogFilepath()
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logfile.log");

        }
    }
}
