#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace NSE.Pagamentos.API.Migrations;

public partial class Pagamentos : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            "Pagamentos",
            table => new
            {
                Id = table.Column<Guid>("uniqueidentifier", nullable: false),
                PedidoId = table.Column<Guid>("uniqueidentifier", nullable: false),
                TipoPagamento = table.Column<int>("int", nullable: false),
                Valor = table.Column<decimal>("decimal(18,2)", nullable: false)
            },
            constraints: table => { table.PrimaryKey("PK_Pagamentos", x => x.Id); });

        migrationBuilder.CreateTable(
            "Transacoes",
            table => new
            {
                Id = table.Column<Guid>("uniqueidentifier", nullable: false),
                CodigoAutorizacao = table.Column<string>("varchar(100)", nullable: true),
                BandeiraCartao = table.Column<string>("varchar(100)", nullable: true),
                DataTransacao = table.Column<DateTime>("datetime2", nullable: true),
                ValorTotal = table.Column<decimal>("decimal(18,2)", nullable: false),
                CustoTransacao = table.Column<decimal>("decimal(18,2)", nullable: false),
                Status = table.Column<int>("int", nullable: false),
                TID = table.Column<string>("varchar(100)", nullable: true),
                NSU = table.Column<string>("varchar(100)", nullable: true),
                PagamentoId = table.Column<Guid>("uniqueidentifier", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Transacoes", x => x.Id);
                table.ForeignKey(
                    "FK_Transacoes_Pagamentos_PagamentoId",
                    x => x.PagamentoId,
                    "Pagamentos",
                    "Id");
            });

        migrationBuilder.CreateIndex(
            "IX_Transacoes_PagamentoId",
            "Transacoes",
            "PagamentoId");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            "Transacoes");

        migrationBuilder.DropTable(
            "Pagamentos");
    }
}