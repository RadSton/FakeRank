namespace io.radston12.fakerank.Commands.FakeRankChildren
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using CommandSystem;

    using Extensions;

    using Exiled.Permissions.Extensions;

    using io.radston12.fakerank.Helpers;
    using static io.radston12.fakerank.FakeRank;


    using Exiled.API.Features;

    /// <summary>
    /// Sets a fakerank for a player
    /// </summary>
    public class Set : ICommand
    {
        public static readonly List<string> AvailableColors = new List<string>()
        {
            "pink","red","brown","silver","light_green","crimson","cyan","aqua","deep_pink","tomato","yellow","magenta",
            "blue_green","orange","lime","green","emerald","carmine","nickel","mint","army_green","pumpkin","default"
        };


        public string Command { get; } = "set";
        public string Description { get; } = "Sets a fakerank (for another user)";
        public string[] Aliases { get; } = new string[] { };

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);
            Player target = player;

            if (!Permissions.CheckPermission(player, "fakerank.all") && !Permissions.CheckPermission(player, "fakerank.set"))
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

            if (arguments.Count < 3)
            {
                response = "[FAKERANK] Usage: fakerank set <RA-User-ID> <COLOR> <FAKEBADGE>!";
                return false;
            }


            target = RAUserIdParser.getByCommandArgument(arguments.At(0));

            if (target == null)
            {
                response = "[FAKERANK] Player not found!";
                return false;
            }
            /*
                        if (target.UserId == player.UserId)
                        {
                            response = "[FAKERANK] To set your rank please use SELF instead of SET";
                            return false;
                        }
            */
            if (!AvailableColors.Contains(arguments.At(1)))
            {
                response = "[FAKERANK] Invalid color!\nValid Colors are: pink, red, brown, silver, light_green, crimson, cyan, aqua, deep_pink, tomato, yellow, magenta, blue_green, orange, lime, green, emerald, carmine, nickel, mint, army_green, pumpkin, default";
                return false;
            }

            int maxLength = Instance.Config.MaxBadgeLength;

            string text = arguments.At(2);
            for (int i = 3; i < arguments.Count; i++)
                text += " " + arguments.At(i);

            if (text.Length > maxLength)
                text = text.Substring(0, maxLength);

            target.RankName = text;
            target.RankColor = arguments.At(1);

            string isBanned = "false";
            Dictionary<string, string> playerData;
            if (FakeRankStorage.Storage.TryGetValue(target.UserId, out playerData))
            {
                bool success = playerData.TryGetValue("banned", out isBanned);
                if(!success) isBanned = "false";
                FakeRankStorage.Storage.Remove(target.UserId);
            }

            FakeRankStorage.Storage.Add(target.UserId,
                        new Dictionary<string, string>()
                            {
                                       { "color", target.RankColor },
                                       { "value", target.RankName },
                                       { "banned", isBanned }
                            }
                    );

            FakeRankStorage.Save();

            response = $"[FAKERANK] Set rank of {target.Nickname} to \"{target.RankName}\" with the color of {target.RankColor}!";

            if (isBanned.Contains("true"))
                response += "\nNote: Player is banned from setting rank himself!";

            return true;
        }
    }
}