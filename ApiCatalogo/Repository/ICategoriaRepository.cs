using System;
using ApiCatalogo.Models;
using ApiCatalogo.Pagination;
using ToDoApi;

namespace ApiCatalogo.Repository;

public interface ICategoriaRepository : IRepository<Categoria>
{
    Task<PagedList<Categoria>> GetCategorias(CategoriasParameters categoriaparameters);
    Task<IEnumerable<Categoria>> GetCategoriasPorProduto();
}

