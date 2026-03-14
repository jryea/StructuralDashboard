using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StructuralDashboard.Api.Migrations
{
    /// <inheritdoc />
    public partial class ModelElements : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Beams_FrameProperties_FramePropertiesId",
                table: "Beams");

            migrationBuilder.DropIndex(
                name: "IX_Beams_FramePropertiesId",
                table: "Beams");

            migrationBuilder.RenameColumn(
                name: "Width",
                table: "FrameProperties",
                newName: "WoodProps_Width");

            migrationBuilder.RenameColumn(
                name: "SectionType",
                table: "FrameProperties",
                newName: "WoodProps_SectionType");

            migrationBuilder.RenameColumn(
                name: "SectionName",
                table: "FrameProperties",
                newName: "WoodProps_SectionName");

            migrationBuilder.RenameColumn(
                name: "Depth",
                table: "FrameProperties",
                newName: "WoodProps_Depth");

            migrationBuilder.AlterColumn<double>(
                name: "WoodProps_Width",
                table: "FrameProperties",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
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

            migrationBuilder.AddColumn<double>(
                name: "ConcreteProps_Depth",
                table: "FrameProperties",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "ConcreteProps_SectionName",
                table: "FrameProperties",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ConcreteProps_SectionType",
                table: "FrameProperties",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "ConcreteProps_Width",
                table: "FrameProperties",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "FrameModifersId",
                table: "FrameProperties",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MaterialId",
                table: "FrameProperties",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "FrameProperties",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SteelProps_SectionName",
                table: "FrameProperties",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "SteelProps_SectionType",
                table: "FrameProperties",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "FramePropertiesId",
                table: "Beams",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateTable(
                name: "Braces",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ModelId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BaseLevelId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TopLevelId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    StartPoint_X = table.Column<double>(type: "float", nullable: false),
                    StartPoint_Y = table.Column<double>(type: "float", nullable: false),
                    StartPoint_Z = table.Column<double>(type: "float", nullable: false),
                    EndPoint_X = table.Column<double>(type: "float", nullable: false),
                    EndPoint_Y = table.Column<double>(type: "float", nullable: false),
                    EndPoint_Z = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Braces", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Braces_Levels_BaseLevelId",
                        column: x => x.BaseLevelId,
                        principalTable: "Levels",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Braces_Levels_TopLevelId",
                        column: x => x.TopLevelId,
                        principalTable: "Levels",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Braces_Models_ModelId",
                        column: x => x.ModelId,
                        principalTable: "Models",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Columns",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ModelId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    StartPoint_X = table.Column<double>(type: "float", nullable: false),
                    StartPoint_Y = table.Column<double>(type: "float", nullable: false),
                    StartPoint_Z = table.Column<double>(type: "float", nullable: false),
                    EndPoint_X = table.Column<double>(type: "float", nullable: false),
                    EndPoint_Y = table.Column<double>(type: "float", nullable: false),
                    EndPoint_Z = table.Column<double>(type: "float", nullable: false),
                    BaseLevelId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TopLevelId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Orientation = table.Column<double>(type: "float", nullable: false),
                    IsLateral = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Columns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Columns_Levels_BaseLevelId",
                        column: x => x.BaseLevelId,
                        principalTable: "Levels",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Columns_Levels_TopLevelId",
                        column: x => x.TopLevelId,
                        principalTable: "Levels",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Columns_Models_ModelId",
                        column: x => x.ModelId,
                        principalTable: "Models",
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
                name: "Footings",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ModelId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Width = table.Column<double>(type: "float", nullable: false),
                    Length = table.Column<double>(type: "float", nullable: false),
                    Thickness = table.Column<double>(type: "float", nullable: false),
                    Point_X = table.Column<double>(type: "float", nullable: false),
                    Point_Y = table.Column<double>(type: "float", nullable: false),
                    Point_Z = table.Column<double>(type: "float", nullable: false),
                    LevelId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Orientation = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Footings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Footings_Levels_LevelId",
                        column: x => x.LevelId,
                        principalTable: "Levels",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Footings_Models_ModelId",
                        column: x => x.ModelId,
                        principalTable: "Models",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "FrameModifiers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Area = table.Column<double>(type: "float", nullable: false),
                    A22 = table.Column<double>(type: "float", nullable: false),
                    A33 = table.Column<double>(type: "float", nullable: false),
                    I22 = table.Column<double>(type: "float", nullable: false),
                    I33 = table.Column<double>(type: "float", nullable: false),
                    Torsion = table.Column<double>(type: "float", nullable: false),
                    Mass = table.Column<double>(type: "float", nullable: false),
                    Weight = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FrameModifiers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Grids",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartPoint_X = table.Column<double>(type: "float", nullable: false),
                    StartPoint_Y = table.Column<double>(type: "float", nullable: false),
                    StartPoint_Z = table.Column<double>(type: "float", nullable: false),
                    EndPoint_X = table.Column<double>(type: "float", nullable: false),
                    EndPoint_Y = table.Column<double>(type: "float", nullable: false),
                    EndPoint_Z = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grids", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Materials",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModelId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DirectionalSymmetryType = table.Column<int>(type: "int", nullable: true),
                    MaterialType = table.Column<int>(type: "int", nullable: true),
                    WeightPerUnitVolume = table.Column<double>(type: "float", nullable: true),
                    MassPerUnitVolume = table.Column<double>(type: "float", nullable: true),
                    ElasticModulus = table.Column<double>(type: "float", nullable: true),
                    PoissonsRatio = table.Column<double>(type: "float", nullable: true),
                    CoefficientOfThermalExpansion = table.Column<double>(type: "float", nullable: true),
                    ShearModulus = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Materials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Materials_Models_ModelId",
                        column: x => x.ModelId,
                        principalTable: "Models",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Openings",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ModelId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LevelId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Points = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Openings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Openings_Levels_LevelId",
                        column: x => x.LevelId,
                        principalTable: "Levels",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Openings_Models_ModelId",
                        column: x => x.ModelId,
                        principalTable: "Models",
                        principalColumn: "Id");
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

            migrationBuilder.CreateTable(
                name: "DeckProperties",
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
                    table.PrimaryKey("PK_DeckProperties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeckProperties_Materials_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materials",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "WallProperties",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ModelId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaterialId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Thickness = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WallProperties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WallProperties_Materials_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materials",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WallProperties_Models_ModelId",
                        column: x => x.ModelId,
                        principalTable: "Models",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "FloorProperties",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModelId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MaterialId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    ModelingType = table.Column<int>(type: "int", nullable: false),
                    SlabType = table.Column<int>(type: "int", nullable: false),
                    Thickness = table.Column<double>(type: "float", nullable: false),
                    DeckPropertiesId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ShearStudProperties_ShearStudDiameter = table.Column<double>(type: "float", nullable: false),
                    ShearStudProperties_ShearStudHeight = table.Column<double>(type: "float", nullable: false),
                    ShearStudProperties_ShearStudTensileStrength = table.Column<double>(type: "float", nullable: false),
                    ShellModifiers_F11 = table.Column<double>(type: "float", nullable: false),
                    ShellModifiers_F22 = table.Column<double>(type: "float", nullable: false),
                    ShellModifiers_F12 = table.Column<double>(type: "float", nullable: false),
                    ShellModifiers_M11 = table.Column<double>(type: "float", nullable: false),
                    ShellModifiers_M22 = table.Column<double>(type: "float", nullable: false),
                    ShellModifiers_M12 = table.Column<double>(type: "float", nullable: false),
                    ShellModifiers_V13 = table.Column<double>(type: "float", nullable: false),
                    ShellModifiers_V23 = table.Column<double>(type: "float", nullable: false),
                    ShellModifiers_Mass = table.Column<double>(type: "float", nullable: false),
                    ShellModifiers_Weight = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FloorProperties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FloorProperties_DeckProperties_DeckPropertiesId",
                        column: x => x.DeckPropertiesId,
                        principalTable: "DeckProperties",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FloorProperties_Materials_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materials",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FloorProperties_Models_ModelId",
                        column: x => x.ModelId,
                        principalTable: "Models",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Walls",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ModelId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    StartPoint_X = table.Column<double>(type: "float", nullable: false),
                    StartPoint_Y = table.Column<double>(type: "float", nullable: false),
                    StartPoint_Z = table.Column<double>(type: "float", nullable: false),
                    EndPoint_X = table.Column<double>(type: "float", nullable: false),
                    EndPoint_Y = table.Column<double>(type: "float", nullable: false),
                    EndPoint_Z = table.Column<double>(type: "float", nullable: false),
                    BaseLevelId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TopLevelId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PropertiesId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsLateral = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Walls", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Walls_Levels_BaseLevelId",
                        column: x => x.BaseLevelId,
                        principalTable: "Levels",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Walls_Levels_TopLevelId",
                        column: x => x.TopLevelId,
                        principalTable: "Levels",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Walls_Models_ModelId",
                        column: x => x.ModelId,
                        principalTable: "Models",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Walls_WallProperties_PropertiesId",
                        column: x => x.PropertiesId,
                        principalTable: "WallProperties",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Floors",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ModelId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LevelId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FloorPropertiesId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DiaphragmId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SurfaceLoadId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SpanDirection = table.Column<double>(type: "float", nullable: false),
                    ShellModifiers_F11 = table.Column<double>(type: "float", nullable: false),
                    ShellModifiers_F22 = table.Column<double>(type: "float", nullable: false),
                    ShellModifiers_F12 = table.Column<double>(type: "float", nullable: false),
                    ShellModifiers_M11 = table.Column<double>(type: "float", nullable: false),
                    ShellModifiers_M22 = table.Column<double>(type: "float", nullable: false),
                    ShellModifiers_M12 = table.Column<double>(type: "float", nullable: false),
                    ShellModifiers_V13 = table.Column<double>(type: "float", nullable: false),
                    ShellModifiers_V23 = table.Column<double>(type: "float", nullable: false),
                    ShellModifiers_Mass = table.Column<double>(type: "float", nullable: false),
                    ShellModifiers_Weight = table.Column<double>(type: "float", nullable: false),
                    Points = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Floors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Floors_Diaphragm_DiaphragmId",
                        column: x => x.DiaphragmId,
                        principalTable: "Diaphragm",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Floors_FloorProperties_FloorPropertiesId",
                        column: x => x.FloorPropertiesId,
                        principalTable: "FloorProperties",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Floors_Levels_LevelId",
                        column: x => x.LevelId,
                        principalTable: "Levels",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Floors_Models_ModelId",
                        column: x => x.ModelId,
                        principalTable: "Models",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Floors_SurfaceLoad_SurfaceLoadId",
                        column: x => x.SurfaceLoadId,
                        principalTable: "SurfaceLoad",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_FrameProperties_FrameModifersId",
                table: "FrameProperties",
                column: "FrameModifersId");

            migrationBuilder.CreateIndex(
                name: "IX_FrameProperties_MaterialId",
                table: "FrameProperties",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_Braces_BaseLevelId",
                table: "Braces",
                column: "BaseLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_Braces_ModelId",
                table: "Braces",
                column: "ModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Braces_TopLevelId",
                table: "Braces",
                column: "TopLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_Columns_BaseLevelId",
                table: "Columns",
                column: "BaseLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_Columns_ModelId",
                table: "Columns",
                column: "ModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Columns_TopLevelId",
                table: "Columns",
                column: "TopLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_DeckProperties_MaterialId",
                table: "DeckProperties",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_FloorProperties_DeckPropertiesId",
                table: "FloorProperties",
                column: "DeckPropertiesId");

            migrationBuilder.CreateIndex(
                name: "IX_FloorProperties_MaterialId",
                table: "FloorProperties",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_FloorProperties_ModelId",
                table: "FloorProperties",
                column: "ModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Floors_DiaphragmId",
                table: "Floors",
                column: "DiaphragmId");

            migrationBuilder.CreateIndex(
                name: "IX_Floors_FloorPropertiesId",
                table: "Floors",
                column: "FloorPropertiesId");

            migrationBuilder.CreateIndex(
                name: "IX_Floors_LevelId",
                table: "Floors",
                column: "LevelId");

            migrationBuilder.CreateIndex(
                name: "IX_Floors_ModelId",
                table: "Floors",
                column: "ModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Floors_SurfaceLoadId",
                table: "Floors",
                column: "SurfaceLoadId");

            migrationBuilder.CreateIndex(
                name: "IX_Footings_LevelId",
                table: "Footings",
                column: "LevelId");

            migrationBuilder.CreateIndex(
                name: "IX_Footings_ModelId",
                table: "Footings",
                column: "ModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Materials_ModelId",
                table: "Materials",
                column: "ModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Openings_LevelId",
                table: "Openings",
                column: "LevelId");

            migrationBuilder.CreateIndex(
                name: "IX_Openings_ModelId",
                table: "Openings",
                column: "ModelId");

            migrationBuilder.CreateIndex(
                name: "IX_WallProperties_MaterialId",
                table: "WallProperties",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_WallProperties_ModelId",
                table: "WallProperties",
                column: "ModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Walls_BaseLevelId",
                table: "Walls",
                column: "BaseLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_Walls_ModelId",
                table: "Walls",
                column: "ModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Walls_PropertiesId",
                table: "Walls",
                column: "PropertiesId");

            migrationBuilder.CreateIndex(
                name: "IX_Walls_TopLevelId",
                table: "Walls",
                column: "TopLevelId");

            migrationBuilder.AddForeignKey(
                name: "FK_FrameProperties_FrameModifiers_FrameModifersId",
                table: "FrameProperties",
                column: "FrameModifersId",
                principalTable: "FrameModifiers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FrameProperties_Materials_MaterialId",
                table: "FrameProperties",
                column: "MaterialId",
                principalTable: "Materials",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FrameProperties_FrameModifiers_FrameModifersId",
                table: "FrameProperties");

            migrationBuilder.DropForeignKey(
                name: "FK_FrameProperties_Materials_MaterialId",
                table: "FrameProperties");

            migrationBuilder.DropTable(
                name: "Braces");

            migrationBuilder.DropTable(
                name: "Columns");

            migrationBuilder.DropTable(
                name: "Floors");

            migrationBuilder.DropTable(
                name: "Footings");

            migrationBuilder.DropTable(
                name: "FrameModifiers");

            migrationBuilder.DropTable(
                name: "Grids");

            migrationBuilder.DropTable(
                name: "Openings");

            migrationBuilder.DropTable(
                name: "Walls");

            migrationBuilder.DropTable(
                name: "Diaphragm");

            migrationBuilder.DropTable(
                name: "FloorProperties");

            migrationBuilder.DropTable(
                name: "SurfaceLoad");

            migrationBuilder.DropTable(
                name: "WallProperties");

            migrationBuilder.DropTable(
                name: "DeckProperties");

            migrationBuilder.DropTable(
                name: "Materials");

            migrationBuilder.DropIndex(
                name: "IX_FrameProperties_FrameModifersId",
                table: "FrameProperties");

            migrationBuilder.DropIndex(
                name: "IX_FrameProperties_MaterialId",
                table: "FrameProperties");

            migrationBuilder.DropColumn(
                name: "ConcreteProps_Depth",
                table: "FrameProperties");

            migrationBuilder.DropColumn(
                name: "ConcreteProps_SectionName",
                table: "FrameProperties");

            migrationBuilder.DropColumn(
                name: "ConcreteProps_SectionType",
                table: "FrameProperties");

            migrationBuilder.DropColumn(
                name: "ConcreteProps_Width",
                table: "FrameProperties");

            migrationBuilder.DropColumn(
                name: "FrameModifersId",
                table: "FrameProperties");

            migrationBuilder.DropColumn(
                name: "MaterialId",
                table: "FrameProperties");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "FrameProperties");

            migrationBuilder.DropColumn(
                name: "SteelProps_SectionName",
                table: "FrameProperties");

            migrationBuilder.DropColumn(
                name: "SteelProps_SectionType",
                table: "FrameProperties");

            migrationBuilder.RenameColumn(
                name: "WoodProps_Width",
                table: "FrameProperties",
                newName: "Width");

            migrationBuilder.RenameColumn(
                name: "WoodProps_SectionType",
                table: "FrameProperties",
                newName: "SectionType");

            migrationBuilder.RenameColumn(
                name: "WoodProps_SectionName",
                table: "FrameProperties",
                newName: "SectionName");

            migrationBuilder.RenameColumn(
                name: "WoodProps_Depth",
                table: "FrameProperties",
                newName: "Depth");

            migrationBuilder.AlterColumn<double>(
                name: "Width",
                table: "FrameProperties",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<double>(
                name: "Depth",
                table: "FrameProperties",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<string>(
                name: "FramePropertiesId",
                table: "Beams",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

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
        }
    }
}
