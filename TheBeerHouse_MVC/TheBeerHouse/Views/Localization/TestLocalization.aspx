<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<h1><%= HttpContext.GetLocalResourceObject("~/Views/Localization/TestLocalization", "Title", System.Globalization.CultureInfo.CurrentUICulture)%></h1>

<h3><%= HttpContext.GetGlobalResourceObject("Messages", "LocalGreeting", System.Globalization.CultureInfo.CurrentUICulture)%></h3>
<p><%= HttpContext.GetLocalResourceObject("~/Views/Localization/TestLocalization", "Message", System.Globalization.CultureInfo.CurrentUICulture)%></p>
<p><b>Localized Currency:</b> <%= ViewData["CurrencyExample"] %></p>
<p><b>Localized Numbers:</b> <%= ViewData["NumberExample"] %></p>
<p><b>Localized Percentages:</b> <%= ViewData["PercentageExample"] %></p>
<p><b>Localized Date & Time:</b> <%= ViewData["DateExample"] %></p>
<p><b><%= HttpContext.GetLocalResourceObject("~/Views/Localization/TestLocalization", "Copywrite", System.Globalization.CultureInfo.CurrentUICulture)%></b></p>

</asp:Content>