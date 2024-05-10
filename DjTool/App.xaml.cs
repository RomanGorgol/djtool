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
        private TrackListViewModel inProgressTrackListViewModel;
        private TrackListViewModel completedTrackListViewModel;

        protected override void OnStartup(StartupEventArgs e)
        {


            XmlConfigurator.Configure(new FileInfo("logconfig.xml"));

            Application.Current.DispatcherUnhandledException += Current_DispatcherUnhandledException;

            inProgressTrackListViewModel = new TrackListViewModel(false);
            completedTrackListViewModel = new TrackListViewModel(true);

            PlaylistsViewModel todoViewModel = new PlaylistsViewModel(log, inProgressTrackListViewModel, completedTrackListViewModel);


            MainWindow = new MainWindow()
            {
                DataContext = todoViewModel
            };
            MainWindow.Show();

            log.Info("start");

            this.MainWindow.Closing += MainWindow_Closing;
                         

            base.OnStartup(e);

        }

        private void MainWindow_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            if (inProgressTrackListViewModel.TrackViewModels.Any(x=>x.Order!= x.SavedOrder)
                || completedTrackListViewModel.TrackViewModels.Any(x=>x.Order != x.SavedOrder))
            {
                var result = MessageBox.Show("Есть треки, у которых изменился порядковый номер, но номера не сохранены. Точно закрыть без сохранения?", "Есть несохраненные изменения", MessageBoxButton.YesNo);

                if (result == MessageBoxResult.No)
                {   
                    e.Cancel = true;
                }
            }

        }


        private void Current_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            var logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logfile.log");
            MessageBox.Show("Упс! Что-то пошло не так. Отправь разработчику файл с логами, и он посмотрит, что случилось " + logPath,
                "Ошибка", MessageBoxButton.OK,
                MessageBoxImage.Error
                );
            log.Error(e.Exception);
            throw e.Exception;
        }
    }

}
