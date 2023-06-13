using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiCatalogo.Context;
using ApiCatalogo.Models;
using ApiCatalogo.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApiCatalogo.Controllers;

[Route("[controller]")]
[ApiController]
public class CategoriasController : Controller
{
    private readonly IUnitOfWork _uof;

    public CategoriasController(IUnitOfWork context )
    {
        _uof = context;
    }

    [HttpGet("produtos")]
    public ActionResult GetCategoriasProdutos()
    {
        var categorias = _uof.categoriaRepository.GetCategoriasPorProduto();
        if (categorias is null)
        {
            return NotFound("Não foram encontrados produtos");
        }
        return Ok(categorias);
    }

    [HttpGet]
    public ActionResult<IEnumerable<Categoria>> Get()
    {

        var categorias = _uof.categoriaRepository.Get().ToList<Categoria>();
        if (categorias is null)
        {
            return NotFound("Não foram encontrados produtos");
        }

        return Ok(categorias);
    }

    [HttpGet("{id:int}", Name ="ObterCategoria")]
    public ActionResult Get(int id)
    {
        var categoria = _uof.categoriaRepository.GetById(p => p.CategoriaId == id);
        if (categoria is null)
            return NotFound("Não foi encontrada uma categoria com este Id");

        return Ok(categoria);
    }

    [HttpPost]
    public ActionResult Post(Categoria categoria)
    {
        _uof.categoriaRepository.Add(categoria);
        _uof.Commit();
        return new CreatedAtRouteResult("ObterCategoria", new {id = categoria.CategoriaId }, categoria);
    }

    [HttpDelete("{id:int}")]
    public ActionResult Delete(int id)
    {
        var categoria = _uof.categoriaRepository.GetById(p => p.CategoriaId == id);
        if(categoria is null)
            return NotFound("Não foi encontrada uma categoria com este Id");

        _uof.categoriaRepository.Delete(categoria);
        _uof.Commit();
        return Ok();
    }

    [HttpPut("{id:int}")]
    public ActionResult Put(int id, Categoria categoria) {
        if (id != categoria.CategoriaId)
            return BadRequest();

        _uof.categoriaRepository.Update(categoria);
        _uof.Commit();
        return new CreatedAtRouteResult("ObterCategoria", new { id = categoria.CategoriaId }, categoria);

    }
}

