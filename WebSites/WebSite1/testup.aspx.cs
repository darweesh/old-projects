using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class testup : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.Files.Count==0)
        {
            //Label1.Text = Request.Files["f1"].FileName;
            for (int i = 0; i < Request.Params.Count; i++)
            {
                Response.Write(Request.Params.AllKeys[i]);
                Response.Write(Request.Params[Request.Params.AllKeys[i]]);
                Response.Write("<br />");
            }
        }
        
    }
    protected void Button1_Click(object sender, EventArgs e)
    {

        
    }
}