using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PriceAggregator.Api.Models;

namespace PriceAggregator.Api.Data
{
    public class PriceAggregatorContext : DbContext
    {
        public PriceAggregatorContext (DbContextOptions<PriceAggregatorContext> options)
            : base(options)
        {
            this.Database.EnsureCreated();
        }

        public DbSet<Price> Prices { get; set; }
    }
}
