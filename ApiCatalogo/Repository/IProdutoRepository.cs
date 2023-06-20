using System;
using ApiCatalogo.Models;
using ApiCatalogo.Pagination;
using ToDoApi;

namespace ApiCatalogo.Repository;

public interface IProdutoRepository : IRepository<Produto>
{
    Task<PagedList<Produto>> GetProdutos(ProdutosParameters produtosParameters);
    Task<IEnumerable<Produto>> GetProdutosPorPreco();
}

