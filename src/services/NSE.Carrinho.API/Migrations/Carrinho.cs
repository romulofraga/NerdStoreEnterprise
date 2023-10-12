#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace NSE.Carrinho.API.Migrations;

public partial class Carrinho : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            "CarrinhoClientes",
            table => new
            {
                Id = table.Column<Guid>("uniqueidentifier", nullable: false),
                ClientId = table.Column<Guid>("uniqueidentifier", nullable: false),
                ValorTotal = table.Column<decimal>("decimal(18,2)", nullable: false)
            },
            constraints: table => { table.PrimaryKey("PK_CarrinhoClientes", x => x.Id); });

        migrationBuilder.CreateTable(
            "CarrinhoItens",
            table => new
            {
                Id = table.Column<Guid>("uniqueidentifier", nullable: false),
                ProdutoId = table.Column<Guid>("uniqueidentifier", nullable: false),
                Nome = table.Column<string>("varchar(100)", nullable: true),
                Quantidadade = table.Column<int>("int", nullable: false),
                Valor = table.Column<decimal>("decimal(18,2)", nullable: false),
                Imagem = table.Column<string>("varchar(100)", nullable: true),
                CarrinhoClienteId = table.Column<Guid>("uniqueidentifier", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_CarrinhoItens", x => x.Id);
                table.ForeignKey(
                    "FK_CarrinhoItens_CarrinhoClientes_CarrinhoClienteId",
                    x => x.CarrinhoClienteId,
                    "CarrinhoClientes",
                    "Id");
            });

        migrationBuilder.CreateIndex(
            "IDX_Cliente",
            "CarrinhoClientes",
            "ClientId");

        migrationBuilder.CreateIndex(
            "IX_CarrinhoItens_CarrinhoClienteId",
            "CarrinhoItens",
            "CarrinhoClienteId");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            "CarrinhoItens");

        migrationBuilder.DropTable(
            "CarrinhoClientes");
    }
}