using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NSE.Carrinho.API.Migrations
{
    public partial class UpdateCarrinho : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Quantidadade",
                table: "CarrinhoItens",
                newName: "Quantidade");

            migrationBuilder.RenameColumn(
                name: "ClientId",
                table: "CarrinhoClientes",
                newName: "ClienteId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Quantidade",
                table: "CarrinhoItens",
                newName: "Quantidadade");

            migrationBuilder.RenameColumn(
                name: "ClienteId",
                table: "CarrinhoClientes",
                newName: "ClientId");
        }
    }
}
