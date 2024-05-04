using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DjTool.ViewModels
{
    internal class FileNameParser
    {
        private Regex regex = new Regex(@"^(?<order>\d+).\s+(?<name>.+)$");

        public FileNameParser() { }

        public TodoItemViewModel ParseFileName(string filepath)
        {
            var filename = System.IO.Path.GetFileName(filepath);

            var match = regex.Match(filename);
            if (match.Success)
            {
                var order = match.Groups["order"].Success
                    ? (int?)int.Parse(match.Groups["order"].Value)
                    : null;

                var name = match.Groups["name"].Value;
                return new TodoItemViewModel(name, filepath, order);
            }
            else
            {
                return new TodoItemViewModel(filename, filepath, null);
            }
        }

    }
}
