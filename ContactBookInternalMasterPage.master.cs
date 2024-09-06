using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ContactBookInternalMasterPage : System.Web.UI.MasterPage
{
    dbcsdlDataContext db = new dbcsdlDataContext();
    public string username;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.Cookies["UserName"] != null)
        {
            var checkuser = (from u in db.admin_Users
                             where u.username_username == Request.Cookies["UserName"].Value
                             select u).First();
            username = checkuser.username_username;
        }
        else
        {
            Response.Redirect("/admin-login");
        }
    }
    protected void btnDangXuat_ServerClick(object sender, EventArgs e)
    {
        HttpCookie ck = new HttpCookie("UserName");
        string s = ck.Value;
        ck.Value = "";  //set a blank value to the cookie 
        ck.Expires = DateTime.Now.AddDays(-1);
        Response.Cookies.Add(ck);
        Response.Redirect("/admin-login");
    }
}
