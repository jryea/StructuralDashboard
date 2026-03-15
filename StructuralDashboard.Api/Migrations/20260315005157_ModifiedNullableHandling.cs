using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StructuralDashboard.Api.Migrations
{
    /// <inheritdoc />
    public partial class ModifiedNullableHandling : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FrameProperties_FrameModifiers_FrameModifersId",
                table: "FrameProperties");

            migrationBuilder.DropTable(
                name: "FrameModifiers");

            migrationBuilder.DropIndex(
                name: "IX_FrameProperties_FrameModifersId",
                table: "FrameProperties");

            migrationBuilder.DropColumn(
                name: "FrameModifersId",
                table: "FrameProperties");

            migrationBuilder.AlterColumn<double>(
                name: "WoodProps_Width",
                table: "FrameProperties",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<int>(
                name: "WoodProps_SectionType",
                table: "FrameProperties",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "WoodProps_SectionName",
                table: "FrameProperties",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<double>(
                name: "WoodProps_Depth",
                table: "FrameProperties",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<int>(
                name: "SteelProps_SectionType",
                table: "FrameProperties",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "SteelProps_SectionName",
                table: "FrameProperties",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<double>(
                name: "ConcreteProps_Width",
                table: "FrameProperties",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<int>(
                name: "ConcreteProps_SectionType",
                table: "FrameProperties",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "ConcreteProps_SectionName",
                table: "FrameProperties",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<double>(
                name: "ConcreteProps_Depth",
                table: "FrameProperties",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AddColumn<double>(
                name: "FrameModifiers_A22",
                table: "FrameProperties",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "FrameModifiers_A33",
                table: "FrameProperties",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "FrameModifiers_Area",
                table: "FrameProperties",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "FrameModifiers_I22",
                table: "FrameProperties",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "FrameModifiers_I33",
                table: "FrameProperties",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "FrameModifiers_Mass",
                table: "FrameProperties",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "FrameModifiers_Torsion",
                table: "FrameProperties",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "FrameModifiers_Weight",
                table: "FrameProperties",
                type: "float",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SurfaceLoadId",
                table: "Floors",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<double>(
                name: "ShellModifiers_Weight",
                table: "Floors",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<double>(
                name: "ShellModifiers_V23",
                table: "Floors",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<double>(
                name: "ShellModifiers_V13",
                table: "Floors",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<double>(
                name: "ShellModifiers_Mass",
                table: "Floors",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<double>(
                name: "ShellModifiers_M22",
                table: "Floors",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<double>(
                name: "ShellModifiers_M12",
                table: "Floors",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<double>(
                name: "ShellModifiers_M11",
                table: "Floors",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<double>(
                name: "ShellModifiers_F22",
                table: "Floors",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<double>(
                name: "ShellModifiers_F12",
                table: "Floors",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<double>(
                name: "ShellModifiers_F11",
                table: "Floors",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<string>(
                name: "DiaphragmId",
                table: "Floors",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<double>(
                name: "ShellModifiers_Weight",
                table: "FloorProperties",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<double>(
                name: "ShellModifiers_V23",
                table: "FloorProperties",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<double>(
                name: "ShellModifiers_V13",
                table: "FloorProperties",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<double>(
                name: "ShellModifiers_Mass",
                table: "FloorProperties",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<double>(
                name: "ShellModifiers_M22",
                table: "FloorProperties",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<double>(
                name: "ShellModifiers_M12",
                table: "FloorProperties",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<double>(
                name: "ShellModifiers_M11",
                table: "FloorProperties",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<double>(
                name: "ShellModifiers_F22",
                table: "FloorProperties",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<double>(
                name: "ShellModifiers_F12",
                table: "FloorProperties",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<double>(
                name: "ShellModifiers_F11",
                table: "FloorProperties",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<double>(
                name: "ShearStudProperties_ShearStudTensileStrength",
                table: "FloorProperties",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<double>(
                name: "ShearStudProperties_ShearStudHeight",
                table: "FloorProperties",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<double>(
                name: "ShearStudProperties_ShearStudDiameter",
                table: "FloorProperties",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AddColumn<double>(
                name: "FrameModifiers_A22",
                table: "Columns",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "FrameModifiers_A33",
                table: "Columns",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "FrameModifiers_Area",
                table: "Columns",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "FrameModifiers_I22",
                table: "Columns",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "FrameModifiers_I33",
                table: "Columns",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "FrameModifiers_Mass",
                table: "Columns",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "FrameModifiers_Torsion",
                table: "Columns",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "FrameModifiers_Weight",
                table: "Columns",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FramePropertiesId",
                table: "Columns",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "FrameModifiers_A22",
                table: "Braces",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "FrameModifiers_A33",
                table: "Braces",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "FrameModifiers_Area",
                table: "Braces",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "FrameModifiers_I22",
                table: "Braces",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "FrameModifiers_I33",
                table: "Braces",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "FrameModifiers_Mass",
                table: "Braces",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "FrameModifiers_Torsion",
                table: "Braces",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "FrameModifiers_Weight",
                table: "Braces",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FramePropertiesId",
                table: "Braces",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "FramePropertiesId",
                table: "Beams",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<double>(
                name: "FrameModifiers_A22",
                table: "Beams",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "FrameModifiers_A33",
                table: "Beams",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "FrameModifiers_Area",
                table: "Beams",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "FrameModifiers_I22",
                table: "Beams",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "FrameModifiers_I33",
                table: "Beams",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "FrameModifiers_Mass",
                table: "Beams",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "FrameModifiers_Torsion",
                table: "Beams",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "FrameModifiers_Weight",
                table: "Beams",
                type: "float",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Columns_FramePropertiesId",
                table: "Columns",
                column: "FramePropertiesId");

            migrationBuilder.CreateIndex(
                name: "IX_Braces_FramePropertiesId",
                table: "Braces",
                column: "FramePropertiesId");

            migrationBuilder.CreateIndex(
                name: "IX_Beams_FramePropertiesId",
                table: "Beams",
                column: "FramePropertiesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Beams_FrameProperties_FramePropertiesId",
                table: "Beams",
                column: "FramePropertiesId",
                principalTable: "FrameProperties",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Braces_FrameProperties_FramePropertiesId",
                table: "Braces",
                column: "FramePropertiesId",
                principalTable: "FrameProperties",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Columns_FrameProperties_FramePropertiesId",
                table: "Columns",
                column: "FramePropertiesId",
                principalTable: "FrameProperties",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Beams_FrameProperties_FramePropertiesId",
                table: "Beams");

            migrationBuilder.DropForeignKey(
                name: "FK_Braces_FrameProperties_FramePropertiesId",
                table: "Braces");

            migrationBuilder.DropForeignKey(
                name: "FK_Columns_FrameProperties_FramePropertiesId",
                table: "Columns");

            migrationBuilder.DropIndex(
                name: "IX_Columns_FramePropertiesId",
                table: "Columns");

            migrationBuilder.DropIndex(
                name: "IX_Braces_FramePropertiesId",
                table: "Braces");

            migrationBuilder.DropIndex(
                name: "IX_Beams_FramePropertiesId",
                table: "Beams");

            migrationBuilder.DropColumn(
                name: "FrameModifiers_A22",
                table: "FrameProperties");

            migrationBuilder.DropColumn(
                name: "FrameModifiers_A33",
                table: "FrameProperties");

            migrationBuilder.DropColumn(
                name: "FrameModifiers_Area",
                table: "FrameProperties");

            migrationBuilder.DropColumn(
                name: "FrameModifiers_I22",
                table: "FrameProperties");

            migrationBuilder.DropColumn(
                name: "FrameModifiers_I33",
                table: "FrameProperties");

            migrationBuilder.DropColumn(
                name: "FrameModifiers_Mass",
                table: "FrameProperties");

            migrationBuilder.DropColumn(
                name: "FrameModifiers_Torsion",
                table: "FrameProperties");

            migrationBuilder.DropColumn(
                name: "FrameModifiers_Weight",
                table: "FrameProperties");

            migrationBuilder.DropColumn(
                name: "FrameModifiers_A22",
                table: "Columns");

            migrationBuilder.DropColumn(
                name: "FrameModifiers_A33",
                table: "Columns");

            migrationBuilder.DropColumn(
                name: "FrameModifiers_Area",
                table: "Columns");

            migrationBuilder.DropColumn(
                name: "FrameModifiers_I22",
                table: "Columns");

            migrationBuilder.DropColumn(
                name: "FrameModifiers_I33",
                table: "Columns");

            migrationBuilder.DropColumn(
                name: "FrameModifiers_Mass",
                table: "Columns");

            migrationBuilder.DropColumn(
                name: "FrameModifiers_Torsion",
                table: "Columns");

            migrationBuilder.DropColumn(
                name: "FrameModifiers_Weight",
                table: "Columns");

            migrationBuilder.DropColumn(
                name: "FramePropertiesId",
                table: "Columns");

            migrationBuilder.DropColumn(
                name: "FrameModifiers_A22",
                table: "Braces");

            migrationBuilder.DropColumn(
                name: "FrameModifiers_A33",
                table: "Braces");

            migrationBuilder.DropColumn(
                name: "FrameModifiers_Area",
                table: "Braces");

            migrationBuilder.DropColumn(
                name: "FrameModifiers_I22",
                table: "Braces");

            migrationBuilder.DropColumn(
                name: "FrameModifiers_I33",
                table: "Braces");

            migrationBuilder.DropColumn(
                name: "FrameModifiers_Mass",
                table: "Braces");

            migrationBuilder.DropColumn(
                name: "FrameModifiers_Torsion",
                table: "Braces");

            migrationBuilder.DropColumn(
                name: "FrameModifiers_Weight",
                table: "Braces");

            migrationBuilder.DropColumn(
                name: "FramePropertiesId",
                table: "Braces");

            migrationBuilder.DropColumn(
                name: "FrameModifiers_A22",
                table: "Beams");

            migrationBuilder.DropColumn(
                name: "FrameModifiers_A33",
                table: "Beams");

            migrationBuilder.DropColumn(
                name: "FrameModifiers_Area",
                table: "Beams");

            migrationBuilder.DropColumn(
                name: "FrameModifiers_I22",
                table: "Beams");

            migrationBuilder.DropColumn(
                name: "FrameModifiers_I33",
                table: "Beams");

            migrationBuilder.DropColumn(
                name: "FrameModifiers_Mass",
                table: "Beams");

            migrationBuilder.DropColumn(
                name: "FrameModifiers_Torsion",
                table: "Beams");

            migrationBuilder.DropColumn(
                name: "FrameModifiers_Weight",
                table: "Beams");

            migrationBuilder.AlterColumn<double>(
                name: "WoodProps_Width",
                table: "FrameProperties",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "WoodProps_SectionType",
                table: "FrameProperties",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "WoodProps_SectionName",
                table: "FrameProperties",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "WoodProps_Depth",
                table: "FrameProperties",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "SteelProps_SectionType",
                table: "FrameProperties",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SteelProps_SectionName",
                table: "FrameProperties",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "ConcreteProps_Width",
                table: "FrameProperties",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ConcreteProps_SectionType",
                table: "FrameProperties",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ConcreteProps_SectionName",
                table: "FrameProperties",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "ConcreteProps_Depth",
                table: "FrameProperties",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FrameModifersId",
                table: "FrameProperties",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SurfaceLoadId",
                table: "Floors",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "ShellModifiers_Weight",
                table: "Floors",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "ShellModifiers_V23",
                table: "Floors",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "ShellModifiers_V13",
                table: "Floors",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "ShellModifiers_Mass",
                table: "Floors",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "ShellModifiers_M22",
                table: "Floors",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "ShellModifiers_M12",
                table: "Floors",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "ShellModifiers_M11",
                table: "Floors",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "ShellModifiers_F22",
                table: "Floors",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "ShellModifiers_F12",
                table: "Floors",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "ShellModifiers_F11",
                table: "Floors",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DiaphragmId",
                table: "Floors",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "ShellModifiers_Weight",
                table: "FloorProperties",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "ShellModifiers_V23",
                table: "FloorProperties",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "ShellModifiers_V13",
                table: "FloorProperties",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "ShellModifiers_Mass",
                table: "FloorProperties",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "ShellModifiers_M22",
                table: "FloorProperties",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "ShellModifiers_M12",
                table: "FloorProperties",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "ShellModifiers_M11",
                table: "FloorProperties",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "ShellModifiers_F22",
                table: "FloorProperties",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "ShellModifiers_F12",
                table: "FloorProperties",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "ShellModifiers_F11",
                table: "FloorProperties",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "ShearStudProperties_ShearStudTensileStrength",
                table: "FloorProperties",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "ShearStudProperties_ShearStudHeight",
                table: "FloorProperties",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "ShearStudProperties_ShearStudDiameter",
                table: "FloorProperties",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FramePropertiesId",
                table: "Beams",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateTable(
                name: "FrameModifiers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    A22 = table.Column<double>(type: "float", nullable: false),
                    A33 = table.Column<double>(type: "float", nullable: false),
                    Area = table.Column<double>(type: "float", nullable: false),
                    I22 = table.Column<double>(type: "float", nullable: false),
                    I33 = table.Column<double>(type: "float", nullable: false),
                    Mass = table.Column<double>(type: "float", nullable: false),
                    Torsion = table.Column<double>(type: "float", nullable: false),
                    Weight = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FrameModifiers", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FrameProperties_FrameModifersId",
                table: "FrameProperties",
                column: "FrameModifersId");

            migrationBuilder.AddForeignKey(
                name: "FK_FrameProperties_FrameModifiers_FrameModifersId",
                table: "FrameProperties",
                column: "FrameModifersId",
                principalTable: "FrameModifiers",
                principalColumn: "Id");
        }
    }
}
