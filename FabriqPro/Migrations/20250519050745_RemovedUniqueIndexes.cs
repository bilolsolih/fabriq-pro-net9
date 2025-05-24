using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FabriqPro.Migrations
{
    /// <inheritdoc />
    public partial class RemovedUniqueIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_spare_part_department_department_to_user_id_spare_part_id_S~",
                table: "spare_part_department");

            migrationBuilder.DropIndex(
                name: "IX_miscellaneous_department_department_to_user_id_miscellaneou~",
                table: "miscellaneous_department");

            migrationBuilder.DropIndex(
                name: "IX_material_to_department_department_to_user_id_material_id_pa~",
                table: "material_to_department");

            migrationBuilder.DropIndex(
                name: "IX_accessory_department_department_to_user_id_accessory_id_Sta~",
                table: "accessory_department");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_spare_part_department_department_to_user_id_spare_part_id_S~",
                table: "spare_part_department",
                columns: new[] { "department", "to_user_id", "spare_part_id", "Status" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_miscellaneous_department_department_to_user_id_miscellaneou~",
                table: "miscellaneous_department",
                columns: new[] { "department", "to_user_id", "miscellaneous_id", "Status" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_material_to_department_department_to_user_id_material_id_pa~",
                table: "material_to_department",
                columns: new[] { "department", "to_user_id", "material_id", "party_id", "Status" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_accessory_department_department_to_user_id_accessory_id_Sta~",
                table: "accessory_department",
                columns: new[] { "department", "to_user_id", "accessory_id", "Status" },
                unique: true);
        }
    }
}
