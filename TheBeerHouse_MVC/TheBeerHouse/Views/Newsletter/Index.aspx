<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">

<% Html.RenderPartial("~/Views/Shared/Message.ascx"); %>
<p>Se você quiser receber nossa newsletter, favor navegar para <a href="/User/UserProfile">seu cadastro</a> e selecionar o tipo de assinatura desejado.</p>

</asp:Content>