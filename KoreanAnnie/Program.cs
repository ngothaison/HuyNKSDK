namespace KoreanAnnie
{
    using System;

    using LeagueSharp;
    using LeagueSharp.Common;

    class Program
    {
        static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
            Notifications.AddNotification("Korean Annie", 10000);
            Notifications.AddNotification("Được Việt Hóa bởi nhóm L# VN!", 10000);
        }

        static void Game_OnGameLoad(EventArgs args)
        {
            if (ObjectManager.Player.ChampionName.ToLowerInvariant() == "annie")
            {
                Annie annie = new Annie();
            }
        }
    }
}
