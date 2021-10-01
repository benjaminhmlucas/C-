using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesApp.Models {
    class Sale : BaseModel {
        [Required]//Annotions work with Entity Framework to set Database configurations for a particular field
        [Range(0,double.MaxValue)]//Range sets value range for field
        public decimal Amount { get; set; }
        [Required]
        public DateTime Date { get; set; }
        //create a one to one relationship, one sales person to one Sale
        public virtual SalesPerson Person { get; set; }
        //create a one to one relationship, one sales person to one Sale
        [Required]
        public int PersonId { get; set; }
        //create a one to one relationship, one sales region to one Sale
        public virtual SalesRegion Region { get; set; }
        //create a one to one relationship, one sales region to one Sale
        [Required]
        public int RegionId { get; set; }
        [Required]
        public SalesStatuses Status { get; set; }

    }
}
