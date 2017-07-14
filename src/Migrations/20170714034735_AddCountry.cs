using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MeetingManagementServer.Migrations
{
    public partial class AddCountry : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            #region Clear Partners Table
            // to clear the table, we drop it and then recreate

            migrationBuilder.DropTable(
                name: "AvailableDates");

            migrationBuilder.DropTable(
                name: "Partners");

            migrationBuilder.CreateTable(
                name: "Partners",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Email = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Partners", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AvailableDates",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Date = table.Column<DateTime>(nullable: false),
                    PartnerId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AvailableDates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AvailableDates_Partners_PartnerId",
                        column: x => x.PartnerId,
                        principalTable: "Partners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AvailableDates_PartnerId",
                table: "AvailableDates",
                column: "PartnerId");

            #endregion

            migrationBuilder.DropForeignKey(
                name: "FK_AvailableDates_Partners_PartnerId",
                table: "AvailableDates");

            migrationBuilder.AddColumn<long>(
                name: "CountryId",
                table: "Partners",
                nullable: false);

            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Partners_CountryId",
                table: "Partners",
                column: "CountryId");

            migrationBuilder.AddForeignKey(
                name: "FK_AvailableDates_Partners_PartnerId",
                table: "AvailableDates",
                column: "PartnerId",
                principalTable: "Partners",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Partners_Countries_CountryId",
                table: "Partners",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AvailableDates_Partners_PartnerId",
                table: "AvailableDates");

            migrationBuilder.DropForeignKey(
                name: "FK_Partners_Countries_CountryId",
                table: "Partners");

            migrationBuilder.DropTable(
                name: "Countries");

            migrationBuilder.DropIndex(
                name: "IX_Partners_CountryId",
                table: "Partners");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "Partners");

            migrationBuilder.AddForeignKey(
                name: "FK_AvailableDates_Partners_PartnerId",
                table: "AvailableDates",
                column: "PartnerId",
                principalTable: "Partners",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
