using System;
using ApiCatalogo.Models;
using ApiCatalogo.Pagination;
using ToDoApi;

namespace ApiCatalogo.Repository;

public interface IProdutoRepository : IRepository<Produto>
{
    PagedList<Produto> GetProdutos(ProdutosParameters produtosParameters);
    IEnumerable<Produto> GetProdutosPorPreco();
}

