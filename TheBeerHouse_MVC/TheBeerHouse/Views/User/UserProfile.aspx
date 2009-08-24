<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<TheBeerHouse.Models.ProfileInformation>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
		
<% Html.RenderPartial("~/Views/Shared/Message.ascx"); %>
		
<form method="post" action="<%= Url.Action("UserProfile", "User") %>">
		
    <div id="SitePreferences" class="form">
		<h2 class="title">Site Preferences</h2>
		<div class="content">
            <p class="field input"><label for="subscriptionType">Subscription Type</label><br />
			    <%= Html.DropDownList("subscriptionType", "subscriptionType")%>	
            <span class="input-message"></span></p>
            
            <p class="field input"><label for="subscriptionType">Languages</label><br />
			    <%= Html.DropDownList("language", "language")%>	
            <span class="input-message"></span></p>
        </div>
     </div>
 
     <div id="PersonalDetails" class="form">
		<h2 class="title">Personal Details</h2>
		<div class="content">      
            <p class="field input"><label for="userName">First Name</label><br />
			    <%= Html.TextBox("firstName")%>
            <span class="input-message"></span></p>

            <p class="field input"><label for="userName">Last Name</label><br />
			    <%= Html.TextBox("lastName")%>
            <span class="input-message"></span></p>

            <p class="field input"><label for="subscriptionType">Gender</label><br />
			    <%= Html.DropDownList("genderType", "genderType")%>	
            <span class="input-message"></span></p>

            <p class="field input"><label for="userName">Birth Date</label><br />
			    <%= Html.TextBox("birthDate", String.Format("{0:d}", Model.BirthDate))%>
            <span class="input-message"></span></p>

            <p class="field input"><label for="subscriptionType">Occupation</label><br />
			    <%= Html.DropDownList("occupation", "occupation")%>	
            <span class="input-message"></span></p>
            
            <p class="field input"><label for="subscriptionType">Website</label><br />
			    <%= Html.TextBox("website")%>	
            <span class="input-message"></span></p>
         </div>
      </div>
        
     <div id="Address" class="form">
		<h2 class="title">Address Information</h2>
		<div class="content">            
            <p class="field input"><label for="subscriptionType">Street</label><br />
			    <%= Html.TextBox("street")%>	
            <span class="input-message"></span></p>
            
            <p class="field input"><label for="subscriptionType">City</label><br />
			    <%= Html.TextBox("city")%>	
            <span class="input-message"></span></p>
            
            <p class="field input"><label for="subscriptionType">State</label><br />
			    <%= Html.TextBox("state")%>	
            <span class="input-message"></span></p>   
            
            <p class="field input"><label for="subscriptionType">Zip / Postal Code:</label><br />
			    <%= Html.TextBox("zipcode")%>	
            <span class="input-message"></span></p>
            
            <p class="field input"><label for="subscriptionType">Country</label><br />
			    <%= Html.DropDownList("country", "country")%>	
            <span class="input-message"></span></p>
        </div>
    </div>
	<hr />
	<p><button type="submit" id="user-login-button">Submit</button></p>
		
</form>
</asp:Content>
