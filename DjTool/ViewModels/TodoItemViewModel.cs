using DjTool.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Controls;

namespace DjTool.ViewModels
{
    public class TodoItemViewModel : INotifyPropertyChanged
    {
        public string Name { get; private set; }

        public string FilePath { get; set; }

        public int? SavedOrder { get; private set; }

        public int? Order { get; private set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public string Description => $"{(Order.HasValue ? Order.Value + " " : "")} {Name}";
        public TrackSpeed Speed { get; set; }


        public TodoItemViewModel(string name, string filePath, int? order)
        {
            this.Name = name;
            FilePath = filePath;

            SavedOrder = order;
            SetOrder(order);
        }

        public void ClearOrder() 
        {
            SetOrder(null);
        }

        public void SetOrder(int? order)
        {
            Order = order;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Description"));
        }

        public void SetName(string name)
        {
            Name = name;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Description"));
        }

        public MoveResult AddOrderToPath()
        {
            var directory = System.IO.Path.GetDirectoryName(FilePath);
            var newFileName = FileNameParser.FormatFileName(Name, Order, Speed);
            var newPath = System.IO.Path.Combine(directory, newFileName);

            var oldPath = FilePath;
            FilePath = newPath;
            SavedOrder = Order;
            return new MoveResult(newPath, oldPath);
        }

        public MoveResult ResetOrder()
        {
            Order = null;
            SavedOrder = null;
            var directory = System.IO.Path.GetDirectoryName(FilePath);
            var newFileName = FileNameParser.FormatFileName(Name, Order, Speed);
            
            var newPath = System.IO.Path.Combine(directory, newFileName);

            var oldPath = FilePath;
            FilePath = newPath;
            return new MoveResult(newPath, oldPath);
        }

        public MoveResult Rename(string newFileName)
        {
            var oldPath = FilePath;

            var directory = System.IO.Path.GetDirectoryName(FilePath);
            var newPath = System.IO.Path.Combine(directory, newFileName);

            FilePath = newPath;

            return new MoveResult(newPath, oldPath);

        }

        public TrackSpeedSelectItem[] TrackSpeedItems
        {
            get
            {
                return new TrackSpeedSelectItem[]
                {
                    new TrackSpeedSelectItem { Value = TrackSpeed.S, Description = "Slow" },
                    new TrackSpeedSelectItem { Value = TrackSpeed.SM, Description = "S-M" },
                    new TrackSpeedSelectItem { Value = TrackSpeed.M, Description = "Middle" },
                    new TrackSpeedSelectItem { Value = TrackSpeed.MF, Description = "M-F" },
                    new TrackSpeedSelectItem { Value = TrackSpeed.F, Description = "Fast" }
                };
            }
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

    public enum TrackSpeed
    {
        None = 0,
        S,
        SM,
        M,
        MF,
        F
    }
}
