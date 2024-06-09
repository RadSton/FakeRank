namespace io.radston12.fakerank.Commands.FakeRankChildren
{
    using System;
    using System.Collections.Generic;

    using CommandSystem;

    using Extensions;

    using Exiled.Permissions.Extensions;

    using io.radston12.fakerank.Helpers;

    using Exiled.API.Features;

    /// <summary>
    /// Bans a player from executing .setrank player command 
    /// </summary>
    public class Ban : ICommand
    {
        public string Command { get; } = "ban";
        public string Description { get; } = "Bans a user from using the player console command .setrank!";
        public string[] Aliases { get; } = new string[] { };

        /// <inheritdoc/>
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);

            if (!Permissions.CheckPermission(player, "fakerank.all"))
            {
                response = "[FAKERANK] You dont have permission to execute this command!";
                return false;
            }

            if (arguments.Count != 1)
            {
                response = "[FAKERANK] Usage: FAKERANK BAN <RA-User-ID>!";
                return false;
            }

            Player target = RAUserIdParser.getByCommandArgument(arguments.At(0));

            if (target == null)
            {
                response = "[FAKERANK] Target player not found!";
                return false;
            }


            if (FakeRankStorage.Storage.ContainsKey(target.UserId))
                FakeRankStorage.Storage.Remove(target.UserId);

            target.RankName = "";
            target.RankColor = "default";

            FakeRankStorage.Storage.Add(player.UserId,
                       new Dictionary<string, string>()
                           {
                                       { "color", "default" },
                                       { "value", "" },
                                       { "banned", "true" },
                           }
                   );

            FakeRankStorage.Save();

            response = $"[FAKERANK] Player {target.Nickname} is now banned from using .setrank in the player console!";
            return true;
        }
    }
}