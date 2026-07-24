using Microsoft.Win32;

namespace UserControlsWPF.FileDialog
{
    public interface IOpenFileService
    {
        string Filter { get; set; }
        string FileName { get; }
        bool? ShowDialog();
    }
    public class OpenFileService : IOpenFileService
    {
        private readonly OpenFileDialog _openFileDialog;
        public string Filter
        {
            get => _openFileDialog.Filter;
            set => _openFileDialog.Filter = value;
        }

        public string FileName => _openFileDialog.FileName;

        public bool? ShowDialog() => _openFileDialog.ShowDialog();
        public OpenFileService()
        {
            _openFileDialog = new OpenFileDialog();
        }
    }
}
