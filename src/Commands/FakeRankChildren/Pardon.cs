using FakeRank.Helpers;

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
    /// Pardons a player from executing .setrank player command 
    /// </summary>
    public class Pardon : ICommand
    {
        public string Command { get; } = "pardon";
        public string Description { get; } = "Unbanns a user from using the player console command .setrank!";
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

            if (arguments.Count != 1)
            {
                response = "[FAKERANK] Usage: FAKERANK PARDON <RA-User-ID>!";
                return false;
            }

            Player target = RAUserIdParser.getByCommandArgument(arguments.At(0));

            if (target == null)
            {
                response = "[FAKERANK] Target player not found!";
                return false;
            }

            string color = "default";
            string value = "";

            Dictionary<string, string> playerData;
            if (FakeRankStorage.Storage.TryGetValue(target.UserId, out playerData))
            {
                bool success = playerData.TryGetValue("color", out color) && playerData.TryGetValue("value", out value);
                FakeRankStorage.Storage.Remove(target.UserId);
            }

            FakeRankStorage.Storage.Add(target.UserId,
                        new Dictionary<string, string>()
                            {
                                       { "color", color },
                                       { "value", value },
                                       { "banned", "false" }
                            }
                    );

            FakeRankStorage.Save();

            response = $"[FAKERANK] Player {target.Nickname} is now unbanned from using .setrank in the player console!";
            return true;
        }
    }
}