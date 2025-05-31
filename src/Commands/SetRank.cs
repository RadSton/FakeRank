using FakeRank.Helpers;

namespace FakeRank.Commands
{
    using System;
    using System.Collections.Generic;

    using CommandSystem;

    using Extensions;

    using Exiled.API.Features;
    using Exiled.Permissions.Extensions;

    using static FakeRank;
    using Helpers;

    /// <summary>
    /// Player Console Command "setrank"
    /// </summary>
    [CommandHandler(typeof(ClientCommandHandler))]
    public class SetRank : ICommand
    {
        public string Command { get; } = "setrank";
        public string Description { get; } = "Setzt Rang. Mehr Info mit .setrank";
        public string[] Aliases { get; } = new string[] { };

        /// <inheritdoc/>
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);

            if (!LabApiPermissions.checkCommandSender(sender, "fakerank.vip")) 
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
                response = "[RANG] Custom Badges " + Instance.Version + "\n -> Verwendung: .setrank (COLOR) (RANG)!\nBei (RANG) kann man mehrere Wörter verwenden jedoch nur begrenzt Sonderzeichen\nGültige (COLOR)-Werte sind: pink, red, brown, silver, light_green, crimson, cyan, aqua, deep_pink, tomato, yellow, magenta, blue_green, orange, lime, green, emerald, carmine, nickel, mint, army_green, pumpkin, default\n\nDu kannst deinen Rang auch wieder mit .clearrank entfernen!";
                return false;
            }

            if (!ColorHelper.isValidBadgeColor(arguments.At(0)))
            {
                response = "[RANG] Ungültige Farbe!\nGültige Farben: pink, red, brown, silver, light_green, crimson, cyan, aqua, deep_pink, tomato, yellow, magenta, blue_green, orange, lime, green, emerald, carmine, nickel, mint, army_green, pumpkin, default";
                return false;
            }

            string suffix = Instance.Config.VipSuffix;
            int maxLength = Instance.Config.MaxBadgeLength - suffix.Length;

            string text = arguments.At(1);
            for (int i = 2; i < arguments.Count; i++)
                text += " " + arguments.At(i);

            text = StringSanitze.strapoutInvalidCharaters(text, maxLength);

            if (StringSanitze.containsBlacklistedWords(text))
            {
                Log.Info($"Blocked FakeRank for {player.Nickname} since they tried to set it to: {text} with color {arguments.At(0)}!");
                response = "[RANG] Dein Rang konnte nicht gesetzt werden!";
                return false;
            }

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