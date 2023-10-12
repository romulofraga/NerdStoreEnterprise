#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace NSE.Clientes.API.Migrations;

public partial class Clientes : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            "Clientes",
            table => new
            {
                Id = table.Column<Guid>("uniqueidentifier", nullable: false),
                Nome = table.Column<string>("varchar(200)", nullable: false),
                Email = table.Column<string>("varchar(245)", nullable: true),
                Cpf = table.Column<string>("varchar(11)", maxLength: 11, nullable: true),
                Excluido = table.Column<bool>("bit", nullable: false)
            },
            constraints: table => { table.PrimaryKey("PK_Clientes", x => x.Id); });

        migrationBuilder.CreateTable(
            "Enderecos",
            table => new
            {
                Id = table.Column<Guid>("uniqueidentifier", nullable: false),
                Logradouro = table.Column<string>("varchar(200)", nullable: false),
                Numero = table.Column<string>("varchar(50)", nullable: false),
                Complemento = table.Column<string>("varchar(250)", nullable: true),
                Bairro = table.Column<string>("varchar(100)", nullable: false),
                Cep = table.Column<string>("varchar(20)", nullable: false),
                Cidade = table.Column<string>("varchar(100)", nullable: false),
                Estado = table.Column<string>("varchar(50)", nullable: false),
                ClientId = table.Column<Guid>("uniqueidentifier", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Enderecos", x => x.Id);
                table.ForeignKey(
                    "FK_Enderecos_Clientes_ClientId",
                    x => x.ClientId,
                    "Clientes",
                    "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateIndex(
            "IX_Enderecos_ClientId",
            "Enderecos",
            "ClientId",
            unique: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            "Enderecos");

        migrationBuilder.DropTable(
            "Clientes");
    }
}