<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">

<% Html.RenderPartial("~/Views/Shared/Message.ascx"); %>
<p>If you would like to subscribe to our newsletter, please go to <a href="/User/UserProfile">your user profile</a>, and select your subscription type.</p>

</asp:Content>