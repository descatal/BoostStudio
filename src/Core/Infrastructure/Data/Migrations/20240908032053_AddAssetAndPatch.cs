using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BoostStudio.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAssetAndPatch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AssetFiles",
                columns: table => new
                {
                    Hash = table.Column<uint>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Order = table.Column<uint>(type: "INTEGER", nullable: false),
                    FileType = table.Column<int>(type: "INTEGER", nullable: false),
                    GameUnitId = table.Column<uint>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetFiles", x => x.Hash);
                    table.ForeignKey(
                        name: "FK_AssetFiles_Units_GameUnitId",
                        column: x => x.GameUnitId,
                        principalTable: "Units",
                        principalColumn: "GameUnitId");
                });

            migrationBuilder.CreateTable(
                name: "Tbl",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    CumulativeAssetIndex = table.Column<uint>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tbl", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PatchFiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    PathInfo_Path = table.Column<string>(type: "TEXT", nullable: true),
                    PathInfo_Order = table.Column<uint>(type: "INTEGER", nullable: true),
                    FileInfo_Version = table.Column<int>(type: "INTEGER", nullable: true),
                    FileInfo_Size1 = table.Column<ulong>(type: "INTEGER", nullable: true),
                    FileInfo_Size2 = table.Column<ulong>(type: "INTEGER", nullable: true),
                    FileInfo_Size3 = table.Column<ulong>(type: "INTEGER", nullable: true),
                    FileInfo_Size4 = table.Column<ulong>(type: "INTEGER", nullable: true),
                    AssetFileHash = table.Column<uint>(type: "INTEGER", nullable: true),
                    TblId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatchFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PatchFiles_AssetFiles_AssetFileHash",
                        column: x => x.AssetFileHash,
                        principalTable: "AssetFiles",
                        principalColumn: "Hash");
                    table.ForeignKey(
                        name: "FK_PatchFiles_Tbl_TblId",
                        column: x => x.TblId,
                        principalTable: "Tbl",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AssetFiles_GameUnitId",
                table: "AssetFiles",
                column: "GameUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_PatchFiles_AssetFileHash",
                table: "PatchFiles",
                column: "AssetFileHash");

            migrationBuilder.CreateIndex(
                name: "IX_PatchFiles_TblId",
                table: "PatchFiles",
                column: "TblId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PatchFiles");

            migrationBuilder.DropTable(
                name: "AssetFiles");

            migrationBuilder.DropTable(
                name: "Tbl");
        }
    }
}
