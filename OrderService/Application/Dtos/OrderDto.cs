namespace OrderService.Application.Dtos
{
        public class OrderDto
        {
            public int Id { get; set; }
            public int ProductId { get; set; }
            public int Quantity { get; set; }
            public decimal TotalPrice { get; set; }
            public string Status { get; set; }
            public DateTime OrderDate { get; set; }
        }

}
