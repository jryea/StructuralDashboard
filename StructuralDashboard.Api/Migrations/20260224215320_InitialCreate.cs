using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StructuralDashboard.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    ProjectNumber = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProjectName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.ProjectNumber);
                });

            migrationBuilder.CreateTable(
                name: "Models",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ModelName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProjectNumber = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SavedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SavedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SourceApplication = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Models", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Models_Projects_ProjectNumber",
                        column: x => x.ProjectNumber,
                        principalTable: "Projects",
                        principalColumn: "ProjectNumber");
                });

            migrationBuilder.CreateTable(
                name: "FrameProperties",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ModelId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Depth = table.Column<double>(type: "float", nullable: false),
                    Width = table.Column<double>(type: "float", nullable: false),
                    SectionName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SectionType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FrameProperties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FrameProperties_Models_ModelId",
                        column: x => x.ModelId,
                        principalTable: "Models",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Levels",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ModelId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Elevation = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Levels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Levels_Models_ModelId",
                        column: x => x.ModelId,
                        principalTable: "Models",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Beams",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ModelId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LevelId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    StartPoint_X = table.Column<double>(type: "float", nullable: false),
                    StartPoint_Y = table.Column<double>(type: "float", nullable: false),
                    StartPoint_Z = table.Column<double>(type: "float", nullable: false),
                    EndPoint_X = table.Column<double>(type: "float", nullable: false),
                    EndPoint_Y = table.Column<double>(type: "float", nullable: false),
                    EndPoint_Z = table.Column<double>(type: "float", nullable: false),
                    IsJoist = table.Column<bool>(type: "bit", nullable: false),
                    IsLateral = table.Column<bool>(type: "bit", nullable: false),
                    FramePropertiesId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Beams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Beams_FrameProperties_FramePropertiesId",
                        column: x => x.FramePropertiesId,
                        principalTable: "FrameProperties",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Beams_Levels_LevelId",
                        column: x => x.LevelId,
                        principalTable: "Levels",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Beams_Models_ModelId",
                        column: x => x.ModelId,
                        principalTable: "Models",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Beams_FramePropertiesId",
                table: "Beams",
                column: "FramePropertiesId");

            migrationBuilder.CreateIndex(
                name: "IX_Beams_LevelId",
                table: "Beams",
                column: "LevelId");

            migrationBuilder.CreateIndex(
                name: "IX_Beams_ModelId",
                table: "Beams",
                column: "ModelId");

            migrationBuilder.CreateIndex(
                name: "IX_FrameProperties_ModelId",
                table: "FrameProperties",
                column: "ModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Levels_ModelId",
                table: "Levels",
                column: "ModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Models_ProjectNumber",
                table: "Models",
                column: "ProjectNumber");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Beams");

            migrationBuilder.DropTable(
                name: "FrameProperties");

            migrationBuilder.DropTable(
                name: "Levels");

            migrationBuilder.DropTable(
                name: "Models");

            migrationBuilder.DropTable(
                name: "Projects");
        }
    }
}
