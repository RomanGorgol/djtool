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

        public int? SavedOrder { get; set; }

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

        public void ResetOrder()
        {
            Order = null;
            SavedOrder = null;
        }

    }

    public class MoveResult
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
