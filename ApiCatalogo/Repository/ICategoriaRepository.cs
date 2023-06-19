using System;
using ApiCatalogo.Models;
using ApiCatalogo.Pagination;
using ToDoApi;

namespace ApiCatalogo.Repository;

public interface ICategoriaRepository : IRepository<Categoria>
{
    PagedList<Categoria> GetCategorias(CategoriasParameters categoriaparameters);
    IEnumerable<Categoria> GetCategoriasPorProduto();
}

