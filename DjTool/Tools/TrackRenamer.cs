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
            var directory = Path.GetDirectoryName(track.FilePath);
            var newFileName = FileNameParser.FormatFileName(track.Name, track.SavedOrder, track.Speed);

            var newPath = Path.Combine(directory, newFileName);

            var oldPath = track.FilePath;

            log.Info($"rename [{oldPath}] -> [{newPath}]");
            track.FilePath = newPath;
            
            File.Move(oldPath, newPath);

            afterRename?.Invoke(track);

        }
    }
}
