﻿namespace ProductService.Services.Dtos
{
    public class UpdateProductDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public int StockQuantity { get; set; }
        public int CategoryId { get; set; }
    }
}
