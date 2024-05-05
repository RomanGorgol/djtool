using System;
using System.Collections.Generic;
using System.Text;

namespace DjTool.ViewModels
{
    public class TodoViewModel
    {
        public TodoListiViewModel InProgressTodoItemListingViewModel { get; }
        public TodoListiViewModel CompletedTodoItemListingViewModel { get; }

        public TodoViewModel(TodoListiViewModel inProgressTodoItemListingViewModel, TodoListiViewModel completedTodoItemListingViewModel)
        {
            InProgressTodoItemListingViewModel = inProgressTodoItemListingViewModel;
            CompletedTodoItemListingViewModel = completedTodoItemListingViewModel;
        }

        public void Add(IEnumerable<TodoItemViewModel> items)
        {
            var itemsWithOrder = new List<TodoItemViewModel>();

            foreach (var item in items)
            {
                if (!item.Order.HasValue)
                    InProgressTodoItemListingViewModel.AddTodoItem(item);
                else
                    itemsWithOrder.Add(item);
            }

            foreach(var item in itemsWithOrder.OrderBy(x=>x.Order.Value))
            {
                CompletedTodoItemListingViewModel.AddTodoItem(item);
            }
        }
    }
}
