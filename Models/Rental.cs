using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sakila.Models
{
    public class Rental
    {
        public int RentalId { get; set; }
        public int InventoryId { get; set; }
        public int CustomerId { get; set; }
        public DateTime RentalDate { get; set; } 
        public DateTime? ReturnDate { get; set; }
       
        public Inventory Inventory { get; set; }
        public Customer Customer{ get; set; }

        // computed property. Räknas ut from andra properties
        // saknas setter så kommer EF ignorera vid add migrations
        public bool Returned
        {
            get
            {
                // är return inte null så betyder det att filmen inte är återlämnad
                return ReturnDate != null;
            }
        }
    }
}
