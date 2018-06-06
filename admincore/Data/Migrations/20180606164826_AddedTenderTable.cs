using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace admincore.Data.Migrations
{
    public partial class AddedTenderTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tenders",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    DocumentId = table.Column<int>(nullable: true),
                    EndDate = table.Column<DateTime>(nullable: false),
                    File1Id = table.Column<int>(nullable: true),
                    File2Id = table.Column<int>(nullable: true),
                    FormDocumentId = table.Column<int>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: false),
                    StarDate = table.Column<DateTime>(nullable: false),
                    TenderDesc = table.Column<string>(nullable: true),
                    TenderStatus = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tenders", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tenders");
        }
    }
}
