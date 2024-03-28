using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuleWayEcommerce.Data.Entity
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        public string CategoryName { get; set; }

        public int MinimumStockQuantity { get; set; }

        public List<Product>? Products { get; set; }

    }

}
