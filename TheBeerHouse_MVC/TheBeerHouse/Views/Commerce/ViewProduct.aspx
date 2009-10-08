<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<TheBeerHouse.Models.Product>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<% Html.RenderPartial("~/Views/Shared/Message.ascx"); %>
<div class="left">
<img src="<%= ViewData.Model.FullImageUrl %>" title="<%= Html.Encode(Model.Title) %>" alt="<%= Html.Encode(Model.Title) %>" class="main-image" />
</div>

<div class="right">
<h3><%= Html.Encode(Model.Title) %></h3>
<p>SKU: <%= Html.Encode(Model.SKU) %></p>
<p><%= Model.Description %></p>

<p>Regular Price: <b><%= ViewData.Model.UnitPrice.ToString("C") %></b> | 
Sale Price: <b><font color="red"><%= ((ViewData.Model.UnitPrice * (100 - ViewData.Model.DiscountPercentage)) / 100 ).ToString("C") %></font></b> | 
Units Available: <b><%= ViewData.Model.UnitsInStock %></b></p>
	
</div>
<br />
<hr />
<form method="post" action="<%= Url.Action("AddShoppingCartItem","Commerce", new { ProductID = ViewData.Model.ProductID,  Price = (ViewData.Model.UnitPrice * (100 - ViewData.Model.DiscountPercentage)) / 100, Title = ViewData.Model.Title, SKU = ViewData.Model.SKU}) %>" class="add-cart">
	<p class="field input"><label for="quantity">Quantity: </label><%= Html.TextBox("quantity", 2, new { style = "width: 50px" })%>
	<p><button type="submit" id="add-item-button">Add to Cart</button></p>
</form> 

</asp:Content>
