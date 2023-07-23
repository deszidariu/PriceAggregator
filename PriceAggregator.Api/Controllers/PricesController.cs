using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using PriceAggregator.Api.Data;
using PriceAggregator.Api.DTOs;
using PriceAggregator.Api.Helpers;
using PriceAggregator.Api.Models;
using PriceAggregator.Api.Repositories;
using PriceAggregator.Api.Services;

namespace PriceAggregator.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PricesController : ControllerBase
    {
        private readonly IUow uow;
        private readonly IExternalSourcePricesFatory _externalSourcePricesFactory;
        private readonly IMapper mapper;
        private readonly ILogger _logger;

        public PricesController(PriceAggregatorContext context,IUow uow, IExternalSourcePricesFatory externalSourcePricesFatory,IMapper mapper, ILogger<PricesController> logger)
        {
            this.uow = uow;
            _externalSourcePricesFactory = externalSourcePricesFatory;
            this.mapper = mapper;
            _logger = logger;
        }

        // GET: api/Prices
        [HttpGet]
        public async Task<ActionResult<Price>> GetPrice([FromQuery(Name = "datetime")] DateTime dateTime, [FromQuery(Name = "convertFrom")] string convertFrom, [FromQuery(Name = "convertTo")] string convertTo)
        {
            // set 0 minutes and 0 seconds
            dateTime = dateTime.SetDateTimeWithZeroMinutesAndSeconds();

            if (Enum.TryParse(convertFrom.ToUpper(), out CurrencyCode from))
            {
                if (Enum.TryParse(convertTo.ToUpper(),out CurrencyCode to))
                {
                    var price = await uow.PriceRepository.GetPriceByDateTimeAsync(dateTime, from, to);

                    if (price == null)
                    {
                        try
                        {
                            var bitfinexPrice = await _externalSourcePricesFactory.GetInstance(typeof(Bitfinex)).GetPriceByHourAsync(from, to, dateTime, dateTime.AddHours(1));

                            var bitstanpPrice = await _externalSourcePricesFactory.GetInstance(typeof(Bitstamp)).GetPriceByHourAsync(from, to, dateTime, dateTime.AddHours(1));

                            if (bitfinexPrice == null || bitstanpPrice == null)
                                return NotFound("Price cannot be retrieved");

                            var newPrice = new Price
                            {
                                Close = (bitfinexPrice.Close + bitstanpPrice.Close) / 2,
                                StartDateTime = dateTime,
                                FromCurrency = from,
                                ToCurrency = to
                            };

                            await uow.PriceRepository.AddPriceAsync(newPrice);
                            await uow.Complete();

                            return Ok(mapper.Map<PriceDto>(newPrice));
                        }
                        catch (Exception ex)
                        {
                            return NotFound("Price cannot be retrieved");
                        }
                    }
                    return Ok(mapper.Map<PriceDto>(price));
                }
                else
                {
                    return BadRequest("Currency not found");
                }
            }
            else
            {
                return BadRequest("Currency not found");
            }
        }

        [HttpGet]
        [Route("getAllPricesBetweenTwoDates")]
        public async Task<ActionResult> GetPricesBetweenDates([FromQuery(Name = "startingDate")] DateTime startingDate, [FromQuery(Name = "endingDate")] DateTime endingDate, [FromQuery(Name = "convertFrom")] string convertFrom, [FromQuery(Name = "convertTo")] string convertTo)
        {

            if (Enum.TryParse(convertFrom.ToUpper(), out CurrencyCode from))
            {
                if (Enum.TryParse(convertTo.ToUpper(), out CurrencyCode to))
                {
                    var allPrices = await uow.PriceRepository.GetAllPricesBetweenTwoDateTimesAsync(startingDate, endingDate, from, to);
                   
                    if (allPrices == null)
                    {
                        return NotFound("No prices where found");
                    }

                    return Ok(mapper.Map<List<PriceDto>>(allPrices));
                }
                else
                {
                    return BadRequest("Currency not found");
                }
            }
            else
            {
                return BadRequest("Currency not found");
            }
        }

        [HttpGet]
        [Route("getallprices")]
        public async Task<IActionResult> GetAllPrices()
        {
            var allPrices = await uow.PriceRepository.GetAllPricesAsync();

            return Ok(mapper.Map<List<PriceDto>>(allPrices));
        }
    }
}
