﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;

namespace DjTool.ViewModels
{
    public class TodoListiViewModel //: ViewModelBase
    {
        private readonly ObservableCollection<TodoItemViewModel> _todoItemViewModels;

        public IEnumerable<TodoItemViewModel> TodoItemViewModels => _todoItemViewModels;

        public TodoListiViewModel()
        {
            _todoItemViewModels = new ObservableCollection<TodoItemViewModel>();

        }

        public void AddTodoItem(TodoItemViewModel item)
        {
            if(!_todoItemViewModels.Contains(item))
            {
                _todoItemViewModels.Add(item);
                item.SetOrder(_todoItemViewModels.Count);
            }
        }

        public void InsertTodoItem(TodoItemViewModel insertedTodoItem, TodoItemViewModel targetTodoItem)
        {
            if(insertedTodoItem == targetTodoItem)
            {
                return;
            }

            int oldIndex = _todoItemViewModels.IndexOf(insertedTodoItem);
            int nextIndex = _todoItemViewModels.IndexOf(targetTodoItem);

            if(oldIndex != -1 && nextIndex != -1)   
            {
                //                insertedTodoItem.SetOrder(_todoItemViewModels.IndexOf(insertedTodoItem) + 1);
                //              targetTodoItem.SetOrder(_todoItemViewModels.IndexOf(targetTodoItem) + 1);

                insertedTodoItem.SetOrder(nextIndex + 1);
                targetTodoItem.SetOrder(oldIndex + 1);

                _todoItemViewModels.Move(oldIndex, nextIndex);            
            }
        }

        public void RemoveTodoItem(TodoItemViewModel item)
        {
            _todoItemViewModels.Remove(item);
        }
    }
}
