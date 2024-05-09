using DjTool.Tools;
using DjTool.ViewModels;
using log4net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DjTool.Views
{
    /// <summary>
    /// Interaction logic for TodoItemListingView.xaml
    /// </summary>
    public partial class TodoItemListingView : UserControl
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(TodoItemListingView));
        private TrackRenamer renamer;


        public TodoItemListingView()
        {
            InitializeComponent();
            renamer = new TrackRenamer(log);
        }

        private TodoItemViewModel CurrentItem = null;
        private TodoItemViewModel TargetItem = null;

        private void TodoItem_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed &&
                sender is FrameworkElement frameworkElement)
            {
                DataObject data = new DataObject();

                var item = this.lvItems.SelectedItem;
                CurrentItem = (TodoItemViewModel)item;

                if (CurrentItem != null)
                {
                    DragDropEffects dragDropResult = DragDrop.DoDragDrop(frameworkElement, new DataObject(DataFormats.Serializable, item), DragDropEffects.Move);

                    if (dragDropResult == DragDropEffects.None)
                    {
                        AddTodoItem(CurrentItem);
                        CurrentItem = null;
                    }
                }

            }
        }


        private void TodoItem_DragOver(object sender, DragEventArgs e)
        {
            if (sender is FrameworkElement element)
            {
                if (element.DataContext is TodoItemViewModel item)
                {
                    TargetItem = item;
                    var insertedItem = e.Data.GetData(DataFormats.Serializable);

                    if (insertedItem != null)
                        Insert((TodoItemViewModel)insertedItem, TargetItem);
                    TargetItem = null;
                }
            }
        }

        private void Insert(TodoItemViewModel current, TodoItemViewModel target)
        {
            var list = (TodoListiViewModel)this.lvItems.DataContext;


            list.InsertTodoItem(current, target);
        }

        private void TodoItemList_DragOver(object sender, DragEventArgs e)
        {
            var todoItem = (TodoItemViewModel)e.Data.GetData(DataFormats.Serializable);

            if (todoItem != null)
            {
                AddTodoItem(todoItem);
            }

            CheckScroll(sender, e);
        }

        private void AddTodoItem(TodoItemViewModel todoItem)
        {
            (this.lvItems.DataContext as TodoListiViewModel).AddTodoItem(todoItem);
        }

        private void TodoItemList_DragLeave(object sender, DragEventArgs e)
        {
            HitTestResult result = VisualTreeHelper.HitTest(lvItems, e.GetPosition(lvItems));

            if (result == null)
            {
                var todoItem = (TodoItemViewModel)e.Data.GetData(DataFormats.Serializable);

                if (todoItem != null)
                    (this.lvItems.DataContext as TodoListiViewModel).RemoveTodoItem(todoItem);

            }

        }

        private void CheckScroll(object sender, System.Windows.DragEventArgs e)
        {
            ListBox li = sender as ListView;
            ScrollViewer sv = FindVisualChild<ScrollViewer>(this.lvItems);

            double tolerance = 20;
            double verticalPos = e.GetPosition(li).Y;
            double offset = 3;

            if (verticalPos < tolerance)
            {
                ScrollUp(sv);
                //               sv.ScrollToVerticalOffset(sv.VerticalOffset - offset); //Scroll up.
            }
            else if (verticalPos > li.ActualHeight - tolerance - 20) //Bottom of visible list?
            {
                ScrollDown(sv);
                //                sv.ScrollToVerticalOffset(sv.VerticalOffset + offset); //Scroll down.    
            }
        }

        private DateTime lastScrollDown = DateTime.MinValue;
        private void ScrollDown(ScrollViewer sv)
        {
            if (DateTime.Now - lastScrollDown < TimeSpan.FromMilliseconds(250))
                return;

            sv.LineDown();
            lastScrollDown = DateTime.Now;

        }

        private DateTime lastScrollUp = DateTime.MinValue;
        private void ScrollUp(ScrollViewer sv)
        {
            if (DateTime.Now - lastScrollUp < TimeSpan.FromMilliseconds(250))
                return;

            sv.LineUp();
            lastScrollUp = DateTime.Now;

        }

        public static childItem FindVisualChild<childItem>(DependencyObject obj) where childItem : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);

                if (child != null && child is childItem)
                    return (childItem)child;

                else
                {
                    childItem childOfChild = FindVisualChild<childItem>(child);

                    if (childOfChild != null)
                        return childOfChild;
                }
            }

            return null;
        }

        private void ComboBox_MouseMove(object sender, MouseEventArgs e)
        {
            e.Handled = true;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = (ComboBox)sender;

            if (comboBox != null)
            {
                var selected = (TrackSpeedSelectItem)comboBox.SelectedItem;

                var item = comboBox.DataContext as TodoItemViewModel;

                log.Info($"change speed [{item.Name}] [{item.Speed}]");

                renamer.RenameTrack(item);
            }
        }

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && e.ClickCount == 2)
            {
                TextBlock? textBlock = sender as TextBlock;
                DockPanel? dockPanel = textBlock.Parent as DockPanel;

                (dockPanel.FindName("TextBox") as TextBox).Visibility = Visibility.Visible;
                textBlock.Visibility = Visibility.Collapsed;
            }

        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                var textBox = sender as TextBox;

                DockPanel? dockPanel = textBox.Parent as DockPanel;
                textBox.Visibility = Visibility.Collapsed;
                (dockPanel.FindName("TextBlock") as TextBlock).Visibility = Visibility.Visible;


                var item = textBox.DataContext as TodoItemViewModel;

                log.Info($"change name [{item.Name}] -> [{textBox.Text}]");
                item.SetName(textBox.Text);

                renamer.RenameTrack(item);
            }
        }
    }

}
