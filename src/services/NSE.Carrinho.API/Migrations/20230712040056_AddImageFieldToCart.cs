using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NSE.Carrinho.API.Migrations
{
    public partial class AddImageFieldToCart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Imagem",
                table: "CarrinhoItems",
                type: "varchar(100)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Imagem",
                table: "CarrinhoItems");
        }
    }
}
