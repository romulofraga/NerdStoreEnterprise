#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace NSE.Carrinho.API.Migrations;

public partial class CarrinhoCascade : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            "FK_CarrinhoItens_CarrinhoClientes_CarrinhoClienteId",
            "CarrinhoItens");

        migrationBuilder.AddForeignKey(
            "FK_CarrinhoItens_CarrinhoClientes_CarrinhoClienteId",
            "CarrinhoItens",
            "CarrinhoClienteId",
            "CarrinhoClientes",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            "FK_CarrinhoItens_CarrinhoClientes_CarrinhoClienteId",
            "CarrinhoItens");

        migrationBuilder.AddForeignKey(
            "FK_CarrinhoItens_CarrinhoClientes_CarrinhoClienteId",
            "CarrinhoItens",
            "CarrinhoClienteId",
            "CarrinhoClientes",
            principalColumn: "Id");
    }
}