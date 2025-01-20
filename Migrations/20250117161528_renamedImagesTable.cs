using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AzureIoTServer.Migrations
{
    /// <inheritdoc />
    public partial class renamedImagesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK__images",
                table: "_images");

            migrationBuilder.RenameTable(
                name: "_images",
                newName: "images");

            migrationBuilder.AddPrimaryKey(
                name: "PK_images",
                table: "images",
                column: "ImageId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_images",
                table: "images");

            migrationBuilder.RenameTable(
                name: "images",
                newName: "_images");

            migrationBuilder.AddPrimaryKey(
                name: "PK__images",
                table: "_images",
                column: "ImageId");
        }
    }
}
