using System.Web.Script.Serialization;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

/// <summary>
/// Summary description for UtilityClass
/// </summary>
public class UtilityClass
{
    public TokenResponse ConvertJsonToObject(string json)
    {
        JavaScriptSerializer js = new JavaScriptSerializer();
        var token = js.Deserialize<TokenResponse>(json);
        return token;
    }

    public List<SFContact> ConvertContactJsonToObject(string json)
    {
        JObject records = JObject.Parse(json);
        List<SFContact> lstContacts = new List<SFContact>();
        foreach (var result in records["records"])
        {
            SFContact contact = new SFContact()
            {
                Id = (string)result["Id"],
                Name = (string)result["Name"],
                Phone = (string)result["Phone"],
                Email = (string)result["Email"]
            };
            lstContacts.Add(contact);
        }
        return lstContacts;
    }
}

public class TokenResponse
{
    public string id { get; set; }
    public string issued_at { get; set; }
    public string refresh_token { get; set; }
    public string instance_url { get; set; }
    public string signature { get; set; }
    public string access_token { get; set; }
}