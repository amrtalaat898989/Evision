using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplication1.Models;


namespace WebApplication1.Controllers
{
    public class ProductController : Controller
    {
        EvisionContext ev = new EvisionContext();
      
        [HttpGet]
        public ActionResult Index(string search)
        {

            List<Product> c =ev.Product.Where(x=>x.Name.StartsWith(search) ||search==null).ToList();
            Session["products"] = c;
            return View(c);
        }

       
        public ActionResult excel()
        {
            
            var gv = new GridView();
            var products = (List<Product>)Session["products"];
            gv.DataSource = products;
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=evision.xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            using (StringWriter sw = new StringWriter())
            {
                using (HtmlTextWriter htw = new HtmlTextWriter(sw))
                {
                    // render the GridView to the HtmlTextWriter
                    gv.RenderControl(htw);
                    // Output the GridView content saved into StringWriter
                    Response.Output.Write(sw.ToString());
                    Response.Flush();
                    Response.End();
                }

            }
            return View();
        }

       
        public ActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        public ActionResult Create(ProductVM p)
        {
            Product pro = new Product();
            var fileName = Path.GetFileName(p.Photo.FileName);
            var DataBasepath = Path.Combine("~/Photos/", fileName);
            var PhotoFolderPath = Path.Combine(Server.MapPath("~/Photos/"), fileName);
            pro.Name = p.Name;
            pro.LastUpdated = DateTime.Now;
            pro.Photo = DataBasepath.ToString();
            pro.Price = p.Price;
            ev.Product.Add(pro);
            ev.SaveChanges();
            p.Photo.SaveAs(PhotoFolderPath);
            return RedirectToAction("index");
        }

       
        public ActionResult Edit(int id)
        {
            Product pro = new Product();
            pro = ev.Product.First(x => x.Id == id);
            return View(pro);
        }

        [HttpPost]
        public ActionResult Edit(ProductVM p)
        {
            Product pro = ev.Product.First(x=>x.Id==p.Id);
            pro.Name = p.Name;
            pro.LastUpdated = DateTime.Now;
            if (p.Photo != null)
            {
                var fileName = Path.GetFileName(p.Photo.FileName);
                var DataBasepath = Path.Combine("~/Photos/", fileName);
                var PhotoFolderPath = Path.Combine(Server.MapPath("~/Photos/"), fileName);
                pro.Photo = DataBasepath.ToString();
               p.Photo.SaveAs(PhotoFolderPath);
            }
            pro.Price = p.Price;
            ev.Entry(pro).State = System.Data.Entity.EntityState.Modified;
           ev.SaveChanges();
            return RedirectToAction("Index");
     
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            Product p = ev.Product.FirstOrDefault(x => x.Id == id);
            ev.Product.Remove(p);
            ev.SaveChanges();
            var redirectUrl = new UrlHelper(Request.RequestContext).Action("Index", "Product");
            return RedirectToAction("Index");
            
        }
       
    }
}
