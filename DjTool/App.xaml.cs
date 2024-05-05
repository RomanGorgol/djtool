using DjTool.ViewModels;
using System.Configuration;
using System.Data;
using System.IO;
using System.Windows;

namespace DjTool
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {

            Application.Current.DispatcherUnhandledException += Current_DispatcherUnhandledException;

            var inProgressTodoItemListingViewModel = new TodoListiViewModel(false);
            var completedTodoItemListingViewModel = new TodoListiViewModel(true);

            TodoViewModel todoViewModel = new TodoViewModel(inProgressTodoItemListingViewModel, completedTodoItemListingViewModel);


            MainWindow = new MainWindow()
            {
                DataContext = todoViewModel
            };
            MainWindow.Show();

            base.OnStartup(e);

        }

        private void Current_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show("error");
            throw new NotImplementedException();
        }
    }

}
