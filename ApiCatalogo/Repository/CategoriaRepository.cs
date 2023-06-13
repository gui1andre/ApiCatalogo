using System;
using ApiCatalogo.Context;
using ApiCatalogo.Models;
using Microsoft.EntityFrameworkCore;
using ToDoApi.Repository;

namespace ApiCatalogo.Repository;

public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
{
	public CategoriaRepository(AppDbContext context) : base(context)
	{

	}

    public IEnumerable<Categoria> GetCategoriasPorProduto()
    {
        return Get().Include(c => c.Produtos).ToList();
    }
}

