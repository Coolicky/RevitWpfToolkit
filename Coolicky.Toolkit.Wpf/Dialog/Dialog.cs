using System;
using System.Linq;
using System.Windows.Forms;

namespace Coolicky.Toolkit.Wpf.Dialog
{
    public static class Dialog
    {
        public static void Info(string message)
        {
            var dialog = new MessageDialog(message);
            dialog.ShowDialog();
        }

        public static void Success(string message)
        {
            var dialog = new MessageDialog(message, MessageLevel.Success);
            dialog.ShowDialog();
        }

        public static void Warning(string message)
        {
            var dialog = new MessageDialog(message, MessageLevel.Warning);
            dialog.ShowDialog();
        }

        public static void Error(string message)
        {
            var dialog = new MessageDialog(message, MessageLevel.Error);
            dialog.ShowDialog();
        }

        public static void Error(Exception e)
        {
            var dialog = new MessageDialog(e.Message, MessageLevel.Error);
            dialog.ShowDialog();
        }

        public static void DetailedError(Exception e)
        {
            var dialog = new MessageDialog($"{e.Message}\n\n{e.StackTrace}", MessageLevel.Error);
            dialog.ShowDialog();
        }

        public static bool Confirm(string message, string yesButtonText = "Yes", string noButtonText = "No")
        {
            var dialog = new BoolDialog(message, yesButtonText, noButtonText);
            return dialog.ShowDialog() == true;
        }

        public static string Input(string message, string label = "", string buttonText = "Save")
        {
            var dialog = new InputDialog(message, label, buttonText);
            return dialog.ShowDialog() != true ? null : dialog.Text;
        }

        public static string SelectFolder(string title, string description = "")
        {
            var dialog = new FolderBrowserDialog();
            dialog.Description = title;
            dialog.Description = description;
            return dialog.ShowDialog() != DialogResult.OK ? null : dialog.SelectedPath;
        }

        public static string OpenFile(string title, string[] fileTypes = null)
        {
            var dialog = new OpenFileDialog();
            dialog.Title = title;
            if (fileTypes != null)
            {
                dialog.Filter = string.Join("|", fileTypes.Select(x => $"{x}|*.{x}"));
            }

            dialog.Multiselect = false;
            dialog.CheckFileExists = true;
            return dialog.ShowDialog() != DialogResult.OK ? null : dialog.FileName;
        }

        public static string[] OpenFiles(string title, string[] fileTypes = null)
        {
            var dialog = new OpenFileDialog();
            dialog.Title = title;
            if (fileTypes != null)
            {
                dialog.Filter = string.Join("|", fileTypes.Select(x => $"{x}|*.{x}"));
            }

            dialog.Multiselect = true;
            dialog.CheckFileExists = true;
            return dialog.ShowDialog() != DialogResult.OK ? null : dialog.FileNames;
        }

        public static string SaveFile(string title, string[] fileTypes = null)
        {
            var dialog = new SaveFileDialog();
            dialog.Title = title;
            dialog.OverwritePrompt = true;
            if (fileTypes != null)
            {
                dialog.Filter = string.Join("|", fileTypes.Select(x => $"{x}|*.{x}"));
                dialog.AddExtension = true;
            }
            
            return dialog.ShowDialog() != DialogResult.OK ? null : dialog.FileName;
        }
    }
}