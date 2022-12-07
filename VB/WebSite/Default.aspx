<%-- BeginRegion Page setup --%>
<%@ Page Language="vb" AutoEventWireup="true" CodeFile="Default.aspx.vb" Inherits="Editors_ListBoxItemFiltering_ListBoxItemFiltering" %>
<%@ Register Assembly="DevExpress.Web.v15.1" Namespace="DevExpress.Web" TagPrefix="dx" %>




<%-- EndRegion --%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
	<title>Dynamic listbox item filtering via a callback with match highlighting</title>
	<%-- BeginRegion CSS --%>
	<style type="text/css">
		.gameTable {
			width: 100%;
		}
		.gameTable td.label {
			padding: 4px 4px 6px 0;
		}
		.gameTable td.gamesMenu {
			padding-top: 4px;
		}
		.listBoxWrapper strong {
			color: #FA1863;
		}
	</style>
	<%-- EndRegion --%>
	<script type="text/javascript">
		var pendingCallback = false;
var previousFilterString = "";
var listBoxFocused = false;

function OnFilterBoxKeyUp(s, e) {
	var filterString = Trim(s.GetText());
	if (pendingCallback || filterString == previousFilterString)
		return;
	pendingCallback = true;
	previousFilterString = filterString;
	window.setTimeout("games.SetValue(null); callbackPanel.PerformCallback()", 300);
}
function OnListBoxKeyUp(e) {
	listBoxFocused = true;
	if(e.keyCode == 8) {
		var currentText = filterBox.GetText();
		filterBox.SetText(currentText.substr(0, currentText.length - 1));
		if(Trim(currentText) != "")
			filterBox.RaiseKeyUp(e);
	}
}
function OnListBoxKeyPress(e) {
	listBoxFocused = true;
	filterBox.SetText(filterBox.GetText() + String.fromCharCode(e.keyCode));
	filterBox.RaiseKeyUp(e); 
}
function OnCallbackPanelEndCallback(s, e) {
	pendingCallback = false;
	if(listBoxFocused)
		games.SetFocus();
}
function Trim(str) {
	return str.replace(/\s*((\S+\s*)*)/, "$1").replace(/((\s*\S+)*)\s*/, "$1");
}
	</script>
</head>
<body>
	<form id="form1" runat="server">

	<div>
		<dx:ASPxRoundPanel ID="rpGames" runat="server" Width="240px" HeaderText="Games">
			<PanelCollection>
				<dx:PanelContent ID="PanelContent1" runat="server">
					<table cellpadding="0" cellspacing="0" class="gameTable">
						<tr>
							<td class="label" valign="top">
								Select:
							</td>
							<td valign="top">
								<%-- Filter box --%>
								<dx:ASPxTextBox ID="tbFilterBox_GameName" runat="server" Width="200px" ClientInstanceName="filterBox" Text="un">
									<ClientSideEvents KeyUp="OnFilterBoxKeyUp" GotFocus="function() { listBoxFocused = false; }" />
								</dx:ASPxTextBox>
							</td>
						</tr>
						<tr>    
							<td colspan="2" class="gamesMenu" align="center" valign="top">
								<%-- Callback panel --%>
								<dx:ASPxCallbackPanel ID="cpCallbackPanel" runat="server" Width="100%" ClientInstanceName="callbackPanel" OnCallback="OnCallback" Height="320px">
									<ClientSideEvents EndCallback="OnCallbackPanelEndCallback" />
									<PanelCollection>
										<dx:PanelContent runat="server">
										   <div class="listBoxWrapper" onkeyup="OnListBoxKeyUp(event);" onkeypress="OnListBoxKeyPress(event);">
											   <%-- Games list --%>
											   <dx:ASPxListBox ID="lbGames" runat="server" Width="240px" Height="300px" ClientInstanceName="games" EncodeHtml="False" />
											   <dx:ASPxLabel ID="lblNoGamesFound" runat="server" Text="No Games Found" ForeColor="#A0A0A0" />
										   </div>
										</dx:PanelContent>
									</PanelCollection>
								</dx:ASPxCallbackPanel>
							</td>
						</tr>
					</table>
				</dx:PanelContent>
			</PanelCollection>
		</dx:ASPxRoundPanel>

	</div>
	</form>
</body>
</html>