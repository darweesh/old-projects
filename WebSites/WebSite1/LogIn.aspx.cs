using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Default2 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.ContentType="text/html";
       
        if (Request.Params["username"] != "")
        {
            System.Threading.Thread.Sleep(1000);
            Usersdataset data = new Usersdataset();
            UsersdatasetTableAdapters.usersTableAdapter ta = new UsersdatasetTableAdapters.usersTableAdapter();
            Usersdataset.usersDataTable dt = new Usersdataset.usersDataTable();
            int result = ta.Fillbyuserdata(dt, Request.Params["username"], Request.Params["password"]);
            if (result != 0)
            {
                Response.Write("<div id=\"login_im2\"></div><div id=\"username\">" + dt[0].UserName + "</div>");
                Session["logged"] = "True";
                Session["user"] = dt[0].UserName;
            }
        }
        else
        {
            Response.Write("<div id=\"login_im\"></div>" + "<div id=\"form1\" class=\"current\">" + "<table id=\"loginform\" style=\"display:table\">" + "<tr><td><label class=\"lable\">User Id</label></td>" + "<td><input  id=\"textf\" class=\"username\" type=\"text\"  ></td></tr>" + "<tr><td><label class=\"lable\">Password</label></td>" + "<td><input  id=\"textf\" class=\"password\" type=\"password\"></td></tr>" + "<tr><td></td><td><img class=\"cursor\" src=\"./Images/buttonGO.gif\"  onclick ='logIn();'></td>	</tr>" + "</table>" + "</div><div>wrong password or username</div>");        
            }
        
    }
}