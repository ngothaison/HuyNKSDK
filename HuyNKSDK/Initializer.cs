using LeagueSharp;
using LeagueSharp.SDK.Core.Utils;
using LeagueSharp.SDK.Core.Enumerations;

namespace HuyNK_Series_SDK
{
    class Initializer
    {
        public static void initialize()
        {
            Logging.Write()(LogLevel.Info, "HuyNK Series SDK: --------------------INITIALIZE-------------------");

            MenuProvider.initialize();

            if(PluginLoader.LoadPlugin(ObjectManager.Player.ChampionName))
            {
                OrbwalkerTargetIndicator.initialize();
            }

            Logging.Write()(LogLevel.Info, "HuyNK Series SDK: -----------------------DONE----------------------");
        }
    }
}
