using System;

namespace Coolicky.Revit.Toolkit.Wpf.Dialog
{
    public interface IDialog
    {
        void Info(string message);
        void Success(string message);
        void Warning(string message);
        void Error(string message);
        void Error(Exception e);
        void DetailedError(Exception e);
        bool Confirm(string message, string yesButtonText = "Yes", string noButtonText = "No");
        string Input(string message, string label = "", string buttonText = "Save");
        string SelectFolder(string title, string description = "");
        string OpenFile(string title, string[] fileTypes = null);
        string[] OpenFiles(string title, string[] fileTypes = null);
        string SaveFile(string title, string[] fileTypes = null);
    }
}