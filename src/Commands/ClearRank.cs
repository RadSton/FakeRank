namespace io.radston12.fakerank.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using CommandSystem;

    using Extensions;

    using Exiled.API.Features;
    using Exiled.Permissions.Extensions;

    using static io.radston12.fakerank.FakeRank;

    /// <summary>
    /// ö-Console command setrank
    /// </summary>
    [CommandHandler(typeof(ClientCommandHandler))]
    public class ClearRank : ICommand
    {

        public string Command { get; } = "clearrank";
        public string Description { get; } = "Entfernt dirseblst einen Rang.";
        public string[] Aliases { get; } = new string[] { };

        /// <inheritdoc/>
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);

            if (!sender.CheckPermission("fakerank.vip"))
            {
                response = "You don't have the permissions to execute this command.";
                return false;
            }


            Dictionary<string, string> playerData;

            if (FakeRankStorage.Storage.TryGetValue(player.UserId, out playerData))
            {
                string isBanned = "false";
                bool found = playerData.TryGetValue("banned", out isBanned);
                if (found)
                    if (isBanned.Contains("true"))
                    {
                        response = "You don't have the permissions to execute this command since you abused it before.";
                        return false;
                    }
            }


            player.RankName = "";
            player.RankColor = "default";

            if (FakeRankStorage.Storage.ContainsKey(player.UserId))
                FakeRankStorage.Storage.Remove(player.UserId);

            FakeRankStorage.Storage.Add(player.UserId,
                        new Dictionary<string, string>()
                            {
                                       { "color", player.RankColor },
                                       { "value", player.RankName },
                                       { "banned", "false" },
                            }
                    );

            FakeRankStorage.Save();

            response = "[RANG] Dein Rang wurde gecleart! Du bekommst deinen default Rang nach einem Rundenneustart/Reconnect wieder!";

            return true;
        }
    }
}