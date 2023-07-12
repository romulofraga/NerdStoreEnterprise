using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NSE.Carrinho.API.Migrations
{
    public partial class Carrinho : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CarrinhoClinhete",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClienteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ValorTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarrinhoClinhete", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CarrinhoItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProdutoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nome = table.Column<string>(type: "varchar(100)", nullable: true),
                    Quantidade = table.Column<int>(type: "int", nullable: false),
                    Valor = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CarrinhoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarrinhoItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CarrinhoItems_CarrinhoClinhete_CarrinhoId",
                        column: x => x.CarrinhoId,
                        principalTable: "CarrinhoClinhete",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IDX_Cliente",
                table: "CarrinhoClinhete",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_CarrinhoItems_CarrinhoId",
                table: "CarrinhoItems",
                column: "CarrinhoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CarrinhoItems");

            migrationBuilder.DropTable(
                name: "CarrinhoClinhete");
        }
    }
}
