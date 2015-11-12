using System;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Shell;

namespace OpenInSublimeText
{
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", Version, IconResourceID = 400)]
    [ProvideOptionPage(typeof(Settings), "Open in SublimeText", "General", 101, 111, true, new string[0], ProvidesLocalizedCategoryName = false)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [Guid(PackageGuids.guidPackageString)]
    public sealed class VSPackage : Package
    {
        public const string Version = "1.0";
        public const string Title = "Open in Sublime Text";
        public static Settings Settings;

        protected override void Initialize()
        {
            Settings = (Settings)GetDialogPage(typeof(Settings));

            Logger.Initialize(this, Title);
            Telemetry.Initialize(this, Version, "7f5bc7fb-da06-481e-b66d-40088746d163");
            OpenSublimeTextCommand.Initialize(this);

            base.Initialize();
        }
    }
}
