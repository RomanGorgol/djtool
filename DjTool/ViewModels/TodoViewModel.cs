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

        public void Add(TodoItemViewModel item)
        {
            if (item.Order.HasValue)
                CompletedTodoItemListingViewModel.AddTodoItem(item);
            else
                InProgressTodoItemListingViewModel.AddTodoItem(item);

        }
    }
}
