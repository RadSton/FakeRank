namespace io.radston12.fakerank.Commands.FakeRankChildren
{
    using System;

    using CommandSystem;

    using Extensions;
    
    using Exiled.Permissions.Extensions;

    using io.radston12.fakerank.Helpers;

    using Exiled.API.Features;

    /// <summary>
    /// Clears a fakerank for the player executing or a custom player
    /// </summary>
    public class Clear : ICommand
    {
        public string Command { get; } = "clear";
        public string Description { get; } = "Clears a fakerank";
        public string[] Aliases { get; } = new string[] { };

        /// <inheritdoc/>
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);
            Player target = player;

            if (!Permissions.CheckPermission(player, "fakerank.all") && !Permissions.CheckPermission(player, "fakerank.set") && !Permissions.CheckPermission(player, "fakerank.self"))
            {
                response = "[FAKERANK] You dont have permission to execute this command!";
                return false;
            }

            if (player.IsHost)
            {
                response = "[FAKERANK] This command is only for players!";
                return false;
            }

            if (!player.RemoteAdminAccess)
            {
                response = "[FAKERANK] Well well well How can u enter remote admin commands without remote admin permissions?";
                return false;
            }

            if (Permissions.CheckPermission(player, "fakerank.set") || Permissions.CheckPermission(player, "fakerank.all"))
            {
                if (arguments.Count != 0)
                    target = RAUserIdParser.getByCommandArgument(arguments.At(0));

                if (target == null)
                {
                    response = "[FAKERANK] Target player not found!";
                    return false;
                }
            }

            if (FakeRankStorage.Storage.ContainsKey(target.UserId))
            {
                FakeRankStorage.Storage.Remove(target.UserId);
                FakeRankStorage.Save();
            }

            target.RankName = "";
            target.RankColor = "default";

            if (arguments.Count != 0 && (Permissions.CheckPermission(player, "fakerank.set") || Permissions.CheckPermission(player, "fakerank.all")))
                response = $"[FAKERANK] Nuked rank of {target.Nickname}! They will get back their normal rank after a round restart";
            else
                response = $"[FAKERANK] Cleared your rank! You will get back your normal rank after a round restart";

            return true;
        }
    }
}