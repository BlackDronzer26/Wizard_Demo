using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Product.Models
{
    public class ManufactureDetails
    {
        [Required(ErrorMessage = "Manufacture Name Is Required")]
        [Display(Name = "Manufacture Name:")]
        [DataType(DataType.Text)]
        public string Mname { get; set; }

        [Required(ErrorMessage = "Address Is Required")]
        [Display(Name = "Address")]
        [DataType(DataType.MultilineText)]
        public string Address { get; set; }

        
        [Required(ErrorMessage = "Mention Atleast One")]
        [Display(Name = "No_Of_Retailer")]
        [DataType(DataType.Currency)]
        [Range(1, 100)]
        public int No_Of_Retailer { get; set; }

        [Required(ErrorMessage = "Please Select Any one Option")]
        [Display(Name = "Online Supplier:")]
        public choice OnlineSupplier { get; set; }

        [Required (ErrorMessage ="Phone Number is Required")]
        [Display(Name = "Contact No.")]
        [DataType (DataType.PhoneNumber)]
        [RegularExpression(@"^(\d{10})$", ErrorMessage = "Wrong mobile")]
        public Int64 Contact_No { get; set; }

        public List<Retailer_Info> retailer { get; set; }
    }


    public enum choice
    {
        Yes,
        No
    }

}