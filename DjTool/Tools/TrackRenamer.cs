using DjTool.ViewModels;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml.Linq;

namespace DjTool.Tools
{
    internal class TrackRenamer
    {
        private ILog log;

        public TrackRenamer(ILog log)
        {
            this.log = log;
        }

        public void RenameTrack(TrackViewModel track, Action<TrackViewModel>? afterRename = null)
        {
            var sourceDirectory = Path.GetDirectoryName(track.SourceFilePath);
            var newFileName = FileNameParser.FormatFileName(track.Name, track.SavedOrder, track.Speed);

            var newPath = Path.Combine(sourceDirectory, newFileName);

            var oldPath = track.SourceFilePath;

            log.Info($"rename [{oldPath}] -> [{newPath}]");
            track.SourceFilePath = newPath;
            
            File.Move(oldPath, newPath);

            afterRename?.Invoke(track);

        }

        public void SaveOrderNumber(string directory, TrackViewModel track)
        {
            var newFileName = FileNameParser.FormatFileName(track.Name, track.Order, track.Speed);

            var newPath = Path.Combine(directory, newFileName);

            if (string.IsNullOrEmpty(track.OutputFilePath))
            {
                File.Copy(track.SourceFilePath, newPath);
                log.Info($"copy track to output [{track.SourceFilePath}] -> [{newPath}]");
            }
            else
            {
                File.Move(track.OutputFilePath, newPath);
                log.Info($"copy track to output [{track.SourceFilePath}] -> [{newPath}]");
            }

            track.OutputFilePath = newPath;
            track.SavedOrder = track.Order;
        }
    }
}
