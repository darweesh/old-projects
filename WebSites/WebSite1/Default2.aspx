<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default2.aspx.cs" Inherits="Default2" %>

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
    });
    var result;

    function logIn() {

        $("#Loading").css("z-index", 2);
        $("#rightdiv").load('http://localhost:63426/WebSite1/LogIn.aspx', { 'username': $(".username").attr('value'), 'password': $('.password').attr('value') }, function (datas) { if (datas == "wrong") alert(datas); else { document.getElementById("upl").innerHTML = "UploadImages"; location.hash = "logged"; $("#upl").click(addImage); } });


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
        $("#news_event").hide("slow", function () { });
        $("#onup").show();
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
        <a id="upl" class="men" href="Default.aspx?u=T" ">UploadImages</a>
        <tr ><td class="listitem"><a class="men" onclick="PatientsF();" href="#">ViewPatients</a></td></tr >
        <%}
          else
          { %>
        <a id="upl" class="men" href="#">Purchase</a>
        <tr ><td class="listitem"><a class="men" href="#">Support</a></td></tr >
        <%} %>
        </td></tr >
		
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
		<form runat="server">
        
       
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
       <ContentTemplate>
           
        <asp:DetailsView ID="DetailsView1" runat="server" AutoGenerateRows="False" 
            CellPadding="4" DataKeyNames="Id" DataSourceID="ObjectDataSource1" 
            Width="516px" ForeColor="#333333" GridLines="None" CssClass="datagrid" 
               AllowPaging="True">
            <AlternatingRowStyle BackColor="White" />
            <CommandRowStyle BackColor="#D1DDF1" Font-Bold="True" />
            <EditRowStyle BackColor="#2461BF" />
            <FieldHeaderStyle BackColor="#DEE8F5" Font-Bold="True" />
            <Fields>
                <asp:BoundField DataField="Id" HeaderText="Id" InsertVisible="False" 
                    ReadOnly="True" SortExpression="Id" >
                <HeaderStyle Width="100px" />
                </asp:BoundField>
                <asp:BoundField DataField="PatientName" HeaderText="PatientName" 
                    SortExpression="PatientName" />
                <asp:BoundField DataField="Wieght" HeaderText="Wieght" 
                    SortExpression="Wieght" />
                <asp:BoundField DataField="Info" HeaderText="Info" SortExpression="Info" />
                <asp:BoundField DataField="TumorSize" HeaderText="TumorSize" 
                    SortExpression="TumorSize" />
                <asp:BoundField DataField="TumorPostion" HeaderText="TumorPostion" 
                    SortExpression="TumorPostion" />
                
                <asp:ImageField DataImageUrlField="Image" HeaderText="Image">
                </asp:ImageField>
                
            </Fields>
            <FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True" />
            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
            <RowStyle BackColor="#EFF3FB" />
        </asp:DetailsView>
           <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" 
               DeleteMethod="Delete" InsertMethod="Insert" 
               OldValuesParameterFormatString="original_{0}" SelectMethod="GetData" 
               TypeName="recordsTableAdapters.recordsTableAdapter" UpdateMethod="Update">
               <DeleteParameters>
                   <asp:Parameter Name="Original_Id" Type="Int32" />
               </DeleteParameters>
               <InsertParameters>
                   <asp:Parameter Name="PatientName" Type="String" />
                   <asp:Parameter Name="Wieght" Type="String" />
                   <asp:Parameter Name="fileName" Type="String" />
                   <asp:Parameter Name="Image" Type="String" />
                   <asp:Parameter Name="Info" Type="String" />
                   <asp:Parameter Name="TumorSize" Type="String" />
                   <asp:Parameter Name="TumorPostion" Type="String" />
               </InsertParameters>
               <UpdateParameters>
                   <asp:Parameter Name="PatientName" Type="String" />
                   <asp:Parameter Name="Wieght" Type="String" />
                   <asp:Parameter Name="fileName" Type="String" />
                   <asp:Parameter Name="Image" Type="String" />
                   <asp:Parameter Name="Info" Type="String" />
                   <asp:Parameter Name="TumorSize" Type="String" />
                   <asp:Parameter Name="TumorPostion" Type="String" />
                   <asp:Parameter Name="Original_Id" Type="Int32" />
               </UpdateParameters>
           </asp:ObjectDataSource>
        </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="DropDownList1" 
                    EventName="SelectedIndexChanged" />
            </Triggers>
         </asp:UpdatePanel>
         <table style="margin-bottom:20px; margin-left:10px;border:1px #0e769b solid;width:528px;" >
         <tr><td> <asp:Label ID="Label1" runat="server" Text="PatientName"></asp:Label></td>
         <td><asp:DropDownList ID="DropDownList1" runat="server" 
            DataSourceID="ObjectDataSource2" DataTextField="PatientName" 
            DataValueField="Id" AutoPostBack="True" 
            onselectedindexchanged="DropDownList1_SelectedIndexChanged">
        </asp:DropDownList></td></tr>
         </table>
       
        
        <asp:ObjectDataSource ID="ObjectDataSource2" runat="server" 
            DeleteMethod="Delete" InsertMethod="Insert" 
            OldValuesParameterFormatString="original_{0}" SelectMethod="GetData" 
            TypeName="recordsTableAdapters.recordsTableAdapter" UpdateMethod="Update">
            <DeleteParameters>
                <asp:Parameter Name="Original_Id" Type="Int32" />
            </DeleteParameters>
            <InsertParameters>
                <asp:Parameter Name="PatientName" Type="String" />
                <asp:Parameter Name="Wieght" Type="String" />
                <asp:Parameter Name="fileName" Type="String" />
                <asp:Parameter Name="Image" Type="String" />
                <asp:Parameter Name="Info" Type="String" />
                <asp:Parameter Name="TumorSize" Type="String" />
                <asp:Parameter Name="TumorPostion" Type="String" />
            </InsertParameters>
            <UpdateParameters>
                <asp:Parameter Name="PatientName" Type="String" />
                <asp:Parameter Name="Wieght" Type="String" />
                <asp:Parameter Name="fileName" Type="String" />
                <asp:Parameter Name="Image" Type="String" />
                <asp:Parameter Name="Info" Type="String" />
                <asp:Parameter Name="TumorSize" Type="String" />
                <asp:Parameter Name="TumorPostion" Type="String" />
                <asp:Parameter Name="Original_Id" Type="Int32" />
            </UpdateParameters>
        </asp:ObjectDataSource>
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
