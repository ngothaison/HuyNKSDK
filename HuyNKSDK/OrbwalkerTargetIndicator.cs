using System;

using LeagueSharp;
using LeagueSharp.SDK.Core;
using LeagueSharp.SDK.Core.Utils;
using LeagueSharp.SDK.Core.Enumerations;

namespace HuyNK_Series_SDK
{
    class OrbwalkerTargetIndicator
    {
        public static void initialize()
        {
            Drawing.OnDraw += Drawing_OnDraw;

            Logging.Write()(LogLevel.Info, "HuyNK Series SDK: OrbwalkerTargetIndicator initialized.");
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            var OrbwalkerTarget = Orbwalker.GetTarget(OrbwalkerMode.Orbwalk);

            if (OrbwalkerTarget != null)
                Drawing.DrawCircle(OrbwalkerTarget.Position, OrbwalkerTarget.BoundingRadius, System.Drawing.Color.Red);
        }
    }
}
