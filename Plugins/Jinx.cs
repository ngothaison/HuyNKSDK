using System;
using System.Linq;
using System.Collections.Generic;

using LeagueSharp;
using LeagueSharp.Common;
using LeagueSharp.SDK.Core;
using LeagueSharp.SDK.Core.UI.IMenu.Values;
using LeagueSharp.SDK.Core.Enumerations;
using LeagueSharp.SDK.Core.Extensions;
using LeagueSharp.SDK.Core.Utils;
using LeagueSharp.SDK.Core.Extensions.SharpDX;

using Collision = LeagueSharp.SDK.Core.Math.Collision;
using Color = System.Drawing.Color;

using SharpDX;
using HitChance = LeagueSharp.SDK.Core.Enumerations.HitChance;
using Menu = LeagueSharp.SDK.Core.UI.IMenu.Menu;
using SkillshotType = LeagueSharp.SDK.Core.Enumerations.SkillshotType;
using Spell = LeagueSharp.SDK.Core.Wrappers.Spell;

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
       
            
           
           
           //Set key
           
            Menu Skill = new Menu("Keys", "Cài đặt kill");
            Skill.Add(new MenuSlider("Q_Max_Range", "Q khi đủ tầm", 1050, 500, 1200));
            Skill.Add(new MenuSlider("W_Max_Range", "W khi đủ tầm", 900, 500, 1500));
            Skill.Add(new MenuSlider("R_Min_Range", "Tầm đánh R nhỏ nhât", 1000, 300, 1200));
            Skill.Add(new MenuSlider("R_Max_Range", "Tầm đánh R lớn nhất", 1000, 500, 20000));
       
            // Combo
            Menu ComboMenu = new Menu("Combo", "Combo");
            ComboMenu.Add(new MenuBool("UseQ", "Dùng  Q", true));
            ComboMenu.Add(new MenuBool("UseW", "Dùng W", true));
            ComboMenu.Add(new MenuBool("UseE", "Dùng E", true));
            ComboMenu.Add(new MenuBool("UseR", "Dùng R", true));
            ComboMenu.Add(new MenuBool("R_Nearest_Killable", "Giết khi gần Chết"));
            //Cấu rỉa
            Menu HarassMenu = new Menu("Harass", "Cấu Rỉa");
            HarassMenu.Add(new MenuBool("UseQ", "Dùng Q", true));
            HarassMenu.Add(new MenuBool("UseW", "Dùng W", true));
            //Đẩy đường
            Menu LaneClearMenu = new Menu("LaneClear", "Đẩy Đường");
            LaneClearMenu.Add(new MenuSeparator("Qseparator", "Q"));
            LaneClearMenu.Add(new MenuBool("UseQ", "Use Q", true));
            


            //Dọn rừng
            Menu JungleClearMenu = new Menu("JungleClear", "Dọn Rừng");
            JungleClearMenu.Add(new MenuBool("UseQ", "Use Q", true));
           
            //Tiện ích
            Menu MiscMenu = new Menu("Misc", "Tiện ích");
            MiscMenu.Add(new MenuBool("UseKillsteal", "Dùng R KS", true));
            MiscMenu.Add(new MenuBool("Misc_Use_WE", "Combo E + W", true));
            //Hiển thị
            Menu DrawingsMenu = new Menu("Drawings", "Hiển Thị");
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
            DrawingsMenu.Add(new MenuBool("Draw_R_Killable", "Tìm Muc tiêu ULti"));
            DrawingsMenu.Add(new MenuBool("Dont_R", "Không dùng R cho tướng"));
         
            DrawingsMenu.Add(new MenuSeparator("Dseparator", "DamageIndicator"));

            var UseDamageIndicator = new MenuBool("UseDamageIndicator", "Use DamageIndicator", true);
            var DamageIndicatorFillColor = new MenuColor("DamageIndicatorFillColor", "Color", SharpDX.Color.Goldenrod);

            UseDamageIndicator.ValueChanged += (object sender, EventArgs e) => { DamageIndicator.Enabled = UseDamageIndicator.Value; };
            DamageIndicatorFillColor.ValueChanged += (object sender, EventArgs e) => { DamageIndicator.FillColor = Color.FromArgb(DamageIndicatorFillColor.Color.ToBgra()); };

            DrawingsMenu.Add(UseDamageIndicator);
            DrawingsMenu.Add(DamageIndicatorFillColor);

            MenuProvider.MainMenu.Add(ComboMenu);
            MenuProvider.MainMenu.Add(Skill);
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
            Logging.Write()(LogLevel.Info, "HuyNK Series SDK: Jinx Loaded!");

        }
        private void Spellbook_OnCastSpell(Spellbook sender, SpellbookCastSpellEventArgs args)
        {
            if (!ObjectManager.Player.IsDead && sender.Owner.IsMe)
                if (args.Slot == SpellSlot.Q)
                    if (LastECastTime > Environment.TickCount - 500)
                        args.Process = false;
                    else
                        LastECastTime = Environment.TickCount;
        }

        private void Game_OnGameUpdate(EventArgs args)
        {
            if (!ObjectManager.Player.IsDead)
            {
                //check if player is dead
                if (ObjectManager.Player.IsDead) return;

                //adjust range
                if (Q.IsReady())
                    Q.Range = MenuProvider.MainMenu["Keys"]["Q_Max_Range"].GetValue<MenuSlider>().Value;
                if (W.IsReady())
                    W.Range = MenuProvider.MainMenu["Keys"]["W_Max_Range"].GetValue<MenuSlider>().Value;
                if (R.IsReady())
                    R.Range = MenuProvider.MainMenu["Keys"]["R_Max_Range"].GetValue<MenuSlider>().Value;


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
              //  CastWE();
                Killsteal();
              
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
                (Q.isReadyPerfectly() ? (float)LeagueSharp.Common.Damage.GetSpellDamage(ObjectManager.Player, Enemy, SpellSlot.Q) : 0);
        }

        private void Combo()
        {
           
            if (MenuProvider.MainMenu["Combo"]["UseQ"].GetValue<MenuBool>().Value)
            {
                if (!ObjectManager.Player.IsWindingUp)
                {
                    Cast_Q();
                }
            }
            if (MenuProvider.MainMenu["Combo"]["UseW"].GetValue<MenuBool>().Value && W.isReadyPerfectly())
            {
                if (!ObjectManager.Player.IsWindingUp)
                    W.CastOnBestTarget(W.Width, true);
            }
            if (MenuProvider.MainMenu["Combo"]["UseE"].GetValue<MenuBool>().Value && E.isReadyPerfectly())
              Cast_E();

        }

        private void Harass()
        {
            
            if (MenuProvider.MainMenu["Harass"]["UseQ"].GetValue<MenuBool>().Value && Q.isReadyPerfectly())
            {
                if (!ObjectManager.Player.IsWindingUp)
                {
                    Cast_Q();
                }
            }
            if (MenuProvider.MainMenu["Harass"]["UseW"].GetValue<MenuBool>().Value && W.isReadyPerfectly())
            {
                if (!ObjectManager.Player.IsWindingUp)
                    W.CastOnBestTarget(W.Width,true);
            }
            if (MenuProvider.MainMenu["Harass"]["UseE"].GetValue<MenuBool>().Value && E.isReadyPerfectly())
                E.CastOnBestTarget(E.Width, true);
        }

        private void LaneClear()
        {
           
        }

        private void JungleClear()
        {
            
        }

        private void Killsteal()
        {

            if (MenuProvider.MainMenu["Misc"]["UseKillsteal"].GetValue<MenuBool>().Value && R.isReadyPerfectly())
                if (MenuProvider.MainMenu["Drawings"]["Draw_R_Killable"].GetValue<MenuBool>().Value)
                {
                    
                    foreach (
                    var unit in
                        ObjectManager.Get<Obj_AI_Hero>()
                            .Where(x => x.IsValid && !x.IsDead && x.IsEnemy)
                            .OrderBy(x => x.Health))
                    {
                        var health = unit.Health + unit.HPRegenRate * 3 + 25;
                        if (Get_R_Dmg(unit) + 300 > health)
                        {
                            Drawing.DrawText(Drawing.Width * 0.39f, Drawing.Height * 0.80f, Color.DarkOrange,
                            "no con 300 mau, Ulti no khong dai ca ");
                        }
                        if (Get_R_Dmg(unit) > health)
                        {
                            Drawing.DrawText(Drawing.Width * 0.39f, Drawing.Height * 0.80f, Color.DarkOrange,
                            " ULTI LA CHET CHAC  ");
                            Vector2 wts = Drawing.WorldToScreen(unit.Position);
                            
                            Drawing.DrawText(wts[0] - 20, wts[1], Color.Red, "Muc tieu ne!!!");
                        

                        }
                    }

                    foreach (
                var unit in
                    ObjectManager.Get<Obj_AI_Hero>()
                        .Where(x => x.IsTargetable && !x.IsDead && x.IsEnemy)
                        .OrderBy(x => x.Health))
                    {
                        
                                var health = unit.Health + unit.HPRegenRate * 3 + 25;
                                if (Get_R_Dmg(unit) > health)
                                {
                                    R.Cast(unit);
                                    return;
                                }
                         
                    }
                }
        }

        private float Get_R_Dmg(Obj_AI_Hero target)
        {
            double dmg = 0;

            dmg += ObjectManager.Player.GetSpellDamage(target, SpellSlot.R);

            var rPred = _r2.GetPrediction(target);
            var collisionCount = rPred.CollisionObjects.Count;

            if (collisionCount >= 7)
                dmg = dmg * .3;
            else if (collisionCount != 0)
                dmg = dmg * ((10 - collisionCount) / 10);

          
            return (float)dmg;
        }
        private void CastWE()
        {
            if (W.IsReady() && E.IsReady())
            {
                var vec = ObjectManager.Player.ServerPosition + Vector3.Normalize(Game.CursorPos - ObjectManager.Player.ServerPosition) * E.Range;

                W.Cast(vec);
                E.Cast(vec);
            }
        }
        private bool IsCannon()
        {
         
            return  ObjectManager.Player.AttackRange > 525;
        }
        private void Cast_Q()
        {
            var dungQ = MenuProvider.MainMenu["Combo"]["UseQ"].GetValue<MenuBool>().Value;
            if (!Q.IsReady()) return;
            if (dungQ)
            {
                Obj_AI_Hero target = LeagueSharp.Common.TargetSelector.GetTarget(Q.Range, LeagueSharp.Common.TargetSelector.DamageType.Physical);

                if (target == null)
                    return;

                float distance = ObjectManager.Player.Position.Distance(target.Position);

                if (IsCannon())
                {
                    if (distance <= 600)
                        Q.Cast();
                }
                else
                {
                    if (distance > 600)
                        Q.Cast();
                }
            }
        }
        private void Cast_E()
        {
            var target = LeagueSharp.Common.TargetSelector.GetTarget(E.Range, LeagueSharp.Common.TargetSelector.DamageType.Magical);

            if (E.IsReady() && target != null)
            {
                 
                       
                       
                            E.CastOnBestTarget(W.Width, true);
                  
                
            }
        }

        
    }
}
