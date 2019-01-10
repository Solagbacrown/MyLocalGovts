using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace MyLocalGovt.Models
{

   
    public class PetitionModel
    {
     
        public List<PetitionCategory> CategoryList { get; set; }
        public List<PetitionComplaint> ComplaintList { get; set; }
      
        public int Selected { get; set; }
      
      public int CompSelected { get; set; }

       

       
        public List<PetitionModel> PetitionsList { get; set; }
        public List<PetitionModel> PetitionsSignList { get; set; }
        public List<PetitionModel> MyPetitionsList { get; set; }
        
        public string Search { get; set; }
       
        public int CategoryNameId { get; set; }
        public int CategoryId { get; set; }
        public string Category { get; set; }
        public string CategoryPic { get; set; }

        [Required]
        [Display(Name = "State")]
        public int StateId { get; set; }
        public int PetitionId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string ToWhom { get; set; }

        public string Phone { get; set; }
        [Required]
        [MaxLength(2000)]
        public string WhySign { get; set; }
        public string NameOfFile { get; set; }
        public string PetDate { get; set; }

        [Required]
        [Display(Name = "Is Approved")]
        public bool IsApproved { get; set; }
        public string UserId { get; set; }
        public int SchoolId { get; set; }

        public List<PetitionModel> AdminList { get; set; }

        [Required, Display(Name = "State")]
        public int State { get; set; }

        [Required, Display(Name = "City")]
        public int City { get; set; }
    }
}