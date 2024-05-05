using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;

namespace DjTool.ViewModels
{
    public class TodoListiViewModel //: ViewModelBase
    {
        private readonly ObservableCollection<TodoItemViewModel> _todoItemViewModels;
        private readonly bool ordered;

        public IEnumerable<TodoItemViewModel> TodoItemViewModels => _todoItemViewModels;

        public TodoListiViewModel(bool ordered)
        {
            _todoItemViewModels = new ObservableCollection<TodoItemViewModel>();
            this.ordered = ordered;
        }

        public void AddTodoItem(TodoItemViewModel item)
        {
            if(!_todoItemViewModels.Contains(item))
            {
                _todoItemViewModels.Add(item);
                if (ordered)
                    item.SetOrder(_todoItemViewModels.Count);
                else
                    item.ClearOrder();
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
                if (ordered)
                {
                    insertedTodoItem.SetOrder(nextIndex + 1);
                    targetTodoItem.SetOrder(oldIndex + 1);
                }

                _todoItemViewModels.Move(oldIndex, nextIndex);            
            }
        }

        public void RemoveTodoItem(TodoItemViewModel item)
        {
            _todoItemViewModels.Remove(item);
            if (ordered)
            {
                for (var i = 0; i < _todoItemViewModels.Count; i++)
                {
                    _todoItemViewModels[i].SetOrder(i + 1);
                }
            }
        }
    }
}
