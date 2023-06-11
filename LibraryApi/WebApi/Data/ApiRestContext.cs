using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using WebApi.Models;

namespace WebApi.Data
{
    public partial class ApiRestContext : DbContext
    {
        public ApiRestContext()
        {
        }

        public ApiRestContext(DbContextOptions<ApiRestContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Books> Bookss { get; set; }

        public virtual DbSet<Borrowers> Borrowerss { get; set; }
        public virtual DbSet<Loan> Loans { get; set; }




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Books>(entity =>
            {
                entity.ToTable("Books");

                entity.Property(e => e.ISBN).ValueGeneratedNever();

                entity.Property(e => e.Title).HasColumnType("VARCHAR(1000)");

                entity.Property(e => e.NameAuthor).HasColumnType("VARCHAR(1000)");
            });

            modelBuilder.Entity<Borrowers>(entity =>
            {
                entity.ToTable("Borrowers");

                entity.Property(e => e.IdBorrowers).ValueGeneratedNever();

                entity.Property(e => e.ISBN).ValueGeneratedNever();

                entity.Property(e => e.Firstname).HasColumnType("VARCHAR(1000)");

                entity.Property(e => e.Lastname).HasColumnType("VARCHAR(1000)");

                entity.Property(e => e.BorrowerAddress).HasColumnType("VARCHAR(1000)");

                /*entity.HasOne(d => d.Books)
                    .WithMany(p => p.Borrowerss)
                    .HasForeignKey(d => d.IdBooks);*/

            });

            modelBuilder.Entity<Loan>(entity =>
            {
                entity.ToTable("Loan");

                entity.Property(e => e.IdLoan).ValueGeneratedNever();

                entity.Property(e => e.IdBorrowers).ValueGeneratedNever();

                entity.Property(e => e.ISBN).ValueGeneratedNever();

                entity.Property(e => e.StartDate).HasColumnType("VARCHAR(1000)");

                entity.Property(e => e.EndDate).HasColumnType("VARCHAR(1000)");

                entity.Property(e => e.ReturnDate).HasColumnType("VARCHAR(1000)");

                /*entity.HasOne(d => d.Books)
                    .WithMany(p => p.Loans)
                    .HasForeignKey(d => d.IdBooks);
                 entity.HasOne(d => d.Borrowers)
                    .WithMany(p => p.Loans)
                    .HasForeignKey(d => d.IdBorrowers);
                 */

            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
