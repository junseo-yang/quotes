using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace QuotesWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Quotes",
                columns: table => new
                {
                    QuoteId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Author = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Like = table.Column<int>(type: "int", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quotes", x => x.QuoteId);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    TagId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.TagId);
                });

            migrationBuilder.CreateTable(
                name: "TagAssignments",
                columns: table => new
                {
                    QuoteId = table.Column<int>(type: "int", nullable: false),
                    TagId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TagAssignments", x => new { x.QuoteId, x.TagId });
                    table.ForeignKey(
                        name: "FK_TagAssignments_Quotes_QuoteId",
                        column: x => x.QuoteId,
                        principalTable: "Quotes",
                        principalColumn: "QuoteId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TagAssignments_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "TagId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Quotes",
                columns: new[] { "QuoteId", "Author", "Description", "LastModified", "Like" },
                values: new object[,]
                {
                    { 1, "Mark Twain", "Good friends, good books, and a sleepy conscience: this is the ideal life.", new DateTime(2023, 11, 14, 22, 48, 56, 749, DateTimeKind.Local).AddTicks(2497), 3 },
                    { 2, "Pearl S. Buck", "Many people lose the small joys in the hope for the big happiness.", new DateTime(2023, 11, 14, 22, 48, 56, 749, DateTimeKind.Local).AddTicks(2501), 4 },
                    { 3, "Leo Tolstoy", "A quiet secluded life in the country, with the possibility of being useful to people to whom it is easy to do good, and who are not accustomed to have it done to them; then work which one hopes may be of some use; then rest, nature, books, music, love for one's neighbor — such is my idea of happiness.", new DateTime(2023, 11, 14, 22, 48, 56, 749, DateTimeKind.Local).AddTicks(2504), 5 }
                });

            migrationBuilder.InsertData(
                table: "Tags",
                columns: new[] { "TagId", "LastModified", "Name" },
                values: new object[,]
                {
                    { 1, new DateTime(2023, 11, 14, 22, 48, 56, 749, DateTimeKind.Local).AddTicks(2657), "books" },
                    { 2, new DateTime(2023, 11, 14, 22, 48, 56, 749, DateTimeKind.Local).AddTicks(2661), "contentment" },
                    { 3, new DateTime(2023, 11, 14, 22, 48, 56, 749, DateTimeKind.Local).AddTicks(2664), "friends" },
                    { 4, new DateTime(2023, 11, 14, 22, 48, 56, 749, DateTimeKind.Local).AddTicks(2668), "friendship" },
                    { 5, new DateTime(2023, 11, 14, 22, 48, 56, 749, DateTimeKind.Local).AddTicks(2671), "life" },
                    { 6, new DateTime(2023, 11, 14, 22, 48, 56, 749, DateTimeKind.Local).AddTicks(2674), "conduct-of-life" },
                    { 7, new DateTime(2023, 11, 14, 22, 48, 56, 749, DateTimeKind.Local).AddTicks(2677), "country" },
                    { 8, new DateTime(2023, 11, 14, 22, 48, 56, 749, DateTimeKind.Local).AddTicks(2680), "happiness" },
                    { 9, new DateTime(2023, 11, 14, 22, 48, 56, 749, DateTimeKind.Local).AddTicks(2683), "music" },
                    { 10, new DateTime(2023, 11, 14, 22, 48, 56, 749, DateTimeKind.Local).AddTicks(2686), "nature" },
                    { 11, new DateTime(2023, 11, 14, 22, 48, 56, 749, DateTimeKind.Local).AddTicks(2689), "work" }
                });

            migrationBuilder.InsertData(
                table: "TagAssignments",
                columns: new[] { "QuoteId", "TagId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 1, 2 },
                    { 1, 3 },
                    { 1, 4 },
                    { 1, 5 },
                    { 2, 2 },
                    { 3, 1 },
                    { 3, 2 },
                    { 3, 5 },
                    { 3, 6 },
                    { 3, 7 },
                    { 3, 8 },
                    { 3, 9 },
                    { 3, 10 },
                    { 3, 11 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_TagAssignments_TagId",
                table: "TagAssignments",
                column: "TagId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TagAssignments");

            migrationBuilder.DropTable(
                name: "Quotes");

            migrationBuilder.DropTable(
                name: "Tags");
        }
    }
}
