using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BoostStudio.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSeries : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PlayableSeries",
                columns: table => new
                {
                    Id = table.Column<byte>(type: "INTEGER", nullable: false),
                    Unk2 = table.Column<byte>(type: "INTEGER", nullable: false),
                    Unk3 = table.Column<byte>(type: "INTEGER", nullable: false),
                    Unk4 = table.Column<byte>(type: "INTEGER", nullable: false),
                    SelectOrder = table.Column<byte>(type: "INTEGER", nullable: false),
                    LogoSpriteIndex = table.Column<byte>(type: "INTEGER", nullable: false),
                    LogoSprite2Index = table.Column<byte>(type: "INTEGER", nullable: false),
                    Unk11 = table.Column<byte>(type: "INTEGER", nullable: false),
                    MovieAssetHash = table.Column<uint>(type: "INTEGER", nullable: true),
                    SlugName = table.Column<string>(type: "TEXT", nullable: false),
                    NameEnglish = table.Column<string>(type: "TEXT", nullable: false),
                    NameJapanese = table.Column<string>(type: "TEXT", nullable: false),
                    NameChinese = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayableSeries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayableSeries_AssetFiles_MovieAssetHash",
                        column: x => x.MovieAssetHash,
                        principalTable: "AssetFiles",
                        principalColumn: "Hash");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlayableSeries_MovieAssetHash",
                table: "PlayableSeries",
                column: "MovieAssetHash",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlayableSeries");
        }
    }
}
