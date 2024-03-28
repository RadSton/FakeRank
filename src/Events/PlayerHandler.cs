namespace io.radston12.fakerank.Events
{
    using System.Collections.Generic;

    using Exiled.API.Features;
    using Exiled.API.Features.Items;
    using Exiled.Events.EventArgs.Player;


    using MEC;

    using io.radston12.fakerank.Extensions;

    /// <summary>
    /// Events bruh
    /// </summary>
    internal sealed class PlayerHandler
    {

        public void OnVerified(VerifiedEventArgs ev)
        {

            Dictionary<string, string> playerData;
            if (FakeRankStorage.Storage.TryGetValue(ev.Player.UserId, out playerData))
            {
                string color = "default";
                string value = "";

                bool shouldApply = playerData.TryGetValue("color", out color) && playerData.TryGetValue("value", out value);

                if (shouldApply)
                {
                    ev.Player.RankName = value;
                    ev.Player.RankColor = color;
                    Log.Info($"Set FakeRank for {ev.Player.Nickname} to {value} with color {color}!");
                }
            }

        }

    }
}
