using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiCatalogo.Context;
using ApiCatalogo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApiCatalogo.Controllers;

[Route("[controller]")]
[ApiController]
public class CategoriasController : Controller
{
    private readonly AppDbContext _context;

    public CategoriasController(AppDbContext context )
    {
        _context = context;
    }

    [HttpGet("produtos")]
    public ActionResult GetCategoriasProdutos()
    {
        var categorias = _context.Categorias.Include(p => p.Produtos).ToList<Categoria>();
        if (categorias is null)
        {
            return NotFound("Não foram encontrados produtos");
        }
        return Ok(categorias);
    }

    [HttpGet]
    public ActionResult<IEnumerable<Categoria>> Get()
    {

        var categorias = _context.Categorias.ToList<Categoria>();
        if (categorias is null)
        {
            return NotFound("Não foram encontrados produtos");
        }

        return Ok(categorias);
    }

    [HttpGet("{id:int}", Name ="ObterCategoria")]
    public ActionResult Get(int id)
    {
        var categoria = _context.Categorias.FirstOrDefault(p => p.CategoriaId == id);
        if (categoria is null)
            return NotFound("Não foi encontrada uma categoria com este Id");

        return Ok(categoria);
    }

    [HttpPost]
    public ActionResult Post(Categoria categoria)
    {
        _context.Categorias.Add(categoria);
        _context.SaveChanges();
        return new CreatedAtRouteResult("ObterCategoria", new {id = categoria.CategoriaId }, categoria);
    }

    [HttpDelete("{id:int}")]
    public ActionResult Delete(int id)
    {
        var categoria = _context.Categorias.FirstOrDefault(p => p.CategoriaId == id);
        if(categoria is null)
            return NotFound("Não foi encontrada uma categoria com este Id");

        _context.Categorias.Remove(categoria);
        _context.SaveChanges();
        return Ok();
    }

    [HttpPut("{id:int}")]
    public ActionResult Put(int id, Categoria categoria) {
        if (id != categoria.CategoriaId)
            return BadRequest();

        _context.Entry(categoria).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        _context.SaveChanges();
        return new CreatedAtRouteResult("ObterCategoria", new { id = categoria.CategoriaId }, categoria);

    }
}

