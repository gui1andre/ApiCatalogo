using System;
using ApiCatalogo.Context;
using ApiCatalogo.Models;
using ApiCatalogo.Pagination;
using Microsoft.EntityFrameworkCore;
using ToDoApi.Repository;

namespace ApiCatalogo.Repository;

public class ProdutoRepository : Repository<Produto>, IProdutoRepository
{
	public ProdutoRepository(AppDbContext context) : base(context)
	{
	}

    public async Task<PagedList<Produto>> GetProdutos(ProdutosParameters produtosParameters)
    {
        //return Get().OrderBy(ob => ob.Nome)
        //    .Skip((produtosParameters.PageNumber - 1) * produtosParameters.PageSize)
        //    .Take(produtosParameters.PageSize)
        //    .ToList();
        return await PagedList<Produto>.ToPagedList(Get().OrderBy(o => o.ProdutoId), produtosParameters.PageNumber, produtosParameters.PageSize);
            
    }

    public async Task<IEnumerable<Produto>> GetProdutosPorPreco() =>  await Get().OrderBy(c => c.Preco).ToListAsync();
}

