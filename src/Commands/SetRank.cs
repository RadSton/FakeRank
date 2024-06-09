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
    using io.radston12.fakerank.Helpers;

    /// <summary>
    /// Player Console Command "setrank"
    /// </summary>
    [CommandHandler(typeof(ClientCommandHandler))]
    public class SetRank : ICommand
    {
        public string Command { get; } = "setrank";
        public string Description { get; } = "Gibt dirseblst einen Rang.";
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


            if (arguments.Count < 2)
            {
                response = "[RANG] Benutzung: .setrank <COLOR> <FAKEBADGE>!";
                return false;
            }

            if (!ColorHelper.isValidBadgeColor(arguments.At(0)))
            {
                response = "[RANG] Ungültige Farbe!\nGültige Farben: pink, red, brown, silver, light_green, crimson, cyan, aqua, deep_pink, tomato, yellow, magenta, blue_green, orange, lime, green, emerald, carmine, nickel, mint, army_green, pumpkin, default";
                return false;
            }

            string suffix = Instance.Config.VIP_Suffix;
            int maxLength = Instance.Config.MaxBadgeLength - suffix.Length;

            string text = arguments.At(2);
            for (int i = 3; i < arguments.Count; i++)
                text += " " + arguments.At(i);

            text = StringSanitze.strapoutInvalidCharaters(text, maxLength);
            text += suffix;

            player.RankName = text;
            player.RankColor = arguments.At(0);

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

            response = $"[RANG] Dein Rang ist nun \"{player.RankName}\" mit der Farbe {player.RankColor}!";

            return true;
        }
    }
}