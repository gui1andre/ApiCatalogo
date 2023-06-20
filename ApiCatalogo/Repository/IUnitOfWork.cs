using System;
namespace ApiCatalogo.Repository;

public interface IUnitOfWork
{
    IProdutoRepository produtoRepository { get; }
    ICategoriaRepository categoriaRepository { get; }
    Task Commit();
}

