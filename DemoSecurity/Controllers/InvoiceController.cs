using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DemoSecurity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private List<Invoice> invoices =
        [
            new Invoice(1, 10000, "Invoice 1", 1),
            new Invoice(2, 12000, "Invoice 2", 2),
            new Invoice(3, 42000, "Invoice 3", 1),
            new Invoice(4, 3000, "Coucou <a href='https://google.be'>ici</a>", 1),
        ];

        // autoriser peut importe le role
        [HttpGet("{id}")]
        public IActionResult GetInvoice([FromRoute]int id)
        {
            var invoice = invoices.Find(i => i.Id == id);
            if (invoice is null)
            {
                return NotFound();
            }
            if (invoice.customerId.ToString() != User.FindFirstValue(ClaimTypes.NameIdentifier) && !User.IsInRole("Admin")) {
                return Forbid();
            }
            return Ok(invoice);
        }

        [HttpGet("")]
        public IActionResult GetInvoices([FromRoute] int id)
        {
            return Ok(invoices);
        }

        [HttpGet("image")]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> GetImage([FromQuery]string url)
        {
            if(!url.StartsWith("https://maBanqueImage.com"))
            {
                return BadRequest();
            }
            using HttpClient c = new HttpClient();
            var r = await c.GetAsync(url);
            Stream s = r.Content.ReadAsStream();
            MemoryStream ms = new();
            await s.CopyToAsync(ms);
            ms.Position = 0;

            return File(ms, "image/jpg");
        }

        [HttpPost("")]
        public IActionResult Add(Invoice invoice)
        {
            invoices.Add(invoice); return Ok();
        }
    }

    public record Invoice(int Id, decimal Amount, string Content, int customerId);

    
}
