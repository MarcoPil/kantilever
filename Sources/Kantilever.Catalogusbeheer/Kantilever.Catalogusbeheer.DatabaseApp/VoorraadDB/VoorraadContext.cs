using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Kantilever.Catalogusbeheer.DatabaseApp
{
    public partial class VoorraadContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            #warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
            optionsBuilder.UseSqlServer(@"Server=.\SQLEXPRESS;Database=Voorraad;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Voorraad>(entity =>
            {
                entity.HasKey(e => e.VrId)
                    .HasName("PK_Voorraad");

                entity.Property(e => e.VrId).HasColumnName("vr_id");

                entity.Property(e => e.VrAantal).HasColumnName("vr_aantal");

                entity.Property(e => e.VrProductId).HasColumnName("vr_product_id");
            });
        }

        public virtual DbSet<Voorraad> Voorraad { get; set; }
    }
}