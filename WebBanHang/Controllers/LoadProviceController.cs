using Models.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace WebBanHang.Controllers
{
    public class LoadProviceController : Controller
    {
        // GET: LoadProvice
        public JsonResult LoadProvince()
        {
            var xmlDoc = XDocument.Load(Server.MapPath(@"~/Content/data/Provinces_Data.xml"));

            var xElements = xmlDoc.Element("Root").Elements("Item").Where(x => x.Attribute("type").Value == "province");
            var list = new List<ProvinceModel>();
            ProvinceModel province = null;
            foreach (var x in xElements)
            {
                province = new ProvinceModel();
                province.ID = int.Parse(x.Attribute("id").Value);
                province.Name = x.Attribute("value").Value;

                list.Add(province);
            }
            return Json(new
            {
                data = list,
                status = true
            });
        }


        public JsonResult LoadDistricts(int provinceID)
        {
            var xmlDoc = XDocument.Load(Server.MapPath(@"~/Content/data/Provinces_Data.xml"));

            var xElement = xmlDoc.Element("Root").Elements("Item").Single(x => x.Attribute("type").Value == "province" && int.Parse(x.Attribute("id").Value) == provinceID);
            var list = new List<DistrictModel>();
            DistrictModel district = null;
            foreach (var x in xElement.Elements("Item").Where(x => x.Attribute("type").Value == "district"))
            {
                district = new DistrictModel();
                district.ID = int.Parse(x.Attribute("id").Value);
                district.Name = x.Attribute("value").Value;
                district.ProvinceID = int.Parse(xElement.Attribute("id").Value);
                list.Add(district);
            }
            return Json(new
            {
                data = list,
                status = true
            });
        }







        public JsonResult LoadPrecinct(int districtID)
        {
            var xmlDoc = XDocument.Load(Server.MapPath(@"~/Content/data/Provinces_Data.xml"));

            var districtElement = xmlDoc.Descendants("Item")
                .Where(x => x.Attribute("type").Value == "district" && int.Parse(x.Attribute("id").Value) == districtID)
                .FirstOrDefault();

                var list = new List<WardModel>();
                foreach (var x in districtElement.Elements("Item").Where(x => x.Attribute("type").Value == "precinct"))
                {
                    WardModel precinct = new WardModel();
                    precinct.ID = int.Parse(x.Attribute("id").Value);
                    precinct.Name = x.Attribute("value").Value;
                    precinct.DistrictId = districtID;
                    list.Add(precinct);
                }

                return Json(new
                {
                    data = list,
                    status = true
                });
            }
       
    }
}