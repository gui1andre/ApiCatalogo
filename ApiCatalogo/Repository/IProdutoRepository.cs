using System;
using ApiCatalogo.Models;
using ToDoApi;

namespace ApiCatalogo.Repository;

public interface IProdutoRepository : IRepository<Produto>
{
    IEnumerable<Produto> GetProdutosPorPreco();
}

