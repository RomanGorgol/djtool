using DjTool.ViewModels;
using log4net;
using log4net.Appender;
using log4net.Config;
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
        private static readonly ILog log = LogManager.GetLogger(typeof(App));

        protected override void OnStartup(StartupEventArgs e)
        {

           
            XmlConfigurator.Configure(new FileInfo("logconfig.xml"));

            Application.Current.DispatcherUnhandledException += Current_DispatcherUnhandledException;

            var inProgressTodoItemListingViewModel = new TodoListiViewModel(false);
            var completedTodoItemListingViewModel = new TodoListiViewModel(true);

            TodoViewModel todoViewModel = new TodoViewModel(inProgressTodoItemListingViewModel, completedTodoItemListingViewModel);


            MainWindow = new MainWindow()
            {
                DataContext = todoViewModel
            };
            MainWindow.Show();

            log.Info("start");

            base.OnStartup(e);

        }

        private void Current_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            var logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logfile.log");
            MessageBox.Show("Упс! Что-то пошло не так. Отправь разработчику файл с логами, и он посмотрит, что случилось "+ logPath, 
                "Ошибка", MessageBoxButton.OK, 
                MessageBoxImage.Error
                );
            log.Error(e.Exception);
            throw e.Exception;
        }
    }

}
