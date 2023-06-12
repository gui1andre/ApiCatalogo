using System;
using ApiCatalogo.Context;
using ApiCatalogo.Models;
using ToDoApi.Repository;

namespace ApiCatalogo.Repository;

public class ProdutoRepository : Repository<Produto>, IProdutoRepository
{
	public ProdutoRepository(AppDbContext context) : base(context)
	{
	}

    public IEnumerable<Produto> GetProdutosPorPreco()
    {
        return Get().OrderBy(c => c.Preco).ToList();
    }
}

