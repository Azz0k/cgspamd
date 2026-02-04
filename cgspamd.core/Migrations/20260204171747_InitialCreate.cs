using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace cgspamd.core.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserName = table.Column<string>(type: "TEXT", nullable: false, collation: "NOCASE"),
                    Hash = table.Column<string>(type: "TEXT", nullable: false, collation: "BINARY"),
                    FullName = table.Column<string>(type: "TEXT", nullable: false, collation: "NOCASE"),
                    Enabled = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: true),
                    Deleted = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false),
                    TokenVersion = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: -2147483648),
                    IsAdmin = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FilterRules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Value = table.Column<string>(type: "TEXT", nullable: false, collation: "NOCASE"),
                    Comment = table.Column<string>(type: "TEXT", nullable: false, defaultValue: "", collation: "NOCASE"),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<string>(type: "TEXT", nullable: false, collation: "NOCASE"),
                    UpdatedAt = table.Column<string>(type: "TEXT", nullable: true, collation: "NOCASE"),
                    CreatedByUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    UpdatedByUserId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FilterRules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FilterRules_Users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FilterRules_Users_UpdatedByUserId",
                        column: x => x.UpdatedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_FilterRules_CreatedByUserId",
                table: "FilterRules",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_FilterRules_UpdatedByUserId",
                table: "FilterRules",
                column: "UpdatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserName",
                table: "Users",
                column: "UserName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FilterRules");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
