using SalesApp.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesApp.Models {
    class SalesRegion : BaseModel, IActive {
        [Required]//Annotions work with Entity Framework to set Database configurations for a particular field
        public bool Active { get; set; }
        [Index(IsUnique =true)]//add indexing for speeding up retrieval
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [Required]
        [StringLength(3)]
        public string Code { get; set; }
        //create a one to many relationship, one sales region to many Sales People
        public virtual ObservableListSource<SalesPerson> People { get; set; }
        //create a one to many relationship, one sales region to many Sales
        public virtual ObservableListSource<Sale> Sales { get; set; }
        [Required]
        [Range(0, double.MaxValue)]
        public decimal SalesTarget { get; set; }

    }
}
