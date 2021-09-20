using System;
using System.Collections.Generic;


namespace SchoolOf.Dtos
{
    public class CartDto
    {
        public long Id { get; set; }
        public string Status { get; set; }
        public ICollection<ProductDto> Products { get; set; }
    }
}
