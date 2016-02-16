using System;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Shell;

namespace OpenInSublimeText
{
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", Vsix.Version, IconResourceID = 400)]
    [ProvideOptionPage(typeof(Settings), "Open in SublimeText", "General", 101, 111, true, new string[0], ProvidesLocalizedCategoryName = false)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [Guid(PackageGuids.guidPackageString)]
    public sealed class VSPackage : Package
    {
        public static Settings Settings;

        protected override void Initialize()
        {
            Settings = (Settings)GetDialogPage(typeof(Settings));

            Logger.Initialize(this, Vsix.Name);
            Telemetry.Initialize(this, Vsix.Version, "7f5bc7fb-da06-481e-b66d-40088746d163");
            OpenSublimeTextCommand.Initialize(this);

            base.Initialize();
        }
    }
}
