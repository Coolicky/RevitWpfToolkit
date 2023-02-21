using System.Windows;
using System.Windows.Input;

namespace Coolicky.Toolkit.Wpf.Dialog
{
    public partial class InputDialog
    {
        public string Message { get; }
        public string Text { get; set; }
        public string Label { get; }
        public string ButtonText { get; }

        public InputDialog(string message, string label = "", string buttonText = "Save")
        {
            Message = message;
            Label = label;
            ButtonText = buttonText;
            InitializeComponent();
            InputBox.Focus();
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void SaveClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void OnEnterClick(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) DialogResult = true;
            if (e.Key == Key.Escape) DialogResult = false;
        }
    }
}