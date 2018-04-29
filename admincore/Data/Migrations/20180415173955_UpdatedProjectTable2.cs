using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace admincore.Data.Migrations
{
    public partial class UpdatedProjectTable2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SmartCityProjects_Documents_DocumentId",
                table: "SmartCityProjects");

            migrationBuilder.AlterColumn<int>(
                name: "DocumentId",
                table: "SmartCityProjects",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_SmartCityProjects_Documents_DocumentId",
                table: "SmartCityProjects",
                column: "DocumentId",
                principalTable: "Documents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SmartCityProjects_Documents_DocumentId",
                table: "SmartCityProjects");

            migrationBuilder.AlterColumn<int>(
                name: "DocumentId",
                table: "SmartCityProjects",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SmartCityProjects_Documents_DocumentId",
                table: "SmartCityProjects",
                column: "DocumentId",
                principalTable: "Documents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
