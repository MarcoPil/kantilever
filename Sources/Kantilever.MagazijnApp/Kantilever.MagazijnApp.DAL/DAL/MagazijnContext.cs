using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Kantilever.MagazijnApp.Models;

namespace Kantilever.MagazijnApp.DAL
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
