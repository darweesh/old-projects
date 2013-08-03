<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">

<head>
<meta content="text/html; charset=utf-8" http-equiv="Content-Type" />
<title>Medical Image Processing</title>
 
    <script src="Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script src="Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>

<link href="main.css" rel="stylesheet" type="text/css" />
   
<script language="javascript" type="text/javascript" >
    $(function () {
        fadeinv();
        $(".menuelist td").hover(fin, fout);
        $("#Loading").css("left", (screen.availWidth / 2) - 90);
        $("#Loading").css("right", screen.availWidth / 2);
        $("#Loading").css("top", screen.availHeight / 2);
        $("#Loading").css("bottom", screen.availHeight / 2);
        $("#Loading").ajaxStart(function () { $(this).show("slow"); });
        $("#Loading").ajaxStop(function () { $(this).hide("slow"); });
        $("#onup").hide();
        <% if(Request.Params["u"]=="T")  Response.Write("addImage();");%>
        
         
    });
    var result;

    function logIn() {

        $("#Loading").css("z-index", 2);
        $("#rightdiv").load('LogIn.aspx', { 'username': $(".username").attr('value'), 'password': $('.password').attr('value') }, function (datas) { if (datas == "wrong") alert(datas); else { document.getElementById("upl").innerHTML = "UploadImages"; location.hash = "logged"; $("#upl").click(addImage); document.getElementById("upl2").innerHTML = "ViewPatients"; $("#upl2").attr("href", 'Default2.aspx'); } });


    }

   
    function fin(evt) {
        $(this).fadeTo("normal", 0.3, function () { });


    }
    function fout(evt) {
        $(this).fadeTo("fast", 1.0, function () { });

    }

    var imageid = 0;

    function fadeinv() {

        $("#guy").fadeOut(1600, callme);


    }
    function callme() {
        $('#guy div:eq(' + imageid + ')').removeClass('current').addClass('previous');
        imageid++;
        if (imageid > 4) { imageid = 0; }
        $('#guy div:eq(' + imageid + ')').addClass('current').removeClass('previous');
        $("#guy").fadeIn(1600, fadeinv);


    }
    function addImage() {
        $("#news_event").hide("slow", function () { $("#onup").show(); });
        
    }
    function PatientsF() {
    
    }

</script>
</head>

<body>
<div id="Loading"><p >LOADING.... </p><img src="Images/ajax-loader.gif" alt="" /> </div> 
<div id="maindiv">
<div id="top_banner"><p class="logo"><a href="#">MRI.MD</a></p><div id="logout"><a href="Default.aspx?logout=y">logOut</a></div></div>
<div id="clear"></div>
<div id="menue">

	<table class="menuelist">
	
		<tr><td class="listitem" ><a class="men"  href="#">Home</a></td></tr>
		<tr ><td class="listitem"><a class="men" href="#">About Us</a></td></tr >
		<tr ><td class="listitem"><a class="men" href="#">What's New</a></td></tr >
		<tr><td  class="listitem">
        <%if (Session["logged"] == "True")
          { %>
        <a id="upl" class="men" href="#" onclick="addImage();">UploadImages</a>
        </td></tr ><tr ><td class="listitem"><a class="men"  href="Default2.aspx">ViewPatients</a></td></tr >
        <%}
          else
          { %>
        <a id="upl" class="men" href="#">Purchase</a>
        </td></tr ><tr ><td class="listitem"><a id="upl2" class="men" href="#">Support</a></td></tr >
        <%} %>
        
		
		<tr ><td class="listitem"><a class="men" href="#">Feedback</a></td></tr >
		<tr ><td class="listitem_last"><a class="men" href="#">Contact Us</a></td></tr >

	
	</table>


</div>
<div id="top_div"><div id="guy">
<div class="current" ><img  src="slideshowimages/slide0.gif" alt=""/></div>
<div ><img  src="slideshowimages/slide1.gif" alt=""/></div>
<div ><img  src="slideshowimages/slide2.gif" alt=""/></div>
<div ><img  src="slideshowimages/slide3.gif" alt=""/></div>
</div><p id="provide">provide solutions for</p><p id="sol">For Medical Image Processing</p> </div>
<div id="clear"></div>
<div id="OurProducts"><div id="ourproduct_top"></div></div>
<div id="MID">
<div id="Middiv">
	<table>
	<td id="Smallguy"></td>
	<td >
	<div id="Wellcom">
		<span class="welcomtext" >Welcome</span>
		<span class="toM"> to MRI.MD</span >
		<p>
			MRI.MD is medical image processing software and software library dedicated to radiologist and computer aided diagnosis (CAD.) software developers.
			
		</p>
       
		</div>
		</td>
	
	</table>
	<div id="news_event">
		<div id="highpan"></div>
		<table id="midpan">
        
		<td id="ima"></td>
			<td id="text">
				Magnetic resonance imaging (MRI) is often the medical imaging 
				method of choice when soft tissue delineation is necessary. This 
				is especially true for any attempt to segment brain tissues, 
				normal or abnormal. Image segmentation is a tool....</td>
				<tr><td></td><td id="moreinfo"><a href="#">more info...</a></td></tr>		
		</table>
		<div id="buttompan"></div>
	</div>
    <div id="onup">
    
    
    <form id="form1" runat="server"  name="form1" class="midpan"  >
		<div style="float:right;margin-right:230px; margin-bottom:20px;margin-left:15px; height:60px;">
        <label style=" margin-right:1px;font-size:16px;font-weight:bold;">Upload new Record File</label><asp:FileUpload ID="FileUpload1" runat="server" />
            <asp:ImageButton ID="ImageButton1" ImageUrl="~/Images/buttonGO.gif" 
                runat="server" onclick="ImageButton1_Click" />
      
        </div>
		</form>
        </div>
        <div id="clear"></div>
	<div id="support">
		<div id="highpan2"></div>
		<table id="midpan2"><td id="text">Software and technical  support is available via e-mail.</td><td id="ima2"></td></table>
		<div id="buttompan"></div>
	</div>
</div></div>
<div id="rightdiv">
<%if (Session["logged"] == "True")
  { %>
<div id="login_im2"></div><div id="username"><%Response.Write(Session["user"]); %></div>
<%}
  else
  { %>
	<div id="login_im"></div>
    
	<div id="form1">
		<table id="loginform" >
		
		
		<tr><td><label class="lable">User Id</label></td>
		<td><input  id="textf" class="username" type="text"  ></tr></td>
		<tr><td><label class="lable">Password</label></td>
		<td><input  id="textf" class="password" type="password"></tr></td>
		<tr><td></td><td><img class="cursor" src="./Images/buttonGO.gif"  onclick ='logIn();'></td>	</tr>
			
		</table>
	</div><%} %>
		
</div>
<div id="buttom">
	<div id="copyrights">
		<p style="margin-bottom:0">Copyrights © 2010 </p><p style="margin-top:0">MRI.MD</p>
	</div>
	<div id="s"><p style="margin-left:-10px; margin-top:13px; margin-bottom:0;">Software Designed & developed by:</p><p style="margin-top:0;">Darwesh Zaid Alshakaa</p></div>
	</div>
    
</div>
	
		
</body>

</html>
