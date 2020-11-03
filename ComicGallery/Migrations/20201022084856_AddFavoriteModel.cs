using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ComicGallery.Migrations
{
    public partial class AddFavoriteModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserFavoriteComics",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<int>(nullable: true),
                    ComicId = table.Column<int>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFavoriteComics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserFavoriteComics_Comics_ComicId",
                        column: x => x.ComicId,
                        principalTable: "Comics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserFavoriteComics_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserFavoriteComics_ComicId",
                table: "UserFavoriteComics",
                column: "ComicId");

            migrationBuilder.CreateIndex(
                name: "IX_UserFavoriteComics_UserId",
                table: "UserFavoriteComics",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserFavoriteComics");
        }
    }
}
