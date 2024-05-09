using DjTool.Tools;
using DjTool.ViewModels;
using log4net;
using Microsoft.Win32;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace DjTool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(MainWindow));
        private TrackRenamer renamer;

        public MainWindow()
        {
            renamer = new TrackRenamer(log);
            InitializeComponent();
        }

        private void SelectFilesButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Multiselect = true;
            dialog.Title = "Select a folder";

            if (dialog.ShowDialog() == true)
            {
                var parser = new FileNameParser();
                var lists = ((TodoViewModel)this.Lists.DataContext);

                var files = dialog.FileNames.Select(file => parser.ParseFileName(file));

                lists.Add(files);
            }

        }

        private void SaveNumbersButton_Click(object sender, RoutedEventArgs e)
        {
            var lists = ((TodoViewModel)this.Lists.DataContext);

            foreach (var item in lists.CompletedTodoItemListingViewModel.TodoItemViewModels)
            {
                log.Info($"save order number [{item.Name}]");
                item.SavedOrder = item.Order;
                renamer.RenameTrack(item);

            }

            log.Info("rename track with order == null");
            foreach (var item in lists.InProgressTodoItemListingViewModel.TodoItemViewModels.Where(x => x.Order != x.SavedOrder && x.Order == null))
            {
                log.Info($"save order number [{item.Name}]");
                item.SavedOrder = item.Order;
                renamer.RenameTrack(item);
            }

            MessageBox.Show("Файлы переименованы");
        }

        private void ClearNumbersButton_Click(object sender, RoutedEventArgs e)
        {
            var lists = ((TodoViewModel)this.Lists.DataContext);

            var sortedList = lists.CompletedTodoItemListingViewModel;

            while (sortedList.TodoItemViewModels.Any())
            {
                var item = sortedList.TodoItemViewModels.First();

                log.Info($"reset order [{item.Name}]");

                item.ResetOrder();
                renamer.RenameTrack(item);

                sortedList.RemoveTodoItem(item);
                lists.InProgressTodoItemListingViewModel.AddTodoItem(item);
            }
            
            MessageBox.Show("Файлы переименованы");
        }
    }
}