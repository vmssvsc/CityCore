using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace admincore.Data.Migrations
{
    public partial class AddedColumnToInitiativeTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "ProjectInitiatives",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "ProjectInitiatives");
        }
    }
}
