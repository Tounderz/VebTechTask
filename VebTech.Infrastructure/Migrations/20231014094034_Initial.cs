using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VebTech.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        private static readonly string[] columnsUser = new[] { "Id", "Age", "Email", "Name" };
        private static readonly string[] columnsAdmin = new[] { "Id", "Email", "Password" };
        private static readonly string[] columnsRole = new[] { "Id", "Name", "UserId" };

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Admins",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admins", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Roles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Admins",
                columns: columnsAdmin,
                values: new object[] { 1, "admin", "$2a$11$0Z.4.eDrWBAs6UbGBA4ExOGEm.7x9j13zQ0ZbEsYf.L8uJe2Sr7m6" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: columnsUser,
                values: new object[,]
                {
                    { 1, 25, "user1@example.com", "User 1" },
                    { 2, 30, "user2@example.com", "User 2" },
                    { 3, 35, "user3@example.com", "User 3" },
                    { 4, 25, "user4@example.com", "User 4" },
                    { 5, 45, "user5@example.com", "User 5" },
                    { 6, 35, "user6@example.com", "User 6" }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: columnsRole,
                values: new object[,]
                {
                    { 1, "Role 1", 1 },
                    { 2, "Role 2", 1 },
                    { 3, "Role 3", 1 },
                    { 4, "Role 1", 2 },
                    { 5, "Role 2", 2 },
                    { 6, "Role 1", 3 },
                    { 7, "Role 4", 3 },
                    { 8, "Role 5", 3 },
                    { 9, "Role 6", 3 },
                    { 10, "Role 7", 4 },
                    { 11, "Role 8", 4 },
                    { 12, "Role 3", 5 },
                    { 13, "Role 8", 5 },
                    { 14, "Role 9", 5 },
                    { 15, "Role 10", 5 },
                    { 16, "Role 12", 6 },
                    { 17, "Role 11", 6 },
                    { 18, "Role 18", 6 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Roles_UserId",
                table: "Roles",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Admins");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
