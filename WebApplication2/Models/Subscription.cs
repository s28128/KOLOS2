namespace WebApplication2.Models;

public class Subscription
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int RenewalPeriod { get; set; } 
    public decimal Amount { get; set; }
    public DateTime EndDate { get; set; }
    public int ClientId { get; set; }
    public Client Client { get; set; }
    public ICollection<Payment> Payments { get; set; }
    public ICollection<Discount> Discounts { get; set; }
}