using System;
using System.Linq;
using System.Collections.Generic;

using LeagueSharp;
using LeagueSharp.SDK.Core;
using LeagueSharp.SDK.Core.UI.IMenu;
using LeagueSharp.SDK.Core.UI.IMenu.Values;
using LeagueSharp.SDK.Core.Enumerations;
using LeagueSharp.SDK.Core.Wrappers;
using LeagueSharp.SDK.Core.Extensions;
using LeagueSharp.SDK.Core.Utils;
using LeagueSharp.SDK.Core.Extensions.SharpDX;

using Collision = LeagueSharp.SDK.Core.Math.Collision;
using Color = System.Drawing.Color;

using SharpDX;

namespace HuyNK_Series_SDK.Plugins
{
    class Jinx
    {
        private Spell Q, W, E, R,_r2;
        private int LastECastTime;
        public Jinx()
        {

            //Spell
            Q = new Spell(SpellSlot.Q, 725);
            //Q.SetSkillshot(0.25f, 60f, 2000f, true, SkillshotType.SkillshotLine);

            W = new Spell(SpellSlot.W, 1500f);
            W.SetSkillshot(0.6f, 60f, 3300f, true, SkillshotType.SkillshotLine);

            E = new Spell(SpellSlot.E, 900f);
            E.SetSkillshot(0.7f, 120f, 1750f, false, SkillshotType.SkillshotCircle);

            R = new Spell(SpellSlot.R, 20000);
            R.SetSkillshot(0.6f, 140f, 1700f, false, SkillshotType.SkillshotLine);

            _r2 = new Spell(SpellSlot.R, 20000);
            _r2.SetSkillshot(0.6f, 140f, 1700f, false, SkillshotType.SkillshotLine);
            //Menu
            //Range SKill
            Menu RangeSkill = new Menu("Tầm Đánh","Rangeskill");
            RangeSkill.Add(new MenuSlider("Use_Q_Max_Range", "Tầm đổi Q", 1200, 500, 1200));
            RangeSkill.Add(new MenuSlider("Use_W_Max_Range", "Tầm Dùng W", 1500, 500, 1500));
            RangeSkill.Add(new MenuSlider("Use_R_Min_Range", "Tầm Dùng R Nhỏ Nhất", 1000, 0, 1000));
            RangeSkill.Add(new MenuSlider("Use_R_Max_Range", "Tầm Dùng R Lớn Nhất", 2000, 0, 20000));
           //combo
            Menu ComboMenu = new Menu("Đánh Nhau", "Combo");
            ComboMenu.Add(new MenuBool("UseQ", "Dùng  Q", true));
            ComboMenu.Add(new MenuBool("UseW", "Dùng W", true));
            ComboMenu.Add(new MenuBool("UseE", "Dùng E", true));
            //Harrass
            Menu HarassMenu = new Menu("Cấu Rỉa", "Harass");
            HarassMenu.Add(new MenuBool("UseQ", "Use Q", true));
            HarassMenu.Add(new MenuBool("UseW", "Use W", true));
            //laneclear
            Menu LaneClearMenu = new Menu("Đẩy Đường", "LaneClear");
            LaneClearMenu.Add(new MenuBool("UseQ", "Use Q", true));
           
            //Jungle 
            Menu JungleClearMenu = new Menu("JungleClear", "JungleClear");
            JungleClearMenu.Add(new MenuBool("UseQ", "Use Q", true));
          
            //Misc
            Menu MiscMenu = new Menu("Misc", "Misc");
            MiscMenu.Add(new MenuBool("UseKillsteal", "Use Killsteal", true));
            MiscMenu.Add(new MenuBool("UseMobsteal", "Use Mobsteal", true));
            // Drawing 
            Menu DrawingsMenu = new Menu("Drawings", "Drawings");
            DrawingsMenu.Add(new MenuSeparator("Qseparator", "Q"));
            DrawingsMenu.Add(new MenuBool("DrawQ", "Draw Q Range", true));
            DrawingsMenu.Add(new MenuColor("QColor", "Color", SharpDX.Color.Blue));

            DrawingsMenu.Add(new MenuSeparator("Wseparator", "W"));
            DrawingsMenu.Add(new MenuBool("DrawW", "Draw W Range"));
            DrawingsMenu.Add(new MenuColor("WColor", "Color", SharpDX.Color.Blue));

            DrawingsMenu.Add(new MenuSeparator("Eseparator", "E"));
            DrawingsMenu.Add(new MenuBool("DrawE", "Draw E Range", true));
            DrawingsMenu.Add(new MenuColor("EColor", "Color", SharpDX.Color.Blue));

            DrawingsMenu.Add(new MenuSeparator("Rseparator", "R"));
            DrawingsMenu.Add(new MenuBool("DrawR", "Draw R Range"));
            DrawingsMenu.Add(new MenuColor("RColor", "Color", SharpDX.Color.Blue));

            DrawingsMenu.Add(new MenuSeparator("Dseparator", "DamageIndicator"));

            var UseDamageIndicator = new MenuBool("UseDamageIndicator", "Use DamageIndicator", true);
            var DamageIndicatorFillColor = new MenuColor("DamageIndicatorFillColor", "Color", SharpDX.Color.Goldenrod);

            UseDamageIndicator.ValueChanged += (object sender, EventArgs e) => { DamageIndicator.Enabled = UseDamageIndicator.Value; };
            DamageIndicatorFillColor.ValueChanged += (object sender, EventArgs e) => { DamageIndicator.FillColor = Color.FromArgb(DamageIndicatorFillColor.Color.ToBgra()); };

            DrawingsMenu.Add(UseDamageIndicator);
            DrawingsMenu.Add(DamageIndicatorFillColor);

            MenuProvider.MainMenu.Add(ComboMenu);
            MenuProvider.MainMenu.Add(HarassMenu);
            MenuProvider.MainMenu.Add(LaneClearMenu);
            MenuProvider.MainMenu.Add(JungleClearMenu);
            MenuProvider.MainMenu.Add(MiscMenu);
            MenuProvider.MainMenu.Add(DrawingsMenu);

            //Event
            Game.OnUpdate += Game_OnGameUpdate;
            Drawing.OnDraw += Game_OnDraw;
            Spellbook.OnCastSpell += Spellbook_OnCastSpell;
            DamageIndicator.DamageToUnit = GetComboDamage;

            //Loaded
            Logging.Write()(LogLevel.Info, "HuyNK Series SDK: Jinx  Loaded!");

        }
        private void Spellbook_OnCastSpell(Spellbook sender, SpellbookCastSpellEventArgs args)
        {
            if (!ObjectManager.Player.IsDead && sender.Owner.IsMe)
                if (args.Slot == SpellSlot.E)
                    if (LastECastTime > Environment.TickCount - 500)
                        args.Process = false;
                    else
                        LastECastTime = Environment.TickCount;
        }

        private void Game_OnGameUpdate(EventArgs args)
        {
            if (!ObjectManager.Player.IsDead)
            {
                switch (Orbwalker.ActiveMode)
                {
                    case OrbwalkerMode.Orbwalk:
                        Combo();
                        break;
                    case OrbwalkerMode.Hybrid:
                        Harass();
                        break;
                    case OrbwalkerMode.LaneClear:
                        LaneClear();
                        JungleClear();
                        break;
                }

                Killsteal();
                Mobsteal();
            }
        }

        private void Game_OnDraw(EventArgs args)
        {
            if (!ObjectManager.Player.IsDead)
            {
                if (MenuProvider.MainMenu["Drawings"]["DrawQ"].GetValue<MenuBool>().Value && Q.isReadyPerfectly())
                    Drawing.DrawCircle(GameObjects.Player.Position, Q.Range, Color.FromArgb(MenuProvider.MainMenu["Drawings"]["QColor"].GetValue<MenuColor>().Color.ToBgra()));

                if (MenuProvider.MainMenu["Drawings"]["DrawW"].GetValue<MenuBool>().Value && W.isReadyPerfectly())
                    Drawing.DrawCircle(GameObjects.Player.Position, W.Range, Color.FromArgb(MenuProvider.MainMenu["Drawings"]["WColor"].GetValue<MenuColor>().Color.ToBgra()));

                if (MenuProvider.MainMenu["Drawings"]["DrawE"].GetValue<MenuBool>().Value && E.isReadyPerfectly())
                    Drawing.DrawCircle(GameObjects.Player.Position, E.Range, Color.FromArgb(MenuProvider.MainMenu["Drawings"]["EColor"].GetValue<MenuColor>().Color.ToBgra()));

                if (MenuProvider.MainMenu["Drawings"]["DrawR"].GetValue<MenuBool>().Value && R.isReadyPerfectly())
                    Drawing.DrawCircle(GameObjects.Player.Position, R.Range, Color.FromArgb(MenuProvider.MainMenu["Drawings"]["RColor"].GetValue<MenuColor>().Color.ToBgra()));
            }
        }

        private float GetComboDamage(Obj_AI_Hero Enemy)
        {
            return
                (Q.isReadyPerfectly() ? (float)LeagueSharp.Common.Damage.GetSpellDamage(ObjectManager.Player, Enemy, SpellSlot.Q) : 0)+
                (W.isReadyPerfectly() ? (float)LeagueSharp.Common.Damage.GetSpellDamage(ObjectManager.Player, Enemy, SpellSlot.W) : 0)+
                (R.isReadyPerfectly() ? (float)LeagueSharp.Common.Damage.GetSpellDamage(ObjectManager.Player, Enemy, SpellSlot.R) : 0);
        }

        private void Combo()
        {
            if (MenuProvider.MainMenu["Combo"]["UseE"].GetValue<MenuBool>().Value && E.isReadyPerfectly())
            {
                if (!ObjectManager.Player.IsRanged)
                    E.CastOnBestTarget();
            }

            if (MenuProvider.MainMenu["Combo"]["UseQ"].GetValue<MenuBool>().Value && Q.isReadyPerfectly())
            {
                if (!ObjectManager.Player.IsWindingUp)
                    Q.CastOnBestTarget();
            }

            if (MenuProvider.MainMenu["Combo"]["UseW"].GetValue<MenuBool>().Value && W.isReadyPerfectly())
            {
                if (!ObjectManager.Player.IsRanged)
                    W.CastOnBestTarget();
            }
               
        }

        private void Harass()                                                                         
        {
            if (MenuProvider.MainMenu["Combo"]["UseE"].GetValue<MenuBool>().Value && E.isReadyPerfectly())
            {
                if (!ObjectManager.Player.CanCast)
                    E.CastOnBestTarget();
            }
            if (MenuProvider.MainMenu["Combo"]["UseQ"].GetValue<MenuBool>().Value && Q.isReadyPerfectly())
            {
                if (!ObjectManager.Player.IsWindingUp)
                    Q.CastOnBestTarget();
            }

            if (MenuProvider.MainMenu["Combo"]["UseW"].GetValue<MenuBool>().Value && W.isReadyPerfectly())
            {
                if (!ObjectManager.Player.IsRanged)
                    W.CastOnBestTarget();
            }
           
        }

        private void LaneClear()
        {
           
        }

        private void JungleClear()
        {
            
        }

        private void Killsteal()
        {
            
        }

        private void Mobsteal()
        {
        }
    }
}
 
