using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Repositories.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Repositories.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string searchText)
        {
            try
            {
                List<Repository> listRep = new List<Repository>();
                if (!string.IsNullOrEmpty(searchText))
                {
                    string url = $"https://api.github.com/search/repositories?q={searchText}";

                    HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                    request.Method = "GET";
                    request.ContentType = "application/json";
                    request.UserAgent = "TestApp"; //for authenticating else it returning 403 error

                    using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                    {
                        StreamReader reader = new StreamReader(response.GetResponseStream());
                        var strJson = reader.ReadToEnd();

                        var jsonObj = (JObject)JsonConvert.DeserializeObject(strJson);
                        if (jsonObj != null)
                        {
                            var jsonItems = jsonObj["items"];
                            if (jsonItems != null && jsonItems.Count() > 0)
                            {
                                Repository rep = null;

                                foreach (var jsonRow in jsonItems)
                                {
                                    rep = new Repository();
                                    rep.Id = jsonRow["id"] != null ? (long)jsonRow["id"] : 0;
                                    rep.Name = jsonRow["name"] != null ? jsonRow["name"].ToString() : "";
                                    rep.ImageSrc = jsonRow["owner"]["avatar_url"] != null ? jsonRow["owner"]["avatar_url"].ToString() : "";
                                    rep.HtmlUrl = jsonRow["html_url"] != null ? jsonRow["html_url"].ToString() : "";
                                    listRep.Add(rep);
                                }
                            }
                        }
                    }
                }
                return View(listRep);
            }
            catch(Exception)
            {
                return View("Something went wrong!");
            }
        }

        [HttpPost]
        public ActionResult AddRepositoryMark(Repository rep)
        {
            List<Repository> repositories = null;

            if (Session["BookmarkRepository"] == null)
            {
                repositories = new List<Repository>();
                repositories.Add(rep);
            }
            else
            {
                repositories = (List<Repository>)Session["BookmarkRepository"];
                bool containsItem = repositories.Any(item => item.Id == rep.Id);
                if (!containsItem)
                    repositories.Add(rep);
            }

            Session["BookmarkRepository"] = repositories;

            return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult BookMarkList()
        {
            List<Repository> repsModel = new List<Repository>();
            if (Session["BookmarkRepository"] != null)
                repsModel = (List<Repository>)Session["BookmarkRepository"];

            return View(repsModel);
        }

    }
}