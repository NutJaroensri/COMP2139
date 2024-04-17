using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GBC_TRAVEL_GROUP_88.Migrations
{
    /// <inheritdoc />
    public partial class change_hotel_id_name : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "Identity",
                table: "Hotels",
                newName: "HotelId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "HotelId",
                schema: "Identity",
                table: "Hotels",
                newName: "Id");
        }
    }
}
