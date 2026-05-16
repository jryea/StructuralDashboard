using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StructuralDashboard.Api.Migrations
{
    /// <inheritdoc />
    public partial class PendingChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FloorProperties_DeckProperties_DeckPropertiesId",
                table: "FloorProperties");

            migrationBuilder.DropForeignKey(
                name: "FK_Floors_Diaphragm_DiaphragmId",
                table: "Floors");

            migrationBuilder.DropForeignKey(
                name: "FK_Floors_SurfaceLoad_SurfaceLoadId",
                table: "Floors");

            migrationBuilder.DropTable(
                name: "DeckProperties");

            migrationBuilder.DropTable(
                name: "Diaphragm");

            migrationBuilder.DropTable(
                name: "SurfaceLoad");

            migrationBuilder.AddColumn<string>(
                name: "ModelId",
                table: "Grids",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "DeckPropertiesEntity",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DeckType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaterialId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RibDepth = table.Column<double>(type: "float", nullable: false),
                    RibWidthTop = table.Column<double>(type: "float", nullable: false),
                    RibWidthBottom = table.Column<double>(type: "float", nullable: false),
                    RibSpacing = table.Column<double>(type: "float", nullable: false),
                    DeckShearThickness = table.Column<double>(type: "float", nullable: false),
                    DeckUnitWeight = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeckPropertiesEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeckPropertiesEntity_Materials_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materials",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DiaphragmEntity",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiaphragmEntity", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SurfaceLoadEntity",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SurfaceLoadEntity", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DeckPropertiesEntity_MaterialId",
                table: "DeckPropertiesEntity",
                column: "MaterialId");

            migrationBuilder.AddForeignKey(
                name: "FK_FloorProperties_DeckPropertiesEntity_DeckPropertiesId",
                table: "FloorProperties",
                column: "DeckPropertiesId",
                principalTable: "DeckPropertiesEntity",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Floors_DiaphragmEntity_DiaphragmId",
                table: "Floors",
                column: "DiaphragmId",
                principalTable: "DiaphragmEntity",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Floors_SurfaceLoadEntity_SurfaceLoadId",
                table: "Floors",
                column: "SurfaceLoadId",
                principalTable: "SurfaceLoadEntity",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FloorProperties_DeckPropertiesEntity_DeckPropertiesId",
                table: "FloorProperties");

            migrationBuilder.DropForeignKey(
                name: "FK_Floors_DiaphragmEntity_DiaphragmId",
                table: "Floors");

            migrationBuilder.DropForeignKey(
                name: "FK_Floors_SurfaceLoadEntity_SurfaceLoadId",
                table: "Floors");

            migrationBuilder.DropTable(
                name: "DeckPropertiesEntity");

            migrationBuilder.DropTable(
                name: "DiaphragmEntity");

            migrationBuilder.DropTable(
                name: "SurfaceLoadEntity");

            migrationBuilder.DropColumn(
                name: "ModelId",
                table: "Grids");

            migrationBuilder.CreateTable(
                name: "DeckProperties",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MaterialId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DeckShearThickness = table.Column<double>(type: "float", nullable: false),
                    DeckType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeckUnitWeight = table.Column<double>(type: "float", nullable: false),
                    RibDepth = table.Column<double>(type: "float", nullable: false),
                    RibSpacing = table.Column<double>(type: "float", nullable: false),
                    RibWidthBottom = table.Column<double>(type: "float", nullable: false),
                    RibWidthTop = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeckProperties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeckProperties_Materials_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materials",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Diaphragm",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Diaphragm", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SurfaceLoad",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SurfaceLoad", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DeckProperties_MaterialId",
                table: "DeckProperties",
                column: "MaterialId");

            migrationBuilder.AddForeignKey(
                name: "FK_FloorProperties_DeckProperties_DeckPropertiesId",
                table: "FloorProperties",
                column: "DeckPropertiesId",
                principalTable: "DeckProperties",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Floors_Diaphragm_DiaphragmId",
                table: "Floors",
                column: "DiaphragmId",
                principalTable: "Diaphragm",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Floors_SurfaceLoad_SurfaceLoadId",
                table: "Floors",
                column: "SurfaceLoadId",
                principalTable: "SurfaceLoad",
                principalColumn: "Id");
        }
    }
}
