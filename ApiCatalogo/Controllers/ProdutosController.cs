using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiCatalogo.Context;
using ApiCatalogo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiCatalogo.Controllers;

[Route("[controller]")]
[ApiController]
public class ProdutosController : ControllerBase
{
    private readonly AppDbContext _context;

    public ProdutosController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Produto>> Get()
    {
        var produtos = _context.Produtos.ToList<Produto>();
        if(produtos is null)
        {
            return NotFound("Não foram encontrados produtos");
        }

        return Ok(produtos);
    }

    [HttpGet("{id:int}", Name ="ObterProdutoId")]
    public ActionResult<Produto> Get(int id)
    {
        var produto = _context.Produtos.FirstOrDefault(p => p.ProdutoId == id);
        if(produto is null)
        {
            return NotFound("Produto não encontrado");
        }

        return Ok(produto);
    }
    [HttpPost]
    public ActionResult Post(Produto produto)
    {
        _context.Add(produto);
        _context.SaveChanges();
        return new CreatedAtRouteResult("ObterProdutoId", new {id = produto.ProdutoId}, produto);

    }
    [HttpPut("{id:int}")]
    public ActionResult Put(int id,Produto produto)
    {
        if(id != produto.ProdutoId)
        {
            return BadRequest();
        }

        _context.Entry(produto).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        _context.SaveChanges();
        return Ok(produto);
    }

    [HttpDelete("{id:int}")]
    public ActionResult Delete(int id)
    {
        var produto = _context.Produtos.FirstOrDefault(p => p.ProdutoId == id);
        if (produto is null)
            return NotFound();

        _context.Remove(produto);
        _context.SaveChanges();
        return Ok(produto);

    }

}
