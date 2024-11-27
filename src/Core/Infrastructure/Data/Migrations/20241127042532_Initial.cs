using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BoostStudio.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
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
                    FileType = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetFiles", x => x.Hash);
                });

            migrationBuilder.CreateTable(
                name: "Configs",
                columns: table => new
                {
                    Key = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Value = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configs", x => x.Key);
                });

            migrationBuilder.CreateTable(
                name: "HitboxGroups",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Hash = table.Column<uint>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HitboxGroups", x => x.Id);
                    table.UniqueConstraint("AK_HitboxGroups_Hash", x => x.Hash);
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
                name: "Hitboxes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Hash = table.Column<uint>(type: "INTEGER", nullable: false),
                    HitboxType = table.Column<uint>(type: "INTEGER", nullable: false),
                    Damage = table.Column<uint>(type: "INTEGER", nullable: false),
                    Unk8 = table.Column<uint>(type: "INTEGER", nullable: false),
                    DownValueThreshold = table.Column<uint>(type: "INTEGER", nullable: false),
                    YorukeValueThreshold = table.Column<uint>(type: "INTEGER", nullable: false),
                    Unk20 = table.Column<uint>(type: "INTEGER", nullable: false),
                    Unk24 = table.Column<uint>(type: "INTEGER", nullable: false),
                    DamageCorrection = table.Column<float>(type: "REAL", nullable: false),
                    SpecialEffect = table.Column<uint>(type: "INTEGER", nullable: false),
                    HitEffect = table.Column<uint>(type: "INTEGER", nullable: false),
                    FlyDirection1 = table.Column<uint>(type: "INTEGER", nullable: false),
                    FlyDirection2 = table.Column<uint>(type: "INTEGER", nullable: false),
                    FlyDirection3 = table.Column<uint>(type: "INTEGER", nullable: false),
                    EnemyCameraShakeMultiplier = table.Column<uint>(type: "INTEGER", nullable: false),
                    PlayerCameraShakeMultiplier = table.Column<uint>(type: "INTEGER", nullable: false),
                    Unk56 = table.Column<uint>(type: "INTEGER", nullable: false),
                    KnockUpAngle = table.Column<uint>(type: "INTEGER", nullable: false),
                    KnockUpRange = table.Column<uint>(type: "INTEGER", nullable: false),
                    Unk68 = table.Column<uint>(type: "INTEGER", nullable: false),
                    MultipleHitIntervalFrame = table.Column<uint>(type: "INTEGER", nullable: false),
                    MultipleHitCount = table.Column<uint>(type: "INTEGER", nullable: false),
                    EnemyStunDuration = table.Column<uint>(type: "INTEGER", nullable: false),
                    PlayerStunDuration = table.Column<uint>(type: "INTEGER", nullable: false),
                    HitVisualEffect = table.Column<uint>(type: "INTEGER", nullable: false),
                    HitVisualEffectSizeMultiplier = table.Column<float>(type: "REAL", nullable: false),
                    HitSoundEffectHash = table.Column<uint>(type: "INTEGER", nullable: false),
                    Unk100 = table.Column<uint>(type: "INTEGER", nullable: false),
                    FriendlyDamageFlag = table.Column<uint>(type: "INTEGER", nullable: false),
                    Unk108 = table.Column<uint>(type: "INTEGER", nullable: false),
                    HitboxGroupHash = table.Column<uint>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hitboxes", x => x.Id);
                    table.UniqueConstraint("AK_Hitboxes_Hash", x => x.Hash);
                    table.ForeignKey(
                        name: "FK_Hitboxes_HitboxGroups_HitboxGroupHash",
                        column: x => x.HitboxGroupHash,
                        principalTable: "HitboxGroups",
                        principalColumn: "Hash",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Units",
                columns: table => new
                {
                    GameUnitId = table.Column<uint>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    NameJapanese = table.Column<string>(type: "TEXT", nullable: false),
                    NameChinese = table.Column<string>(type: "TEXT", nullable: false),
                    HitboxGroupHash = table.Column<uint>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Units", x => x.GameUnitId);
                    table.ForeignKey(
                        name: "FK_Units_HitboxGroups_HitboxGroupHash",
                        column: x => x.HitboxGroupHash,
                        principalTable: "HitboxGroups",
                        principalColumn: "Hash");
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
                name: "UnitStats",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    GameUnitId = table.Column<uint>(type: "INTEGER", nullable: false),
                    FileSignature = table.Column<uint>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnitStats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UnitStats_Units_GameUnitId",
                        column: x => x.GameUnitId,
                        principalTable: "Units",
                        principalColumn: "GameUnitId");
                });

            migrationBuilder.CreateTable(
                name: "Projectiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Hash = table.Column<uint>(type: "INTEGER", nullable: false),
                    ProjectileType = table.Column<uint>(type: "INTEGER", nullable: false),
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
                    Unk56 = table.Column<float>(type: "REAL", nullable: false),
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

            migrationBuilder.CreateTable(
                name: "Ammo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Hash = table.Column<uint>(type: "INTEGER", nullable: false),
                    AmmoType = table.Column<uint>(type: "INTEGER", nullable: false),
                    MaxAmmo = table.Column<uint>(type: "INTEGER", nullable: false),
                    InitialAmmo = table.Column<uint>(type: "INTEGER", nullable: false),
                    TimedDurationFrame = table.Column<uint>(type: "INTEGER", nullable: false),
                    Unk16 = table.Column<uint>(type: "INTEGER", nullable: false),
                    ReloadType = table.Column<uint>(type: "INTEGER", nullable: false),
                    CooldownDurationFrame = table.Column<uint>(type: "INTEGER", nullable: false),
                    ReloadDurationFrame = table.Column<uint>(type: "INTEGER", nullable: false),
                    AssaultBurstReloadDurationFrame = table.Column<uint>(type: "INTEGER", nullable: false),
                    BlastBurstReloadDurationFrame = table.Column<uint>(type: "INTEGER", nullable: false),
                    Unk40 = table.Column<uint>(type: "INTEGER", nullable: false),
                    Unk44 = table.Column<uint>(type: "INTEGER", nullable: false),
                    InactiveUnk48 = table.Column<uint>(type: "INTEGER", nullable: false),
                    InactiveCooldownDurationFrame = table.Column<uint>(type: "INTEGER", nullable: false),
                    InactiveReloadDurationFrame = table.Column<uint>(type: "INTEGER", nullable: false),
                    InactiveAssaultBurstReloadDurationFrame = table.Column<uint>(type: "INTEGER", nullable: false),
                    InactiveBlastBurstReloadDurationFrame = table.Column<uint>(type: "INTEGER", nullable: false),
                    InactiveUnk68 = table.Column<uint>(type: "INTEGER", nullable: false),
                    InactiveUnk72 = table.Column<uint>(type: "INTEGER", nullable: false),
                    BurstReplenish = table.Column<uint>(type: "INTEGER", nullable: false),
                    Unk80 = table.Column<uint>(type: "INTEGER", nullable: false),
                    Unk84 = table.Column<uint>(type: "INTEGER", nullable: false),
                    Unk88 = table.Column<uint>(type: "INTEGER", nullable: false),
                    ChargeInput = table.Column<uint>(type: "INTEGER", nullable: false),
                    ChargeDurationFrame = table.Column<uint>(type: "INTEGER", nullable: false),
                    AssaultBurstChargeDurationFrame = table.Column<uint>(type: "INTEGER", nullable: false),
                    BlastBurstChargeDurationFrame = table.Column<uint>(type: "INTEGER", nullable: false),
                    Unk108 = table.Column<uint>(type: "INTEGER", nullable: false),
                    Unk112 = table.Column<uint>(type: "INTEGER", nullable: false),
                    ReleaseChargeLingerDurationFrame = table.Column<uint>(type: "INTEGER", nullable: false),
                    MaxChargeLevel = table.Column<uint>(type: "INTEGER", nullable: false),
                    Unk124 = table.Column<uint>(type: "INTEGER", nullable: false),
                    Unk128 = table.Column<uint>(type: "INTEGER", nullable: false),
                    Order = table.Column<int>(type: "INTEGER", nullable: false),
                    UnitStatId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ammo", x => x.Id);
                    table.UniqueConstraint("AK_Ammo_Hash", x => x.Hash);
                    table.ForeignKey(
                        name: "FK_Ammo_UnitStats_UnitStatId",
                        column: x => x.UnitStatId,
                        principalTable: "UnitStats",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Stats",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    UnitCost = table.Column<int>(type: "INTEGER", nullable: false),
                    UnitCost2 = table.Column<int>(type: "INTEGER", nullable: false),
                    MaxHp = table.Column<int>(type: "INTEGER", nullable: false),
                    DownValueThreshold = table.Column<int>(type: "INTEGER", nullable: false),
                    YorukeValueThreshold = table.Column<int>(type: "INTEGER", nullable: false),
                    Unk20 = table.Column<int>(type: "INTEGER", nullable: false),
                    Unk24 = table.Column<int>(type: "INTEGER", nullable: false),
                    Unk28 = table.Column<int>(type: "INTEGER", nullable: false),
                    MaxBoost = table.Column<int>(type: "INTEGER", nullable: false),
                    Unk36 = table.Column<float>(type: "REAL", nullable: false),
                    Unk40 = table.Column<float>(type: "REAL", nullable: false),
                    Unk44 = table.Column<int>(type: "INTEGER", nullable: false),
                    GravityMultiplierAir = table.Column<float>(type: "REAL", nullable: false),
                    GravityMultiplierLand = table.Column<float>(type: "REAL", nullable: false),
                    Unk56 = table.Column<float>(type: "REAL", nullable: false),
                    Unk60 = table.Column<float>(type: "REAL", nullable: false),
                    Unk64 = table.Column<float>(type: "REAL", nullable: false),
                    Unk68 = table.Column<float>(type: "REAL", nullable: false),
                    Unk72 = table.Column<float>(type: "REAL", nullable: false),
                    Unk76 = table.Column<float>(type: "REAL", nullable: false),
                    Unk80 = table.Column<float>(type: "REAL", nullable: false),
                    CameraZoomMultiplier = table.Column<float>(type: "REAL", nullable: false),
                    Unk88 = table.Column<float>(type: "REAL", nullable: false),
                    Unk92 = table.Column<float>(type: "REAL", nullable: false),
                    Unk96 = table.Column<float>(type: "REAL", nullable: false),
                    Unk100 = table.Column<float>(type: "REAL", nullable: false),
                    Unk104 = table.Column<float>(type: "REAL", nullable: false),
                    Unk108 = table.Column<float>(type: "REAL", nullable: false),
                    SizeMultiplier = table.Column<float>(type: "REAL", nullable: false),
                    Unk116 = table.Column<float>(type: "REAL", nullable: false),
                    Unk120 = table.Column<float>(type: "REAL", nullable: false),
                    Unk124 = table.Column<float>(type: "REAL", nullable: false),
                    Unk128 = table.Column<float>(type: "REAL", nullable: false),
                    Unk132 = table.Column<float>(type: "REAL", nullable: false),
                    Unk136 = table.Column<float>(type: "REAL", nullable: false),
                    Unk140 = table.Column<float>(type: "REAL", nullable: false),
                    Unk144 = table.Column<float>(type: "REAL", nullable: false),
                    Unk148 = table.Column<float>(type: "REAL", nullable: false),
                    Unk152 = table.Column<float>(type: "REAL", nullable: false),
                    Unk156 = table.Column<float>(type: "REAL", nullable: false),
                    Unk160 = table.Column<float>(type: "REAL", nullable: false),
                    Unk164 = table.Column<float>(type: "REAL", nullable: false),
                    Unk168 = table.Column<float>(type: "REAL", nullable: false),
                    Unk172 = table.Column<float>(type: "REAL", nullable: false),
                    Unk176 = table.Column<float>(type: "REAL", nullable: false),
                    Unk180 = table.Column<float>(type: "REAL", nullable: false),
                    Unk184 = table.Column<float>(type: "REAL", nullable: false),
                    RedLockRangeMelee = table.Column<float>(type: "REAL", nullable: false),
                    RedLockRange = table.Column<float>(type: "REAL", nullable: false),
                    Unk196 = table.Column<float>(type: "REAL", nullable: false),
                    Unk200 = table.Column<float>(type: "REAL", nullable: false),
                    Unk204 = table.Column<int>(type: "INTEGER", nullable: false),
                    Unk208 = table.Column<int>(type: "INTEGER", nullable: false),
                    BoostReplenish = table.Column<int>(type: "INTEGER", nullable: false),
                    Unk216 = table.Column<int>(type: "INTEGER", nullable: false),
                    BoostInitialConsumption = table.Column<int>(type: "INTEGER", nullable: false),
                    BoostFuwaInitialConsumption = table.Column<int>(type: "INTEGER", nullable: false),
                    BoostFlyConsumption = table.Column<int>(type: "INTEGER", nullable: false),
                    BoostGroundStepInitialConsumption = table.Column<int>(type: "INTEGER", nullable: false),
                    BoostGroundStepConsumption = table.Column<int>(type: "INTEGER", nullable: false),
                    BoostAirStepInitialConsumption = table.Column<int>(type: "INTEGER", nullable: false),
                    BoostAirStepConsumption = table.Column<int>(type: "INTEGER", nullable: false),
                    BoostBdInitialConsumption = table.Column<int>(type: "INTEGER", nullable: false),
                    BoostBdConsumption = table.Column<int>(type: "INTEGER", nullable: false),
                    Unk256 = table.Column<int>(type: "INTEGER", nullable: false),
                    Unk260 = table.Column<int>(type: "INTEGER", nullable: false),
                    Unk264 = table.Column<int>(type: "INTEGER", nullable: false),
                    Unk268 = table.Column<int>(type: "INTEGER", nullable: false),
                    BoostTransformInitialConsumption = table.Column<int>(type: "INTEGER", nullable: false),
                    BoostTransformConsumption = table.Column<int>(type: "INTEGER", nullable: false),
                    BoostNonVernierActionConsumption = table.Column<int>(type: "INTEGER", nullable: false),
                    BoostPostActionConsumption = table.Column<int>(type: "INTEGER", nullable: false),
                    BoostRainbowStepInitialConsumption = table.Column<int>(type: "INTEGER", nullable: false),
                    Unk292 = table.Column<int>(type: "INTEGER", nullable: false),
                    Unk296 = table.Column<int>(type: "INTEGER", nullable: false),
                    Unk300 = table.Column<int>(type: "INTEGER", nullable: false),
                    Unk304 = table.Column<int>(type: "INTEGER", nullable: false),
                    Unk308 = table.Column<int>(type: "INTEGER", nullable: false),
                    Unk312 = table.Column<int>(type: "INTEGER", nullable: false),
                    Unk316 = table.Column<int>(type: "INTEGER", nullable: false),
                    Unk320 = table.Column<int>(type: "INTEGER", nullable: false),
                    Unk324 = table.Column<int>(type: "INTEGER", nullable: false),
                    Unk328 = table.Column<int>(type: "INTEGER", nullable: false),
                    Unk332 = table.Column<int>(type: "INTEGER", nullable: false),
                    AssaultBurstRedLockMelee = table.Column<float>(type: "REAL", nullable: false),
                    AssaultBurstRedLock = table.Column<float>(type: "REAL", nullable: false),
                    AssaultBurstDamageDealtMultiplier = table.Column<float>(type: "REAL", nullable: false),
                    AssaultBurstDamageTakenMultiplier = table.Column<float>(type: "REAL", nullable: false),
                    AssaultBurstMobilityMultiplier = table.Column<float>(type: "REAL", nullable: false),
                    AssaultBurstDownValueDealtMultiplier = table.Column<float>(type: "REAL", nullable: false),
                    AssaultBurstBoostConsumptionMultiplier = table.Column<float>(type: "REAL", nullable: false),
                    Unk364 = table.Column<int>(type: "INTEGER", nullable: false),
                    Unk368 = table.Column<int>(type: "INTEGER", nullable: false),
                    AssaultBurstDamageDealtBurstGaugeIncreaseMultiplier = table.Column<float>(type: "REAL", nullable: false),
                    AssaultBurstDamageTakenBurstGaugeIncreaseMultiplier = table.Column<float>(type: "REAL", nullable: false),
                    Unk380 = table.Column<int>(type: "INTEGER", nullable: false),
                    Unk384 = table.Column<float>(type: "REAL", nullable: false),
                    Unk388 = table.Column<float>(type: "REAL", nullable: false),
                    Unk392 = table.Column<float>(type: "REAL", nullable: false),
                    Unk396 = table.Column<float>(type: "REAL", nullable: false),
                    BlastBurstRedLockMelee = table.Column<float>(type: "REAL", nullable: false),
                    BlastBurstRedLock = table.Column<float>(type: "REAL", nullable: false),
                    BlastBurstDamageDealtMultiplier = table.Column<float>(type: "REAL", nullable: false),
                    BlastBurstDamageTakenMultiplier = table.Column<float>(type: "REAL", nullable: false),
                    BlastBurstMobilityMultiplier = table.Column<float>(type: "REAL", nullable: false),
                    BlastBurstDownValueDealtMultiplier = table.Column<float>(type: "REAL", nullable: false),
                    BlastBurstBoostConsumptionMultiplier = table.Column<float>(type: "REAL", nullable: false),
                    Unk428 = table.Column<int>(type: "INTEGER", nullable: false),
                    Unk432 = table.Column<int>(type: "INTEGER", nullable: false),
                    BlastBurstDamageDealtBurstGaugeIncreaseMultiplier = table.Column<float>(type: "REAL", nullable: false),
                    BlastBurstDamageTakenBurstGaugeIncreaseMultiplier = table.Column<float>(type: "REAL", nullable: false),
                    Unk444 = table.Column<int>(type: "INTEGER", nullable: false),
                    Unk448 = table.Column<float>(type: "REAL", nullable: false),
                    Unk452 = table.Column<float>(type: "REAL", nullable: false),
                    Unk456 = table.Column<float>(type: "REAL", nullable: false),
                    Unk460 = table.Column<float>(type: "REAL", nullable: false),
                    ThirdBurstRedLockMelee = table.Column<float>(type: "REAL", nullable: false),
                    ThirdBurstRedLock = table.Column<float>(type: "REAL", nullable: false),
                    ThirdBurstDamageDealtMultiplier = table.Column<float>(type: "REAL", nullable: false),
                    ThirdBurstDamageTakenMultiplier = table.Column<float>(type: "REAL", nullable: false),
                    ThirdBurstMobilityMultiplier = table.Column<float>(type: "REAL", nullable: false),
                    ThirdBurstDownValueDealtMultiplier = table.Column<float>(type: "REAL", nullable: false),
                    ThirdBurstBoostConsumptionMultiplier = table.Column<float>(type: "REAL", nullable: false),
                    Unk492 = table.Column<int>(type: "INTEGER", nullable: false),
                    Unk496 = table.Column<int>(type: "INTEGER", nullable: false),
                    ThirdBurstDamageDealtBurstGaugeIncreaseMultiplier = table.Column<float>(type: "REAL", nullable: false),
                    ThirdBurstDamageTakenBurstGaugeIncreaseMultiplier = table.Column<float>(type: "REAL", nullable: false),
                    Unk508 = table.Column<int>(type: "INTEGER", nullable: false),
                    Unk512 = table.Column<float>(type: "REAL", nullable: false),
                    Unk516 = table.Column<float>(type: "REAL", nullable: false),
                    Unk520 = table.Column<float>(type: "REAL", nullable: false),
                    Unk524 = table.Column<float>(type: "REAL", nullable: false),
                    FourthBurstRedLockMelee = table.Column<float>(type: "REAL", nullable: false),
                    FourthBurstRedLock = table.Column<float>(type: "REAL", nullable: false),
                    FourthBurstDamageDealtMultiplier = table.Column<float>(type: "REAL", nullable: false),
                    FourthBurstDamageTakenMultiplier = table.Column<float>(type: "REAL", nullable: false),
                    FourthBurstMobilityMultiplier = table.Column<float>(type: "REAL", nullable: false),
                    FourthBurstDownValueDealtMultiplier = table.Column<float>(type: "REAL", nullable: false),
                    FourthBurstBoostConsumptionMultiplier = table.Column<float>(type: "REAL", nullable: false),
                    Unk572 = table.Column<int>(type: "INTEGER", nullable: false),
                    Unk576 = table.Column<int>(type: "INTEGER", nullable: false),
                    FourthBurstDamageDealtBurstGaugeIncreaseMultiplier = table.Column<float>(type: "REAL", nullable: false),
                    FourthBurstDamageTakenBurstGaugeIncreaseMultiplier = table.Column<float>(type: "REAL", nullable: false),
                    Unk588 = table.Column<int>(type: "INTEGER", nullable: false),
                    Unk592 = table.Column<float>(type: "REAL", nullable: false),
                    Unk596 = table.Column<float>(type: "REAL", nullable: false),
                    Unk600 = table.Column<float>(type: "REAL", nullable: false),
                    Unk604 = table.Column<float>(type: "REAL", nullable: false),
                    Unk608 = table.Column<int>(type: "INTEGER", nullable: false),
                    Order = table.Column<int>(type: "INTEGER", nullable: false),
                    UnitStatId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Stats_UnitStats_UnitStatId",
                        column: x => x.UnitStatId,
                        principalTable: "UnitStats",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UnitAmmoSlots",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    SlotOrder = table.Column<int>(type: "INTEGER", nullable: false),
                    AmmoHash = table.Column<uint>(type: "INTEGER", nullable: false),
                    UnitStatId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnitAmmoSlots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UnitAmmoSlots_Ammo_AmmoHash",
                        column: x => x.AmmoHash,
                        principalTable: "Ammo",
                        principalColumn: "Hash",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UnitAmmoSlots_UnitStats_UnitStatId",
                        column: x => x.UnitStatId,
                        principalTable: "UnitStats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ammo_UnitStatId",
                table: "Ammo",
                column: "UnitStatId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetFileUnit_UnitsGameUnitId",
                table: "AssetFileUnit",
                column: "UnitsGameUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Hitboxes_HitboxGroupHash",
                table: "Hitboxes",
                column: "HitboxGroupHash");

            migrationBuilder.CreateIndex(
                name: "IX_PatchFiles_AssetFileHash",
                table: "PatchFiles",
                column: "AssetFileHash");

            migrationBuilder.CreateIndex(
                name: "IX_PatchFiles_TblId",
                table: "PatchFiles",
                column: "TblId");

            migrationBuilder.CreateIndex(
                name: "IX_Projectiles_HitboxHash",
                table: "Projectiles",
                column: "HitboxHash");

            migrationBuilder.CreateIndex(
                name: "IX_Projectiles_UnitProjectileId",
                table: "Projectiles",
                column: "UnitProjectileId");

            migrationBuilder.CreateIndex(
                name: "IX_Stats_UnitStatId",
                table: "Stats",
                column: "UnitStatId");

            migrationBuilder.CreateIndex(
                name: "IX_UnitAmmoSlots_AmmoHash",
                table: "UnitAmmoSlots",
                column: "AmmoHash");

            migrationBuilder.CreateIndex(
                name: "IX_UnitAmmoSlots_UnitStatId",
                table: "UnitAmmoSlots",
                column: "UnitStatId");

            migrationBuilder.CreateIndex(
                name: "IX_UnitProjectiles_GameUnitId",
                table: "UnitProjectiles",
                column: "GameUnitId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Units_HitboxGroupHash",
                table: "Units",
                column: "HitboxGroupHash");

            migrationBuilder.CreateIndex(
                name: "IX_UnitStats_GameUnitId",
                table: "UnitStats",
                column: "GameUnitId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssetFileUnit");

            migrationBuilder.DropTable(
                name: "Configs");

            migrationBuilder.DropTable(
                name: "PatchFiles");

            migrationBuilder.DropTable(
                name: "Projectiles");

            migrationBuilder.DropTable(
                name: "Stats");

            migrationBuilder.DropTable(
                name: "UnitAmmoSlots");

            migrationBuilder.DropTable(
                name: "AssetFiles");

            migrationBuilder.DropTable(
                name: "Tbl");

            migrationBuilder.DropTable(
                name: "Hitboxes");

            migrationBuilder.DropTable(
                name: "UnitProjectiles");

            migrationBuilder.DropTable(
                name: "Ammo");

            migrationBuilder.DropTable(
                name: "UnitStats");

            migrationBuilder.DropTable(
                name: "Units");

            migrationBuilder.DropTable(
                name: "HitboxGroups");
        }
    }
}
