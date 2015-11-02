using System;
using System.Diagnostics.Eventing.Reader;
using System.Drawing.Printing;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Security.AccessControl;
using LeagueSharp;
using LeagueSharp.Common;
using LeagueSharp.Common.Data;

using Color = System.Drawing.Color;

namespace PewPewTristana
{
    internal class Program
    {
        public const string ChampName = "Tristana";
        public static HpBarIndicator Hpi = new HpBarIndicator();
        public static Menu Config;
        public static Orbwalking.Orbwalker Orbwalker;
        public static Spell Q;
        public static Spell W;
        public static Spell E;
        public static Spell R;
        public static int SpellRangeTick;
        private static SpellSlot Ignite;
        private static readonly Obj_AI_Hero player = ObjectManager.Player;

        private static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += OnLoad;
        }

        private static void OnLoad(EventArgs args)
        {
            if (player.ChampionName != ChampName)
                return;

            Notifications.AddNotification("PewPewTristana loaded!", 5000);
            Notifications.AddNotification("Được Việt Hóa bởi nhóm L# VN!", 5000);
            //Ability Information - Range - Variables.
            Q = new Spell(SpellSlot.Q, 585);

            //RocketJump Settings
            W = new Spell(SpellSlot.W, 900);
            W.SetSkillshot(0.25f, 150, 1200, false, SkillshotType.SkillshotCircle);

            E = new Spell(SpellSlot.E, 630);
            R = new Spell(SpellSlot.R, 630);


            Config = new Menu("PewPewTristana", "Tristana", true);
            Orbwalker = new Orbwalking.Orbwalker(Config.AddSubMenu(new Menu("Thả Diều", "Orbwalker")));
            TargetSelector.AddToMenu(Config.AddSubMenu(new Menu("Chọn đối tượng", "Target Selector")));

            //COMBOMENU

            var combo = Config.AddSubMenu(new Menu("Cài đặt tự dùng kỹ năng", "Combo Settings"));
            var harass = Config.AddSubMenu(new Menu("Cài đặt tự rỉa máu", "Harass Settings"));
            var drawing = Config.AddSubMenu(new Menu("Cài đặt vẽ", "Draw"));

            combo.SubMenu("Giới hạn dùng mana").AddItem(new MenuItem("wmana", "[W] Mana %").SetValue(new Slider(10, 100, 0)));
            combo.SubMenu("Giới hạn dùng mana").AddItem(new MenuItem("emana", "[E] Mana %").SetValue(new Slider(10, 100, 0)));
            combo.SubMenu("Giới hạn dùng mana").AddItem(new MenuItem("rmana", "[R] Mana %").SetValue(new Slider(15, 100, 0)));

            combo.SubMenu("Cài đặt Q").AddItem(new MenuItem("UseQ", "Dùng Q").SetValue(true));

            combo.SubMenu("Cài đặt Q").AddItem(new MenuItem("QonE", "Dùng Q nếu E được kích hoạt trên mục tiêu").SetValue(false));


            combo.SubMenu("Cài đặt W").AddItem(new MenuItem("UseW", "Dùng Cú Nhảy Tên Lửa").SetValue(false));
            combo.SubMenu("Cài đặt W")
                .AddItem(new MenuItem("wnear", "Enemy Count").SetValue(new Slider(2, 5, 1)));
            combo.SubMenu("Cài đặt W").AddItem(new MenuItem("whp", "Own HP %").SetValue(new Slider(75, 100, 0)));
            combo.SubMenu("Cài đặt W")
                .AddItem(new MenuItem("wturret", "Không nhảy vào trụ ").SetValue(true));

            combo.SubMenu("Cài đặt E").AddItem(new MenuItem("UseE", "Dùng Explosive Charge").SetValue(true));
            combo.SubMenu("Cài đặt E")
                .AddItem(new MenuItem("UseEW", "Dùng W on E stack count").SetValue(false));
            combo.SubMenu("Cài đặt E").AddItem(new MenuItem("estack", "Đếm E stack").SetValue(new Slider(3, 4, 1)));
            combo.SubMenu("Cài đặt E")
            .AddItem(new MenuItem("enear", "Enemy Count").SetValue(new Slider(2, 5, 1)));
            combo.SubMenu("Cài đặt E").AddItem(new MenuItem("ehp", "Máu của địch %").SetValue(new Slider(45, 100, 0)));
            combo.SubMenu("Cài đặt E").AddItem(new MenuItem("ohp", "Máu bản thân HP %").SetValue(new Slider(65, 100, 0)));

            combo.SubMenu("Cài đặt R")
            .AddItem(new MenuItem("UseR", "Dùng R [Kết Thúc] (Luôn Dùng) ").SetValue(new KeyBind('K', KeyBindType.Toggle)));
            combo.SubMenu("Cài đặt R").AddItem(new MenuItem("UseRE", "Dùng ER [Kêt thúc combo]").SetValue(true));
            combo.SubMenu("Cài đặt R").AddItem(new MenuItem("manualr", "Dùng R lên mục tiêu").SetValue(new KeyBind('R', KeyBindType.Press)));



            combo.SubMenu("Cài đặt tự dùng trang bị")
                .AddItem(new MenuItem("useGhostblade", "Dùng Kiếm Ma Youmuuo").SetValue(true));
            combo.SubMenu("Cài đặt tự dùng trang bị")
                .AddItem(new MenuItem("UseBOTRK", "Dùng Gươm Vô Danh").SetValue(true));
            combo.SubMenu("Cài đặt tự dùng trang bị")
                .AddItem(new MenuItem("eL", "  Phần trăm máu địch").SetValue(new Slider(80, 100, 0)));
            combo.SubMenu("Cài đặt tự dùng trang bị")
                .AddItem(new MenuItem("oL", "  Phần trăm máu bản thân").SetValue(new Slider(65, 100, 0)));
            combo.SubMenu("Cài đặt tự dùng trang bị").AddItem(new MenuItem("UseBilge", "Dùng kiếm hải tặc").SetValue(true));
            combo.SubMenu("Cài đặt tự dùng trang bị")
                .AddItem(new MenuItem("HLe", "  Phần trăm máu địch").SetValue(new Slider(80, 100, 0)));
            combo.SubMenu("Cài đặt phép bổ trợ").AddItem(new MenuItem("UseIgnite", "Dùng Thiêu đốt").SetValue(true));


            //LANECLEARMENU
            Config.SubMenu("Cài đặt đẩy đường")
                .AddItem(new MenuItem("laneQ", "Dùng Q").SetValue(true));
            Config.SubMenu("Cài đặt đẩy đường")
                .AddItem(new MenuItem("laneE", "Dùng E").SetValue(true));
            Config.SubMenu("Cài đặt đẩy đường")
                .AddItem(new MenuItem("eturret", "Dùng E lên trụ").SetValue(true));
            Config.SubMenu("Cài đặt đẩy đường")
                .AddItem(new MenuItem("laneclearmana", "Phần trăm mana").SetValue(new Slider(30, 100, 0)));

            //JUNGLEFARMMENU
            Config.SubMenu("Cài đặt đi rừng")
                .AddItem(new MenuItem("jungleQ", "Dùng Q").SetValue(true));
            Config.SubMenu("Cài đặt đi rừng")
                .AddItem(new MenuItem("jungleE", "Dùng E").SetValue(true));
            Config.SubMenu("Cài đặt đi rừng")
                .AddItem(new MenuItem("jungleclearmana", "Phần trăm mana").SetValue(new Slider(30, 100, 0)));

            drawing.AddItem(new MenuItem("Draw_Disabled", "Tắt vẽ tất cả").SetValue(false));
            drawing.SubMenu("Cài đặt vẽ").AddItem(new MenuItem("drawRtoggle", "Draw R finisher toggle").SetValue(true));
            drawing.SubMenu("Cài đặt vẽ").AddItem(new MenuItem("drawtargetcircle", "Vẽ mục tiêu được chọn").SetValue(true));

            drawing.AddItem(new MenuItem("Qdraw", "Vẽ vòng Q").SetValue(new Circle(true, Color.Orange)));
            drawing.AddItem(new MenuItem("Wdraw", "Vẽ vòng W").SetValue(new Circle(true, Color.DarkOrange)));
            drawing.AddItem(new MenuItem("Edraw", "Vẽ vòng E").SetValue(new Circle(true, Color.AntiqueWhite)));
            drawing.AddItem(new MenuItem("Rdraw", "Vẽ vòng R").SetValue(new Circle(true, Color.LawnGreen)));
            drawing.AddItem(new MenuItem("CircleThickness", "Độ dày của vòng tròn").SetValue(new Slider(1, 30, 0)));

            harass.AddItem(new MenuItem("harassQ", "Dùng Q").SetValue(true));
            harass.AddItem(new MenuItem("harassE", "Dùng E").SetValue(true));
            harass.AddItem(new MenuItem("harassmana", "Phần trăm mana").SetValue(new Slider(30, 100, 0)));

            drawing.AddItem(new MenuItem("disable.dmg", "Tắt tính sát thương").SetValue(false));
            drawing.AddItem(new MenuItem("dmgdrawer", "[Tính sát thương]:", true).SetValue(new StringList(new[] { "Custom", "Common" }, 1)));

            Config.SubMenu("Cài đặt hỗn hợp").AddItem(new MenuItem("interrupt", "Interrupt Spells").SetValue(true));
            Config.SubMenu("Cài đặt hỗn hợp").AddItem(new MenuItem("antigap", "Antigapcloser").SetValue(true));
            Config.SubMenu("Cài đặt hỗn hợp").AddItem(new MenuItem("AntiRengar", "Chống Rengar nhảy vào").SetValue(true));
            Config.SubMenu("Cài đặt hỗn hợp").AddItem(new MenuItem("AntiKhazix", "Chống Khazix nhảy vào").SetValue(true));

            Config.AddToMainMenu();

            Drawing.OnDraw += OnDraw;
            Obj_AI_Base.OnLevelUp += TristRange;
            Game.OnUpdate += Game_OnGameUpdate;
            Drawing.OnEndScene += OnEndScene;
            Interrupter2.OnInterruptableTarget += Interrupter2_OnInterruptableTarget;
            AntiGapcloser.OnEnemyGapcloser += AntiGapCloser_OnEnemyGapcloser;
            GameObject.OnCreate += GameObject_OnCreate;



        }

        private static void TristRange(Obj_AI_Base sender, EventArgs args)
        {
            var lvl = (7 * (player.Level - 1));
            Q.Range = 605 + lvl;
            E.Range = 635 + lvl;
            R.Range = 635 + lvl;
        }



        private static void GameObject_OnCreate(GameObject sender, EventArgs args)
        {

            var rengar = HeroManager.Enemies.Find(h => h.ChampionName.Equals("Rengar")); //<---- Credits to Asuna (Couldn't figure out how to cast R to Sender so I looked at his vayne ^^
            if (rengar != null)

            if (sender.Name == ("Rengar_LeapSound.troy") && Config.Item("AntiRengar").GetValue<bool>() &&
                sender.Position.Distance(player.Position) < R.Range)
                R.Cast(rengar);

            var khazix = HeroManager.Enemies.Find(h => h.ChampionName.Equals("Khazix"));
            if (khazix != null)

                if (sender.Name == ("Khazix_Base_E_Tar.troy") && Config.Item("AntiKhazix").GetValue<bool>() &&
                   sender.Position.Distance(player.Position) <= 300)
                    R.Cast(khazix);

        }
        private static void Interrupter2_OnInterruptableTarget(Obj_AI_Hero sender, Interrupter2.InterruptableTargetEventArgs args)
        {

            if (R.IsReady() && sender.IsValidTarget(E.Range) && Config.Item("interrupt").GetValue<bool>())
                R.CastOnUnit(sender);
        }

        private static void AntiGapCloser_OnEnemyGapcloser(ActiveGapcloser gapcloser)
        {
            if (R.IsReady() && gapcloser.Sender.IsValidTarget(E.Range) && Config.Item("antigap").GetValue<bool>())
                R.CastOnUnit(gapcloser.Sender);
        }

        private static void OnEndScene(EventArgs args)
        {
            if (Config.Item("disable.dmg").GetValue<bool>())
            {
                Utility.HpBarDamageIndicator.Enabled = false;
                return;
            }
            int mode = Config.Item("dmgdrawer", true).GetValue<StringList>().SelectedIndex;
            if (mode == 0)
            {
                foreach (var enemy in
                    ObjectManager.Get<Obj_AI_Hero>().Where(ene => !ene.IsDead && ene.IsEnemy && ene.IsVisible))
                {
                    Hpi.unit = enemy;
                    Hpi.drawDmg(CalcDamage(enemy), Color.Green);
                    Utility.HpBarDamageIndicator.Enabled = false;
                }             
            }
            if (mode == 1)
            {
                Utility.HpBarDamageIndicator.DamageToUnit = CalcDamage;
                Utility.HpBarDamageIndicator.Color = Color.Aqua;
                Utility.HpBarDamageIndicator.Enabled = true;

            }
        }

        private static void combo()
        {
            var target = TargetSelector.GetTarget(W.Range, TargetSelector.DamageType.Physical);
            if (target == null || !target.IsValidTarget())
                return;

            if (Q.IsReady() && target.IsValidTarget(Q.Range))
                qlogic();

            var emana = Config.Item("emana").GetValue<Slider>().Value;

            if (E.IsReady() && Config.Item("UseE").GetValue<bool>()
            && player.ManaPercent >= emana)
                E.CastOnUnit(target);


            var wmana = Config.Item("wmana").GetValue<Slider>().Value;
            if (W.IsReady() && target.IsValidTarget(W.Range) && target.HasBuff("tristanaecharge"))
            {
                wlogic();
            }

            if (R.IsReady() && target.IsValidTarget(R.Range))
            {
                rlogic();
            }

            if (Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Combo)
                items();

            if (Config.Item("wturret").GetValue<bool>() && target.Position.UnderTurret(true))
                return;

            if (target.HasBuff("deathdefiedbuff"))
                return;

            if (target.HasBuff("KogMawIcathianSurprise"))
                return;

            if (target.IsInvulnerable)
                return;

    
            if (W.IsReady() && target.IsValidTarget(W.Range)
            && Config.Item("UseW").GetValue<bool>()
            && target.Position.CountEnemiesInRange(700) <= Config.Item("wnear").GetValue<Slider>().Value
            && player.HealthPercent >= Config.Item("whp").GetValue<Slider>().Value
            && CalcDamage(target) > target.Health - 100
            && player.ManaPercent >= wmana)

                W.Cast(target.Position);
        }
        public static float CalcDamage(Obj_AI_Base target)
        {
            //Calculate Combo Damage
            float damage = (float)player.GetAutoAttackDamage(target, true) * (1 + player.Crit);

            Ignite = player.GetSpellSlot("summonerdot");

            if (Ignite.IsReady())
                damage += (float)player.GetSummonerSpellDamage(target, Damage.SummonerSpell.Ignite);

            if (Items.HasItem(3153) && Items.CanUseItem(3153))
                damage += (float)player.GetItemDamage(target, Damage.DamageItems.Botrk); //ITEM BOTRK

            if (Items.HasItem(3144) && Items.CanUseItem(3144))
                damage += (float)player.GetItemDamage(target, Damage.DamageItems.Bilgewater); //ITEM BOTRK

            if (Config.Item("UseE").GetValue<bool>()) // edamage
            {
                if (E.IsReady())
                {
                    damage += E.GetDamage(target);
                }
            }

            if (R.IsReady() && Config.Item("UseR").GetValue<KeyBind>().Active) // rdamage
            {

                damage += R.GetDamage(target);
            }

            if (W.IsReady() && Config.Item("UseW").GetValue<bool>())
            {
                damage += W.GetDamage(target);
            }
            return damage;


        }
        private static void wlogic()
        {
            var wmana = Config.Item("wmana").GetValue<Slider>().Value;

            var target = TargetSelector.GetTarget(W.Range, TargetSelector.DamageType.Physical);
            if (target == null || !target.IsValidTarget())
                return;

            if (Config.Item("wturret").GetValue<bool>() && target.Position.UnderTurret(true))
                return;
            if (target.HasBuff("deathdefiedbuff"))
                return;
            if (target.HasBuff("KogMawIcathianSurprise", true))
                return;
            if (target.IsInvulnerable)
                return;

            if (target.Buffs.Find(buff => buff.Name == "tristanaecharge").Count >= Config.Item("estack").GetValue<Slider>().Value
                && Config.Item("UseEW").GetValue<bool>()
                && target.Position.CountEnemiesInRange(700) <= Config.Item("enear").GetValue<Slider>().Value
                && target.HealthPercent <= Config.Item("ehp").GetValue<Slider>().Value
                && player.HealthPercent >= Config.Item("ohp").GetValue<Slider>().Value
                && player.ManaPercent >= wmana)

                W.Cast(target);
        }

        private static void qlogic()
        {
            var target = TargetSelector.GetTarget(Q.Range, TargetSelector.DamageType.Physical);
            if (target == null || !target.IsValid) return;

            if (Config.Item("QonE").GetValue<bool>() && !target.HasBuff("tristanaecharge"))
                return;

            if (Q.IsReady() && Config.Item("UseQ").GetValue<bool>() && target.IsValidTarget(Q.Range))
                Q.Cast(player);
        }




        private static void rlogic()
        {
            var target = TargetSelector.GetTarget(R.Range, TargetSelector.DamageType.Magical);
            var estacks = target.Buffs.Find(buff => buff.Name == "tristanaecharge").Count;
            var erdamage = (E.GetDamage(target) * ((0.30 * estacks) + 1) + R.GetDamage(target));
            if (target == null || !target.IsValid)
                return;

            if (Config.Item("manualr").GetValue<KeyBind>().Active && R.IsReady())
                R.CastOnUnit(target);

            if (Config.Item("UseRE").GetValue<bool>()
                && R.IsReady()
                && Config.Item("UseR").GetValue<KeyBind>().Active
                && target.HasBuff("tristanaecharge") && erdamage - 2 * target.Level > target.Health)
            {
                R.CastOnUnit(target);
            }

            if (Config.Item("UseR").GetValue<KeyBind>().Active && R.IsReady() &&
                R.GetDamage(target) > target.Health)
            {
                R.CastOnUnit(target);
            }

        }
        private static float IgniteDamage(Obj_AI_Hero target)
        {
            if (Ignite == SpellSlot.Unknown || player.Spellbook.CanUseSpell(Ignite) != SpellState.Ready)
                return 0f;
            return (float)player.GetSummonerSpellDamage(target, Damage.SummonerSpell.Ignite);
        }

        private static void items()
        {
            Ignite = player.GetSpellSlot("summonerdot");
            var target = TargetSelector.GetTarget(Q.Range, TargetSelector.DamageType.Physical);
            if (target == null || !target.IsValidTarget())
                return;

            var botrk = LeagueSharp.Common.Data.ItemData.Blade_of_the_Ruined_King.GetItem();
            var Ghost = LeagueSharp.Common.Data.ItemData.Youmuus_Ghostblade.GetItem();
            var cutlass = LeagueSharp.Common.Data.ItemData.Bilgewater_Cutlass.GetItem();

            if (botrk.IsReady() && botrk.IsOwned(player) && botrk.IsInRange(target)
            && target.HealthPercent <= Config.Item("eL").GetValue<Slider>().Value
            && Config.Item("UseBOTRK").GetValue<bool>())

                botrk.Cast(target);

            if (botrk.IsReady() && botrk.IsOwned(player) && botrk.IsInRange(target)
                && player.HealthPercent <= Config.Item("oL").GetValue<Slider>().Value
                && Config.Item("UseBOTRK").GetValue<bool>())

                botrk.Cast(target);

            if (cutlass.IsReady() && cutlass.IsOwned(player) && cutlass.IsInRange(target) &&
                target.HealthPercent <= Config.Item("HLe").GetValue<Slider>().Value
                && Config.Item("UseBilge").GetValue<bool>())

                cutlass.Cast(target);

            if (Ghost.IsReady() && Ghost.IsOwned(player) && target.IsValidTarget(E.Range)
                && Config.Item("useGhostblade").GetValue<bool>())

                Ghost.Cast();

            if (player.Distance(target) <= 600 && IgniteDamage(target) > target.Health &&
                Config.Item("UseIgnite").GetValue<bool>())
                player.Spellbook.CastSpell(Ignite, target);
        }
        private static void Game_OnGameUpdate(EventArgs args)
        {
            switch (Orbwalker.ActiveMode)
            {
                case Orbwalking.OrbwalkingMode.Combo:
                    combo();
                    break;
                case Orbwalking.OrbwalkingMode.Mixed:
                    harass();
                    break;
                case Orbwalking.OrbwalkingMode.LaneClear:
                    Laneclear();
                    Jungleclear();
                    break;
            }
        }

        private static void harass()
        {
            var harassmana = Config.Item("harassmana").GetValue<Slider>().Value;
            var target = TargetSelector.GetTarget(Orbwalking.GetRealAutoAttackRange(player), TargetSelector.DamageType.Physical);
            if (target == null || !target.IsValid)
                return;


            if (E.IsReady()
                && Config.Item("harassE").GetValue<bool>()
                && target.IsValidTarget(E.Range)
                && player.ManaPercent >= harassmana)

                E.CastOnUnit(target);

            if (Q.IsReady()
                && Config.Item("harassQ").GetValue<bool>()
                && target.IsValidTarget(Orbwalking.GetRealAutoAttackRange(player))
                && player.ManaPercent >= harassmana)

                Q.Cast(player);
        }

        private static void Laneclear()
        {
            var lanemana = Config.Item("laneclearmana").GetValue<Slider>().Value;
            var MinionsQ = MinionManager.GetMinions(ObjectManager.Player.ServerPosition, Orbwalking.GetRealAutoAttackRange(player));
            var allMinionsE = MinionManager.GetMinions(ObjectManager.Player.ServerPosition, E.Range);
            var AA = MinionManager.GetMinions(ObjectManager.Player.ServerPosition, Orbwalking.GetRealAutoAttackRange(player));

            var Efarmpos = W.GetCircularFarmLocation(allMinionsE, 200);


            if (MinionsQ.Count >= 2
                && Config.Item("laneQ").GetValue<bool>()
                && player.ManaPercent >= lanemana)
            {
                Q.Cast(player);
            }

            foreach (var minion in allMinionsE)
            {
                if (minion == null) return;

                if (Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.LaneClear
                    && minion.IsValidTarget(E.Range) && Efarmpos.MinionsHit > 2
                    && allMinionsE.Count >= 2 && Config.Item("laneE").GetValue<bool>()
                    && player.ManaPercent >= lanemana)

                    E.CastOnUnit(minion);
            }


            foreach (var turret in
                ObjectManager.Get<Obj_AI_Turret>()
                    .Where(t =>t.IsValidTarget() && player.Distance(t.Position) < Orbwalking.GetRealAutoAttackRange(player) && t != null))
            {
                if (Config.Item("eturret").GetValue<bool>())
                {
                    E.Cast(turret);
                }
            }

        }
        private static void Jungleclear()
        {
            var jlanemana = Config.Item("jungleclearmana").GetValue<Slider>().Value;
            var MinionsQ = MinionManager.GetMinions(ObjectManager.Player.ServerPosition, Orbwalking.GetRealAutoAttackRange(player), MinionTypes.All, MinionTeam.Neutral, MinionOrderTypes.MaxHealth);
            var MinionsE = MinionManager.GetMinions(ObjectManager.Player.ServerPosition, E.Range + W.Width - 50, MinionTypes.All, MinionTeam.Neutral, MinionOrderTypes.MaxHealth);

            var Efarmpos = W.GetCircularFarmLocation(MinionsE, W.Width - +100);
            var AA = MinionManager.GetMinions(ObjectManager.Player.ServerPosition, Orbwalking.GetRealAutoAttackRange(player));

            if (Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.LaneClear
                && MinionsQ.Count >= 1 && Config.Item("jungleQ").GetValue<bool>()
                && player.ManaPercent >= jlanemana)
                Q.Cast(player);

            foreach (var minion in MinionsE)
                if (Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.LaneClear && minion.IsValidTarget(E.Range)
                    && Efarmpos.MinionsHit >= 1
                    && MinionsE.Count >= 1
                    && Config.Item("jungleE").GetValue<bool>()
                    && player.ManaPercent >= jlanemana)

                    E.CastOnUnit(minion);

        }

        private static void OnDraw(EventArgs args)
        {
            {

            }

            //Draw Skill Cooldown on Champ
            var pos = Drawing.WorldToScreen(ObjectManager.Player.Position);

            if (Config.Item("UseR").GetValue<KeyBind>().Active && Config.Item("drawRtoggle").GetValue<bool>())
                Drawing.DrawText(pos.X - 50, pos.Y + 50, Color.LawnGreen, "[R] Finisher: ON");
            else if (Config.Item("drawRtoggle").GetValue<bool>())
                Drawing.DrawText(pos.X - 50, pos.Y + 50, Color.Tomato, "[R] Finisher:OFF");

            if (Config.Item("Draw_Disabled").GetValue<bool>())
                return;

           // if (Config.Item("Qdraw").GetValue<Circle>().Active)
              //  if (Q.Level > 0)
                //    Render.Circle.DrawCircle(ObjectManager.Player.Position, Q.Range, Q.IsReady() ? Config.Item("Qdraw").GetValue<Circle>().Color : Color.Red,
                      //                                  Config.Item("CircleThickness").GetValue<Slider>().Value);
            if (Config.Item("Qdraw").GetValue<Circle>().Active)
                if (Q.Level > 0)
                    Render.Circle.DrawCircle(ObjectManager.Player.Position, Q.Range, Q.IsReady() ? Config.Item("Qdraw").GetValue<Circle>().Color : Color.Red,
                                                        Config.Item("CircleThickness").GetValue<Slider>().Value);

            if (Config.Item("Wdraw").GetValue<Circle>().Active)
                if (W.Level > 0)
                    Render.Circle.DrawCircle(ObjectManager.Player.Position, W.Range, W.IsReady() ? Config.Item("Wdraw").GetValue<Circle>().Color : Color.Red,
                                                        Config.Item("CircleThickness").GetValue<Slider>().Value);

            if (Config.Item("Edraw").GetValue<Circle>().Active)
                if (E.Level > 0)
                    Render.Circle.DrawCircle(ObjectManager.Player.Position, 550 + 7 * player.Level,
                        E.IsReady() ? Config.Item("Edraw").GetValue<Circle>().Color : Color.Red,
                                                        Config.Item("CircleThickness").GetValue<Slider>().Value);

            if (Config.Item("Rdraw").GetValue<Circle>().Active)
                if (R.Level > 0)
                    Render.Circle.DrawCircle(ObjectManager.Player.Position, 550 + 7 * player.Level,
                        R.IsReady() ? Config.Item("Rdraw").GetValue<Circle>().Color : Color.Red,
                                                        Config.Item("CircleThickness").GetValue<Slider>().Value);

            var orbtarget = Orbwalker.GetTarget();
            if (Config.Item("drawtargetcircle").GetValue<bool>() && orbtarget != null)
            Render.Circle.DrawCircle(orbtarget.Position, 100, Color.DarkOrange, 10);

        }
    }
}
