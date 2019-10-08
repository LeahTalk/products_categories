using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
namespace Products_Categories.Models
{
    public class Association
    {
        // auto-implemented properties need to match the columns in your table
        // the [Key] attribute is used to mark the Model property being used for your table's Primary Key
        [Key]
        public int AssociationId { get; set; }
        // MySQL VARCHAR and TEXT types can be represeted by a string
        public int ProductId { get; set; }

        public int CategoryId { get; set; }

        public Product Product { get; set; }

        public Category Category { get; set; }

    }
}