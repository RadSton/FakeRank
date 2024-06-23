namespace io.radston12.fakerank
{
    using System.ComponentModel;
    using System.IO;
    using System.Collections.Generic;

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

        [Description("The suffix for players using player console instead of remote admin!\nNOTE: This string is very limited in chraracters for example \"[\",\"]\" do not work!")]
        public string VIP_Suffix { get; set; } = " (VIP)";

        [Description("Blacklisted words for player console commands (ignores lowercase or uppercase ; looks if some of these words are INCLUDED in the fake rank so you dont have to add everything)")]
        public List<string> BlackListedWords { get; private set; } = new List<string> {
            "owner",
            "admin",
            "moderator",
            "support",
            "team",
        };
    }
}