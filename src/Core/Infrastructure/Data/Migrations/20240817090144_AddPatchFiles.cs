using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BoostStudio.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPatchFiles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PatchFiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Hash = table.Column<uint>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Path = table.Column<string>(type: "TEXT", nullable: false),
                    Version = table.Column<int>(type: "INTEGER", nullable: false),
                    Size1 = table.Column<ulong>(type: "INTEGER", nullable: false),
                    Size2 = table.Column<ulong>(type: "INTEGER", nullable: false),
                    Size3 = table.Column<ulong>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatchFiles", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PatchFiles");
        }
    }
}
