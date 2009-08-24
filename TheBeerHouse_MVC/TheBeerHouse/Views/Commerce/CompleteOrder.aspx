<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<h3>Order Sucess</h3>
<p>Your order has been sucessfully recieved. Your order number is <strong><%= ViewData["OrderNumber"].ToString() %></strong>. You will recieve an email when your order has shipped out, thank you for your business.</p>
</asp:Content>
