using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace DjTool.ViewModels
{
    public class TrackListViewModel
    {
        private readonly ObservableCollection<TrackViewModel> trackViewModels;
        private readonly bool ordered;

        public IEnumerable<TrackViewModel> TrackViewModels => trackViewModels;

        public TrackListViewModel(bool ordered)
        {
            trackViewModels = new ObservableCollection<TrackViewModel>();
            this.ordered = ordered;
        }

        public void AddTrack(TrackViewModel item)
        {
            if (!trackViewModels.Contains(item))
            {
                trackViewModels.Add(item);
                if (ordered)
                    item.SetOrder(trackViewModels.Count);
                else
                    item.ClearOrder();
            }
        }

        public void InsertTrackItem(TrackViewModel insertedTrackItem, TrackViewModel targetTrackItem)
        {
            if (insertedTrackItem == targetTrackItem)
            {
                return;
            }

            int oldIndex = trackViewModels.IndexOf(insertedTrackItem);
            int nextIndex = trackViewModels.IndexOf(targetTrackItem);

            if (oldIndex != -1 && nextIndex != -1)
            {
                trackViewModels.Move(oldIndex, nextIndex);
                if (ordered)
                {
                    insertedTrackItem.SetOrder(nextIndex + 1);
                    targetTrackItem.SetOrder(oldIndex + 1);

                    if (Math.Abs(nextIndex - oldIndex) > 1)
                    {
                        for (var i = Math.Min(nextIndex, oldIndex); i <= Math.Max(nextIndex, oldIndex); i++)
                        {
                            trackViewModels[i].SetOrder(i + 1);
                        }
                    }
                }
            }
        }

        public void RemoveTrack(TrackViewModel item)
        {
            trackViewModels.Remove(item);
            if (ordered)
            {
                for (var i = 0; i < trackViewModels.Count; i++)
                {
                    trackViewModels[i].SetOrder(i + 1);
                }
            }
        }

        public void RemoveDuplicates()
        {
            var hashset = new HashSet<string>();

            var i = 0;
            while (i < trackViewModels.Count)
            {
                var item = trackViewModels[i];

                if (hashset.Contains(item.FilePath))
                {
                    RemoveTrack(item);
                }
                else
                {
                    i++;
                    hashset.Add(item.FilePath);
                }
            }
        }

        public override string? ToString()
        {
            return JsonSerializer.Serialize(TrackViewModels, options: new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
                WriteIndented = true
            });
        }
    }
}
