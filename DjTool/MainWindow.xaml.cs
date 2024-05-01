using DjTool.ViewModels;
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
        private Regex regex = new Regex(@"^(?<order>\d+).\s+(?<name>.+)$");

        public MainWindow()
        {
            InitializeComponent();
        }

        private void SelectFilesButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Multiselect = true;
            dialog.Title = "Select a folder";

            if (dialog.ShowDialog() == true)
            {

                var lists = ((TodoViewModel)this.Lists.DataContext);

                var files = dialog.FileNames;

                foreach (var file in files)
                {
                    var item = Parse(file);
                    if (item.Order.HasValue)
                        lists.CompletedTodoItemListingViewModel.AddTodoItem(item);
                    else
                        lists.InProgressTodoItemListingViewModel.AddTodoItem(item);
                }
            }

        }

        private TodoItemViewModel Parse(string filepath)
        {
            var filename = System.IO.Path.GetFileName(filepath);

            var match = regex.Match(filename);
            if (match.Success)
            {
                var order = match.Groups["order"].Success
                    ? (int?)int.Parse(match.Groups["order"].Value)
                    : null;

                var name = match.Groups["name"].Value;
                return new TodoItemViewModel(name, filepath, order);
            }
            else
            {
                return new TodoItemViewModel(filename, filepath, null);
            }
        }

        private void SaveNumbersButton_Click(object sender, RoutedEventArgs e)
        {
            var lists = ((TodoViewModel)this.Lists.DataContext);

            try
            {

                foreach (var item in lists.CompletedTodoItemListingViewModel.TodoItemViewModels)
                {
                    var result = item.AddOrderToPath();

                    File.Move(result.OldPath, result.NewPath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            MessageBox.Show("Файлы переименованы");
        }

        private void ClearNumbersButton_Click(object sender, RoutedEventArgs e)
        {
            var lists = ((TodoViewModel)this.Lists.DataContext);

            try
            {
                var sortedList = lists.CompletedTodoItemListingViewModel;

                while(sortedList.TodoItemViewModels.Any())
                {
                    var item = sortedList.TodoItemViewModels.First();
                    var result = item.ResetOrder();
                    File.Move(result.OldPath, result.NewPath);

                    sortedList.RemoveTodoItem(item);
                    lists.InProgressTodoItemListingViewModel.AddTodoItem(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            MessageBox.Show("Файлы переименованы");
        }
    }
}