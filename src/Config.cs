namespace io.radston12.fakerank
{
    using System.ComponentModel;
    using System.IO;

    using Exiled.API.Features;
    using Exiled.API.Interfaces;

    public sealed class Config : IConfig
    {

        /// <summary>
        /// FakeRank Config
        /// </summary>
        public Config()
        {
            Folder = Paths.Configs;
            FullPath = Path.Combine(Folder, "fakeRanks.yml");
        }

        public bool IsEnabled { get; set; } = true;

        public bool Debug { get; set; }

        [Description("Max length for badges")]
        public int MaxBadgeLength { get; set; } = 24;

        [Description("The permissions folder path")]
        public string Folder { get; private set; }

        [Description("The permissions full path")]
        public string FullPath { get; private set; }

        [Description("The suffix for players using ,-Console instead of remote admin!\nNOTE: This string is very limited in chraracters for example \"[\",\"]\" do not work!")]
        public string VIP_Suffix { get; set; } = " (VIP)";
    }
}