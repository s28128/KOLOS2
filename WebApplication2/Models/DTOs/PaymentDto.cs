namespace WebApplication2.Models.DTOs;


    public class PaymentDto
    {
        public int IdClient { get; set; }
        public int IdSubscription { get; set; }
        public decimal Payment { get; set; }
    }
