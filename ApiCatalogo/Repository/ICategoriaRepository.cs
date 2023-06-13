using System;
using ApiCatalogo.Models;
using ToDoApi;

namespace ApiCatalogo.Repository;

public interface ICategoriaRepository : IRepository<Categoria>
{
    IEnumerable<Categoria> GetCategoriasPorProduto();
}

