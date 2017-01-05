using Kantilever.Magazijnbeheer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kantilever.Magazijnbeheer.DAL
{
    public class MagazijnContext : DbContext
    {
        public MagazijnContext(DbContextOptions<MagazijnContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<ArtikelVoorraad> Voorraad { get; set; }
    }
}
