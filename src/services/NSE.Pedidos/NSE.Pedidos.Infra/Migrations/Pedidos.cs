#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace NSE.Pedidos.Infra.Migrations;

public partial class Pedidos : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateSequence<int>(
            "MinhaSequencia",
            startValue: 1000L);

        migrationBuilder.CreateTable(
            "Pedidos",
            table => new
            {
                Id = table.Column<Guid>("uniqueidentifier", nullable: false),
                Codigo = table.Column<int>("int", nullable: false, defaultValueSql: "NEXT VALUE FOR MinhaSequencia"),
                ClienteId = table.Column<Guid>("uniqueidentifier", nullable: false),
                VoucherId = table.Column<Guid>("uniqueidentifier", nullable: true),
                VoucherUtilizado = table.Column<bool>("bit", nullable: false),
                Desconto = table.Column<decimal>("decimal(18,2)", nullable: false),
                ValorTotal = table.Column<decimal>("decimal(18,2)", nullable: false),
                DataCadastro = table.Column<DateTime>("datetime2", nullable: false),
                PedidoStatus = table.Column<int>("int", nullable: false),
                Logradouro = table.Column<string>("varchar(100)", nullable: true),
                Numero = table.Column<string>("varchar(100)", nullable: true),
                Complemento = table.Column<string>("varchar(100)", nullable: true),
                Bairro = table.Column<string>("varchar(100)", nullable: true),
                Cep = table.Column<string>("varchar(100)", nullable: true),
                Cidade = table.Column<string>("varchar(100)", nullable: true),
                Estado = table.Column<string>("varchar(100)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Pedidos", x => x.Id);
                table.ForeignKey(
                    "FK_Pedidos_Vouchers_VoucherId",
                    x => x.VoucherId,
                    "Vouchers",
                    "Id");
            });

        migrationBuilder.CreateTable(
            "PedidoItems",
            table => new
            {
                Id = table.Column<Guid>("uniqueidentifier", nullable: false),
                PedidoId = table.Column<Guid>("uniqueidentifier", nullable: false),
                ProdutoId = table.Column<Guid>("uniqueidentifier", nullable: false),
                ProdutoNome = table.Column<string>("varchar(250)", nullable: false),
                Quantidade = table.Column<int>("int", nullable: false),
                ValorUnitario = table.Column<decimal>("decimal(18,2)", nullable: false),
                ProdutoImagem = table.Column<string>("varchar(100)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_PedidoItems", x => x.Id);
                table.ForeignKey(
                    "FK_PedidoItems_Pedidos_PedidoId",
                    x => x.PedidoId,
                    "Pedidos",
                    "Id");
            });

        migrationBuilder.CreateIndex(
            "IX_PedidoItems_PedidoId",
            "PedidoItems",
            "PedidoId");

        migrationBuilder.CreateIndex(
            "IX_Pedidos_VoucherId",
            "Pedidos",
            "VoucherId");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            "PedidoItems");

        migrationBuilder.DropTable(
            "Pedidos");

        migrationBuilder.DropSequence(
            "MinhaSequencia");
    }
}