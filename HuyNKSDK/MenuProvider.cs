using LeagueSharp;
using LeagueSharp.SDK.Core.UI.IMenu;
using LeagueSharp.SDK.Core.UI.IMenu.Values;
using LeagueSharp.SDK.Core.Utils;
using LeagueSharp.SDK.Core.Enumerations;

namespace HuyNK_Series_SDK
{
    class MenuProvider
    {
        public static Menu MainMenu;

        public static void initialize()
        {
            MainMenu = new Menu("HuyNK Series SDK", "[HuyNK.VN] SDK: " + ObjectManager.Player.ChampionName, true, ObjectManager.Player.ChampionName).Attach();

            if(!PluginLoader.CanLoadPlugin(ObjectManager.Player.ChampionName))
                MainMenu.Add(new MenuSeparator("notsupported", "sorry, " + ObjectManager.Player.ChampionName + " is not supported."));

            Logging.Write()(LogLevel.Info, "HuyNK Series SDK: MenuProvider initialized.");
        }
    }
}