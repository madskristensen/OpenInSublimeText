namespace OpenInSublimeText
{
    using System;
    
    /// <summary>
    /// Helper class that exposes all GUIDs used across VS Package.
    /// </summary>
    internal sealed partial class PackageGuids
    {
        public const string guidPackageString = "f5b36ea2-9430-4d77-83dc-c7f6a39e6083";
        public const string guidOpenInVsCmdSetString = "bcea1810-8ea5-45b9-9df0-7ea5baf757cd";
        public const string guidIconsString = "bcea1810-8ea5-45b9-9df0-7ea5baf757ce";
        public static Guid guidPackage = new Guid(guidPackageString);
        public static Guid guidOpenInVsCmdSet = new Guid(guidOpenInVsCmdSetString);
        public static Guid guidIcons = new Guid(guidIconsString);
    }
    /// <summary>
    /// Helper class that encapsulates all CommandIDs uses across VS Package.
    /// </summary>
    internal sealed partial class PackageIds
    {
        public const int OpenInSublimeText = 0x0100;
        public const int FileOpenMenuGroup = 0x0101;
        public const int OpenCurrentFileInSublimeText = 0x0102;
        public const int Sublime = 0x0001;
    }
}
