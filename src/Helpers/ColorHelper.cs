namespace FakeRank.Helpers
{

    using System;
    using System.Collections.Generic;

    public static class ColorHelper
    {

        public static readonly List<string> AvailableColors = new List<string>()
        {
            "pink","red","brown","silver","light_green","crimson","cyan","aqua","deep_pink","tomato","yellow","magenta",
            "blue_green","orange","lime","green","emerald","carmine","nickel","mint","army_green","pumpkin","default"
        };

        public static bool isValidBadgeColor(string color)
        {
            return AvailableColors.Contains(color);
        }
    }
}