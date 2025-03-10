﻿using DjTool.Tools;
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
using MessageBox = System.Windows.MessageBox;

namespace DjTool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(MainWindow));
        private TrackRenamer renamer;
        private string outputDirectory;

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
                var lists = ((PlaylistsViewModel)this.Lists.DataContext);

                var files = dialog.FileNames.Select(file => parser.ParseFileName(file));

                lists.Add(files);
            }

        }

        private void SaveNumbersButton_Click(object sender, RoutedEventArgs e)
        {
            var lists = ((PlaylistsViewModel)this.Lists.DataContext);


            foreach (var item in lists.CompletedTrackListViewModel.TrackViewModels)
            {
                try
                {
                    log.Info($"save order number [{item.Name}]");
                    item.SavedOrder = item.Order;
                    renamer.RenameTrack(item);
                    if (!string.IsNullOrEmpty(outputDirectory))
                    {
                        try
                        {
                            var fileName = System.IO.Path.GetFileName(item.FilePath);
                            var outputFilePath = System.IO.Path.Combine(outputDirectory, fileName);
                            File.Copy(item.FilePath, outputFilePath, false);

                        }catch (Exception ex)
                        {
                            log.Error($"error while copiyng track to output [{item.Name}] [{item.Order}] [{item.Speed}] [{item.FilePath}]", ex);

                            MessageBox.Show($"Упс! Что-то пошло не так и получилось сохранить в папку с результатами [{item.Name}]." +
                                $"Но ничего страшного, это не помешает остальным трекам." +
                                $"Будет не лишним скинуть разработчику файл с логами {{LogFilePathProvider.GetLogFilepath()}}");
                        }
                    }
                }
                catch(Exception ex)
                {
                    log.Error($"error while saving number for track [{item.Name}] [{item.Order}] [{item.Speed}] [{item.FilePath}]", ex);

                    MessageBox.Show($"Упс! Что-то пошло не так и получилось сохранить номер для трека [{item.Name}]." +
                        $"Но ничего страшного, это не помешает остальным трекам. А этот нужно будет еще раз пронумеровать." +
                        $"Будет не лишним скинуть разработчику файл с логами {LogFilePathProvider.GetLogFilepath()}");
                }


            }

            log.Info("rename track with order == null");
            foreach (var item in lists.InProgressTrackListViewModel.TrackViewModels.Where(x => x.Order != x.SavedOrder && x.Order == null))
            {
                log.Info($"save order number [{item.Name}]");
                item.SavedOrder = item.Order;
                renamer.RenameTrack(item);
            }

            MessageBox.Show("Файлы переименованы");
        }

        private void ClearNumbersButton_Click(object sender, RoutedEventArgs e)
        {
            var lists = ((PlaylistsViewModel)this.Lists.DataContext);

            var sortedList = lists.CompletedTrackListViewModel;

            while (sortedList.TrackViewModels.Any())
            {
                var item = sortedList.TrackViewModels.First();

                log.Info($"reset order [{item.Name}]");

                item.ResetOrder();
                renamer.RenameTrack(item, true);

                sortedList.RemoveTrack(item);
                lists.InProgressTrackListViewModel.AddTrack(item);
            }

            lists.InProgressTrackListViewModel.RemoveDuplicates();
            
            MessageBox.Show("Файлы переименованы");
        }

        private void SelectOutputFolderButton_Click(object sender, RoutedEventArgs e)
        {
            var selectFolderDialog = new Microsoft.Win32.OpenFolderDialog();
            var result = selectFolderDialog.ShowDialog();

            if (result == true)
            {
                outputDirectory = selectFolderDialog.FolderName;
                this.OutputFolder.Text = outputDirectory;
            }
        }


    }
}