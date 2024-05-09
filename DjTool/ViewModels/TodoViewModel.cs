using log4net;
using System;
using System.Collections.Generic;
using System.Text;

namespace DjTool.ViewModels
{
    public class TodoViewModel
    {
        private readonly ILog log;

        public TodoListiViewModel InProgressTodoItemListingViewModel { get; }
        public TodoListiViewModel CompletedTodoItemListingViewModel { get; }

        public TodoViewModel(ILog log, TodoListiViewModel inProgressTodoItemListingViewModel, TodoListiViewModel completedTodoItemListingViewModel)
        {
            this.log = log;
            InProgressTodoItemListingViewModel = inProgressTodoItemListingViewModel;
            CompletedTodoItemListingViewModel = completedTodoItemListingViewModel;
        }

        public void Add(IEnumerable<TodoItemViewModel> items)
        {
            var itemsWithOrder = new List<TodoItemViewModel>();

            foreach (var item in items)
            {
                log.Info($"add track[{item.Name}] [{item.Speed}] [{item.SavedOrder}] [{item.FilePath}]");

                if (!item.Order.HasValue)
                    InProgressTodoItemListingViewModel.AddTodoItem(item);
                else
                    itemsWithOrder.Add(item);
            }

            foreach (var item in itemsWithOrder.OrderBy(x => x.Order.Value))
            {
                CompletedTodoItemListingViewModel.AddTodoItem(item);
            }
        }
    }
}
