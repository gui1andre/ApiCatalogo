using System;
using ApiCatalogo.Context;

namespace ApiCatalogo.Repository;

public class UnitOfWork : IUnitOfWork
{
    private ProdutoRepository _produtoRepository;
    private CategoriaRepository _categoriaRepository;
    public AppDbContext _context;

    public UnitOfWork(AppDbContext context) => _context = context;

    public IProdutoRepository produtoRepository
    {
        get{
            return _produtoRepository =_produtoRepository ?? new ProdutoRepository(_context);
        }
    }

    public ICategoriaRepository categoriaRepository
    {
        get
        {
            return _categoriaRepository = _categoriaRepository ?? new CategoriaRepository(_context);
        }
    }

    public void Commit() => _context.SaveChanges();

    public void Dispose() => _context.Dispose();
}

