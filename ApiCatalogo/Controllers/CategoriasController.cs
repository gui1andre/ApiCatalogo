﻿using ApiCatalogo.DTOs;
using ApiCatalogo.Models;
using ApiCatalogo.Pagination;
using ApiCatalogo.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApiCatalogo.Controllers;

[Route("[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class CategoriasController : Controller
{
    private readonly IUnitOfWork _uof;
    private readonly IMapper _mapper;

    public CategoriasController(IUnitOfWork context, IMapper mapper)
    {
        _uof = context;
        _mapper = mapper;
    }

    [HttpGet("produtos")]
    public async Task<ActionResult<CategoriaDTO>> GetCategoriasProdutos()
    {
        var categorias = await _uof.categoriaRepository.GetCategoriasPorProduto();
        if (categorias is null)
        {
            return NotFound("Não foram encontrados produtos");
        }

        var categoriasDTO = _mapper.Map<List<CategoriaDTO>>(categorias);
        return Ok(categoriasDTO);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoriaDTO>>> Get([FromQuery] CategoriasParameters categoriasParameters)
    {

        var categorias = await _uof.categoriaRepository.GetCategorias(categoriasParameters);
        if (categorias is null)
        {
            return NotFound("Não foram encontrados produtos");
        }

        var metadata = new
        {
            categorias.TotalCount,
            categorias.PageSize,
            categorias.Currentpage,
            categorias.TotalPages,
            categorias.HasNext,
            categorias.HasPrevious
        };

        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

        var categoriasDTO = _mapper.Map<List<CategoriaDTO>>(categorias);
        return Ok(categoriasDTO);
    }

    [HttpGet("{id:int}", Name ="ObterCategoria")]
    public async Task<ActionResult<CategoriaDTO>> Get(int id)
    {
        var categoria = await _uof.categoriaRepository.GetById(p => p.CategoriaId == id);
        if (categoria is null)
            return NotFound("Não foi encontrada uma categoria com este Id");
        var categoriaDTO = _mapper.Map<CategoriaDTO>(categoria);
        return Ok(categoriaDTO);
    }

    [HttpPost]
    public async Task<ActionResult<CategoriaDTO>> Post(CategoriaDTO categoriaDto)
    {
        var categoria = _mapper.Map<Categoria>(categoriaDto);
        _uof.categoriaRepository.Add(categoria);
        await _uof.Commit();

        var categoriaDTO = _mapper.Map<CategoriaDTO>(categoria);
        return new CreatedAtRouteResult("ObterCategoria", new {id = categoria.CategoriaId }, categoriaDTO);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<CategoriaDTO>> Delete(int id)
    {
        var categoria = await _uof.categoriaRepository.GetById(p => p.CategoriaId == id);
        if(categoria is null)
            return NotFound("Não foi encontrada uma categoria com este Id");

        _uof.categoriaRepository.Delete(categoria);
        await _uof.Commit();
        var categoriaDTO = _mapper.Map<CategoriaDTO>(categoria);
        return Ok(categoriaDTO);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> Put(int id, CategoriaDTO categoriaDto) {
        if (id != categoriaDto.CategoriaId)
            return BadRequest();
        var categoria = _mapper.Map<Categoria>(categoriaDto);
        _uof.categoriaRepository.Update(categoria);
        await _uof.Commit();
        var categoriaDTO = _mapper.Map<CategoriaDTO>(categoria);
        return new CreatedAtRouteResult("ObterCategoria", new { id = categoria.CategoriaId }, categoriaDTO);

    }
}

