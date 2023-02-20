using System;
using System.Linq;
using System.Windows.Forms;

namespace Coolicky.Revit.Toolkit.Wpf.Dialog
{
    public class Dialog : IDialog
    {
        public void Info(string message)
        {
            var dialog = new MessageDialog(message);
            dialog.ShowDialog();
        }

        public void Success(string message)
        {
            var dialog = new MessageDialog(message, MessageLevel.Success);
            dialog.ShowDialog();
        }

        public void Warning(string message)
        {
            var dialog = new MessageDialog(message, MessageLevel.Warning);
            dialog.ShowDialog();
        }

        public void Error(string message)
        {
            var dialog = new MessageDialog(message, MessageLevel.Error);
            dialog.ShowDialog();
        }

        public void Error(Exception e)
        {
            var dialog = new MessageDialog(e.Message, MessageLevel.Error);
            dialog.ShowDialog();
        }

        public void DetailedError(Exception e)
        {
            var dialog = new MessageDialog($"{e.Message}\n\n{e.StackTrace}", MessageLevel.Error);
            dialog.ShowDialog();
        }

        public bool Confirm(string message, string yesButtonText = "Yes", string noButtonText = "No")
        {
            var dialog = new BoolDialog(message, yesButtonText, noButtonText);
            return dialog.ShowDialog() == true;
        }

        public string Input(string message, string label = "", string buttonText = "Save")
        {
            var dialog = new InputDialog(message, label, buttonText);
            return dialog.ShowDialog() != true ? null : dialog.Text;
        }

        public string SelectFolder(string title, string description = "")
        {
            var dialog = new FolderBrowserDialog();
            dialog.Description = title;
            dialog.Description = description;
            return dialog.ShowDialog() != DialogResult.OK ? null : dialog.SelectedPath;
        }

        public string OpenFile(string title, string[] fileTypes = null)
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

        public string[] OpenFiles(string title, string[] fileTypes = null)
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

        public string SaveFile(string title, string[] fileTypes = null)
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