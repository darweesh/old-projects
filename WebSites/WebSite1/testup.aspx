<%@ Page Language="C#" AutoEventWireup="true" CodeFile="testup.aspx.cs" Inherits="testup" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        </div>
        <input id="File1" name="f1" type="file" /><asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
        <br />
        <input type="submit" />
    <asp:FileUpload ID="FileUpload1" runat="server" />
        </form>
</body>
</html>
