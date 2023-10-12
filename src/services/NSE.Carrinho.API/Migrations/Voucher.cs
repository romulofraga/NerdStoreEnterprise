#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace NSE.Carrinho.API.Migrations;

public partial class Voucher : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<decimal>(
            "Desconto",
            "CarrinhoClientes",
            "decimal(18,2)",
            nullable: false,
            defaultValue: 0m);

        migrationBuilder.AddColumn<decimal>(
            "Percentual",
            "CarrinhoClientes",
            "decimal(18,2)",
            nullable: true);

        migrationBuilder.AddColumn<int>(
            "TipoDesconto",
            "CarrinhoClientes",
            "int",
            nullable: true);

        migrationBuilder.AddColumn<decimal>(
            "ValorDesconto",
            "CarrinhoClientes",
            "decimal(18,2)",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            "VoucherCodigo",
            "CarrinhoClientes",
            "varchar(50)",
            nullable: true);

        migrationBuilder.AddColumn<bool>(
            "VoucherUtilizado",
            "CarrinhoClientes",
            "bit",
            nullable: false,
            defaultValue: false);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            "Desconto",
            "CarrinhoClientes");

        migrationBuilder.DropColumn(
            "Percentual",
            "CarrinhoClientes");

        migrationBuilder.DropColumn(
            "TipoDesconto",
            "CarrinhoClientes");

        migrationBuilder.DropColumn(
            "ValorDesconto",
            "CarrinhoClientes");

        migrationBuilder.DropColumn(
            "VoucherCodigo",
            "CarrinhoClientes");

        migrationBuilder.DropColumn(
            "VoucherUtilizado",
            "CarrinhoClientes");
    }
}