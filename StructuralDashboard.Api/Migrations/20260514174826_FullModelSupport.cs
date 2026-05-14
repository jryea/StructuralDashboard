using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StructuralDashboard.Api.Migrations
{
    /// <inheritdoc />
    public partial class FullModelSupport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FloorProperties_DeckPropertiesEntity_DeckPropertiesId",
                table: "FloorProperties");

            migrationBuilder.DropTable(
                name: "DeckPropertiesEntity");

            migrationBuilder.DropIndex(
                name: "IX_FloorProperties_DeckPropertiesId",
                table: "FloorProperties");

            migrationBuilder.DropColumn(
                name: "DeckPropertiesId",
                table: "FloorProperties");

            migrationBuilder.AlterColumn<string>(
                name: "PropertiesId",
                table: "Walls",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<double>(
                name: "Thickness",
                table: "WallProperties",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AddColumn<double>(
                name: "ETABSModifiers_F11",
                table: "WallProperties",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ETABSModifiers_F12",
                table: "WallProperties",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ETABSModifiers_F22",
                table: "WallProperties",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ETABSModifiers_M11",
                table: "WallProperties",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ETABSModifiers_M12",
                table: "WallProperties",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ETABSModifiers_M22",
                table: "WallProperties",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ETABSModifiers_Mass",
                table: "WallProperties",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ETABSModifiers_V13",
                table: "WallProperties",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ETABSModifiers_V23",
                table: "WallProperties",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ETABSModifiers_Weight",
                table: "WallProperties",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MaterialType",
                table: "WallProperties",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "UnitWeightForSelfWeight",
                table: "WallProperties",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ConcreteProps_Fc",
                table: "Materials",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ConcreteProps_ShearStrengthReductionFactor",
                table: "Materials",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ConcreteProps_WeightClass",
                table: "Materials",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "SteelProps_Fu",
                table: "Materials",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "SteelProps_Fue",
                table: "Materials",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "SteelProps_Fy",
                table: "Materials",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "SteelProps_Fye",
                table: "Materials",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MaterialType",
                table: "FrameProperties",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "FloorPropertiesId",
                table: "Floors",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<double>(
                name: "DeckProperties_DeckShearThickness",
                table: "FloorProperties",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeckProperties_DeckType",
                table: "FloorProperties",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "DeckProperties_DeckUnitWeight",
                table: "FloorProperties",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeckProperties_MaterialId",
                table: "FloorProperties",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "DeckProperties_RibDepth",
                table: "FloorProperties",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "DeckProperties_RibSpacing",
                table: "FloorProperties",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "DeckProperties_RibWidthBottom",
                table: "FloorProperties",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "DeckProperties_RibWidthTop",
                table: "FloorProperties",
                type: "float",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FramePropertiesId",
                table: "Columns",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "FramePropertiesId",
                table: "Braces",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "FramePropertiesId",
                table: "Beams",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ETABSModifiers_F11",
                table: "WallProperties");

            migrationBuilder.DropColumn(
                name: "ETABSModifiers_F12",
                table: "WallProperties");

            migrationBuilder.DropColumn(
                name: "ETABSModifiers_F22",
                table: "WallProperties");

            migrationBuilder.DropColumn(
                name: "ETABSModifiers_M11",
                table: "WallProperties");

            migrationBuilder.DropColumn(
                name: "ETABSModifiers_M12",
                table: "WallProperties");

            migrationBuilder.DropColumn(
                name: "ETABSModifiers_M22",
                table: "WallProperties");

            migrationBuilder.DropColumn(
                name: "ETABSModifiers_Mass",
                table: "WallProperties");

            migrationBuilder.DropColumn(
                name: "ETABSModifiers_V13",
                table: "WallProperties");

            migrationBuilder.DropColumn(
                name: "ETABSModifiers_V23",
                table: "WallProperties");

            migrationBuilder.DropColumn(
                name: "ETABSModifiers_Weight",
                table: "WallProperties");

            migrationBuilder.DropColumn(
                name: "MaterialType",
                table: "WallProperties");

            migrationBuilder.DropColumn(
                name: "UnitWeightForSelfWeight",
                table: "WallProperties");

            migrationBuilder.DropColumn(
                name: "ConcreteProps_Fc",
                table: "Materials");

            migrationBuilder.DropColumn(
                name: "ConcreteProps_ShearStrengthReductionFactor",
                table: "Materials");

            migrationBuilder.DropColumn(
                name: "ConcreteProps_WeightClass",
                table: "Materials");

            migrationBuilder.DropColumn(
                name: "SteelProps_Fu",
                table: "Materials");

            migrationBuilder.DropColumn(
                name: "SteelProps_Fue",
                table: "Materials");

            migrationBuilder.DropColumn(
                name: "SteelProps_Fy",
                table: "Materials");

            migrationBuilder.DropColumn(
                name: "SteelProps_Fye",
                table: "Materials");

            migrationBuilder.DropColumn(
                name: "MaterialType",
                table: "FrameProperties");

            migrationBuilder.DropColumn(
                name: "DeckProperties_DeckShearThickness",
                table: "FloorProperties");

            migrationBuilder.DropColumn(
                name: "DeckProperties_DeckType",
                table: "FloorProperties");

            migrationBuilder.DropColumn(
                name: "DeckProperties_DeckUnitWeight",
                table: "FloorProperties");

            migrationBuilder.DropColumn(
                name: "DeckProperties_MaterialId",
                table: "FloorProperties");

            migrationBuilder.DropColumn(
                name: "DeckProperties_RibDepth",
                table: "FloorProperties");

            migrationBuilder.DropColumn(
                name: "DeckProperties_RibSpacing",
                table: "FloorProperties");

            migrationBuilder.DropColumn(
                name: "DeckProperties_RibWidthBottom",
                table: "FloorProperties");

            migrationBuilder.DropColumn(
                name: "DeckProperties_RibWidthTop",
                table: "FloorProperties");

            migrationBuilder.AlterColumn<string>(
                name: "PropertiesId",
                table: "Walls",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Thickness",
                table: "WallProperties",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FloorPropertiesId",
                table: "Floors",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeckPropertiesId",
                table: "FloorProperties",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "FramePropertiesId",
                table: "Columns",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FramePropertiesId",
                table: "Braces",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FramePropertiesId",
                table: "Beams",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "DeckPropertiesEntity",
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
                    table.PrimaryKey("PK_DeckPropertiesEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeckPropertiesEntity_Materials_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materials",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_FloorProperties_DeckPropertiesId",
                table: "FloorProperties",
                column: "DeckPropertiesId");

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
        }
    }
}
