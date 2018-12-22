using System;
using System.Activities.Expressions;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Security.Cryptography;


/// <summary>
/// Summary description for WebService
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class WebService : System.Web.Services.WebService
{
    
    public WebService()
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    DataClassesDataContext dc = new DataClassesDataContext();
    
    [WebMethod]
    public string HelloWorld()
    {
        return "Hello World";
    }

    [WebMethod]
    public void getSimilarWebsites(string key, string domain)
    {

        if(key == "nil")
        {

            if (!(dc.websites.Any(w => w.websiteName == domain)))
            { //if given domain is not currently in website table

                website newSite = new website //create new website object
                {
                    websiteName = domain
                };

                dc.websites.InsertOnSubmit(newSite);
                dc.SubmitChanges(); //insert into table
            }

            var relatedKeys = (from w1 in dc.websites
                       from k1 in dc.keywords
                       from wk1 in dc.websiteKeywords
                       where (w1.wID == wk1.wID) && (k1.kID == wk1.kID) && (w1.websiteName == domain)
                       select new { k1.keyword1 }).AsQueryable();

            //query for getting the similar websites by keywords
            var website = from w in dc.websites
                          from k in dc.keywords
                          from wk in dc.websiteKeywords
                          where (w.wID == wk.wID) && (k.kID == wk.kID)
                          select new { w.websiteName };


            /*var keywords = from w in dc.websites
                           from k in dc.keywords
                           from wk in dc.websiteKeywords
                           where (w.wID == wk.wID) && (k.kID == wk.kID) && (w.websiteName == domain)
                           select new { k.keyword1 };

            var website = from w in dc.websites
                          from k in dc.keywords
                          from wk in dc.websiteKeywords
                          where (w.wID == wk.wID) && (k.kID == wk.kID) && (keywords.Intersect(k.keyword1))
                          select new { w.websiteName };*/




            JavaScriptSerializer jss = new JavaScriptSerializer();

            string json = jss.Serialize(website);

            string strCallback = Context.Request.QueryString["callback"]; // Get callback method name. e.g. jQuery17019982320107502116_1378635607531
            json = strCallback + "" + json + ""; // e.g. jQuery17019982320107502116_1378635607531(....)

            Context.Response.Clear();
            Context.Response.ContentType = "application/json";
            Context.Response.AddHeader("content-length", json.Length.ToString());
            Context.Response.Flush();

            Context.Response.Write(json);

        }
        else
        {
            if (!(dc.websites.Any(w => w.websiteName == domain)))
            { //if given domain is not currently in website table

                website newSite = new website //create new website object
                {
                    websiteName = domain
                };

                dc.websites.InsertOnSubmit(newSite);
                dc.SubmitChanges(); //insert into table
            }
            if (!(dc.keywords.Any(k => k.keyword1 == key))) //if given keyword is not currently in keyword table
            {

                keyword newKey = new keyword //create new keyword object
                {
                    keyword1 = key
                };

                dc.keywords.InsertOnSubmit(newKey);
                dc.SubmitChanges(); //insert into table
            }

            //gets the wID of the given domain
            var webID = (from w in dc.websites
                         where w.websiteName == domain
                         select w.wID).SingleOrDefault(); //.SingleOrDefault converts value into integer

            //gets the kID of the given keyword
            var keyID = (from k in dc.keywords
                         where k.keyword1 == key
                         select k.kID).SingleOrDefault();

            //checks to see if given keyword and domain is an already established connected, returns 1 if was found
            var inWK = (from wk in dc.websiteKeywords
                        where (wk.wID == webID) && (wk.kID == keyID)
                        select 1).SingleOrDefault();

            if (inWK != 1) //if anything other than 1, means the domain and keyword have not been established in the table yet
            {
                websiteKeyword newWK = new websiteKeyword //creates a new websiteKeyword object
                {
                    wID = webID,
                    kID = keyID
                };
                dc.websiteKeywords.InsertOnSubmit(newWK);
                dc.SubmitChanges(); //submits the new relationship
            }



            //query for getting the similar websites by keywords
            var website = from w in dc.websites
                          from k in dc.keywords
                          from wk in dc.websiteKeywords
                          where (w.wID == wk.wID) && (k.kID == wk.kID) && (k.keyword1 == key) && (w.websiteName != domain)
                          select new { w.websiteName };




            JavaScriptSerializer jss = new JavaScriptSerializer();

            string json = jss.Serialize(website);

            string strCallback = Context.Request.QueryString["callback"]; // Get callback method name. e.g. jQuery17019982320107502116_1378635607531
            json = strCallback + "" + json + ""; // e.g. jQuery17019982320107502116_1378635607531(....)

            Context.Response.Clear();
            Context.Response.ContentType = "application/json";
            Context.Response.AddHeader("content-length", json.Length.ToString());
            Context.Response.Flush();

            Context.Response.Write(json);
        }


        


        //return json;
    }

    [WebMethod]
    public void addNewWebsite(string id)
    {
        /*websitesLiked newWeb = new websitesLiked();
        newWeb.WebsiteName = id;

        dc.websitesLikeds.InsertOnSubmit(newWeb);
        dc.SubmitChanges(); */
    }

    [WebMethod]
    public void addNewRelationShip(string rootName, string similar)
    {
        /*websiteRelationship newRel = new websiteRelationship();
        newRel.WebsiteName = rootName;
        newRel.SimilarName = similar;

        dc.websiteRelationships.InsertOnSubmit(newRel);
        dc.SubmitChanges(); */
    }

}
