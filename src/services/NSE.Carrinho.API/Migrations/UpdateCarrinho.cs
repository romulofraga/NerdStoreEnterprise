#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace NSE.Carrinho.API.Migrations;

public partial class UpdateCarrinho : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.RenameColumn(
            "Quantidadade",
            "CarrinhoItens",
            "Quantidade");

        migrationBuilder.RenameColumn(
            "ClientId",
            "CarrinhoClientes",
            "ClienteId");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.RenameColumn(
            "Quantidade",
            "CarrinhoItens",
            "Quantidadade");

        migrationBuilder.RenameColumn(
            "ClienteId",
            "CarrinhoClientes",
            "ClientId");
    }
}