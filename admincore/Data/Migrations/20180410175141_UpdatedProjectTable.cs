using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace admincore.Data.Migrations
{
    public partial class UpdatedProjectTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectInitiatives_Projects_ProjectId",
                table: "ProjectInitiatives");

            migrationBuilder.DropIndex(
                name: "IX_ProjectInitiatives_ProjectId",
                table: "ProjectInitiatives");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "ProjectInitiatives");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProjectId",
                table: "ProjectInitiatives",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectInitiatives_ProjectId",
                table: "ProjectInitiatives",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectInitiatives_Projects_ProjectId",
                table: "ProjectInitiatives",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
