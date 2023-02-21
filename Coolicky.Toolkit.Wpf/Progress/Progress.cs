using System;
using System.Threading;

namespace Coolicky.Toolkit.Wpf.Progress
{
    public class Progress
    {
        private static ProgressWindow _window;
        private static EventWaitHandle _eventWaitHandle;

        public void Start()
        {
            using (_eventWaitHandle = new AutoResetEvent(false))
            {
                var newProgressWindowThread = new Thread(ShowWindow);
                newProgressWindowThread.SetApartmentState(ApartmentState.STA);
                newProgressWindowThread.IsBackground = true;
                newProgressWindowThread.Start();

                _eventWaitHandle.WaitOne();
            }
        }

        private static void ShowWindow()
        {
            _window = new ProgressWindow();
            _window.Show();
            _window.Closed += window_Closed;
            _eventWaitHandle.Set();

            System.Windows.Threading.Dispatcher.Run();
        }

        private static void window_Closed(object sender, EventArgs e)
        {
            System.Windows.Threading.Dispatcher.CurrentDispatcher.InvokeShutdown();
        }

        public void Update(string message, int current, int total)
        {
            _window?.Update(message, current, total);
        }

        public void Update(string message)
        {
            _window?.Update(message);
        }

        public void Finish()
        {
            _window?.Dispatcher.Invoke(_window.Close);
        }
    }
}