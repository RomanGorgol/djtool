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
        private Regex regex = new Regex(@"^(?<order>\d+).?\s*(?<speed>(S|(SM)|M|(MF)|F) )?\s*(?<name>.+)$");

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

                var item = new TodoItemViewModel(name, filepath, order);

                if (match.Groups["speed"].Success && Enum.TryParse<TrackSpeed>(match.Groups["speed"].Value.Trim(), out var speed))
                {
                    item.Speed = speed;
                }

                return item;
            }
            else
            {
                return new TodoItemViewModel(filename, filepath, null);
            }
        }

        public static string FormatFileName(string trackName, int? order, TrackSpeed? speed)
        {
            return $"{(order.HasValue ? $"{order:00}. " : "") + (speed != TrackSpeed.None ? speed.ToString() + " " : "")+trackName}";
        }
    }
}
