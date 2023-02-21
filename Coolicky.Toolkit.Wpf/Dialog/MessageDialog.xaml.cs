using System.Windows;
using System.Windows.Input;

namespace Coolicky.Toolkit.Wpf.Dialog
{
    public partial class MessageDialog
    {
        public string Message { get; }

        public MessageDialog(string message, MessageLevel level = MessageLevel.Information)
        {
            Message = message;
            InitializeComponent();
            ShowIcon(level);
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }

        private void ShowIcon(MessageLevel level)
        {
            switch (level)
            {
                case MessageLevel.Success:
                    SuccessIcon.Visibility = Visibility.Visible;
                    break;
                case MessageLevel.Information:
                    InfoIcon.Visibility = Visibility.Visible;
                    break;
                case MessageLevel.Warning:
                    WarningIcon.Visibility = Visibility.Visible;
                    break;
                case MessageLevel.Error:
                    ErrorIcon.Visibility = Visibility.Visible;
                    break;
                default:
                    InfoIcon.Visibility = Visibility.Visible;
                    break;
            }
        }

        private void OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void OnConfirm(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Escape) Close();
        }
    }

    public enum MessageLevel
    {
        Success,
        Information,
        Warning,
        Error
    }
}