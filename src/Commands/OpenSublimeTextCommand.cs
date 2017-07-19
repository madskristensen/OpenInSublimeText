using System;
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
                var openFileMenuCommandID = new CommandID(PackageGuids.guidOpenInVsCmdSet, PackageIds.OpenInSublimeText);
                var openFileMenuItem = new MenuCommand(OpenPath, openFileMenuCommandID);
                commandService.AddCommand(openFileMenuItem);

                var openCurrentFileMenuCommandID = new CommandID(PackageGuids.guidOpenInVsCmdSet, PackageIds.OpenCurrentFileInSublimeText);
                var openCurrentFileMenuItem = new MenuCommand(OpenCurrentFile, openCurrentFileMenuCommandID);
                commandService.AddCommand(openCurrentFileMenuItem);
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
                string path = ProjectHelpers.GetSelectedPath(dte, VSPackage.Settings.OpenSolutionProjectAsRegularFile);
                string exe = VSPackage.Settings.FolderPath;

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

        private void OpenCurrentFile(object sender, EventArgs e)
        {
            var dte = (DTE2)ServiceProvider.GetService(typeof(DTE));

            try
            {
                string path = dte.ActiveDocument.FullName;
                int line = 0;

                TextSelection selection = dte.ActiveDocument.Selection as TextSelection;
                if (selection != null)
                {
                    line = selection.ActivePoint.Line;
                }

                string exe = VSPackage.Settings.FolderPath;

                if (!string.IsNullOrEmpty(path) && !string.IsNullOrEmpty(exe))
                {
                    OpenSublimeText(exe, path, line);
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

        private static void OpenSublimeText(string exe, string path, int line = 0)
        {
            bool isDirectory = Directory.Exists(path);

            var start = new System.Diagnostics.ProcessStartInfo()
            {
                WorkingDirectory = path,
                FileName = $"\"{exe}\"",
                Arguments = isDirectory ? "." : $"\"{path}\":{line}",
                CreateNoWindow = true,
                WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden
            };

            using (System.Diagnostics.Process.Start(start))
            {
                string evt = isDirectory ? "directory" : "file";
            }
        }
    }
}
