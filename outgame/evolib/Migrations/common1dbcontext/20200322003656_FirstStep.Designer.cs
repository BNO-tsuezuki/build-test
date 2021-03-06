// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using evolib.Databases.common1;

namespace evolib.Migrations.common1dbcontext
{
    [DbContext(typeof(Common1DBContext))]
    [Migration("20200322003656_FirstStep")]
    partial class FirstStep
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.11-servicing-32099")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("evolib.Databases.common1.Account", b =>
                {
                    b.Property<string>("account")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("inserted")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("playerId");

                    b.HasKey("account");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("evolib.Databases.common1.PlayerId", b =>
                {
                    b.Property<long>("playerId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("account")
                        .IsRequired();

                    b.Property<DateTime>("inserted")
                        .ValueGeneratedOnAdd();

                    b.HasKey("playerId");

                    b.ToTable("PlayerIds");
                });
#pragma warning restore 612, 618
        }
    }
}
