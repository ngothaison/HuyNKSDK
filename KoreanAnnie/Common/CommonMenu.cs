namespace KoreanAnnie.Common
{
    using LeagueSharp.Common;

    internal class CommonMenu
    {
        #region Fields

        private readonly Menu comboMenu;

        private readonly Menu drwaingsMenu;

        private readonly Menu harasMenu;

        private readonly Menu laneClearMenu;

        private readonly Menu mainMenu;

        private readonly string menuName;

        private readonly Menu miscMenu;

        #endregion

        #region Constructors and Destructors

        public CommonMenu(string displayName, bool misc)
        {
            menuName = displayName.Replace(" ", "_").ToLowerInvariant();

            mainMenu = new Menu(displayName, menuName, true);

            AddOrbwalker(mainMenu);
            AddTargetSelector(mainMenu);

            Menu modes = new Menu("Chế độ", string.Format("{0}.modes", MenuName));
            mainMenu.AddSubMenu(modes);

            harasMenu = AddHarasMenu(modes);
            laneClearMenu = AddLaneClearMenu(modes);
            comboMenu = AddComboMenu(modes);

            if (misc)
            {
                miscMenu = AddMiscMenu(mainMenu);
            }

            drwaingsMenu = AddDrawingMenu(mainMenu);

            mainMenu.AddToMainMenu();
        }

        #endregion

        #region Public Properties

        public Menu ComboMenu
        {
            get
            {
                return comboMenu;
            }
        }

        public Menu DrawingMenu
        {
            get
            {
                return drwaingsMenu;
            }
        }

        public Menu HarasMenu
        {
            get
            {
                return harasMenu;
            }
        }

        public Menu LaneClearMenu
        {
            get
            {
                return laneClearMenu;
            }
        }

        public Menu MainMenu
        {
            get
            {
                return mainMenu;
            }
        }

        public Menu MiscMenu
        {
            get
            {
                return miscMenu;
            }
        }

        public Orbwalking.Orbwalker Orbwalker { get; private set; }

        #endregion

        #region Properties

        private string MenuName
        {
            get
            {
                return menuName;
            }
        }

        #endregion

        #region Methods

        private Menu AddComboMenu(Menu rootMenu)
        {
            Menu newMenu = new Menu("Combo", string.Format("{0}.combo", MenuName));
            rootMenu.AddSubMenu(newMenu);

            newMenu.AddItem(new MenuItem(string.Format("{0}.useqtocombo", MenuName), "Dùng Q").SetValue(true));
            newMenu.AddItem(new MenuItem(string.Format("{0}.usewtocombo", MenuName), "Dùng W").SetValue(true));
            newMenu.AddItem(new MenuItem(string.Format("{0}.useetocombo", MenuName), "Dùng E").SetValue(true));
            newMenu.AddItem(new MenuItem(string.Format("{0}.usertocombo", MenuName), "Dùng R").SetValue(true));
            newMenu.AddItem(
                new MenuItem(string.Format("{0}.minenemiestor", MenuName), "Dùng R if it Will Hit at Least").SetValue(
                    new Slider(3, 1, 5)));
            newMenu.AddItem(
                new MenuItem(string.Format("{0}.disableaa", MenuName), "Không đánh thường khi").SetValue(
                    new StringList(
                        new[] { "Không", "Luôn luôn", "1 vài chiêu đã sẵn sàng", "Rỉa máu-Combo sẵn sàng", "4 chiêu sẵn sàng" })));
            newMenu.AddItem(
                new MenuItem(
                    string.Format("{0}.forceultusingmouse", MenuName),
                    "Force Ultimate Using Mouse-buttons (Cursor Sprite)").SetValue(true));

            return newMenu;
        }

        private Menu AddDrawingMenu(Menu rootMenu)
        {
            Menu newMenu = new Menu("Cài đặt vẽ", string.Format("{0}.drawings", MenuName));
            rootMenu.AddSubMenu(newMenu);

            newMenu.AddItem(new MenuItem(string.Format("{0}.drawskillranges", MenuName), "Tầm đánh").SetValue(true));
            newMenu.AddItem(
                new MenuItem(string.Format("{0}.damageindicator", MenuName), "Máu tổn thất khi combo").SetValue(true));
            newMenu.AddItem(
                new MenuItem(string.Format("{0}.damageindicatorcolor", MenuName), "Bảng màu").SetValue(
                    new StringList(new string[] { "Normal", "Colorblind" })));
            newMenu.AddItem(
                new MenuItem(string.Format("{0}.killableindicator", MenuName), "Có thể hạ gục bằng 1 combo").SetValue(true));

            return newMenu;
        }

        private Menu AddHarasMenu(Menu rootMenu)
        {
            Menu newMenu = new Menu("Rỉa máu", string.Format("{0}.haras", MenuName));
            rootMenu.AddSubMenu(newMenu);

            newMenu.AddItem(new MenuItem(string.Format("{0}.useqtoharas", MenuName), "Dùng Q").SetValue(true));
            newMenu.AddItem(new MenuItem(string.Format("{0}.usewtoharas", MenuName), "Dùng W").SetValue(true));
            newMenu.AddItem(new MenuItem(string.Format("{0}.useetoharas", MenuName), "Dùng E").SetValue(true));

            MenuItem manaLimitToHaras =
                new MenuItem(string.Format("{0}.manalimittoharas", MenuName), "Giới hạn mana").SetValue(
                    new Slider(0, 0, 100));
            newMenu.AddItem(manaLimitToHaras);

            return newMenu;
        }

        private Menu AddLaneClearMenu(Menu rootMenu)
        {
            Menu newMenu = new Menu("Đẩy đường", string.Format("{0}.laneclear", MenuName));
            rootMenu.AddSubMenu(newMenu);

            newMenu.AddItem(new MenuItem(string.Format("{0}.useqtolaneclear", MenuName), "Dùng Q").SetValue(true));
            newMenu.AddItem(new MenuItem(string.Format("{0}.usewtolaneclear", MenuName), "Dùng W").SetValue(true));
            newMenu.AddItem(new MenuItem(string.Format("{0}.useetolaneclear", MenuName), "Dùng E").SetValue(true));
            newMenu.AddItem(
                new MenuItem(string.Format("{0}.manalimittolaneclear", MenuName), "Giới hạn mana").SetValue(
                    new Slider(50, 0, 100)));
            newMenu.AddItem(
                new MenuItem(string.Format("{0}.harasonlaneclear", MenuName), "Rỉa máu khi đẩy đường").SetValue(true));

            return newMenu;
        }

        private Menu AddMiscMenu(Menu rootMenu)
        {
            Menu newMenu = new Menu("Tùy chọn", string.Format("{0}.misc", MenuName));
            rootMenu.AddSubMenu(newMenu);

            return newMenu;
        }

        private void AddOrbwalker(Menu rootMenu)
        {
            Menu orbwalkerMenu = new Menu("Thả diều", string.Format("{0}.orbwalker", MenuName));
            Orbwalker = new Orbwalking.Orbwalker(orbwalkerMenu);
            rootMenu.AddSubMenu(orbwalkerMenu);
        }

        private void AddTargetSelector(Menu rootMenu)
        {
            Menu targetselectorMenu = new Menu("Chọn mục tiêu", string.Format("{0}.targetselector", MenuName));
            TargetSelector.AddToMenu(targetselectorMenu);
            rootMenu.AddSubMenu(targetselectorMenu);
        }

        #endregion
    }
}
