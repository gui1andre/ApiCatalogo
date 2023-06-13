using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiCatalogo.Context;
using ApiCatalogo.Models;
using ApiCatalogo.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiCatalogo.Controllers;

[Route("[controller]")]
[ApiController]
public class ProdutosController : ControllerBase
{
    private readonly IUnitOfWork _uof;

    public ProdutosController(IUnitOfWork context)
    {
        _uof = context;
    }

    [HttpGet("menorpreco")]
    public ActionResult<IEnumerable<Produto>> GetProdutosPreco()
    {
        return _uof.produtoRepository.GetProdutosPorPreco().ToList();
    }

    [HttpGet]
    public ActionResult<IEnumerable<Produto>> Get()
    {
        var produtos = _uof.produtoRepository.Get().ToList<Produto>();
        if (produtos is null)
        {
            return NotFound("Não foram encontrados produtos");
        }

        return Ok(produtos);
    }

    [HttpGet("{id:int}", Name = "ObterProdutoId")]
    public ActionResult<Produto> Get(int id)
    {
        var produto = _uof.produtoRepository.GetById(p => p.ProdutoId == id);
        if (produto is null)
        {
            return NotFound("Produto não encontrado");
        }

        return Ok(produto);
    }
    [HttpPost]
    public ActionResult Post(Produto produto)
    {
        _uof.produtoRepository.Add(produto);
        _uof.Commit();
        return new CreatedAtRouteResult("ObterProdutoId", new { id = produto.ProdutoId }, produto);

    }
    [HttpPut("{id:int}")]
    public ActionResult Put(int id, Produto produto)
    {
        if (id != produto.ProdutoId)
        {
            return BadRequest();
        }

        _uof.produtoRepository.Update(produto);
        _uof.Commit();
        return Ok(produto);
    }

    [HttpDelete("{id:int}")]
    public ActionResult Delete(int id)
    {
        var produto = _uof.produtoRepository.GetById(p => p.ProdutoId == id);
        if (produto is null)
            return NotFound();

        _uof.produtoRepository.Delete(produto);
        _uof.Commit();
        return Ok(produto);

    }

}
