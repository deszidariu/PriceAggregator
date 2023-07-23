using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PriceAggregator.Api.Data;
using PriceAggregator.Api.Models;
using PriceAggregator.Api.Services;

namespace PriceAggregator.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PricesController : ControllerBase
    {
        private readonly PriceAggregatorContext _context;
        private readonly IExternalSourcePricesFatory _externalSourcePricesFactory;
        private readonly ILogger _logger;

        public PricesController(PriceAggregatorContext context, IExternalSourcePricesFatory externalSourcePricesFatory, ILogger<PricesController> logger)
        {
            _context = context;
            _externalSourcePricesFactory = externalSourcePricesFatory;
            _logger = logger;
        }

        // GET: api/Prices
        [HttpGet]
        [Route("datetime:DateTime")]
        public async Task<ActionResult<IEnumerable<Price>>> GetPrice(DateTime datetime)
        {
          var price = await _externalSourcePricesFactory.GetInstance(typeof(Bitfinex)).GetPriceByHourAsync(CurrencyCode.BTC, CurrencyCode.USD, datetime, datetime.AddHours(1));

          var price2 = await _externalSourcePricesFactory.GetInstance(typeof(Bitstamp)).GetPriceByHourAsync(CurrencyCode.BTC, CurrencyCode.USD, datetime, datetime.AddHours(1));
            if (_context.Price == null)
          {
              return NotFound();
          }
            return new List<Price>() { price, price2};
        }

        // GET: api/Prices/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Price>> GetPrice(int id)
        {
          if (_context.Price == null)
          {
              return NotFound();
          }
            var price = await _context.Price.FindAsync(id);

            if (price == null)
            {
                return NotFound();
            }

            return price;
        }

        // PUT: api/Prices/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPrice(int id, Price price)
        {
            if (id != price.Id)
            {
                return BadRequest();
            }

            _context.Entry(price).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PriceExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Prices
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Price>> PostPrice(Price price)
        {
          if (_context.Price == null)
          {
              return Problem("Entity set 'PriceAggregatorContext.Price'  is null.");
          }
            _context.Price.Add(price);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPrice", new { id = price.Id }, price);
        }

        // DELETE: api/Prices/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePrice(int id)
        {
            if (_context.Price == null)
            {
                return NotFound();
            }
            var price = await _context.Price.FindAsync(id);
            if (price == null)
            {
                return NotFound();
            }

            _context.Price.Remove(price);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PriceExists(int id)
        {
            return (_context.Price?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
