using System;
using System.Linq;
using System.ComponentModel.Design;
using System.IO;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;

namespace OpenInSublimeText
{
    internal sealed class OpenSublimeTextCommand
    {
        private readonly Package _package;

        private OpenSublimeTextCommand(Package package)
        {
            _package = package;

            OleMenuCommandService commandService = ServiceProvider.GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if (commandService != null)
            {
                var menuCommandID = new CommandID(PackageGuids.guidOpenInVsCmdSet, PackageIds.OpenInSublimeText);
                var menuItem = new MenuCommand(OpenPath, menuCommandID);
                commandService.AddCommand(menuItem);
            }
        }

        public static OpenSublimeTextCommand Instance { get; private set; }

        private IServiceProvider ServiceProvider
        {
            get { return _package; }
        }

        public static void Initialize(Package package)
        {
            Instance = new OpenSublimeTextCommand(package);
        }

        private void OpenPath(object sender, EventArgs e)
        {
            var dte = (DTE2)ServiceProvider.GetService(typeof(DTE));

            try
            {
                string path = ProjectHelpers.GetSelectedPath(dte);
                string exe = FindSublimeText();

                if (!string.IsNullOrEmpty(path) && !string.IsNullOrEmpty(exe))
                {
                    OpenSublimeText(exe, path);
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Couldn't resolve the folder");
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
            }
        }

        private static void OpenSublimeText(string exe, string path)
        {
            bool isDirectory = Directory.Exists(path);

            var start = new System.Diagnostics.ProcessStartInfo()
            {
                WorkingDirectory = path,
                FileName = $"\"{exe}\"",
                Arguments = isDirectory ? "." : $"\"{path}\"",
                CreateNoWindow = true,
                WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden
            };

            using (System.Diagnostics.Process.Start(start))
                Telemetry.TrackEvent("Open in Sublime Text");
        }

        private static string FindSublimeText()
        {
            string programFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles).Replace(" (x86)", string.Empty);
            var dirs = Directory.EnumerateDirectories(programFiles, "Sublime Text*").Reverse();

            foreach (string dir in dirs)
            {
                string path = Path.Combine(dir, "subl.exe");

                if (File.Exists(path))
                    return path;
            }

            return null;
        }
    }
}
