<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">

<% Html.RenderPartial("~/Views/Shared/Message.ascx"); %>
<p>Em construção, favor navegar para website <a href="http://www.trendware.com.br">TRENDnet Brasil</a> em vigor.</p>
<p>Para receber a newsletter TRENDnet Brasil, favor navegar para <a href="/User/UserProfile">seu cadastro</a> e selecionar o tipo de assinatura.</p>

</asp:Content>