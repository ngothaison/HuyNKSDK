using System;

namespace HuyNK_Series_SDK
{
    class Program
    {
        public static void Main(string[] args)
        {
            LeagueSharp.SDK.Core.Events.Load.OnLoad += Load_OnLoad;
            LeagueSharp.SDK.Core.Utils.Logging.Write()(LeagueSharp.SDK.Core.Enumerations.LogLevel.Info, "HuyNK_Series_SDK: Loaded.");
        }

        private static void Load_OnLoad(object sender, EventArgs e)
        {
            Initializer.initialize();
        }
    }
}
