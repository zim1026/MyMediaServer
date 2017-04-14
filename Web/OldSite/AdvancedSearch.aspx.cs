using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class AdvancedSearch:EnhancedPage {
    protected bool IsAdmin() {
        return Catalog.Users.ValidUser(Session["username"], Session["admin"]);
    }

    protected void Page_Load(object sender, EventArgs e) {
        if(!IsPostBack) {
            try {
                if(!Catalog.Users.ValidUser(Session["username"], Session["admin"]))
                    Response.Redirect("~/Login.aspx");
            } catch(Exception ex) { Response.Redirect("~/Login.aspx"); }
        }

        try {
            Master.BaseURL_Prefix = "../";
            SetPageHeading("Advanced");
            SelectTreeNode();

            Params.Clear();
            GV = gvData;
            RecordCountLabel = lblRecCount;
            DDLPageSize = ddlPageSize;
            ProcName = "pa_misc.advanced_search";
            SubjectMatter = "Advanced Search";

            gvData.PageSize = GetPageSize();

            if(!IsPostBack) {
                Reset();
                pnlZipDownlaod.Visible = false;

                /*
                if(Request.QueryString["DateAdded"] != null) {
                    try {
                        txtCreateDate.Text = DateTime.Parse(Request.QueryString["DateAdded"].ToString()).ToShortDateString();
                        if(!regexDateEval.IsValid)
                            txtCreateDate.Text = DefaultDate;
                    } catch {txtCreateDate.Text = DefaultDate;}
                }
                else
                    txtCreateDate.Text = DefaultDate;
                */

                GV.SelectedIndex = -1;

                if(Cache["data"] != null)
                    Cache.Remove("data");
                Search(true);
            }
        } catch(Exception ex) {
            ex.Source = "View/NewSongs.aspx: Page_Load()--->"+ex.Source;
            HandleError(ex, pnlError, lblError);
        }
    }

    protected void gvData_SelectedIndexChanged(object sender, EventArgs e) {
        try {
            System.Drawing.ColorConverter colConvert = new System.Drawing.ColorConverter();
            pnlSongDataHeader.BackColor = (System.Drawing.Color)colConvert.ConvertFromString("Maroon");

            pnlSongDataHeader.Visible = true;
            pnlSongData.Visible = true;

            this.SongDataExtender.Collapsed = false;
            this.SongDataExtender.ClientState = "false";
            SongDataExtender.Collapsed = false;
            SongDataExtender.ClientState = "false";

            this.pnlSearchResultsExtender.Collapsed = true;
            this.pnlSearchResultsExtender.ClientState = "true";
            pnlSearchResultsExtender.Collapsed = true;
            pnlSearchResultsExtender.ClientState = "true";
            //pnlSearchResultsExtender.Collapsed = true;

            //this.SearchResultsExtender.Collapsed = true;
            //this.SearchResultsExtender.ClientState = "true";

            //SearchResultsExtender.Collapsed = true;
            //SearchResultsExtender.Collapsed = true;
            //SearchResultsExtender.ClientState = "true";

            //SongData.ScreenMode = "Database";
            //SongData.TempImagePath = "~/Media/temp_art/";
            //SongData.MusicLibPath = Server.MapPath("~/Media/iTunes/");

            //lblRecCount.Text = gvData.SelectedDataKey.Value.ToString();

            
            if(gvData.SelectedRow != null) {
                int songID = Convert.ToInt32(gvData.SelectedDataKey.Value);
                Literal1.Text = @"<iframe name='embeddedFrame'
                                width='100%'
                                height='1000px'
                                frameborder='0'
                                scrolling='NO'
                                src='../Song.aspx?SongID=" + songID.ToString() + "' />";
            }
            //Request.Params("Page") & Params & "' />"

            //SongData.Process(Convert.ToInt32(gvData.SelectedDataKey.Value));
            //SongData.Process(songID);

        } catch(Exception ex) {
            ex.Source = "NewSongs.aspx: gvData_SelectedIndexChanged()--->"+ex.Source;
            HandleError(ex, pnlError, lblError);
        }
    }

    protected DataView BuildEmptyTable() {
        using(DataTable table = new DataTable()) {
            table.Columns.Add(new DataColumn("song_id", typeof(int)));
            table.Columns.Add(new DataColumn("artist_name", typeof(string)));
            table.Columns.Add(new DataColumn("artist_name_sort", typeof(string)));
            table.Columns.Add(new DataColumn("album_name", typeof(string)));
            table.Columns.Add(new DataColumn("album_name_sort", typeof(string)));
            table.Columns.Add(new DataColumn("file_path", typeof(string)));
            table.Columns.Add(new DataColumn("silverlight_path", typeof(string)));
            table.Columns.Add(new DataColumn("abs_file_path", typeof(string)));
            table.Columns.Add(new DataColumn("song_title", typeof(string)));
            table.Columns.Add(new DataColumn("song_title_sort", typeof(string)));
            table.Columns.Add(new DataColumn("track_num", typeof(string)));
            table.Columns.Add(new DataColumn("duration", typeof(string)));
            table.Columns.Add(new DataColumn("year", typeof(string)));
            table.Columns.Add(new DataColumn("genre", typeof(string)));
            table.Columns.Add(new DataColumn("date_added_short", typeof(string)));

            DataRow row = table.NewRow();
            row["song_id"] = -1026;
            row["artist_name"] = "empty";
            row["artist_name_sort"] = "empty";
            row["album_name"] = "empty";
            row["album_name_sort"] = "empty";
            row["file_path"] = "empty";
            row["silverlight_path"] = "empty";
            row["abs_file_path"] = "empty";
            row["song_title"] = "empty";
            row["song_title_sort"] = "empty";
            row["track_num"] = "empty";
            row["Duration"] = "empty";
            row["year"] = "empty";
            row["Genre"] = "empty";
            row["date_added_short"] = "empty";

            table.Rows.Add(row);

            return new DataView(table);
        }
    }

    new protected void Search(bool Bind) {
        try {
            if(!AbortSearch()) {
                pnlSearchResultsHeader.Visible = true;
                pnlSearchResults.Visible = true;

                pnlSearchResultsExtender.Collapsed = false;
                pnlSearchResultsExtender.ClientState = "false";

                if(Cache["data"] == null)
                    Cache["data"] = GetDataView();

                if(Bind) {
                    GV.DataSource = Cache["data"];
                    GV.DataBind();
                }
            }
            else {
                if(Cache["data"] != null)
                    Cache.Remove("data");
                GV.DataSource = null;
                GV.DataBind();

                GV.DataSource = BuildEmptyTable();
                GV.DataBind();

                lblRecCount.Text = string.Empty;
                tblGroupRow.Visible = false;
            }
        } catch(Exception ex) {
            ex.Source = GetPageName() + ".Search("+ Convert.ToString(Bind) + ")--->"+ex.Source;
            HandleError(ex, pnlError, lblError);
        }
    }

    new protected DataView GetDataView() {
        try {
            return new DataView(GetDataTable());
        } catch(Exception ex) {
            ex.Source = GetPageName() + ".GetDataView()--->"+ex.Source;
            throw;
        }
    }

    new protected DataTable GetDataTable() {
        try {
            DataSet oDS = null;
            using(ASC_DBI.ODP dbUtil = new ASC_DBI.ODP(GetConnectionString, false)) {
                //LoadCache(dbUtil);
                using(OracleCommand oCmd = new OracleCommand()) {
                    oCmd.CommandType = CommandType.StoredProcedure;
                    oCmd.CommandText = "pa_misc.advanced_search";
                    dbUtil.GetStoredProcParameters(oCmd);

                    oCmd.Parameters["in_artist_match"].Value= BoolToInt(chkMatchArtist.Checked);
                    oCmd.Parameters["in_album_match"].Value=BoolToInt(chkMatchAlbum.Checked);
                    oCmd.Parameters["in_song_match"].Value=BoolToInt(chkMatchSong.Checked);
                    oCmd.Parameters["in_genre_match"].Value=BoolToInt(chkGenre.Checked);
                    oCmd.Parameters["in_year_match"].Value=BoolToInt(chkYear.Checked);

                    oCmd.Parameters["is_artist_phrase"].Value = dbUtil.ProcessParameterValue(txtArtistPhrase.Text);
                    oCmd.Parameters["is_album_phrase"].Value = dbUtil.ProcessParameterValue(txtAlbumPhrase.Text);

                    oCmd.Parameters["is_song_phrase"].Value = dbUtil.ProcessParameterValue(txtSongPhrase.Text);
                    //oCmd.Parameters["is_song_phrase"].Value = dbUtil.ProcessParameterValue("one");

                    oCmd.Parameters["is_genre_phrase"].Value = dbUtil.ProcessParameterValue(txtGenre.Text);
                    oCmd.Parameters["is_year_phrase"].Value = dbUtil.ProcessParameterValue(txtYear.Text);

                    oDS = dbUtil.ExecuteQueryAsDataSet(oCmd);
                    //FetchCache(dbUtil);                    

                    if(oDS != null) {
                        if(oDS.Tables.Count > 0) {
                            if(oDS.Tables[0].Rows.Count > 0) {
                                pnlSearchResults.Visible = true;
                                pnlSearchResultsHeader.Visible = true;
                                lblRecCount.Text = oDS.Tables[0].Rows.Count + " match(es) found";
                                tblGroupRow.Visible = true;
                            }
                            else {
                                lblRecCount.Text = "No match(es) found";
                                tblGroupRow.Visible = false;
                                pnlSearchResults.Visible = false;
                                pnlSearchResultsHeader.Visible = false;
                            }
                        }
                        else {
                            lblRecCount.Text = "No match(es) found";
                            tblGroupRow.Visible = false;
                            pnlSearchResults.Visible = false;
                            pnlSearchResultsHeader.Visible = false;
                        }
                    }
                    else {
                        lblRecCount.Text = "No match(es) found";
                        tblGroupRow.Visible = false;
                        pnlSearchResults.Visible = false;
                        pnlSearchResultsHeader.Visible = false;
                    }
                }
            }

            if(oDS != null) {
                if(oDS.Tables.Count > 0) {
                    oDS.Tables[0].Columns.Add(new DataColumn("RecNum", typeof(Int32)));
                    for(Int32 i = 0;i<oDS.Tables[0].Rows.Count;i++) {
                        oDS.Tables[0].Rows[i]["RecNum"] = i + 1;
                    }

                    return oDS.Tables[0];
                }
                else
                    return new DataTable();
            }
            else
                return new DataTable();
        } catch(Exception ex) {
            ex.Source = System.IO.Path.GetFileName(Request.PhysicalPath) + ".GetDataTable(" + ProcName + ")--->" + ex.Source;
            throw;
        }
    }

    protected bool AbortSearch() {
        if(String.IsNullOrEmpty(txtArtistPhrase.Text.Trim()) &&
               String.IsNullOrEmpty(txtAlbumPhrase.Text.Trim()) &&
               String.IsNullOrEmpty(txtSongPhrase.Text.Trim()) &&
               String.IsNullOrEmpty(txtGenre.Text.Trim()) &&
               String.IsNullOrEmpty(txtYear.Text.Trim()))
            return true;
        else
            return false;
    }

    protected void Reset() {
        //pnlSearchCriteriaHeader.Visible = false;
        //pnlSearchCriteria.Visible = true;

        //this.SearchCriteriaExtender.Collapsed = false;
        //this.SearchCriteriaExtender.ClientState = "false";

        pnlSearchResultsHeader.Visible = false;
        pnlSearchResults.Visible=false;

        pnlSearchResultsExtender.Collapsed = true;
        pnlSearchResultsExtender.ClientState = "true";

        pnlSongDataHeader.Visible = false;
        pnlSongData.Visible = false;

        chkMatchArtist.Checked = false;
        txtArtistPhrase.Text = "";

        chkMatchAlbum.Checked = false;
        txtAlbumPhrase.Text = "";

        chkMatchSong.Checked = false;
        txtSongPhrase.Text = "";

        gvData.DataSourceID = null;

        lblRecCount.Text = "";
        tblGroupRow.Visible = false;
    }

    protected void lnkSearch_Click(object sender, EventArgs e) {
        if(Cache["data"] != null)
            Cache.Remove("data");
        Search(true);
    }

    protected void cmdReset_Click(object sender, EventArgs e) {
        Reset();
    }

    protected void cmdSelectAll_Click(object sender, EventArgs e) {
        ProcessSelection(SelectionAction.SelectAll, gvData);
    }

    protected void cmdTransfer_Click(object sender, EventArgs e) {
        ProcessSelection(SelectionAction.Transfer, gvData);
    }

    protected void cmdZipAndDownload_Click(object sender, EventArgs e) {
        ProcessSelection(SelectionAction.Transfer, gvData);
        ZipFiles(pnlZipDownlaod, lnkZipDownload);
    }

    protected void cmdReTag_Click(object sender, EventArgs e) {
        ProcessSelection(SelectionAction.ReTag, gvData);
    }

    protected void cmdRecommend_Click(object sender, EventArgs e) {
        ProcessSelection(SelectionAction.Recommend, gvData);
    }

    protected void cmdFavorites_Click(object sender, EventArgs e) {
        ProcessSelection(SelectionAction.AddToFavorites, gvData);
    }

    protected void cmdDelete_Click(object sender, EventArgs e) {
        ProcessSelection(SelectionAction.Delete, gvData);
    }

    protected void cmdUnSelectAll_Click(object sender, EventArgs e) {
        ProcessSelection(SelectionAction.UnSelectAll, gvData);
    }
}
