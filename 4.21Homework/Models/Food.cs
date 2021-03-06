using System;
using System.Collections.Generic;

namespace _4._21Homework.Models
{
    public partial class Food
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Style { get; set; } = null!;
        public int Stars { get; set; }
        public decimal Price { get; set; }
        public string? Comment { get; set; }
    }
}
