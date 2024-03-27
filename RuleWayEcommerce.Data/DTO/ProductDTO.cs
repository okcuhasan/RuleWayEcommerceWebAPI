using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuleWayEcommerce.Data.DTO
{
    public class ProductDTO
    {
        [Required(ErrorMessage = "Title field is required")]
        [MaxLength(200)]
        public string Title { get; set; }

        public string Description { get; set; }

        public int StockQuantity { get; set; }

        public int CategoryId { get; set; }
    }
}
