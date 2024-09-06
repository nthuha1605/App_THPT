using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class SoLienLacMasterPage : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.Cookies["PhuHuynhVietNhat"] != null) /*Request.Cookies["HocSinh"] != null*/
        {
            string[] arrListStr = Request.Cookies["PhuHuynhVietNhat"].Value.Split(',');
            if (arrListStr[0] == "hocsinh")// nếu là học sinh đăng nhập
                itemHome.HRef = "/slldt-home";
            else if (arrListStr[0] == "phuhuynh")
                itemHome.HRef = "/trang-chu";
        }
    }
}
