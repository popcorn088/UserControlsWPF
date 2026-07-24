using Microsoft.Win32;

namespace UserControlsWPF.FileDialog
{
    public interface ISaveFileService
    {
        string Filter { get; set; }
        string FileName { get; }
        bool? ShowDialog();
    }
    public class SaveFileService : ISaveFileService
    {
        private readonly SaveFileDialog _saveFileDialog;
        public string Filter
        {
            get => _saveFileDialog.Filter;
            set => _saveFileDialog.Filter = value;
        }
        public string FileName
        {
            get => _saveFileDialog.FileName;
        }

        public bool? ShowDialog() => _saveFileDialog.ShowDialog();

        public SaveFileService()
        {
            _saveFileDialog = new SaveFileDialog();
        }
    }
}
