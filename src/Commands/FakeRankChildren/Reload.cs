namespace FakeRank.Commands.FakeRankChildren
{
    using System;
    using System.Collections.Generic;

    using CommandSystem;

    using Extensions;

    using Exiled.Permissions.Extensions;

    using global::FakeRank.Helpers;

    using Exiled.API.Features;

    /// <summary>
    /// Reloads custom config 
    /// </summary>
    public class Reload : ICommand
    {
        public string Command { get; } = "reload";
        public string Description { get; } = "Reloads custom config";
        public string[] Aliases { get; } = new string[] { };

        /// <inheritdoc/>
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);

            if (!LabApiPermissions.checkCommandSender(sender, "fakerank.all"))
            {
                response = "[FAKERANK] You dont have permission to execute this command!";
                return false;
            }
        
            FakeRankStorage.Reload();

            response = $"[FAKERANK] Config Reload complete!";
            return true;
        }
    }
}