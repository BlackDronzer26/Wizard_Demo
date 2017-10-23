using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Product.Models
{
    public class ReatailerDetails
    {
        //[Required(ErrorMessage = "Mention Atleast One")]
        //[Display(Name = "No_Of_Retailer")]
        //[DataType(DataType.PhoneNumber)]
        //[Range(1, 100)]
        //public Int32 No_Of_Retailer { get; set; }

        //[Required(ErrorMessage = "Contact Number Is Required")]
        //[Display(Name = "Reatiler Contact Number")]
        //[DataType(DataType.PhoneNumber)]
        //public Int32 Number { get; set; }

        //[Required(ErrorMessage = "Please Select Any one Option")]
        //[Display(Name = "Online Supplier:")]
        //[DataType(DataType.Text)]
        //public string Mname { get; set; }

       
        //[Display(Name = "Retailer Email Id")]
        //[DataType(DataType.EmailAddress)]
        //public Int32 Email { get; set; }

        public List<Retailer_Info> Details { get; set; }

    }

    public class Retailer_Info
    {
        //public int id { get; set; }
        public string name { get; set; }
        //public string address { get; set; }
        //[DataType (DataType.PhoneNumber)]
        //[RegularExpression(@"^(\d{10})$", ErrorMessage = "Wrong mobile")]
        //public Int64 number { get; set; }
        //[DataType (DataType.EmailAddress)]
        //public string Email { get; set; }

    }
}