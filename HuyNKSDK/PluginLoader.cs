using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LeagueSharp;
using LeagueSharp.SDK.Core.Utils;
using LeagueSharp.SDK.Core.Enumerations;

using HuyNK_Series_SDK.Plugins;

namespace HuyNK_Series_SDK
{
    class PluginLoader
    {
        public static bool LoadPlugin(string PluginName)
        {
            
            if (CanLoadPlugin(PluginName))
            {
                Logging.Write()(LogLevel.Info, "HuyNK_Series_SDK: " + ObjectManager.Player.ChampionName + " is supported.");
                DynamicInitializer.NewInstance(Type.GetType("HuyNK_Series_SDK.Plugins." + ObjectManager.Player.ChampionName));
                return true;
            }
            else
            {
                Logging.Write()(LogLevel.Warn, "HuyNK_Series_SDK: " + ObjectManager.Player.ChampionName + " is not supported.");
                return false;
            }
        }

        public static bool CanLoadPlugin(string PluginName)
        {
            return Type.GetType("HuyNK_Series_SDK.Plugins." + ObjectManager.Player.ChampionName) != null;
        }
    }
}
