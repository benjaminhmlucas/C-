using SalesApp.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesApp.Models {
    [Table("SalesPeople")]//renames table, entity framework just adds an 's' to the name of the class,
    //this allows you to use proper english for special cases like this
    class SalesPerson : BaseModel, IActive {
        [Required]//Annotions work with Entity Framework to set Database configurations for a particular field
        public bool Active { get; set; }
        [Index("FullName", Order = 2)]//creates index for speedy retrieval, using both first name and last name
        [Required]
        [StringLength(100)]
        public string FirstName { get; set; }
        [Index("FullName",Order = 1)]//creates index for speedy retrieval, using both first name and last name
        [Required]
        [StringLength(100)]
        public string LastName { get; set; }
        //create a one to one relationship, one sales region to one Sales People
        public virtual SalesRegion Region { get; set; }
        //create a one to one relationship, one sales region to one Sales People
        [Required]
        public int RegionId { get; set; }
        //create a one to many relationship, one sales region to many Sales People
        public virtual ObservableListSource<Sale> Sales { get; set; }
        [Required]
        [Range(0, double.MaxValue)]
        [Column("Target")]//rename/customize column names
        public decimal SalesTarget { get; set; }

        public string FullName {
            get {
                return string.Format("{0} {1}", FirstName, LastName).Trim();
            }
        }

    }
}
