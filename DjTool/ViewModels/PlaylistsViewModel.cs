using log4net;
using System;
using System.Collections.Generic;
using System.Text;

namespace DjTool.ViewModels
{
    public class PlaylistsViewModel
    {
        private readonly ILog log;

        public TrackListViewModel InProgressTrackListViewModel { get; }
        public TrackListViewModel CompletedTrackListViewModel { get; }

        public PlaylistsViewModel(ILog log, TrackListViewModel inProgressTrackListViewModel, TrackListViewModel completedTrackListViewModel)
        {
            this.log = log;
            InProgressTrackListViewModel = inProgressTrackListViewModel;
            CompletedTrackListViewModel = completedTrackListViewModel;
        }

        public void Add(IEnumerable<TrackViewModel> items)
        {
            var itemsWithOrder = new List<TrackViewModel>();

            foreach (var item in items)
            {
                log.Info($"add track[{item.Name}] [{item.Speed}] [{item.SavedOrder}] [{item.SourceFilePath}]");

                if (!item.Order.HasValue)
                    InProgressTrackListViewModel.AddTodoItem(item);
                else
                    itemsWithOrder.Add(item);
            }

            foreach (var item in itemsWithOrder.OrderBy(x => x.Order.Value))
            {
                CompletedTrackListViewModel.AddTodoItem(item);
            }
        }
    }
}
