using DjTool.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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

        public TodoItemListingView()
        {
            InitializeComponent();
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

                DragDropEffects dragDropResult = DragDrop.DoDragDrop(frameworkElement, new DataObject(DataFormats.Serializable, item), DragDropEffects.Move);

                if (dragDropResult == DragDropEffects.None)
                {
                    AddTodoItem(CurrentItem);
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

                    Insert((TodoItemViewModel)insertedItem, TargetItem);
//                    TargetItem = null;
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

            AddTodoItem(todoItem);

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
            // Search immediate children first (breadth-first)
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
    }

}
