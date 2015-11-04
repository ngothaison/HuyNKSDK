namespace KoreanAnnie
{
    using KoreanAnnie.Common;

    using LeagueSharp.Common;

    internal static class AnnieCustomMenu
    {
        #region Public Methods and Operators

        public static void Load(CommonMenu mainMenu)
        {
            RemoveItems(mainMenu);
            LoadLaneClear(mainMenu);
            LoadCombo(mainMenu);
            LoadMisc(mainMenu);
        }

        #endregion

        #region Methods

        private static void LoadCombo(CommonMenu mainMenu)
        {

        }

        private static void LoadLaneClear(CommonMenu mainMenu)
        {
            mainMenu.LaneClearMenu.AddItem(
                new MenuItem(KoreanUtils.ParamName(mainMenu, "saveqtofarm"), "Giữ Q để đánh lính").SetValue(true));
            mainMenu.LaneClearMenu.AddItem(
                new MenuItem(KoreanUtils.ParamName(mainMenu, "minminionstow"), "Dùng W nếu giết được từ...").SetValue(
                    new Slider(3, 1, 6)));
        }

        private static void LoadMisc(CommonMenu mainMenu)
        {
            Menu passiveStunMenu =
                mainMenu.MiscMenu.AddSubMenu(
                    new Menu("Cài đặt thêm", KoreanUtils.ParamName(mainMenu, "passivestunmenu")));
            passiveStunMenu.AddItem(
                new MenuItem(KoreanUtils.ParamName(mainMenu, "useetostack"), "Dùng E lấy choáng").SetValue(true));
            passiveStunMenu.AddItem(
                new MenuItem(KoreanUtils.ParamName(mainMenu, "manalimitforstacking"), "Giới hạn mana dùng E")
                    .SetValue(new Slider(30, 0, 100)));
            passiveStunMenu.AddItem(
                new MenuItem(KoreanUtils.ParamName(mainMenu, "savestunforcombo"), "Giữ choáng cho Combo hoặc Rỉa máu").SetValue
                    (false));
            passiveStunMenu.AddItem(
                new MenuItem(KoreanUtils.ParamName(mainMenu, "showeeasybutton"), "Hiện nút tùy chỉnh choáng").SetValue(true));
            passiveStunMenu.AddItem(
                new MenuItem(KoreanUtils.ParamName(mainMenu, "easybuttonpositionx"), "Button Position X (Read Only)")
                    .SetValue(0));
            passiveStunMenu.AddItem(
                new MenuItem(KoreanUtils.ParamName(mainMenu, "easybuttonpositiony"), "Button Position Y (Read Only)")
                    .SetValue(0));

            Menu flashTibbers =
                mainMenu.MiscMenu.AddSubMenu(
                    new Menu("Tốc biến + Thả Gấu", KoreanUtils.ParamName(mainMenu, "flashtibbersmenu")));
            flashTibbers.AddItem(
                new MenuItem(KoreanUtils.ParamName(mainMenu, "flashtibbers"), "Phím").SetValue(
                    new KeyBind('T', KeyBindType.Press)));
            flashTibbers.AddItem(
                new MenuItem(KoreanUtils.ParamName(mainMenu, "minenemiestoflashr"), "Dùng khi nếu bằng cài đặt hoặc nhiều hơn")
                    .SetValue(new Slider(2, 1, 5)));
            flashTibbers.AddItem(
                new MenuItem(KoreanUtils.ParamName(mainMenu, "orbwalktoflashtibbers"), "Khi ấn phím là di chuyển theo chuột").SetValue(false));

            mainMenu.MiscMenu.AddItem(
                new MenuItem(KoreanUtils.ParamName(mainMenu, "supportmode"), "Chế độ hỗ trợ (đường dưới)").SetValue(false));

            mainMenu.MiscMenu.AddItem(
                new MenuItem(KoreanUtils.ParamName(mainMenu, "useqtofarm"), "Dùng Q để đánh lính").SetValue(true));
            mainMenu.MiscMenu.AddItem(
                new MenuItem(KoreanUtils.ParamName(mainMenu, "useeagainstaa"), "Dùng E để chống địch đánh thường").SetValue(true));
            mainMenu.MiscMenu.AddItem(
                new MenuItem(KoreanUtils.ParamName(mainMenu, "antigapcloser"), "Chống tiếp cận Annie").SetValue(true));
            mainMenu.MiscMenu.AddItem(
                new MenuItem(
                    KoreanUtils.ParamName(mainMenu, "interruptspells"),
                    "Ngắt phép thuật nguy hiểm nếu có thể").SetValue(true));
            mainMenu.MiscMenu.AddItem(
                new MenuItem(KoreanUtils.ParamName(mainMenu, "autotibbers"), "Tự động điều khiển Gấu").SetValue(true));

            Menu DontUseComboMenu = mainMenu.MiscMenu.AddSubMenu(new Menu("Không rỉa máu hoặc Combo", "dontusecomboon"));

            foreach (var enemy in HeroManager.Enemies)
            {
                DontUseComboMenu.AddItem(
                    new MenuItem(
                        KoreanUtils.ParamName(mainMenu, "combo" + enemy.ChampionName.ToLowerInvariant()),
                        enemy.ChampionName).SetValue(true));
            }

            DontUseComboMenu.AddItem(
                new MenuItem(KoreanUtils.ParamName(mainMenu, "dontuselabel1"), "------------------------------"));
            DontUseComboMenu.AddItem(
                new MenuItem(KoreanUtils.ParamName(mainMenu, "dontuselabel2"), "QUAN TRỌNG : Các mục tiêu đã tắt sẽ được ..."));
            DontUseComboMenu.AddItem(
                new MenuItem(KoreanUtils.ParamName(mainMenu, "dontuselabel3"), "... tấn công nếu đang một mình hoặc máu thấp thấp"));
        }

        private static void RemoveItems(CommonMenu mainMenu)
        {
            mainMenu.HarasMenu.Items.Remove(mainMenu.HarasMenu.Item(KoreanUtils.ParamName(mainMenu, "useetoharas")));
            mainMenu.LaneClearMenu.Items.Remove(
                mainMenu.LaneClearMenu.Item(KoreanUtils.ParamName(mainMenu, "useetolaneclear")));
            mainMenu.ComboMenu.Items.Remove(mainMenu.ComboMenu.Item(KoreanUtils.ParamName(mainMenu, "useetocombo")));
        }

        #endregion
    }
}
