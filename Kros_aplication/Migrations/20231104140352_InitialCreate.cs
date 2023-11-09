using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kros_aplication.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Worker",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    Surname = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    PhoneNumber = table.Column<int>(type: "int", nullable: true),
                    email = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    title = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Worker", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Firm",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    id_manager = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Firm", x => x.id);
                    table.ForeignKey(
                        name: "R_1",
                        column: x => x.id_manager,
                        principalTable: "Worker",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Division",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    id_manager = table.Column<int>(type: "int", nullable: false),
                    firm_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Division", x => x.id);
                    table.ForeignKey(
                        name: "R_2",
                        column: x => x.id_manager,
                        principalTable: "Worker",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "R_5",
                        column: x => x.firm_id,
                        principalTable: "Firm",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Project",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    id_manager = table.Column<int>(type: "int", nullable: false),
                    division_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Project", x => x.id);
                    table.ForeignKey(
                        name: "R_3",
                        column: x => x.id_manager,
                        principalTable: "Worker",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "R_7",
                        column: x => x.division_id,
                        principalTable: "Division",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Department",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    id_manager = table.Column<int>(type: "int", nullable: false),
                    project_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Department", x => x.id);
                    table.ForeignKey(
                        name: "R_4",
                        column: x => x.id_manager,
                        principalTable: "Worker",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "R_6",
                        column: x => x.project_id,
                        principalTable: "Project",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Department_id_manager",
                table: "Department",
                column: "id_manager");

            migrationBuilder.CreateIndex(
                name: "IX_Department_project_id",
                table: "Department",
                column: "project_id");

            migrationBuilder.CreateIndex(
                name: "IX_Division_firm_id",
                table: "Division",
                column: "firm_id");

            migrationBuilder.CreateIndex(
                name: "IX_Division_id_manager",
                table: "Division",
                column: "id_manager");

            migrationBuilder.CreateIndex(
                name: "IX_Firm_id_manager",
                table: "Firm",
                column: "id_manager");

            migrationBuilder.CreateIndex(
                name: "IX_Project_division_id",
                table: "Project",
                column: "division_id");

            migrationBuilder.CreateIndex(
                name: "IX_Project_id_manager",
                table: "Project",
                column: "id_manager");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Department");

            migrationBuilder.DropTable(
                name: "Project");

            migrationBuilder.DropTable(
                name: "Division");

            migrationBuilder.DropTable(
                name: "Firm");

            migrationBuilder.DropTable(
                name: "Worker");
        }
    }
}
