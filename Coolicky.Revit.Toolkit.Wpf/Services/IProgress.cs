namespace Coolicky.Revit.Toolkit.Wpf.Services
{
    public interface IProgress
    {
        void Start();
        void Update(string message, int current, int total);
        void Update(string message);
        void Finish();
    }
}