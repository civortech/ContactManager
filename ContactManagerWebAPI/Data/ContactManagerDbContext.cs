using System;
using Microsoft.EntityFrameworkCore;
using ContactManagerWebAPI.Data.Entities;

#nullable disable

namespace ContactManagerWebAPI.Data
{
    public partial class ContactManagerDbContext : DbContext
    {
        public ContactManagerDbContext()
        {
        }

        public ContactManagerDbContext(DbContextOptions<ContactManagerDbContext> options)
            : base(options)
        { }

        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Name> Names { get; set; }
        public virtual DbSet<Person> Persons { get; set; }
        public virtual DbSet<Supplier> Suppliers { get; set; }
        public virtual DbSet<VwContact> VwContacts { get; set; }
        public virtual DbSet<VwPerson> VwPersons { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.Property(e => e.Birthday).HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Person)
                    .WithMany(p => p.Customers)
                    .HasForeignKey(d => d.PersonId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Customers_Person");
            });

            modelBuilder.Entity<Name>(entity =>
            {
                entity.Property(e => e.First).HasMaxLength(50);

                entity.Property(e => e.Last).HasMaxLength(50);
            });

            modelBuilder.Entity<Person>(entity =>
            {
                entity.HasOne(d => d.Name)
                    .WithMany(p => p.People)
                    .HasForeignKey(d => d.NameId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Person_Name");
            });

            modelBuilder.Entity<Supplier>(entity =>
            {
                entity.Property(e => e.Telephone)
                    .IsRequired()
                    .HasMaxLength(12);

                entity.HasOne(d => d.Person)
                    .WithMany(p => p.Suppliers)
                    .HasForeignKey(d => d.PersonId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Supplier_Person");
            });

            modelBuilder.Entity<VwContact>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vw_Contacts");

                entity.Property(e => e.Birthday).HasColumnType("datetime");

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.First).HasMaxLength(50);

                entity.Property(e => e.Last).HasMaxLength(50);

                entity.Property(e => e.Telephone).HasMaxLength(12);

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<VwPerson>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vw_Persons");

                entity.Property(e => e.First).HasMaxLength(50);

                entity.Property(e => e.Last).HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
