using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp.SDK.Core.UI.IMenu.Values;

namespace HuyNK_Series_SDK
{
    class _Getmenu :MenuProvider
    {
        public static bool get_bool(String a,String b)
        {
            bool get_bool;
            get_bool = MenuProvider.MainMenu[a][b].GetValue<MenuBool>().Value;
            return get_bool;
        }
        public static int get_slider(String a,String b)
        {
            int get_slider;
            get_slider = MenuProvider.MainMenu[a][b].GetValue<MenuSlider>().Value;
            return get_slider;
        }
    }
}
