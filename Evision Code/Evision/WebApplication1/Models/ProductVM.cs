using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class ProductVM
    {
        public int Id { get; set; }


         [Required]
        public string Name { get; set; }

       
        
        public int Price { get; set; }
        
        public HttpPostedFileBase Photo { get; set; }

    }
}