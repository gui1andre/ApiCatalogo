using System;
namespace ApiCatalogo.DTOs;

public class CategoriaDTO
{
    public int CategoriaId { get; set; }
    public string? NomeCategoria { get; set; }
    public string? ImagemUrl { get; set; }
    public ICollection<ProdutoDTO> Produtos { get; set; }
}

