using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CampusGuideWSG.Migrations
{
    /// <inheritdoc />
    public partial class AddedRoomsBuildingManyToMany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "ROOMS_BUILDINGS_FK",
                table: "rooms");

            migrationBuilder.DropIndex(
                name: "ROOMS_BUILDINGS_FK",
                table: "rooms");

            migrationBuilder.DropColumn(
                name: "building_id",
                table: "rooms");

            migrationBuilder.CreateTable(
                name: "rooms_buildings",
                columns: table => new
                {
                    room_id = table.Column<int>(type: "int(11)", nullable: false),
                    building_id = table.Column<int>(type: "int(11)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rooms_buildings", x => new { x.room_id, x.building_id });
                    table.ForeignKey(
                        name: "ROOMS_BUILDINGS_BUILDINGS_FK",
                        column: x => x.building_id,
                        principalTable: "buildings",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "ROOMS_BUILDINGS_ROOMS_FK",
                        column: x => x.room_id,
                        principalTable: "rooms",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_rooms_buildings_building_id",
                table: "rooms_buildings",
                column: "building_id");

            migrationBuilder.CreateIndex(
                name: "ROOMS_BUILDINGS_IX",
                table: "rooms_buildings",
                columns: new[] { "room_id", "building_id" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "rooms_buildings");

            migrationBuilder.AddColumn<int>(
                name: "building_id",
                table: "rooms",
                type: "int(11)",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "ROOMS_BUILDINGS_FK",
                table: "rooms",
                column: "building_id");

            migrationBuilder.AddForeignKey(
                name: "ROOMS_BUILDINGS_FK",
                table: "rooms",
                column: "building_id",
                principalTable: "buildings",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
