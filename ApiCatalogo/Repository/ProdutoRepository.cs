using System;
using ApiCatalogo.Context;
using ApiCatalogo.Models;
using ApiCatalogo.Pagination;
using ToDoApi.Repository;

namespace ApiCatalogo.Repository;

public class ProdutoRepository : Repository<Produto>, IProdutoRepository
{
	public ProdutoRepository(AppDbContext context) : base(context)
	{
	}

    public PagedList<Produto> GetProdutos(ProdutosParameters produtosParameters)
    {
        //return Get().OrderBy(ob => ob.Nome)
        //    .Skip((produtosParameters.PageNumber - 1) * produtosParameters.PageSize)
        //    .Take(produtosParameters.PageSize)
        //    .ToList();

        return PagedList<Produto>.ToPagedList(Get().OrderBy(o => o.ProdutoId), produtosParameters.PageNumber, produtosParameters.PageSize);
            
    }

    public IEnumerable<Produto> GetProdutosPorPreco()
    {
        return Get().OrderBy(c => c.Preco).ToList();
    }
}

