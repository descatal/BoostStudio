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
            migrationBuilder.DropUniqueConstraint(
                name: "AK_Units_GameUnitId",
                table: "Units");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Units",
                table: "Units");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Units");

            migrationBuilder.AlterColumn<uint>(
                name: "GameUnitId",
                table: "Units",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(uint),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Units",
                table: "Units",
                column: "GameUnitId");

            migrationBuilder.CreateTable(
                name: "AssetFiles",
                columns: table => new
                {
                    Hash = table.Column<uint>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Order = table.Column<uint>(type: "INTEGER", nullable: false),
                    FileType = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetFiles", x => x.Hash);
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
                name: "AssetFileUnit",
                columns: table => new
                {
                    AssetFilesHash = table.Column<uint>(type: "INTEGER", nullable: false),
                    UnitsGameUnitId = table.Column<uint>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetFileUnit", x => new { x.AssetFilesHash, x.UnitsGameUnitId });
                    table.ForeignKey(
                        name: "FK_AssetFileUnit_AssetFiles_AssetFilesHash",
                        column: x => x.AssetFilesHash,
                        principalTable: "AssetFiles",
                        principalColumn: "Hash",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AssetFileUnit_Units_UnitsGameUnitId",
                        column: x => x.UnitsGameUnitId,
                        principalTable: "Units",
                        principalColumn: "GameUnitId",
                        onDelete: ReferentialAction.Cascade);
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
                name: "IX_AssetFileUnit_UnitsGameUnitId",
                table: "AssetFileUnit",
                column: "UnitsGameUnitId");

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
                name: "AssetFileUnit");

            migrationBuilder.DropTable(
                name: "PatchFiles");

            migrationBuilder.DropTable(
                name: "AssetFiles");

            migrationBuilder.DropTable(
                name: "Tbl");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Units",
                table: "Units");

            migrationBuilder.AlterColumn<uint>(
                name: "GameUnitId",
                table: "Units",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(uint),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "Units",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Units_GameUnitId",
                table: "Units",
                column: "GameUnitId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Units",
                table: "Units",
                column: "Id");
        }
    }
}
