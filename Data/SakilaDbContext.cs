using Microsoft.EntityFrameworkCore;
using Sakila.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sakila.Data
{
    public class SakilaDbContext : DbContext
    {
        public DbSet<Film> Films { get; set; }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<FilmActor> FilmActors { get; set; }
        public DbSet<Inventory> Inventory { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Rental> Rentals { get; set; }


        public SakilaDbContext(DbContextOptions<SakilaDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelbuilder)
        {
            modelbuilder.Entity<FilmActor>()
                .HasKey(sc => new { sc.FilmId, sc.ActorId });

            modelbuilder.Entity<FilmActor>()
                .HasOne(sc => sc.Film)
                .WithMany(s => s.FilmActors)
                .HasForeignKey(sc => sc.FilmId);
            //.OnDelete(DeleteBehavior.Restrict)

            modelbuilder.Entity<FilmActor>()
                .HasOne(sc => sc.Actor)
                .WithMany(c => c.FilmActors)
                .HasForeignKey(sc => sc.ActorId);
            //.OnDelete(DeleteBehavior.Restrict)
        }
    }
}
