using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesApp.Models {
    //This class is used to track Ids  and auditing information for all Entities.  
    //Every Entity inherits this class
    class BaseModel {
        //Annotions work with Entity Framework to set Database configurations for properties
        [Key]//makes below named field as a key in the database
        [Required]//makes below named field as required in the database
        public int Id { get; set; }
        [Required]
        [StringLength(100)]//sets max length for string data for this field in the database
        public string CreatedBy { get; set; }
        [Required]
        public DateTime CreatedDate { get; set; }
        [Required]
        [StringLength(100)]
        public string UpdatedBy { get; set; }
        [Required]
        public DateTime UpdatedDate { get; set; }
    }
}
