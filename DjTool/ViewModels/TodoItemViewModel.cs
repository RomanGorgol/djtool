using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace DjTool.ViewModels
{
    public class TodoItemViewModel : INotifyPropertyChanged
    {
        public string Name { get; private set; }

        public string FilePath { get; set; }

        public int? Order { get; private set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public string Description { get; set; }


        public TodoItemViewModel(string name, string filePath, int? order)
        {
            this.Name = name;
            FilePath = filePath;
            
            SetOrder(order);
        }

        public void SetOrder(int? order)
        {
            Order = order;
            Description = $"{(order.HasValue ? order.Value + " " : "")} {Name}";
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Description"));
        }

        public MoveResult AddOrderToPath()
        {
            var directory = System.IO.Path.GetDirectoryName(FilePath);
            var newFileName = $"{Order:00}. {Name}";
            var newPath = System.IO.Path.Combine(directory, newFileName);

            var oldPath = FilePath;
            FilePath = newPath;
            return new MoveResult(newPath, oldPath);
        }

        public MoveResult ResetOrder()
        {
            var directory = System.IO.Path.GetDirectoryName(FilePath);
            var newFileName = $"{Name}";
            var newPath = System.IO.Path.Combine(directory, newFileName);

            Order = null;
            var oldPath = FilePath;
            FilePath = newPath;
            return new MoveResult(newPath, oldPath);
        }
    }

    public  class MoveResult
    {
        public string OldPath { get; private set; }
        public string NewPath { get; private set; }

        public MoveResult(string newPath, string oldPath)
        {
            NewPath = newPath;
            OldPath = oldPath;
        }
    }
}
