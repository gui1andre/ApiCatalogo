using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiCatalogo.Migrations;

/// <inheritdoc />
public partial class PopulaCategoria : Migration
{
    protected override void Up(MigrationBuilder mb)
    {
        mb.Sql("Insert into public.\"Categorias\"(\"NomeCategoria\", \"ImagemUrl\") Values('Bebidas','bebidas.jpg')");
        mb.Sql("Insert into public.\"Categorias\"(\"NomeCategoria\", \"ImagemUrl\") Values('Lanches','lanches.jpg')");
        mb.Sql("Insert into public.\"Categorias\"(\"NomeCategoria\", \"ImagemUrl\") Values('Sobremesas','sobremesas.jpg')");
    }

    protected override void Down(MigrationBuilder mb)
    {
        mb.Sql("Delete from Categorias");
    }
}