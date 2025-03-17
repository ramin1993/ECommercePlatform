namespace OrderService.Domain.Entities
{

    public class Order
    {
        public int Id { get; private set; }
        public int ProductId { get; private set; }
        public int Quantity { get; private set; }
        public decimal TotalPrice { get; private set; }
        public string Status { get; private set; }
        public DateTime OrderDate { get; private set; }

        private Order() { } // ORM 

        public Order(int productId, int quantity, decimal unitPrice)
        {
            if (productId <= 0)
                throw new ArgumentException("Product ID must be greater than zero.");
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero.");
            if (unitPrice < 0)
                throw new ArgumentException("Unit price cannot be negative.");

            ProductId = productId;
            Quantity = quantity;
            TotalPrice = quantity * unitPrice;
            OrderDate = DateTime.UtcNow;
            Status = "Pending";
        }

        public void Complete()
        {
            if (Status != "Pending")
                throw new InvalidOperationException("Only pending orders can be completed.");
            Status = "Completed";
        }

        public void Cancel()
        {
            if (Status == "Completed")
                throw new InvalidOperationException("Completed orders cannot be cancelled.");
            Status = "Cancelled";
        }
    }
}

