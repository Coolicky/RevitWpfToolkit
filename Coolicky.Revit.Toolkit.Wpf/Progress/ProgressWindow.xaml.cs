using System;

namespace Coolicky.Revit.Toolkit.Wpf.Progress
{
    public partial class ProgressWindow
    {
        public ProgressWindow()
        {
            InitializeComponent();
        }

        public void Update(string message)
        {
            Dispatcher.Invoke(new Action<string>(
                    delegate(string m) { MessageText.Text = $"{m}"; }),
                System.Windows.Threading.DispatcherPriority.Background, message);
        }

        public void Update(string text, int current, int total)
        {
            Dispatcher.Invoke(new Action<string, int, int>(
                delegate(string m, int v, int t)
                {
                    ProgressBar.Maximum = Convert.ToDouble(t);
                    ProgressBar.Value = Convert.ToDouble(v);
                    MessageText.Text = $"{m}";
                }), System.Windows.Threading.DispatcherPriority.Background, text, current, total);
        }

        private void Border_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ChangedButton == System.Windows.Input.MouseButton.Left)
            {
                DragMove();
            }
        }
    }
}