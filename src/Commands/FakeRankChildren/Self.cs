using FakeRank.Helpers;

namespace FakeRank.Commands.FakeRankChildren
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using CommandSystem;

    using Extensions;

    using Exiled.API.Features;
    using Exiled.Permissions.Extensions;

    using static FakeRank;
    using global::FakeRank.Helpers;

    /// <summary>
    /// Sets a fakerank for the player executing
    /// </summary>
    public class Self : ICommand
    {
        public string Command { get; } = "self";
        public string Description { get; } = "Sets a fakerank for yourself.";
        public string[] Aliases { get; } = new string[] { };

        /// <inheritdoc/>
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);

            if (!sender.CheckPermission("fakerank.all") && !sender.CheckPermission("fakerank.self"))
            {
                response = "[FAKERANK] You dont have permission to execute this command!";
                return false;
            }

            if (arguments.Count < 2)
            {
                response = "[FAKERANK] Usage: fakerank self <COLOR> <FAKEBADGE>!";
                return false;
            }

            if (!ColorHelper.isValidBadgeColor(arguments.At(0)))
            {
                response = "[FAKERANK] Invalid color!\nValid Colors are: pink, red, brown, silver, light_green, crimson, cyan, aqua, deep_pink, tomato, yellow, magenta, blue_green, orange, lime, green, emerald, carmine, nickel, mint, army_green, pumpkin, default";
                return false;
            }

            string text = arguments.At(1);
            for (int i = 2; i < arguments.Count; i++)
                text += " " + arguments.At(i);

            text = StringSanitze.strapoutInvalidCharaters(text, Instance.Config.MaxBadgeLength);


            player.RankName = text;
            player.RankColor = arguments.At(0);

            if (FakeRankStorage.Storage.ContainsKey(player.UserId))
                FakeRankStorage.Storage.Remove(player.UserId);

            FakeRankStorage.Storage.Add(player.UserId,
                        new Dictionary<string, string>()
                            {
                                       { "color", player.RankColor },
                                       { "value", player.RankName },
                                       { "banned", "false" }
                            }
                    );

            FakeRankStorage.Save();

            response = $"[FAKERANK] Set your rank to \"{player.RankName}\" with the color of {player.RankColor}!";

            return true;
        }
    }
}