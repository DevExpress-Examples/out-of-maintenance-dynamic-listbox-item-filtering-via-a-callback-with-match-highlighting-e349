// Developer Express Code Central Example:
// Dynamic listbox item filtering via a callback with match highlighting
// 
// This example demonstrates how to implement dynamic listbox item filtering via a
// callback. The matched part of the item text becomes highlighted.
// 
// You can find sample updates and versions for different programming languages here:
// http://www.devexpress.com/example=E349

using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml;
using DevExpress.Web;
using System.Text;

public partial class Editors_ListBoxItemFiltering_ListBoxItemFiltering : System.Web.UI.Page {
    protected void Page_Load(object sender, EventArgs e) {
        PopulateGameList();
    }
    protected void OnCallback(object source, DevExpress.Web.CallbackEventArgsBase e) {
        PopulateGameList();
    }

    private void PopulateGameList() {
        XmlDocument doc = new XmlDocument();
        doc.Load(MapPath("~/App_Data/Games.xml"));
        lbGames.Items.Clear();
        PopulateListBoxWithGamesCore(lbGames, doc, tbFilterBox_GameName.Text);
        lblNoGamesFound.Visible = lbGames.Items.Count == 0;
    }
    private void PopulateListBoxWithGamesCore(ASPxListBox listBox, XmlDocument doc, string filteringString) {
        XmlNodeList genres = doc.SelectNodes("/games/genre");
        foreach(XmlNode genre in genres) {
            XmlNodeList genreGames = GetGenreGames(genre, filteringString);
            if(genreGames.Count > 0)
                AddListBoxItems(lbGames, genreGames, filteringString);
        }
    }
    private XmlNodeList GetGenreGames(XmlNode genreNode, string filteringString) {
        if(string.IsNullOrEmpty(filteringString))
            return genreNode.ChildNodes;
        else {
            string xPath = string.Format("game[contains(translate(@name, 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'), '{0}')]",
                filteringString.ToLowerInvariant());
            return genreNode.SelectNodes(xPath);
        }
    }
    private string GetItemText(string gameNameAndYear, string filteringString) {
        string pattern = filteringString.Trim();
        if(pattern.Length == 0)
            return gameNameAndYear;
        int indexOfFirstMatch = gameNameAndYear.IndexOf(filteringString, StringComparison.InvariantCultureIgnoreCase);
        if(indexOfFirstMatch < 0)
            return gameNameAndYear;
        StringBuilder sb = new StringBuilder();
        sb.Append(gameNameAndYear.Substring(0, indexOfFirstMatch));
        sb.Append("<strong>");
        sb.Append(gameNameAndYear.Substring(indexOfFirstMatch, pattern.Length));
        sb.Append("</strong>");
        sb.Append(gameNameAndYear.Substring(indexOfFirstMatch + pattern.Length));
        return sb.ToString();
    }
    private void AddListBoxItems(ASPxListBox listBox, XmlNodeList games, string filteringString) {
        foreach(XmlNode game in games) {
            string gameNameAndYear = string.Format("{0} ({1})", game.Attributes["name"].Value, game.Attributes["year"].Value);
            string itemText = GetItemText(gameNameAndYear, filteringString);
            listBox.Items.Add(itemText);
        }
    }
}