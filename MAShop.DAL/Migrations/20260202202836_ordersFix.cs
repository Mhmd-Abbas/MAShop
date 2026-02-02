using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MAShop.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ordersFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PaymentStatus",
                table: "Orders",
                newName: "PaymentMethod");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PaymentMethod",
                table: "Orders",
                newName: "PaymentStatus");
        }
    }
}
