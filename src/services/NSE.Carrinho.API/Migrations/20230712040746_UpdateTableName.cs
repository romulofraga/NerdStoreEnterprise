using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NSE.Carrinho.API.Migrations
{
    public partial class UpdateTableName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarrinhoItems_CarrinhoClinhete_CarrinhoId",
                table: "CarrinhoItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CarrinhoClinhete",
                table: "CarrinhoClinhete");

            migrationBuilder.RenameTable(
                name: "CarrinhoClinhete",
                newName: "CarrinhoCliente");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CarrinhoCliente",
                table: "CarrinhoCliente",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CarrinhoItems_CarrinhoCliente_CarrinhoId",
                table: "CarrinhoItems",
                column: "CarrinhoId",
                principalTable: "CarrinhoCliente",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarrinhoItems_CarrinhoCliente_CarrinhoId",
                table: "CarrinhoItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CarrinhoCliente",
                table: "CarrinhoCliente");

            migrationBuilder.RenameTable(
                name: "CarrinhoCliente",
                newName: "CarrinhoClinhete");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CarrinhoClinhete",
                table: "CarrinhoClinhete",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CarrinhoItems_CarrinhoClinhete_CarrinhoId",
                table: "CarrinhoItems",
                column: "CarrinhoId",
                principalTable: "CarrinhoClinhete",
                principalColumn: "Id");
        }
    }
}
