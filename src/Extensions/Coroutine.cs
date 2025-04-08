namespace FakeRank.Extensions
{
    using Exiled.API.Features;
    using MEC;
    using System.Collections.Generic;
    
    /// <summary>
    /// Apply the FakeRank to every player every x seconds.
    /// </summary>
    
    public static class Coroutine
    {
        private static bool _enabled = true;

        public static void Start()
        {
            Timing.RunCoroutine(MainLoop());
        }
        
        public static void Stop()
        {
            _enabled = false;
        }
        
        private static IEnumerator<float> MainLoop()
        {
            while(_enabled)
            {
                Log.Debug("Checking for new FakeRanks.");

                CheckAndApplyFakeRanks();
                
                yield return Timing.WaitForSeconds(10f);
            }
        }
        
        private static void CheckAndApplyFakeRanks()
        {
            foreach (Player player in Player.List)
            {
                if (FakeRankStorage.Storage.TryGetValue(player.UserId, out var playerData))
                {
                    if (!playerData.TryGetValue("color", out string color) ||
                        !playerData.TryGetValue("value", out string value))
                        return;

                    if(value.Length < 2)
                        return;

                    player.RankName = value;
                    player.RankColor = color;
                    Log.Debug($"Set FakeRank for {player.Nickname} to {value} with color {color}!");
                }
            }
        }
    }
}