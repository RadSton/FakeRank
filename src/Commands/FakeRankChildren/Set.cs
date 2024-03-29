namespace io.radston12.fakerank.Commands.FakeRankChildren
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using CommandSystem;

    using Extensions;

    using io.radston12.fakerank.Helpers;


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

            if (!AvailableColors.Contains(arguments.At(1)))
            {
                response = "[FAKERANK] Invalid color!\nValid Colors are: pink, red, brown, silver, light_green, crimson, cyan, aqua, deep_pink, tomato, yellow, magenta, blue_green, orange, lime, green, emerald, carmine, nickel, mint, army_green, pumpkin, default";
                return false;
            }

            string text = arguments.At(2);
            for (int i = 3; i < arguments.Count; i++)
                text += " " + arguments.At(i);

            target.RankName = text;
            target.RankColor = arguments.At(1);

            if (FakeRankStorage.Storage.ContainsKey(target.UserId))
                FakeRankStorage.Storage.Remove(target.UserId);

            FakeRankStorage.Storage.Add(target.UserId,
                        new Dictionary<string, string>()
                            {
                                       { "color", target.RankColor },
                                       { "value", target.RankName }
                            }
                    );

            FakeRankStorage.Save();

            response = $"[FAKERANK] Set rank of {target.Nickname} to \"{target.RankName}\" with the color of {target.RankColor}!";

            return false;
        }
    }
}