
namespace io.radston12.fakerank.Commands
{
    using System;
    using System.Collections.Generic;

    using CommandSystem;

    using Exiled.API.Features;
    using Exiled.API.Features.Pickups;

    using io.radston12.fakerank.Extensions;

    /// <summary>
    /// FakeRank command via RemoteAdmin
    /// </summary>
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class FakeRank : ICommand
    {
        public static readonly List<string> AvailableColors = new List<string>()
        {
            "pink","red","brown","silver","light_green","crimson","cyan","aqua","deep_pink","tomato","yellow","magenta",
            "blue_green","orange","lime","green","emerald","carmine","nickel","mint","army_green","pumpkin","default"
        };

        public string Command { get; } = "fakerank";
        public string[] Aliases { get; } = new[] { "frank", "fakebadge", "fbadge" };
        public string Description { get; } = "Sets a fake rank for players.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);

            if (player.IsHost)
            {
                response = "[FAKERANK] This command is only for players!";
                return false;
            }

            if (!player.RemoteAdminAccess)
            {
                response = "[FAKERANK] Well well well How can u enter remote admin commands without remote admin permissions?7";
                return false;
            }

            if (!(arguments.Count >= 2))
            {

                if (arguments.Count == 1 && arguments.At(0) == "clear")
                {
                    player.RankName = "";
                    player.RankColor = "default";

                    if (FakeRankStorage.Storage.ContainsKey(player.UserId))
                    {
                        FakeRankStorage.Storage.Remove(player.UserId);
                        FakeRankStorage.Save();
                    }
                    response = "[FAKERANK] Your badge was nuked to be empty!";

                    return false;
                }

                response = "[FAKERANK] Usage: frank <rankColor> <rankName> /n Or if you want to remove the fake badge: \"frank clear\"!";
                return false;
            }

            if (!AvailableColors.Contains(arguments.At(0)))
            {
                response = "[FAKERANK] Invalid color!\nValid Colors are: pink, red, brown, silver, light_green, crimson, cyan, aqua, deep_pink, tomato, yellow, magenta, blue_green, orange, lime, green, emerald, carmine, nickel, mint, army_green, pumpkin, default";
                return false;
            }

            string text = arguments.At(1);

            for (int i = 2; i < arguments.Count; i++)
                text += " " + arguments.At(i);


            player.RankName = text;
            player.RankColor = arguments.At(0);

            FakeRankStorage.Storage.Add(player.UserId,
                        new Dictionary<string, string>()
                            {
                                { "color", player.RankColor },
                                { "value", player.RankName }
                            }
                    );

            FakeRankStorage.Save();

            response = "[FAKERANK] Your Badge is now \"" + text + "\" with the color " + arguments.At(0) + "!";

            return true;
        }
    }

}