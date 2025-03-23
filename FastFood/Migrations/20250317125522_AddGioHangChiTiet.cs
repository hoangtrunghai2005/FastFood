using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FastFood.Migrations
{
    /// <inheritdoc />
    public partial class AddGioHangChiTiet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GioHangChiTiet_DoAnNhanhs_DoAnNhanhID",
                table: "GioHangChiTiet");

            migrationBuilder.DropForeignKey(
                name: "FK_GioHangChiTiet_GioHangs_GioHangID",
                table: "GioHangChiTiet");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GioHangChiTiet",
                table: "GioHangChiTiet");

            migrationBuilder.RenameTable(
                name: "GioHangChiTiet",
                newName: "GioHangChiTiets");

            migrationBuilder.RenameIndex(
                name: "IX_GioHangChiTiet_GioHangID",
                table: "GioHangChiTiets",
                newName: "IX_GioHangChiTiets_GioHangID");

            migrationBuilder.RenameIndex(
                name: "IX_GioHangChiTiet_DoAnNhanhID",
                table: "GioHangChiTiets",
                newName: "IX_GioHangChiTiets_DoAnNhanhID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GioHangChiTiets",
                table: "GioHangChiTiets",
                column: "GioHangChiTietID");

            migrationBuilder.AddForeignKey(
                name: "FK_GioHangChiTiets_DoAnNhanhs_DoAnNhanhID",
                table: "GioHangChiTiets",
                column: "DoAnNhanhID",
                principalTable: "DoAnNhanhs",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GioHangChiTiets_GioHangs_GioHangID",
                table: "GioHangChiTiets",
                column: "GioHangID",
                principalTable: "GioHangs",
                principalColumn: "GioHangID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GioHangChiTiets_DoAnNhanhs_DoAnNhanhID",
                table: "GioHangChiTiets");

            migrationBuilder.DropForeignKey(
                name: "FK_GioHangChiTiets_GioHangs_GioHangID",
                table: "GioHangChiTiets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GioHangChiTiets",
                table: "GioHangChiTiets");

            migrationBuilder.RenameTable(
                name: "GioHangChiTiets",
                newName: "GioHangChiTiet");

            migrationBuilder.RenameIndex(
                name: "IX_GioHangChiTiets_GioHangID",
                table: "GioHangChiTiet",
                newName: "IX_GioHangChiTiet_GioHangID");

            migrationBuilder.RenameIndex(
                name: "IX_GioHangChiTiets_DoAnNhanhID",
                table: "GioHangChiTiet",
                newName: "IX_GioHangChiTiet_DoAnNhanhID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GioHangChiTiet",
                table: "GioHangChiTiet",
                column: "GioHangChiTietID");

            migrationBuilder.AddForeignKey(
                name: "FK_GioHangChiTiet_DoAnNhanhs_DoAnNhanhID",
                table: "GioHangChiTiet",
                column: "DoAnNhanhID",
                principalTable: "DoAnNhanhs",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GioHangChiTiet_GioHangs_GioHangID",
                table: "GioHangChiTiet",
                column: "GioHangID",
                principalTable: "GioHangs",
                principalColumn: "GioHangID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
