using System;
using ApiCatalogo.Context;
using ApiCatalogo.Models;
using ApiCatalogo.Pagination;
using Microsoft.EntityFrameworkCore;
using ToDoApi.Repository;

namespace ApiCatalogo.Repository;

public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
{
	public CategoriaRepository(AppDbContext context) : base(context)
	{

	}

    public PagedList<Categoria> GetCategorias(CategoriasParameters categoriaparameters)
    {
        return PagedList<Categoria>.ToPagedList(Get().OrderBy(o => o.CategoriaId), categoriaparameters.PageNumber, categoriaparameters.PageSize);
    }

    public IEnumerable<Categoria> GetCategoriasPorProduto()
    {
        return Get().Include(c => c.Produtos).ToList();
    }
}

