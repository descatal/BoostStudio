using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BoostStudio.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddStuff : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Units",
                newName: "NameEnglish");

            migrationBuilder.AddColumn<string>(
                name: "SlugName",
                table: "Units",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "StagingDirectoryPath",
                table: "Units",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Series",
                columns: table => new
                {
                    Id = table.Column<byte>(type: "INTEGER", nullable: false),
                    SlugName = table.Column<string>(type: "TEXT", nullable: false),
                    NameEnglish = table.Column<string>(type: "TEXT", nullable: false),
                    NameJapanese = table.Column<string>(type: "TEXT", nullable: false),
                    NameChinese = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Series", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlayableCharacters",
                columns: table => new
                {
                    UnitId = table.Column<uint>(type: "INTEGER", nullable: false),
                    UnitIndex = table.Column<byte>(type: "INTEGER", nullable: false),
                    SeriesId = table.Column<byte>(type: "INTEGER", nullable: false),
                    Unk2 = table.Column<ushort>(type: "INTEGER", nullable: false),
                    FString = table.Column<string>(type: "TEXT", nullable: true),
                    FOutString = table.Column<string>(type: "TEXT", nullable: true),
                    PString = table.Column<string>(type: "TEXT", nullable: true),
                    UnitSelectOrderInSeries = table.Column<byte>(type: "INTEGER", nullable: false),
                    ArcadeSmallSpriteIndex = table.Column<byte>(type: "INTEGER", nullable: false),
                    ArcadeUnitNameSpriteIndex = table.Column<byte>(type: "INTEGER", nullable: false),
                    Unk27 = table.Column<byte>(type: "INTEGER", nullable: false),
                    Unk112 = table.Column<byte>(type: "INTEGER", nullable: false),
                    FigurineSpriteIndex = table.Column<byte>(type: "INTEGER", nullable: false),
                    Unk114 = table.Column<ushort>(type: "INTEGER", nullable: false),
                    Unk124 = table.Column<uint>(type: "INTEGER", nullable: false),
                    Unk128 = table.Column<uint>(type: "INTEGER", nullable: false),
                    CatalogStorePilotCostume2TString = table.Column<string>(type: "TEXT", nullable: true),
                    CatalogStorePilotCostume2String = table.Column<string>(type: "TEXT", nullable: true),
                    CatalogStorePilotCostume3TString = table.Column<string>(type: "TEXT", nullable: true),
                    CatalogStorePilotCostume3String = table.Column<string>(type: "TEXT", nullable: true),
                    Unk156 = table.Column<uint>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayableCharacters", x => x.UnitId);
                    table.ForeignKey(
                        name: "FK_PlayableCharacters_Series_SeriesId",
                        column: x => x.SeriesId,
                        principalTable: "Series",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlayableCharacters_Units_UnitId",
                        column: x => x.UnitId,
                        principalTable: "Units",
                        principalColumn: "GameUnitId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlayableSeries",
                columns: table => new
                {
                    SeriesId = table.Column<byte>(type: "INTEGER", nullable: false),
                    Unk2 = table.Column<byte>(type: "INTEGER", nullable: false),
                    Unk3 = table.Column<byte>(type: "INTEGER", nullable: false),
                    Unk4 = table.Column<byte>(type: "INTEGER", nullable: false),
                    SelectOrder = table.Column<byte>(type: "INTEGER", nullable: false),
                    LogoSpriteIndex = table.Column<byte>(type: "INTEGER", nullable: false),
                    LogoSprite2Index = table.Column<byte>(type: "INTEGER", nullable: false),
                    Unk11 = table.Column<byte>(type: "INTEGER", nullable: false),
                    MovieAssetHash = table.Column<uint>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayableSeries", x => x.SeriesId);
                    table.ForeignKey(
                        name: "FK_PlayableSeries_AssetFiles_MovieAssetHash",
                        column: x => x.MovieAssetHash,
                        principalTable: "AssetFiles",
                        principalColumn: "Hash");
                    table.ForeignKey(
                        name: "FK_PlayableSeries_Series_SeriesId",
                        column: x => x.SeriesId,
                        principalTable: "Series",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlayableCharacters_SeriesId",
                table: "PlayableCharacters",
                column: "SeriesId");

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
                name: "PlayableCharacters");

            migrationBuilder.DropTable(
                name: "PlayableSeries");

            migrationBuilder.DropTable(
                name: "Series");

            migrationBuilder.DropColumn(
                name: "NameEnglish",
                table: "Units");

            migrationBuilder.DropColumn(
                name: "StagingDirectoryPath",
                table: "Units");

            migrationBuilder.RenameColumn(
                name: "SlugName",
                table: "Units",
                newName: "Name");
        }
    }
}
