using System;
using System.Web;

namespace TTTN
{
    public static class Thongbao
    {
        public static bool has_flash()
        {
            if (HttpContext.Current.Session["Thong_bao"] == null)
            {
                return false;
            }
            return true;
        }
        public static void set_flash(String smg, String smg_type)
        {
            ThongbaoModel tb = new ThongbaoModel();
            tb.smg = smg;
            tb.smg_type = smg_type;
            HttpContext.Current.Session["Thong_bao"] = tb;
        }
        public static ThongbaoModel get_flash()
        {
            ThongbaoModel thongbao = (ThongbaoModel)HttpContext.Current.Session["Thong_bao"];
            HttpContext.Current.Session["Thong_bao"] = null;
            return thongbao;
        }
    }
}