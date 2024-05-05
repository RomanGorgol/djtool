using DjTool.ViewModels;
using System.Configuration;
using System.Data;
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
            TodoListiViewModel inProgressTodoItemListingViewModel = new TodoListiViewModel(false);

            for (int i = 0; i < 10; i++)
            {
          //      inProgressTodoItemListingViewModel.AddTodoItem(new TodoItemViewModel("item "+i));

            }
            //inProgressTodoItemListingViewModel.AddTodoItem(new TodoItemViewModel("Go jogging"));
            //inProgressTodoItemListingViewModel.AddTodoItem(new TodoItemViewModel("Walk the dog"));
            //inProgressTodoItemListingViewModel.AddTodoItem(new TodoItemViewModel("Make videos"));

            TodoListiViewModel completedTodoItemListingViewModel = new TodoListiViewModel(true);
//            completedTodoItemListingViewModel.AddTodoItem(new TodoItemViewModel("Take a shower"));
 //           completedTodoItemListingViewModel.AddTodoItem(new TodoItemViewModel("Eat breakfast"));

            TodoViewModel todoViewModel = new TodoViewModel(inProgressTodoItemListingViewModel, completedTodoItemListingViewModel);

            MainWindow = new MainWindow()
            {
                DataContext = todoViewModel
            };
            MainWindow.Show();

            base.OnStartup(e);

        }
    }

}
