using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;

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

        public void AddTodoItem(TrackViewModel item)
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

        public void InsertTodoItem(TrackViewModel insertedTodoItem, TrackViewModel targetTodoItem)
        {
            if (insertedTodoItem == targetTodoItem)
            {
                return;
            }

            int oldIndex = trackViewModels.IndexOf(insertedTodoItem);
            int nextIndex = trackViewModels.IndexOf(targetTodoItem);

            if (oldIndex != -1 && nextIndex != -1)
            {
                if (ordered)
                {
                    insertedTodoItem.SetOrder(nextIndex + 1);
                    targetTodoItem.SetOrder(oldIndex + 1);
                }

                trackViewModels.Move(oldIndex, nextIndex);
            }
        }

        public void RemoveTodoItem(TrackViewModel item)
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
    }
}
