namespace PigeonDLCore.Models.ViewModels
{
    public class FileView
    {
        public string Name { get; set; }

        public string DateUploaded { get; set; }

        public long Size { get; set; }

        public int Downloads { get; set; }

        public string URL { get; set; }

        public bool ShowDelete { get; set; }
    }
}
