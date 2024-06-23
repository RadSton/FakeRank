namespace io.radston12.fakerank
{
    using System.Collections.Generic;

    using Exiled.API.Enums;
    using Exiled.API.Features;

    using MEC;

    using Events;

    /// <summary>
    /// A fake rank plugin
    /// </summary>
    public class FakeRank : Plugin<Config>
    {
        private static FakeRank Singleton;
        public static FakeRank Instance => Singleton;
        private PlayerHandler playerHandler;
        public override PluginPriority Priority { get; } = PluginPriority.Last;

        public override string Author { get; } = "radston12";


        public override void OnEnabled()
        {
            Singleton = this;

            playerHandler = new PlayerHandler();
            Exiled.Events.Handlers.Player.Verified += playerHandler.OnVerified;

            Timing.CallDelayed(
                5f,
                () =>
                {
                    Extensions.FakeRankStorage.Create();
                    Extensions.FakeRankStorage.Reload();
                });

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Exiled.Events.Handlers.Player.Verified -= playerHandler.OnVerified;

            Extensions.FakeRankStorage.Storage.Clear();

            base.OnDisabled();
        }
    }
}