﻿namespace GloriaFestasCatalogo.Shared.Models.Products
{
    public class ProductCategory
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public List<Product> Products { get; set; } = new List<Product>();
    }
}
