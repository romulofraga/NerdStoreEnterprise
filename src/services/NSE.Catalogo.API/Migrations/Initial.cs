#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace NSE.Catalogo.API.Migrations;

public partial class Initial : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            "Produtos",
            table => new
            {
                Id = table.Column<Guid>("uniqueidentifier", nullable: false),
                Nome = table.Column<string>("varchar(250)", nullable: false),
                Descricao = table.Column<string>("varchar(500)", nullable: false),
                Ativo = table.Column<bool>("bit", nullable: false),
                Valor = table.Column<decimal>("decimal(18,2)", nullable: false),
                DataCadastro = table.Column<DateTime>("datetime2", nullable: false),
                Imagem = table.Column<string>("varchar(250)", nullable: false),
                QuantidadeEstoque = table.Column<int>("int", nullable: false)
            },
            constraints: table => { table.PrimaryKey("PK_Produtos", x => x.Id); });
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            "Produtos");
    }
}