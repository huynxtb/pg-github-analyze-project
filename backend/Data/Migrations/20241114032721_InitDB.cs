using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace GithubAnalyzeAPI.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Clones",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Timestamp = table.Column<string>(type: "longtext", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false),
                    Uniques = table.Column<int>(type: "int", nullable: false),
                    RepoId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clones", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "HistorySyncs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Summary = table.Column<string>(type: "longtext", nullable: false),
                    SyncAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Type = table.Column<string>(type: "longtext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistorySyncs", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Repositories",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    RepoId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: false),
                    FullName = table.Column<string>(type: "longtext", nullable: false),
                    HtmlUrl = table.Column<string>(type: "longtext", nullable: false),
                    Language = table.Column<string>(type: "longtext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Repositories", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Views",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Timestamp = table.Column<string>(type: "longtext", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false),
                    Uniques = table.Column<int>(type: "int", nullable: false),
                    RepoId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Views", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Clones");

            migrationBuilder.DropTable(
                name: "HistorySyncs");

            migrationBuilder.DropTable(
                name: "Repositories");

            migrationBuilder.DropTable(
                name: "Views");
        }
    }
}
