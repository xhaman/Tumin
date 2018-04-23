using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Tumin.Data.Migrations
{
    public partial class Avatar : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AvatarImage",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    IsCurrentAvatar = table.Column<bool>(nullable: false),
                    UploadDate = table.Column<DateTime>(nullable: false),
                    Url = table.Column<string>(nullable: true),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AvatarImage", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AvatarImage");
        }
    }
}
