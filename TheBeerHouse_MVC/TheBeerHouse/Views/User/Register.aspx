<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<TheBeerHouse.Models.UserInformation>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<% Html.RenderPartial("~/Views/Shared/Message.ascx"); %>

<form method="post" action="<%= Url.Action("Register", "User") %>" class="user-registration">
		
	<input type="hidden" name="returnUrl" value="<%= Html.If(!String.IsNullOrEmpty(Request.QueryString["returnUrl"]), () => Request.QueryString["returnUrl"])
														 .ElseIf(Request.UrlReferrer != null, () => Request.UrlReferrer.ToString())
														 .Else(() => String.Empty) %>" />
		
	<p class="field input"><label for="userName">User Name</label><br />
			<%= Html.TextBox("UserName")%>
        <span class="input-message"></span></p>
		
	<p class="field input"><label for="password">Password</label><br />
	        <%= Html.Password("Password")%>
		<span class="input-message"></span></p>	
	
    <p class="field input"><label for="ConfirmPassword">Confirm Password</label><br />
	        <%= Html.Password("ConfirmPassword")%>
		<span class="input-message"></span></p>	
		
	<p class="field input"><label for="Email">Email</label><br />
	        <%= Html.TextBox("Email")%>
		<span class="input-message"></span></p>	
		
	<p class="field input"><label for="SecretQuestion">Secret Question</label><br />
	        <%= Html.TextBox("SecretQuestion")%>
		<span class="input-message"></span></p>	
		
	<p class="field input"><label for="SecretAnswer">Secret Answer</label><br />
	        <%= Html.TextBox("SecretAnswer")%>
		<span class="input-message"></span></p>	
		
	<hr />
	<p><button type="submit" id="user-registration-button">Submit</button></p>
		
</form>

</asp:Content>

<asp:Content ID="ScriptContent" ContentPlaceHolderID="ScriptContent" runat="server">
<script type="text/javascript" src="/content/scripts/register.js"></script>
<% if (IsPostBack) { %>
<script type="text/javascript">
	ValidateRegistration();
</script>
<% } %>
</asp:Content>