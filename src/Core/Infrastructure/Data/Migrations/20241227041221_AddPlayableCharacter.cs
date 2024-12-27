using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BoostStudio.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPlayableCharacter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                    ArcadeSelectionCostume1SpriteAssetHash = table.Column<uint>(type: "INTEGER", nullable: false),
                    ArcadeSelectionCostume2SpriteAssetHash = table.Column<uint>(type: "INTEGER", nullable: true),
                    ArcadeSelectionCostume3SpriteAssetHash = table.Column<uint>(type: "INTEGER", nullable: true),
                    LoadingLeftCostume1SpriteAssetHash = table.Column<uint>(type: "INTEGER", nullable: false),
                    LoadingLeftCostume2SpriteAssetHash = table.Column<uint>(type: "INTEGER", nullable: true),
                    LoadingLeftCostume3SpriteAssetHash = table.Column<uint>(type: "INTEGER", nullable: true),
                    LoadingRightCostume1SpriteAssetHash = table.Column<uint>(type: "INTEGER", nullable: false),
                    LoadingRightCostume2SpriteAssetHash = table.Column<uint>(type: "INTEGER", nullable: true),
                    LoadingRightCostume3SpriteAssetHash = table.Column<uint>(type: "INTEGER", nullable: true),
                    GenericSelectionCostume1SpriteAssetHash = table.Column<uint>(type: "INTEGER", nullable: false),
                    GenericSelectionCostume2SpriteAssetHash = table.Column<uint>(type: "INTEGER", nullable: true),
                    GenericSelectionCostume3SpriteAssetHash = table.Column<uint>(type: "INTEGER", nullable: true),
                    LoadingTargetUnitSpriteAssetHash = table.Column<uint>(type: "INTEGER", nullable: false),
                    LoadingTargetPilotCostume1SpriteAssetHash = table.Column<uint>(type: "INTEGER", nullable: false),
                    LoadingTargetPilotCostume2SpriteAssetHash = table.Column<uint>(type: "INTEGER", nullable: true),
                    LoadingTargetPilotCostume3SpriteAssetHash = table.Column<uint>(type: "INTEGER", nullable: true),
                    InGameSortieAndAwakeningPilotCostume1SpriteAssetHash = table.Column<uint>(type: "INTEGER", nullable: false),
                    InGameSortieAndAwakeningPilotCostume2SpriteAssetHash = table.Column<uint>(type: "INTEGER", nullable: true),
                    InGameSortieAndAwakeningPilotCostume3SpriteAssetHash = table.Column<uint>(type: "INTEGER", nullable: true),
                    SpriteFramesAssetHash = table.Column<uint>(type: "INTEGER", nullable: false),
                    ResultSmallUnitSpriteHash = table.Column<uint>(type: "INTEGER", nullable: false),
                    Unk112 = table.Column<byte>(type: "INTEGER", nullable: false),
                    FigurineSpriteIndex = table.Column<byte>(type: "INTEGER", nullable: false),
                    Unk114 = table.Column<ushort>(type: "INTEGER", nullable: false),
                    FigurineSpriteAssetHash = table.Column<uint>(type: "INTEGER", nullable: false),
                    LoadingTargetUnitSmallSpriteAssetHash = table.Column<uint>(type: "INTEGER", nullable: false),
                    Unk124 = table.Column<uint>(type: "INTEGER", nullable: false),
                    Unk128 = table.Column<uint>(type: "INTEGER", nullable: false),
                    CatalogStorePilotCostume2SpriteAssetHash = table.Column<uint>(type: "INTEGER", nullable: true),
                    CatalogStorePilotCostume2TString = table.Column<string>(type: "TEXT", nullable: true),
                    CatalogStorePilotCostume2String = table.Column<string>(type: "TEXT", nullable: true),
                    CatalogStorePilotCostume3SpriteAssetHash = table.Column<uint>(type: "INTEGER", nullable: true),
                    CatalogStorePilotCostume3TString = table.Column<string>(type: "TEXT", nullable: true),
                    CatalogStorePilotCostume3String = table.Column<string>(type: "TEXT", nullable: true),
                    Unk156 = table.Column<uint>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayableCharacters", x => x.UnitId);
                    table.ForeignKey(
                        name: "FK_PlayableCharacters_AssetFiles_ArcadeSelectionCostume1SpriteAssetHash",
                        column: x => x.ArcadeSelectionCostume1SpriteAssetHash,
                        principalTable: "AssetFiles",
                        principalColumn: "Hash",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlayableCharacters_AssetFiles_ArcadeSelectionCostume2SpriteAssetHash",
                        column: x => x.ArcadeSelectionCostume2SpriteAssetHash,
                        principalTable: "AssetFiles",
                        principalColumn: "Hash");
                    table.ForeignKey(
                        name: "FK_PlayableCharacters_AssetFiles_ArcadeSelectionCostume3SpriteAssetHash",
                        column: x => x.ArcadeSelectionCostume3SpriteAssetHash,
                        principalTable: "AssetFiles",
                        principalColumn: "Hash");
                    table.ForeignKey(
                        name: "FK_PlayableCharacters_AssetFiles_CatalogStorePilotCostume2SpriteAssetHash",
                        column: x => x.CatalogStorePilotCostume2SpriteAssetHash,
                        principalTable: "AssetFiles",
                        principalColumn: "Hash");
                    table.ForeignKey(
                        name: "FK_PlayableCharacters_AssetFiles_CatalogStorePilotCostume3SpriteAssetHash",
                        column: x => x.CatalogStorePilotCostume3SpriteAssetHash,
                        principalTable: "AssetFiles",
                        principalColumn: "Hash");
                    table.ForeignKey(
                        name: "FK_PlayableCharacters_AssetFiles_FigurineSpriteAssetHash",
                        column: x => x.FigurineSpriteAssetHash,
                        principalTable: "AssetFiles",
                        principalColumn: "Hash",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlayableCharacters_AssetFiles_GenericSelectionCostume1SpriteAssetHash",
                        column: x => x.GenericSelectionCostume1SpriteAssetHash,
                        principalTable: "AssetFiles",
                        principalColumn: "Hash",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlayableCharacters_AssetFiles_GenericSelectionCostume2SpriteAssetHash",
                        column: x => x.GenericSelectionCostume2SpriteAssetHash,
                        principalTable: "AssetFiles",
                        principalColumn: "Hash");
                    table.ForeignKey(
                        name: "FK_PlayableCharacters_AssetFiles_GenericSelectionCostume3SpriteAssetHash",
                        column: x => x.GenericSelectionCostume3SpriteAssetHash,
                        principalTable: "AssetFiles",
                        principalColumn: "Hash");
                    table.ForeignKey(
                        name: "FK_PlayableCharacters_AssetFiles_InGameSortieAndAwakeningPilotCostume1SpriteAssetHash",
                        column: x => x.InGameSortieAndAwakeningPilotCostume1SpriteAssetHash,
                        principalTable: "AssetFiles",
                        principalColumn: "Hash",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlayableCharacters_AssetFiles_InGameSortieAndAwakeningPilotCostume2SpriteAssetHash",
                        column: x => x.InGameSortieAndAwakeningPilotCostume2SpriteAssetHash,
                        principalTable: "AssetFiles",
                        principalColumn: "Hash");
                    table.ForeignKey(
                        name: "FK_PlayableCharacters_AssetFiles_InGameSortieAndAwakeningPilotCostume3SpriteAssetHash",
                        column: x => x.InGameSortieAndAwakeningPilotCostume3SpriteAssetHash,
                        principalTable: "AssetFiles",
                        principalColumn: "Hash");
                    table.ForeignKey(
                        name: "FK_PlayableCharacters_AssetFiles_LoadingLeftCostume1SpriteAssetHash",
                        column: x => x.LoadingLeftCostume1SpriteAssetHash,
                        principalTable: "AssetFiles",
                        principalColumn: "Hash",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlayableCharacters_AssetFiles_LoadingLeftCostume2SpriteAssetHash",
                        column: x => x.LoadingLeftCostume2SpriteAssetHash,
                        principalTable: "AssetFiles",
                        principalColumn: "Hash");
                    table.ForeignKey(
                        name: "FK_PlayableCharacters_AssetFiles_LoadingLeftCostume3SpriteAssetHash",
                        column: x => x.LoadingLeftCostume3SpriteAssetHash,
                        principalTable: "AssetFiles",
                        principalColumn: "Hash");
                    table.ForeignKey(
                        name: "FK_PlayableCharacters_AssetFiles_LoadingRightCostume1SpriteAssetHash",
                        column: x => x.LoadingRightCostume1SpriteAssetHash,
                        principalTable: "AssetFiles",
                        principalColumn: "Hash",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlayableCharacters_AssetFiles_LoadingRightCostume2SpriteAssetHash",
                        column: x => x.LoadingRightCostume2SpriteAssetHash,
                        principalTable: "AssetFiles",
                        principalColumn: "Hash");
                    table.ForeignKey(
                        name: "FK_PlayableCharacters_AssetFiles_LoadingRightCostume3SpriteAssetHash",
                        column: x => x.LoadingRightCostume3SpriteAssetHash,
                        principalTable: "AssetFiles",
                        principalColumn: "Hash");
                    table.ForeignKey(
                        name: "FK_PlayableCharacters_AssetFiles_LoadingTargetPilotCostume1SpriteAssetHash",
                        column: x => x.LoadingTargetPilotCostume1SpriteAssetHash,
                        principalTable: "AssetFiles",
                        principalColumn: "Hash",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlayableCharacters_AssetFiles_LoadingTargetPilotCostume2SpriteAssetHash",
                        column: x => x.LoadingTargetPilotCostume2SpriteAssetHash,
                        principalTable: "AssetFiles",
                        principalColumn: "Hash");
                    table.ForeignKey(
                        name: "FK_PlayableCharacters_AssetFiles_LoadingTargetPilotCostume3SpriteAssetHash",
                        column: x => x.LoadingTargetPilotCostume3SpriteAssetHash,
                        principalTable: "AssetFiles",
                        principalColumn: "Hash");
                    table.ForeignKey(
                        name: "FK_PlayableCharacters_AssetFiles_LoadingTargetUnitSmallSpriteAssetHash",
                        column: x => x.LoadingTargetUnitSmallSpriteAssetHash,
                        principalTable: "AssetFiles",
                        principalColumn: "Hash",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlayableCharacters_AssetFiles_LoadingTargetUnitSpriteAssetHash",
                        column: x => x.LoadingTargetUnitSpriteAssetHash,
                        principalTable: "AssetFiles",
                        principalColumn: "Hash",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlayableCharacters_AssetFiles_ResultSmallUnitSpriteHash",
                        column: x => x.ResultSmallUnitSpriteHash,
                        principalTable: "AssetFiles",
                        principalColumn: "Hash",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlayableCharacters_AssetFiles_SpriteFramesAssetHash",
                        column: x => x.SpriteFramesAssetHash,
                        principalTable: "AssetFiles",
                        principalColumn: "Hash",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlayableCharacters_Units_UnitId",
                        column: x => x.UnitId,
                        principalTable: "Units",
                        principalColumn: "GameUnitId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlayableCharacters_ArcadeSelectionCostume1SpriteAssetHash",
                table: "PlayableCharacters",
                column: "ArcadeSelectionCostume1SpriteAssetHash",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlayableCharacters_ArcadeSelectionCostume2SpriteAssetHash",
                table: "PlayableCharacters",
                column: "ArcadeSelectionCostume2SpriteAssetHash",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlayableCharacters_ArcadeSelectionCostume3SpriteAssetHash",
                table: "PlayableCharacters",
                column: "ArcadeSelectionCostume3SpriteAssetHash",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlayableCharacters_CatalogStorePilotCostume2SpriteAssetHash",
                table: "PlayableCharacters",
                column: "CatalogStorePilotCostume2SpriteAssetHash",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlayableCharacters_CatalogStorePilotCostume3SpriteAssetHash",
                table: "PlayableCharacters",
                column: "CatalogStorePilotCostume3SpriteAssetHash",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlayableCharacters_FigurineSpriteAssetHash",
                table: "PlayableCharacters",
                column: "FigurineSpriteAssetHash",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlayableCharacters_GenericSelectionCostume1SpriteAssetHash",
                table: "PlayableCharacters",
                column: "GenericSelectionCostume1SpriteAssetHash",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlayableCharacters_GenericSelectionCostume2SpriteAssetHash",
                table: "PlayableCharacters",
                column: "GenericSelectionCostume2SpriteAssetHash",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlayableCharacters_GenericSelectionCostume3SpriteAssetHash",
                table: "PlayableCharacters",
                column: "GenericSelectionCostume3SpriteAssetHash",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlayableCharacters_InGameSortieAndAwakeningPilotCostume1SpriteAssetHash",
                table: "PlayableCharacters",
                column: "InGameSortieAndAwakeningPilotCostume1SpriteAssetHash",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlayableCharacters_InGameSortieAndAwakeningPilotCostume2SpriteAssetHash",
                table: "PlayableCharacters",
                column: "InGameSortieAndAwakeningPilotCostume2SpriteAssetHash",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlayableCharacters_InGameSortieAndAwakeningPilotCostume3SpriteAssetHash",
                table: "PlayableCharacters",
                column: "InGameSortieAndAwakeningPilotCostume3SpriteAssetHash",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlayableCharacters_LoadingLeftCostume1SpriteAssetHash",
                table: "PlayableCharacters",
                column: "LoadingLeftCostume1SpriteAssetHash",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlayableCharacters_LoadingLeftCostume2SpriteAssetHash",
                table: "PlayableCharacters",
                column: "LoadingLeftCostume2SpriteAssetHash",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlayableCharacters_LoadingLeftCostume3SpriteAssetHash",
                table: "PlayableCharacters",
                column: "LoadingLeftCostume3SpriteAssetHash",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlayableCharacters_LoadingRightCostume1SpriteAssetHash",
                table: "PlayableCharacters",
                column: "LoadingRightCostume1SpriteAssetHash",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlayableCharacters_LoadingRightCostume2SpriteAssetHash",
                table: "PlayableCharacters",
                column: "LoadingRightCostume2SpriteAssetHash",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlayableCharacters_LoadingRightCostume3SpriteAssetHash",
                table: "PlayableCharacters",
                column: "LoadingRightCostume3SpriteAssetHash",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlayableCharacters_LoadingTargetPilotCostume1SpriteAssetHash",
                table: "PlayableCharacters",
                column: "LoadingTargetPilotCostume1SpriteAssetHash",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlayableCharacters_LoadingTargetPilotCostume2SpriteAssetHash",
                table: "PlayableCharacters",
                column: "LoadingTargetPilotCostume2SpriteAssetHash",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlayableCharacters_LoadingTargetPilotCostume3SpriteAssetHash",
                table: "PlayableCharacters",
                column: "LoadingTargetPilotCostume3SpriteAssetHash",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlayableCharacters_LoadingTargetUnitSmallSpriteAssetHash",
                table: "PlayableCharacters",
                column: "LoadingTargetUnitSmallSpriteAssetHash",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlayableCharacters_LoadingTargetUnitSpriteAssetHash",
                table: "PlayableCharacters",
                column: "LoadingTargetUnitSpriteAssetHash",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlayableCharacters_ResultSmallUnitSpriteHash",
                table: "PlayableCharacters",
                column: "ResultSmallUnitSpriteHash",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlayableCharacters_SpriteFramesAssetHash",
                table: "PlayableCharacters",
                column: "SpriteFramesAssetHash",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlayableCharacters");
        }
    }
}
