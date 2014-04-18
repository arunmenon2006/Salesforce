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

public partial class Login : System.Web.UI.Page
{
    string consumer_key = System.Configuration.ConfigurationManager.AppSettings["ConsumerKey"].ToString();
    string consumer_secret = System.Configuration.ConfigurationManager.AppSettings["ComsumerSecret"].ToString();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["code"] != null)
        {
            GetToken(Request.QueryString["code"]);

        }
        else
        {
            
        }
    }

    protected void LogIn(object sender, EventArgs e)
    {
        if (IsValid)
        {
            
        }
    }
    protected void btnSFLogin_Click(object sender, EventArgs e)
    {
        AuthenticateToSalesforce();
    }

    public void AuthenticateToSalesforce()
    {

        var authURI = new StringBuilder();
        authURI.Append("https://login.salesforce.com/services/oauth2/authorize?");
        authURI.Append("response_type=code");
        authURI.Append("&client_id=" + consumer_key);
        authURI.Append("&redirect_uri=");
        authURI.Append(System.Configuration.ConfigurationManager.AppSettings["RedirectUrl"].ToString());
        Response.Redirect(authURI.ToString());

    }

    private void GetToken(string code)
    {        
        try
        {
            string URI = "https://login.salesforce.com/services/oauth2/token";

            StringBuilder body = new StringBuilder();
            body.Append("code=" + code + "&");
            body.Append("grant_type=authorization_code&");
            body.Append("client_id=" + consumer_key + "&");
            body.Append("client_secret=" + consumer_secret + "&");
            body.Append("redirect_uri=");
            body.Append(System.Configuration.ConfigurationManager.AppSettings["RedirectUrl"].ToString());

            HttpWebRequest httpWReq = (HttpWebRequest)WebRequest.Create(URI);

            ASCIIEncoding encoding = new ASCIIEncoding();
            string postData = body.ToString();
            byte[] data = encoding.GetBytes(postData);

            httpWReq.Method = "POST";
            httpWReq.ContentType = "application/x-www-form-urlencoded";
            httpWReq.ContentLength = data.Length;

            using (Stream stream = httpWReq.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            HttpWebResponse restResponse = (HttpWebResponse)httpWReq.GetResponse();

            string responseString = new StreamReader(restResponse.GetResponseStream()).ReadToEnd();
            UtilityClass utility = new UtilityClass();
            TokenResponse tokenResponse = utility.ConvertJsonToObject(responseString);
            if (tokenResponse != null)
            {
                Session["TokenResponse"] = tokenResponse;
                Response.Redirect("~/Members/Dashboard.aspx", true);
            }
        }
        catch (Exception ex)
        {
            //forceConnection.TraceExceptions(ex);
        }
    }

}