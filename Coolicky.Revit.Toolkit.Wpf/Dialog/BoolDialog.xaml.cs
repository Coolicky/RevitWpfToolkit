using System.Windows;
using System.Windows.Input;

namespace Coolicky.Revit.Toolkit.Wpf.Dialog
{
    public partial class BoolDialog
    {
        public string Description { get; }
        public string YesButtonText { get; }
        public string NoButtonText { get; }

        public BoolDialog(string description, string yesButtonText = "Yes", string noButtonText = "No")
        {
            Description = description;
            YesButtonText = yesButtonText;
            NoButtonText = noButtonText;
            InitializeComponent();
            
        }

        private void NoClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
        private void YesClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void OnEnterClick(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) DialogResult = true;
            if (e.Key == Key.Escape) DialogResult = false;
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }
    }
}