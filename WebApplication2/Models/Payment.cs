namespace WebApplication2.Models;

public class Payment
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    public Client Client { get; set; }
    public int SubscriptionId { get; set; }
    public Subscription Subscription { get; set; }
    public decimal Amount { get; set; }
    public DateTime CreatedAt { get; set; }
}