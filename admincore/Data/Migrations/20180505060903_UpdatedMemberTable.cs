using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace admincore.Data.Migrations
{
    public partial class UpdatedMemberTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TeamMembers_Documents_ImageDocumentId",
                table: "TeamMembers");

            migrationBuilder.AlterColumn<int>(
                name: "ImageDocumentId",
                table: "TeamMembers",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_TeamMembers_Documents_ImageDocumentId",
                table: "TeamMembers",
                column: "ImageDocumentId",
                principalTable: "Documents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TeamMembers_Documents_ImageDocumentId",
                table: "TeamMembers");

            migrationBuilder.AlterColumn<int>(
                name: "ImageDocumentId",
                table: "TeamMembers",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TeamMembers_Documents_ImageDocumentId",
                table: "TeamMembers",
                column: "ImageDocumentId",
                principalTable: "Documents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
