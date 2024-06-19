using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BoostStudio.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddProjectile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UnitHitboxes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    GameUnitId = table.Column<uint>(type: "INTEGER", nullable: false),
                    FileSignature = table.Column<uint>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnitHitboxes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UnitHitboxes_Units_GameUnitId",
                        column: x => x.GameUnitId,
                        principalTable: "Units",
                        principalColumn: "GameUnitId");
                });

            migrationBuilder.CreateTable(
                name: "UnitProjectiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    GameUnitId = table.Column<uint>(type: "INTEGER", nullable: false),
                    FileSignature = table.Column<uint>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnitProjectiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UnitProjectiles_Units_GameUnitId",
                        column: x => x.GameUnitId,
                        principalTable: "Units",
                        principalColumn: "GameUnitId");
                });

            migrationBuilder.CreateTable(
                name: "Hitboxes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Hash = table.Column<uint>(type: "INTEGER", nullable: false),
                    UnitHitboxId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hitboxes", x => x.Id);
                    table.UniqueConstraint("AK_Hitboxes_Hash", x => x.Hash);
                    table.ForeignKey(
                        name: "FK_Hitboxes_UnitHitboxes_UnitHitboxId",
                        column: x => x.UnitHitboxId,
                        principalTable: "UnitHitboxes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Projectiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Hash = table.Column<uint>(type: "INTEGER", nullable: false),
                    HitboxHash = table.Column<uint>(type: "INTEGER", nullable: true),
                    ModelHash = table.Column<uint>(type: "INTEGER", nullable: false),
                    SkeletonIndex = table.Column<uint>(type: "INTEGER", nullable: false),
                    AimType = table.Column<uint>(type: "INTEGER", nullable: false),
                    TranslateY = table.Column<float>(type: "REAL", nullable: false),
                    TranslateZ = table.Column<float>(type: "REAL", nullable: false),
                    TranslateX = table.Column<float>(type: "REAL", nullable: false),
                    RotateX = table.Column<float>(type: "REAL", nullable: false),
                    RotateZ = table.Column<float>(type: "REAL", nullable: false),
                    CosmeticHash = table.Column<uint>(type: "INTEGER", nullable: false),
                    Unk44 = table.Column<uint>(type: "INTEGER", nullable: false),
                    Unk48 = table.Column<uint>(type: "INTEGER", nullable: false),
                    Unk52 = table.Column<uint>(type: "INTEGER", nullable: false),
                    Unk56 = table.Column<uint>(type: "INTEGER", nullable: false),
                    AmmoConsumption = table.Column<uint>(type: "INTEGER", nullable: false),
                    DurationFrame = table.Column<uint>(type: "INTEGER", nullable: false),
                    MaxTravelDistance = table.Column<float>(type: "REAL", nullable: false),
                    InitialSpeed = table.Column<float>(type: "REAL", nullable: false),
                    Acceleration = table.Column<float>(type: "REAL", nullable: false),
                    AccelerationStartFrame = table.Column<uint>(type: "INTEGER", nullable: false),
                    Unk84 = table.Column<float>(type: "REAL", nullable: false),
                    MaxSpeed = table.Column<float>(type: "REAL", nullable: false),
                    Reserved92 = table.Column<float>(type: "REAL", nullable: false),
                    Reserved96 = table.Column<float>(type: "REAL", nullable: false),
                    Reserved100 = table.Column<float>(type: "REAL", nullable: false),
                    Reserved104 = table.Column<float>(type: "REAL", nullable: false),
                    Reserved108 = table.Column<float>(type: "REAL", nullable: false),
                    Reserved112 = table.Column<float>(type: "REAL", nullable: false),
                    Reserved116 = table.Column<uint>(type: "INTEGER", nullable: false),
                    HorizontalGuidance = table.Column<float>(type: "REAL", nullable: false),
                    HorizontalGuidanceAngle = table.Column<float>(type: "REAL", nullable: false),
                    VerticalGuidance = table.Column<float>(type: "REAL", nullable: false),
                    VerticalGuidanceAngle = table.Column<float>(type: "REAL", nullable: false),
                    Reserved136 = table.Column<uint>(type: "INTEGER", nullable: false),
                    Reserved140 = table.Column<uint>(type: "INTEGER", nullable: false),
                    Reserved144 = table.Column<float>(type: "REAL", nullable: false),
                    Reserved148 = table.Column<float>(type: "REAL", nullable: false),
                    Reserved152 = table.Column<float>(type: "REAL", nullable: false),
                    Reserved156 = table.Column<float>(type: "REAL", nullable: false),
                    Reserved160 = table.Column<float>(type: "REAL", nullable: false),
                    Reserved164 = table.Column<float>(type: "REAL", nullable: false),
                    Reserved168 = table.Column<uint>(type: "INTEGER", nullable: false),
                    Reserved172 = table.Column<float>(type: "REAL", nullable: false),
                    Size = table.Column<float>(type: "REAL", nullable: false),
                    Reserved180 = table.Column<uint>(type: "INTEGER", nullable: false),
                    Reserved184 = table.Column<uint>(type: "INTEGER", nullable: false),
                    SoundEffectHash = table.Column<uint>(type: "INTEGER", nullable: false),
                    Reserved192 = table.Column<uint>(type: "INTEGER", nullable: false),
                    Reserved196 = table.Column<uint>(type: "INTEGER", nullable: false),
                    ChainedProjectileHash = table.Column<uint>(type: "INTEGER", nullable: false),
                    Reserved204 = table.Column<uint>(type: "INTEGER", nullable: false),
                    Reserved208 = table.Column<uint>(type: "INTEGER", nullable: false),
                    Reserved212 = table.Column<uint>(type: "INTEGER", nullable: false),
                    Reserved216 = table.Column<uint>(type: "INTEGER", nullable: false),
                    Reserved220 = table.Column<float>(type: "REAL", nullable: false),
                    Reserved224 = table.Column<float>(type: "REAL", nullable: false),
                    Reserved228 = table.Column<float>(type: "REAL", nullable: false),
                    Reserved232 = table.Column<float>(type: "REAL", nullable: false),
                    Reserved236 = table.Column<float>(type: "REAL", nullable: false),
                    Reserved240 = table.Column<float>(type: "REAL", nullable: false),
                    Reserved244 = table.Column<float>(type: "REAL", nullable: false),
                    Reserved248 = table.Column<float>(type: "REAL", nullable: false),
                    Reserved252 = table.Column<float>(type: "REAL", nullable: false),
                    Reserved256 = table.Column<float>(type: "REAL", nullable: false),
                    Reserved260 = table.Column<float>(type: "REAL", nullable: false),
                    Reserved264 = table.Column<float>(type: "REAL", nullable: false),
                    Reserved268 = table.Column<float>(type: "REAL", nullable: false),
                    Reserved272 = table.Column<float>(type: "REAL", nullable: false),
                    Reserved276 = table.Column<float>(type: "REAL", nullable: false),
                    UnitProjectileId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projectiles", x => x.Id);
                    table.UniqueConstraint("AK_Projectiles_Hash", x => x.Hash);
                    table.ForeignKey(
                        name: "FK_Projectiles_Hitboxes_HitboxHash",
                        column: x => x.HitboxHash,
                        principalTable: "Hitboxes",
                        principalColumn: "Hash");
                    table.ForeignKey(
                        name: "FK_Projectiles_UnitProjectiles_UnitProjectileId",
                        column: x => x.UnitProjectileId,
                        principalTable: "UnitProjectiles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Hitboxes_UnitHitboxId",
                table: "Hitboxes",
                column: "UnitHitboxId");

            migrationBuilder.CreateIndex(
                name: "IX_Projectiles_HitboxHash",
                table: "Projectiles",
                column: "HitboxHash",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Projectiles_UnitProjectileId",
                table: "Projectiles",
                column: "UnitProjectileId");

            migrationBuilder.CreateIndex(
                name: "IX_UnitHitboxes_GameUnitId",
                table: "UnitHitboxes",
                column: "GameUnitId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UnitProjectiles_GameUnitId",
                table: "UnitProjectiles",
                column: "GameUnitId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Projectiles");

            migrationBuilder.DropTable(
                name: "Hitboxes");

            migrationBuilder.DropTable(
                name: "UnitProjectiles");

            migrationBuilder.DropTable(
                name: "UnitHitboxes");
        }
    }
}
