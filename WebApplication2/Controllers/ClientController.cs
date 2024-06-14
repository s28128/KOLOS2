using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using WebApplication2.Models;
using WebApplication2.Models.DTOs;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ClientController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ClientDto>> GetClient(int id)
        {

            var client = await _context.Clients
                .Include(c => c.Subscriptions)
                .ThenInclude(s => s.Payments)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (client == null)
            {
                return NotFound("Client not found");
            }

           
            var clientDto = new ClientDto
            {
                FirstName = client.FirstName,
                LastName = client.LastName,
                Email = client.Email,
                Phone = client.Phone,
                Subscriptions = client.Subscriptions.Select(s => new SubscriptionDto
                {
                    IdSubscription = s.Id,
                    Name = s.Name,
                    TotalPaidAmount = s.Payments.Sum(p => p.Amount)
                }).ToList()
            };

            return Ok(clientDto);
        }

        [HttpPost("payment")]
        public async Task<ActionResult<int>> AddPayment(PaymentDto paymentDto)
        {
            try
            {
      
                var client = await _context.Clients.FindAsync(paymentDto.IdClient);
                if (client == null)
                {
                    return NotFound("Client not found");
                }

                
                var subscription = await _context.Subscriptions.FindAsync(paymentDto.IdSubscription);
                if (subscription == null)
                {
                    return NotFound("Subscription not found");
                }

              
                if (subscription.EndDate < DateTime.Now)
                {
                    return BadRequest("Subscription is not active");
                }

         
                var existingPayment = await _context.Payments
                    .Where(p => p.SubscriptionId == paymentDto.IdSubscription)
                    .OrderByDescending(p => p.CreatedAt)
                    .FirstOrDefaultAsync();

                if (existingPayment != null)
                {
                    var nextRenewalDate = existingPayment.CreatedAt.AddMonths(subscription.RenewalPeriod);
                    var currentDate = DateTime.Now;

                  
                    if (currentDate >= existingPayment.CreatedAt && currentDate < nextRenewalDate)
                    {
                        return BadRequest("Payment already made for this period");
                    }
                }

                
                var totalAmount = subscription.Amount;
                var activeDiscount = await _context.Discounts
                    .Where(d => d.SubscriptionId == paymentDto.IdSubscription && d.IsActive)
                    .OrderByDescending(d => d.Value)
                    .FirstOrDefaultAsync();

                if (activeDiscount != null)
                {
                    totalAmount -= totalAmount * (activeDiscount.Value / 100);
                }

                if (paymentDto.Payment != totalAmount)
                {
                    return BadRequest("Incorrect payment amount");
                }

                
                var payment = new Payment
                {
                    ClientId = paymentDto.IdClient,
                    SubscriptionId = paymentDto.IdSubscription,
                    Amount = paymentDto.Payment,
                    CreatedAt = DateTime.Now
                };

                _context.Payments.Add(payment);
                await _context.SaveChangesAsync();

               
                return Ok(payment.Id);
            }
            catch (Exception ex)
            {
                
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
