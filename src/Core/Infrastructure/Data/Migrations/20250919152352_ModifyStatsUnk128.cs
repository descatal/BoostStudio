using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BoostStudio.Infrastructure.src.Core.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class ModifyStatsUnk128 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Unk128",
                table: "Ammo",
                newName: "ChargeMultiLockFlag");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ChargeMultiLockFlag",
                table: "Ammo",
                newName: "Unk128");
        }
    }
}
