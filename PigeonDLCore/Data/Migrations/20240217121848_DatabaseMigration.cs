using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PigeonDLCore.Data.Migrations
{
    /// <inheritdoc />
    public partial class DatabaseMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Files",
                columns: table => new
                {
                    IDFile = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IDUser = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IDFolder = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "varchar(200)", nullable: false),
                    DateUploaded = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    Size = table.Column<long>(type: "bigint", nullable: false),
                    URL = table.Column<string>(type: "varchar(32)", nullable: false),
                    Downloads = table.Column<int>(type: "int", nullable: false, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Files", x => x.IDFile);
                    table.ForeignKey(
                        name: "FK_Files_AspNetUsers_IDUser",
                        column: x => x.IDUser,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Folders",
                columns: table => new
                {
                    IDFolder = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IDUser = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "varchar(200)", nullable: false),
                    DateUploaded = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    URL = table.Column<string>(type: "varchar(32)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Folders", x => x.IDFolder);
                    table.ForeignKey(
                        name: "FK_Folders_AspNetUsers_IDUser",
                        column: x => x.IDUser,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FoldersShared",
                columns: table => new
                {
                    IDShared = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IDUser = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IDFolder = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoldersShared", x => x.IDShared);
                    table.ForeignKey(
                        name: "FK_FoldersShared_AspNetUsers_IDUser",
                        column: x => x.IDUser,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "News",
                columns: table => new
                {
                    IDNews = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IDUser = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Title = table.Column<string>(type: "varchar(50)", nullable: false),
                    Content = table.Column<string>(type: "varchar(200)", nullable: false),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_News", x => x.IDNews);
                    table.ForeignKey(
                        name: "FK_News_AspNetUsers_IDUser",
                        column: x => x.IDUser,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Files_IDUser",
                table: "Files",
                column: "IDUser");

            migrationBuilder.CreateIndex(
                name: "IX_Files_URL",
                table: "Files",
                column: "URL",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Folders_IDUser",
                table: "Folders",
                column: "IDUser");

            migrationBuilder.CreateIndex(
                name: "IX_Folders_URL",
                table: "Folders",
                column: "URL",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FoldersShared_IDUser",
                table: "FoldersShared",
                column: "IDUser");

            migrationBuilder.CreateIndex(
                name: "IX_News_IDUser",
                table: "News",
                column: "IDUser");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Files");

            migrationBuilder.DropTable(
                name: "Folders");

            migrationBuilder.DropTable(
                name: "FoldersShared");

            migrationBuilder.DropTable(
                name: "News");
        }
    }
}
