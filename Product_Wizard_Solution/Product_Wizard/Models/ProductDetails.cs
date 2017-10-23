using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Product.Models
{
    public class ProductDetails
    {
        [Required (ErrorMessage = "Product Name Is Required")]
        [Display (Name ="Product Name:")]
        [DataType (DataType.Text)]
        
        public string Pname { get; set; }

        [Required (ErrorMessage = "Product Type Is Required")]
        [Display (Name ="Product Type")]
        [DataType (DataType.Text)]
        public string Ptype { get; set; }

        [Required (ErrorMessage = "Price Is Required")]
        [Display (Name ="Price")]
        [DataType(DataType.Currency)]
        public Int32 Pamount { get; set; }
    }
}