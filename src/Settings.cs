using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.Shell;

namespace OpenInSublimeText
{
    public class Settings : DialogPage
    {
        [Category("General")]
        [DisplayName("Install path")]
        [Description("The absolute path to the \"sublime_text.exe\" or \"subl.exe\" file.")]
        public string FolderPath { get; set; }

        [Category("General")]
        [DisplayName("Open solution/project as regular file")]
        [Description("When true, opens solutions/projects as regular files and does not load folder path into Sublime.")]
        public bool OpenSolutionProjectAsRegularFile { get; set; }

        public override void LoadSettingsFromStorage()
        {
            base.LoadSettingsFromStorage();

            if (string.IsNullOrEmpty(FolderPath))
            {
                FolderPath = FindSublimeText();
            }
        }

        private static string FindSublimeText()
        {
            var programFiles = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles));
            var dirs = programFiles.Parent.GetDirectories(programFiles.Name.Replace(" (x86)", string.Empty) + "*");

            foreach (DirectoryInfo parent in dirs)
                foreach (DirectoryInfo dir in parent.GetDirectories("Sublime*").Reverse())
                {
                    string path = Path.Combine(dir.FullName, "sublime_text.exe");

                    if (File.Exists(path))
                        return path;
                }

            return null;
        }
    }
}
