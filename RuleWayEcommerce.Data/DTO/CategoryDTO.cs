using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuleWayEcommerce.Data.DTO
{
    public class CategoryDTO
    {
        [Required(ErrorMessage = "Required Field")]
        public string CategoryName { get; set; }

        [Required(ErrorMessage = "Required Field")]
        public int MinimumStockQuantity { get; set; }
    }
}
