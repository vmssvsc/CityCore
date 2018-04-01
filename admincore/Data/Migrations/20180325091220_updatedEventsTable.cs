using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace admincore.Data.Migrations
{
    public partial class updatedEventsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_Documents_DocumentId",
                table: "Events");

            migrationBuilder.AlterColumn<int>(
                name: "ImageDocumentId",
                table: "Events",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "DocumentId",
                table: "Events",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Documents_DocumentId",
                table: "Events",
                column: "DocumentId",
                principalTable: "Documents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_Documents_DocumentId",
                table: "Events");

            migrationBuilder.AlterColumn<int>(
                name: "ImageDocumentId",
                table: "Events",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DocumentId",
                table: "Events",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Documents_DocumentId",
                table: "Events",
                column: "DocumentId",
                principalTable: "Documents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
