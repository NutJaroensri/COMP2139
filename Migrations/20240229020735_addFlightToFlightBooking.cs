using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GBC_Travel_Group_83.Migrations
{
    /// <inheritdoc />
    public partial class addFlightToFlightBooking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FlightId",
                table: "FlightBookings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_FlightBookings_FlightId",
                table: "FlightBookings",
                column: "FlightId");

            migrationBuilder.AddForeignKey(
                name: "FK_FlightBookings_Flights_FlightId",
                table: "FlightBookings",
                column: "FlightId",
                principalTable: "Flights",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FlightBookings_Flights_FlightId",
                table: "FlightBookings");

            migrationBuilder.DropIndex(
                name: "IX_FlightBookings_FlightId",
                table: "FlightBookings");

            migrationBuilder.DropColumn(
                name: "FlightId",
                table: "FlightBookings");
        }
    }
}
