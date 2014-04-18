using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Collections.Specialized;
using System.Web.Script.Serialization;
using System.Text;
using System.IO;

public partial class Members_Dashboard : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindContacts();
        }
    }

    public void BindContacts()
    {
        UtilityClass utility = new UtilityClass();
        TokenResponse tokenResponse = Session["TokenResponse"] != null ? (TokenResponse)Session["TokenResponse"] : new TokenResponse();
        string userId = tokenResponse.id.Substring(tokenResponse.id.LastIndexOf('/') + 1);
        string ar = tokenResponse.id.Substring(tokenResponse.id.LastIndexOf("id") + 2);
        string orgId = ar.Substring(0, ar.LastIndexOf("/"));

        string URI = tokenResponse.instance_url + "/services/data/v29.0/query?q=SELECT Id, Name, Email, Phone FROM Contact Where OwnerId='" + userId + "'";
        /*httpWReq = (HttpWebRequest)WebRequest.Create(URI);
        httpWReq.Headers.Add("Authorization: OAuth " + tokenResponse.access_token);
        httpWReq.Method = "GET";
        httpWReq.ContentType = "application/json";*/

        System.Net.WebRequest req = System.Net.WebRequest.Create(URI);
        req.Headers.Add("Authorization: OAuth " + tokenResponse.access_token);
        WebResponse response1 = req.GetResponse();
        StreamReader sr = new System.IO.StreamReader(response1.GetResponseStream());
        string resp = sr.ReadToEnd().Trim();
        List<SFContact> lstContacts = utility.ConvertContactJsonToObject(resp);
        gvContacts.DataSource = lstContacts;
        gvContacts.DataBind();
    }
}