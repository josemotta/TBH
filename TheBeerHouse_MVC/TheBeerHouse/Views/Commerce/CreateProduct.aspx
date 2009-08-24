<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<TheBeerHouse.Models.Product>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<% Html.RenderPartial("~/Views/Shared/Message.ascx"); %>

<form method="post" action="<%= Url.Action(this.ViewContext.RouteData.Values["action"] as string, "Commerce") %>" class="product-create">
	
	<p class="field input"><label for="departmentID">Department</label><br />
		<%= Html.DropDownList("departmentID")%>
		<span class="input-message"></span></p>
		
	<p class="field input"><label for="title">Title</label><br />
		<%= Html.TextBox("title")%>
		<span class="input-message"></span></p>
		
	<p class="field input"><label for="description">Description</label><br />
		<%= Html.TextArea("description")%>
		<span class="input-message"></span></p>
		
	<p class="field input"><label for="sku">SKU</label><br />
		<%= Html.TextBox("sku")%>
		<span class="input-message"></span></p>
		
	<p class="field input"><label for="unitPrice">Unit Price</label><br />
		<%= Html.TextBox("unitPrice")%>
		<span class="input-message"></span></p>	
		
	<p class="field input"><label for="discountPercentage">Discount Percentage</label><br />
		<%= Html.TextBox("discountPercentage")%>
		<span class="input-message"></span></p>	
		
	<p class="field input"><label for="unitsInStock">Units In Stock</label><br />
		<%= Html.TextBox("unitsInStock")%>
		<span class="input-message"></span></p>						
	
	<h3>Image Data</h3>
	
	<p class="field input"><label for="smallImageURL">Thumnail URL</label><br />
		<%= Html.TextBox("smallImageURL")%>
		<span class="input-message"></span></p>	
		
	<p class="field input"><label for="fullImageURL">Full Image URL</label><br />
		<%= Html.TextBox("fullImageURL")%>
		<span class="input-message"></span></p>		

    <%= Html.Hidden("id", ViewData["id"])%>
	<hr />
	<p><button type="submit" id="product-create-button">Save Product</button></p>

</form>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="SidebarContent" runat="server">
<% Html.RenderPartial("~/Views/Shared/Commerce/CommerceSidebar.ascx"); %>
</asp:Content>

<asp:Content ID="ScriptContent" ContentPlaceHolderID="ScriptContent" runat="server">
<script type="text/javascript" src="/content/scripts/tiny_mce/tiny_mce_src.js"></script>
<script type="text/javascript" src="/content/scripts/manage-product.js"></script>
<% if (IsPostBack) { %>
<script type="text/javascript">
	ValidateArticle();
</script>
<% } %>
</asp:Content>
