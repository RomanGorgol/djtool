using DjTool.Tools;
using DjTool.Tools.Backup;
using DjTool.ViewModels;
using log4net;
using log4net.Appender;
using log4net.Config;
using System.Configuration;
using System.Data;
using System.IO;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
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
        private PlaylistsViewModel playlistsViewModel;

        protected override void OnStartup(StartupEventArgs e)
        {


            XmlConfigurator.Configure(new FileInfo("logconfig.xml"));

            Application.Current.DispatcherUnhandledException += Current_DispatcherUnhandledException;

            inProgressTrackListViewModel = new TrackListViewModel(false);
            completedTrackListViewModel = new TrackListViewModel(true);

            playlistsViewModel = new PlaylistsViewModel(log, inProgressTrackListViewModel, completedTrackListViewModel);


            MainWindow = new MainWindow()
            {
                DataContext = playlistsViewModel
            };
            MainWindow.Show();

            log.Info("start");

            this.MainWindow.Closing += MainWindow_Closing;

            this.MainWindow.ContentRendered += MainWindow_ContentRendered;

            base.OnStartup(e);

        }

        private void MainWindow_ContentRendered(object? sender, EventArgs e)
        {
            //var parser = new FileNameParser();
            //todoViewModel.Add(Directory.GetFiles(@"C:\Data\Тестовая папка с музыкой").Select(x => parser.ParseFileName(x)).ToArray());
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
            MessageBox.Show("Упс! Что-то пошло не так. Отправь разработчику файл с логами, и он посмотрит, что случилось " + LogFilePathProvider.GetLogFilepath(),
                "Ошибка", MessageBoxButton.OK,
                MessageBoxImage.Error
                );
            log.Error(e.Exception);

            var backupData = new BackupData()
            {
                InProgressTrackListViewModel = playlistsViewModel.InProgressTrackListViewModel.TrackViewModels.ToList(),
                CompletedTrackListViewModel = playlistsViewModel.CompletedTrackListViewModel.TrackViewModels.ToList(),
            };

            var serialized = JsonSerializer.Serialize(backupData, options: new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
                WriteIndented = true
            });

            log.Info($"dump [{serialized}]");

            e.Handled = true;
        }
    }

}