using SchoolOf.Common.Enums;
using SchoolOf.Data.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolOf.Data.Models
{
   public class Cart : BaseEntityModel
    {
        public CartStatus Status { get; set; }
        // Adaugati in Cart o proprietate =>
        // Products care este de tip lista de Product + migrare + update baza de date
        public List<Product> Products { get; set; }

    }
}
