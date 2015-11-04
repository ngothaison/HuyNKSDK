    using System.Drawing;
    using LeagueSharp.Common;

namespace Slutty_ryze
{
    internal class MenuManager
    {
        #region Variable Declaration
        public const string Menuname = "Slutty Ryze";

        public static Orbwalking.Orbwalker Orbwalker;
        private static Menu _config;
        #endregion
        #region Public Functions
        public static Menu GetMenu()
        {
            _config = new Menu(Menuname, Menuname, true);
            _config.AddSubMenu(HumanizerMenu());
            _config.AddSubMenu(DrawingMenu());
            _config.AddSubMenu(ComboMenu());
            _config.AddSubMenu(MixedMenu());
            _config.AddSubMenu(FarmMenu());
            _config.AddSubMenu(MiscMenu());
            _config.AddSubMenu(OrbWalkingMenu());
            _config.AddItem(new MenuItem("test", "Level 3-5 Oriented Combo").SetValue(new KeyBind('Z', KeyBindType.Press)));
            _config.AddItem(new MenuItem("ABC", "Được Việt Hóa bởi Huỳnh Nhật Nam"));
            Orbwalker = new Orbwalking.Orbwalker(_config.SubMenu("Orbwalking"));
            return _config;
        }
        #endregion
        #region Private Functions

        private static Menu HumanizerMenu()
        {
            var humanizerMenu = new Menu("Đánh giống người", "Humanizer");

            humanizerMenu.SubMenu("Đánh giống người").AddItem(new MenuItem("minDelay", "Độ trễ tối thiểu để tung chiêu (ms)").SetValue(new Slider(0, 0, 200)));
            humanizerMenu.SubMenu("Đánh giống người").AddItem(new MenuItem("maxDelay", "Độ trễ tối đa để tung chiêu (ms)").SetValue(new Slider(0,  0, 250)));
            humanizerMenu.SubMenu("Đánh giống người").AddItem(new MenuItem("minCreepHPOffset", "Minimum HP for a Minion to Have Before CSing Damage >= HP+(%)").SetValue(new Slider(5, 0, 25)));
            humanizerMenu.SubMenu("Đánh giống người").AddItem(new MenuItem("maxCreepHPOffset", "Maximum HP for a Minion to Have Before CSing Damage >= HP+(%)").SetValue(new Slider(15, 0, 25)));
            humanizerMenu.SubMenu("Đánh giống người").AddItem(new MenuItem("doHuman", "Bật đánh giống người").SetValue(false));

            return humanizerMenu;
        }

        private static Menu OrbWalkingMenu()
        {
            Menu orbWalkingMenu = new Menu("Thả diều", "Orbwalking");
            var targetSelectorMenu = new Menu("Chọn mục tiêu", "Target Selector");
            TargetSelector.AddToMenu(targetSelectorMenu);
            orbWalkingMenu.AddSubMenu(targetSelectorMenu);
            return orbWalkingMenu;
        }
        private static Menu DrawingMenu()
        {
            var drawMenu = new Menu("Cài đặt vẽ", "Vẽ");
            drawMenu
                .AddItem(
                    new MenuItem("drawoptions", "Chế độ vẽ").SetValue(
                        new StringList(new[] { "Bình thường", "Chế độ tự cài đặt" })));
            drawMenu.SubMenu("Vẽ").AddItem(new MenuItem("Draw", "Hiển thị Vẽ").SetValue(true));
            drawMenu.SubMenu("Vẽ").AddItem(new MenuItem("qDraw", "Vẽ Q").SetValue(true));
            drawMenu.SubMenu("Vẽ").AddItem(new MenuItem("eDraw", "Vẽ E").SetValue(true));
            drawMenu.SubMenu("Vẽ").AddItem(new MenuItem("wDraw", "Vẽ W").SetValue(true));
            drawMenu.SubMenu("Vẽ").AddItem(new MenuItem("stackDraw", "Điểm tích nội tại").SetValue(true));
            drawMenu.SubMenu("Vẽ").AddItem(new MenuItem("notdraw", "Vẽ trạng thái").SetValue(true));
            drawMenu.SubMenu("Vẽ").AddItem(new MenuItem("keyBindDisplay", "Hiển thị các phím").SetValue(true));  

            var drawDamageMenu = new MenuItem("RushDrawEDamage", "Máu tổn thất sau 1 combo").SetValue(true);
            var drawFill =
                new MenuItem("RushDrawWDamageFill", "Màu máu tổn thất").SetValue(new Circle(true, Color.SeaGreen));
            drawMenu.SubMenu("Vẽ").AddItem(drawDamageMenu);
            drawMenu.SubMenu("Vẽ").AddItem(drawFill);

            //GlobalManager.EnableDrawingDamage = drawDamageMenu.GetValue<bool>();
            //GlobalManager.EnableFillDamage = drawFill.GetValue<Circle>().Active;
            //GlobalManager.DamageFillColor = drawFill.GetValue<Circle>().Color;

            return drawMenu;
        }

        private static Menu ComboMenu()
        {
            var combo1Menu = new Menu("Cài đặt Combo", "combospells");
            {
                combo1Menu
                    .AddItem(
                        new MenuItem("combooptions", "Chế độ Combo").SetValue(
                            new StringList(new[] {"Cải tiến"})));
                combo1Menu.AddItem(new MenuItem("useQ", "Dùng Q").SetValue(true));
                combo1Menu.AddItem(new MenuItem("useW", "Dùng W").SetValue(true));
                combo1Menu.AddItem(new MenuItem("useE", "Dùng E").SetValue(true));
                combo1Menu.AddItem(new MenuItem("useR", "Dùng R").SetValue(true));
                combo1Menu.AddItem(new MenuItem("useRww", "Chỉ dùng R nếu mục tiêu trúng W").SetValue(true));
                combo1Menu.AddItem(new MenuItem("AAblock", "Chặn tự động đánh thường trong Combo").SetValue(false));
                combo1Menu.AddItem(
                    new MenuItem("minaarange", "Hủy đánh thường nếu khoảng cách múc tiêu >").SetValue(new Slider(550,
                        100, 550)));
            }
            return combo1Menu;
        }

        private static Menu MixedMenu()
        {

            var mixedMenu = new Menu("Trộn 2 chế độ", "mixedsettings");
            {
                mixedMenu.AddItem(new MenuItem("mMin", "Mana tối thiểu").SetValue(new Slider(40)));
                mixedMenu.AddItem(new MenuItem("UseQM", "Dùng Q").SetValue(true));
                mixedMenu.AddItem(new MenuItem("UseQMl", "Dùng Q để Giết (last hit) lính").SetValue(true));
                mixedMenu.AddItem(new MenuItem("UseEM", "Dùng E").SetValue(false));
                mixedMenu.AddItem(new MenuItem("UseWM", "Dùng W").SetValue(false));
                mixedMenu.AddItem(new MenuItem("UseQauto", "Tự động dùng Q").SetValue(false));
            }
            return mixedMenu;
        }

        private static Menu FarmMenu()
        {
            var farmMenu = new Menu("Đi đường", "farmingsettings");
            var laneMenu = new Menu("Dọn lính nhanh", "lanesettings");
            {
                laneMenu.AddItem(
                    new MenuItem("disablelane", "Bật dọn lính").SetValue(new KeyBind('T', KeyBindType.Toggle)));
                laneMenu.AddItem(new MenuItem("useEPL", "Giới hạn % mana").SetValue(new Slider(50)));
                laneMenu.AddItem(new MenuItem("passiveproc", "không dùng chiêu nếu được 4 điểm nội tại").SetValue(true));
                laneMenu.AddItem(new MenuItem("useQlc", "Dùng Q để Giết (last hit)").SetValue(true));
                laneMenu.AddItem(new MenuItem("useWlc", "Dùng W để Giết (last hit)").SetValue(false));
                laneMenu.AddItem(new MenuItem("useElc", "Dùng E để Giết (last hit)").SetValue(false));
                laneMenu.AddItem(new MenuItem("useQ2L", "Dùng Q để dọn lính").SetValue(true));
                laneMenu.AddItem(new MenuItem("useW2L", "Dùng W để dọn lính").SetValue(false));
                laneMenu.AddItem(new MenuItem("useE2L", "Dùng E để dọn lính").SetValue(false));
                laneMenu.AddItem(new MenuItem("useRl", "Dùng R để dọn lính").SetValue(false));
                laneMenu.AddItem(new MenuItem("rMin", "Số lính tối thiểu để Dùng R").SetValue(new Slider(3, 1, 20)));
            }

            var jungleMenu = new Menu("Dọn quái rừng", "junglesettings");
            {
                jungleMenu.AddItem(new MenuItem("useJM", "Giới hạn % mana").SetValue(new Slider(50)));
                jungleMenu.AddItem(new MenuItem("useQj", "Dùng Q").SetValue(true));
                jungleMenu.AddItem(new MenuItem("useWj", "Dùng W").SetValue(true));
                jungleMenu.AddItem(new MenuItem("useEj", "Dùng E").SetValue(true));
                jungleMenu.AddItem(new MenuItem("useRj", "Dùng R").SetValue(true));
            }


            var lastMenu = new Menu("Đòn đánh kết liễu", "lastsettings");
            {
                lastMenu.AddItem(new MenuItem("useQl2h", "Dùng Q").SetValue(true));
                lastMenu.AddItem(new MenuItem("useWl2h", "Dùng W").SetValue(false));
                lastMenu.AddItem(new MenuItem("useEl2h", "Dùng E").SetValue(false));
            }

            farmMenu.AddSubMenu(laneMenu);
            farmMenu.AddSubMenu(jungleMenu);
            farmMenu.AddSubMenu(lastMenu);
            return farmMenu;
        }

        private static Menu MiscMenu()
        {
            var miscMenu = new Menu("Hỗn hợp", "miscsettings");

            var passiveMenu = new Menu("Tự động lấy Nội tại", "passivesettings");
            {
                passiveMenu.AddItem(new MenuItem("ManapSlider", "Giới hạn % mana"))
                    .SetValue(new Slider(30));
                passiveMenu.AddItem(
                    new MenuItem("autoPassive", "Tự động tích điểm nội tại").SetValue(new KeyBind('Z', KeyBindType.Toggle)));
                passiveMenu.AddItem(new MenuItem("stackSlider", "Giữ điểm Nội Tại ở"))
                    .SetValue(new Slider(3, 1, 4));
                passiveMenu.AddItem(new MenuItem("autoPassiveTimer", "Làm mới điểm nội tại mỗi (s)"))
                    .SetValue(new Slider(5, 1, 10));
                passiveMenu.AddItem(new MenuItem("stackMana", "Giới hạn % mana")).SetValue(new Slider(50));
            }

            var itemMenu = new Menu("Trang bị", "itemsettings");
            {
                itemMenu.AddItem(new MenuItem("tearS", "Tự động tích điểm Nước Mắt Nữ Thần").SetValue(new KeyBind('G', KeyBindType.Toggle)));
                itemMenu.AddItem(new MenuItem("tearoptions", "Chỉ tích điểm ở Bệ Đá Cổ").SetValue(false));
                itemMenu.AddItem(new MenuItem("tearSM", "Giới hạn % Mana").SetValue(new Slider(95)));
                itemMenu.AddItem(new MenuItem("staff", "Dùng Quyền Trượng Đại Thiên Sứ").SetValue(true));
                itemMenu.AddItem(new MenuItem("staffhp", "Khi % máu <").SetValue(new Slider(30)));
                itemMenu.AddItem(new MenuItem("muramana", "Dùng Thần Kiếm Muramana").SetValue(true));
            }

            var hpMenu = new Menu("Tự động bơm máu/mana...", "hpsettings");
            {
                hpMenu.AddItem(new MenuItem("autoPO", "Bật tự bơm").SetValue(true));
                hpMenu.AddItem(new MenuItem("HP", "Tự dùng bình Máu")).SetValue(true);
                hpMenu.AddItem(new MenuItem("HPSlider", "% máu tối thiểu để bơm máu")).SetValue(new Slider(30));
                hpMenu.AddItem(new MenuItem("MANA", "Tự dùng bình Mana").SetValue(true));
                hpMenu.AddItem(new MenuItem("MANASlider", "% Mana tối thiểu để bơm Mana")).SetValue(new Slider(30));
                hpMenu.AddItem(new MenuItem("Biscuit", "Tự dùng bánh quy").SetValue(true));
                hpMenu.AddItem(new MenuItem("bSlider", "% máu tối thiểu để ăn bánh quy")).SetValue(new Slider(30));
                hpMenu.AddItem(new MenuItem("flask", "Tự dùng Lọ Pha Lê").SetValue(true));
                hpMenu.AddItem(new MenuItem("fSlider", "% Máu tối thiểu để kích hoạt Lọ Pha Lê")).SetValue(new Slider(30));
            }

            var eventMenu = new Menu("Sự kiện", "eventssettings");
            {
                eventMenu.AddItem(new MenuItem("useW2I", "Làm gián đoạn với W").SetValue(true));
                eventMenu.AddItem(new MenuItem("useQW2D", "Nếu mục tiêu dính W thì tự động Q").SetValue(true));
                eventMenu.AddItem(new MenuItem("level", "Tự động cộng chiêu").SetValue(true));
                eventMenu.AddItem(new MenuItem("autow", "Tự động W khi mục tiêu trong trụ").SetValue(true));
            }

            var ksMenu = new Menu("Cướp mạng", "kssettings");
            {
                ksMenu.AddItem(new MenuItem("KS", "Bật/tắt")).SetValue(true);
                ksMenu.AddItem(new MenuItem("useQ2KS", "Dùng Q").SetValue(true));
                ksMenu.AddItem(new MenuItem("useW2KS", "Dùng W").SetValue(true));
                ksMenu.AddItem(new MenuItem("useE2KS", "Dùng E").SetValue(true));
            }

            var chase = new Menu("Đuổi theo mục tiêu", "Chase Target");
            {
                chase.AddItem(new MenuItem("chase", "Bật/tắt")).SetValue(new KeyBind('A', KeyBindType.Press));
                chase.AddItem(new MenuItem("usewchase", "Dùng W")).SetValue(true);
                chase.AddItem(new MenuItem("chaser", "Dùng R")).SetValue(false);
            }

            miscMenu.AddSubMenu(passiveMenu);
            miscMenu.AddSubMenu(itemMenu);
            miscMenu.AddSubMenu(hpMenu);
            miscMenu.AddSubMenu(eventMenu);
            miscMenu.AddSubMenu(ksMenu);
            miscMenu.AddSubMenu(chase);
            return miscMenu;
        }
        #endregion
    }

}
