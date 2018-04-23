' Developer Express Code Central Example:
' Dynamic listbox item filtering via a callback with match highlighting
' 
' This example demonstrates how to implement dynamic listbox item filtering via a
' callback. The matched part of the item text becomes highlighted.
' 
' You can find sample updates and versions for different programming languages here:
' http://www.devexpress.com/example=E349


Imports Microsoft.VisualBasic
Imports System
Imports System.Data
Imports System.Configuration
Imports System.Collections
Imports System.Web
Imports System.Web.Security
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.WebControls.WebParts
Imports System.Web.UI.HtmlControls
Imports System.Xml
Imports DevExpress.Web.ASPxEditors
Imports System.Text

Partial Public Class Editors_ListBoxItemFiltering_ListBoxItemFiltering
	Inherits System.Web.UI.Page
	Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs)
		PopulateGameList()
	End Sub
	Protected Sub OnCallback(ByVal source As Object, ByVal e As DevExpress.Web.ASPxClasses.CallbackEventArgsBase)
		PopulateGameList()
	End Sub

	Private Sub PopulateGameList()
		Dim doc As New XmlDocument()
		doc.Load(MapPath("~/App_Data/Games.xml"))
		lbGames.Items.Clear()
		PopulateListBoxWithGamesCore(lbGames, doc, tbFilterBox_GameName.Text)
		lblNoGamesFound.Visible = lbGames.Items.Count = 0
	End Sub
	Private Sub PopulateListBoxWithGamesCore(ByVal listBox As ASPxListBox, ByVal doc As XmlDocument, ByVal filteringString As String)
		Dim genres As XmlNodeList = doc.SelectNodes("/games/genre")
		For Each genre As XmlNode In genres
			Dim genreGames As XmlNodeList = GetGenreGames(genre, filteringString)
			If genreGames.Count > 0 Then
				AddListBoxItems(lbGames, genreGames, filteringString)
			End If
		Next genre
	End Sub
	Private Function GetGenreGames(ByVal genreNode As XmlNode, ByVal filteringString As String) As XmlNodeList
		If String.IsNullOrEmpty(filteringString) Then
			Return genreNode.ChildNodes
		Else
			Dim xPath As String = String.Format("game[contains(translate(@name, 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'), '{0}')]", filteringString.ToLowerInvariant())
			Return genreNode.SelectNodes(xPath)
		End If
	End Function
	Private Function GetItemText(ByVal gameNameAndYear As String, ByVal filteringString As String) As String
		Dim pattern As String = filteringString.Trim()
		If pattern.Length = 0 Then
			Return gameNameAndYear
		End If
		Dim indexOfFirstMatch As Integer = gameNameAndYear.IndexOf(filteringString, StringComparison.InvariantCultureIgnoreCase)
		If indexOfFirstMatch < 0 Then
			Return gameNameAndYear
		End If
		Dim sb As New StringBuilder()
		sb.Append(gameNameAndYear.Substring(0, indexOfFirstMatch))
		sb.Append("<strong>")
		sb.Append(gameNameAndYear.Substring(indexOfFirstMatch, pattern.Length))
		sb.Append("</strong>")
		sb.Append(gameNameAndYear.Substring(indexOfFirstMatch + pattern.Length))
		Return sb.ToString()
	End Function
	Private Sub AddListBoxItems(ByVal listBox As ASPxListBox, ByVal games As XmlNodeList, ByVal filteringString As String)
		For Each game As XmlNode In games
			Dim gameNameAndYear As String = String.Format("{0} ({1})", game.Attributes("name").Value, game.Attributes("year").Value)
			Dim itemText As String = GetItemText(gameNameAndYear, filteringString)
			listBox.Items.Add(itemText)
		Next game
	End Sub
End Class