using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using MeetingManagementServer.Services.EntityFramework;

namespace MeetingManagementServer.Migrations
{
    [DbContext(typeof(EfDataStore))]
    [Migration("20170713162649_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("MeetingManagementServer.Models.AvailableDate", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Date");

                    b.Property<long>("PartnerId");

                    b.HasKey("Id");

                    b.HasIndex("PartnerId");

                    b.ToTable("AvailableDates");
                });

            modelBuilder.Entity("MeetingManagementServer.Models.Partner", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Partners");
                });

            modelBuilder.Entity("MeetingManagementServer.Models.AvailableDate", b =>
                {
                    b.HasOne("MeetingManagementServer.Models.Partner", "Partner")
                        .WithMany()
                        .HasForeignKey("PartnerId")
                        .OnDelete(DeleteBehavior.Restrict);
                });
        }
    }
}
