namespace FakeRank
{
    using System;
    using Exiled.API.Enums;
    using Exiled.API.Features;
    using MEC;
    using Extensions;
    using Events;

    /// <summary>
    /// A fake rank plugin
    /// </summary>
    public class FakeRank : Plugin<Config>
    {
        
        public override string Prefix => "FakeRank";
        public override string Name => "FakeRank";
        public override string Author => "radston12";
        public override Version Version => new Version(1, 4, 5);
        
        private static FakeRank Singleton;
        public static FakeRank Instance => Singleton;
        public static PlayerHandler playerHandler;
        public override PluginPriority Priority { get; } = PluginPriority.Last;
        
        public override void OnEnabled()
        {
            Singleton = this;

            playerHandler = new PlayerHandler();
            Exiled.Events.Handlers.Player.Verified += playerHandler.OnVerified;
            
            Coroutine.Start();

            Timing.CallDelayed(
                5f,
                () =>
                {
                    FakeRankStorage.Create();
                    FakeRankStorage.Reload();
                });

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            FakeRankStorage.Storage.Clear();
            
            Coroutine.Stop();

            base.OnDisabled();
        }
    }
}