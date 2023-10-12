#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace NSE.Pedidos.Infra.Migrations;

public partial class Voucher : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            "Vouchers",
            table => new
            {
                Id = table.Column<Guid>("uniqueidentifier", nullable: false),
                Codigo = table.Column<string>("varchar(100)", nullable: false),
                Percentual = table.Column<decimal>("decimal(18,2)", nullable: true),
                ValorDesconto = table.Column<decimal>("decimal(18,2)", nullable: true),
                Quantidade = table.Column<int>("int", nullable: false),
                TipoDesconto = table.Column<int>("int", nullable: false),
                DataCriacao = table.Column<DateTime>("datetime2", nullable: false),
                DataUtilizacao = table.Column<DateTime>("datetime2", nullable: true),
                DataValidade = table.Column<DateTime>("datetime2", nullable: false),
                Ativo = table.Column<bool>("bit", nullable: false),
                Utilizado = table.Column<bool>("bit", nullable: false)
            },
            constraints: table => { table.PrimaryKey("PK_Vouchers", x => x.Id); });
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            "Vouchers");
    }
}