using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Sakila.Models
{
    public class Inventory
    {
        public int InventoryId { get; set; }
        public int FilmId { get; set; }
        public Film Film { get; set; }
        public List<Rental> Rentals { get; set; }
        // computed property. Räknas ut from andra properties
        // saknas setter så kommer EF ignorera vid add migrations
        public bool Available {
            get
            {
                // ifall den skulle vara null är den ny. Då är den tillgänglig!
                if (Rentals == null)
                    return true;
                // ifall den aldrig hyrts ut är den tillgänglig!
                else if (Rentals.Count == 0)
                    return true;
                // har alla filmer lämnats tillbaka är den tillgänglig
                else if (Rentals.All(r => r.Returned))
                    return true;
                // annars är den inte tillgänglig
                else
                {
                    return false;
                }
            }
        }
    }
}
