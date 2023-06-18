using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiCatalogo.Context;
using ApiCatalogo.DTOs;
using ApiCatalogo.Models;
using ApiCatalogo.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiCatalogo.Controllers;

[Route("[controller]")]
[ApiController]
public class ProdutosController : ControllerBase
{
    private readonly IUnitOfWork _uof;
    private readonly IMapper _mapper;

    public ProdutosController(IUnitOfWork context,IMapper mapper)
    {
        _uof = context;
        _mapper = mapper;
    }

    [HttpGet("menorpreco")]
    public ActionResult<IEnumerable<ProdutoDTO>> GetProdutosPreco()
    {
        var produtos = _uof.produtoRepository.GetProdutosPorPreco().ToList();
        var produtoDTO = _mapper.Map<List<ProdutoDTO>>(produtos);
        return Ok(produtoDTO);
    }

    [HttpGet]
    public ActionResult<IEnumerable<ProdutoDTO>> Get()
    {
        var produtos = _uof.produtoRepository.Get().ToList<Produto>();
        if (produtos is null)
        {
            return NotFound("Não foram encontrados produtos");
        }
        var produtosDTO = _mapper.Map<List<ProdutoDTO>>(produtos);
        return Ok(produtosDTO);
    }

    [HttpGet("{id:int}", Name = "ObterProdutoId")]
    public ActionResult<ProdutoDTO> Get(int id)
    {
        var produto = _uof.produtoRepository.GetById(p => p.ProdutoId == id);
        if (produto is null)
        {
            return NotFound("Produto não encontrado");
        }

        var produtoDTO = _mapper.Map<ProdutoDTO>(produto);
        return Ok(produtoDTO);
    }

    
    [HttpPost]
    public ActionResult Post(ProdutoDTO produtoDto)
    {
        var produto = _mapper.Map<Produto>(produtoDto);
        _uof.produtoRepository.Add(produto);
        _uof.Commit();

        var produtoDTO = _mapper.Map<ProdutoDTO>(produto);
        return new CreatedAtRouteResult("ObterProdutoId", new { id = produto.ProdutoId }, produtoDTO);

    }


    [HttpPut("{id:int}")]
    public ActionResult Put(int id, ProdutoDTO produtoDto)
    {
        if (id != produtoDto.ProdutoId)
        {
            return BadRequest();
        }
        var produto = _mapper.Map<Produto>(produtoDto);

        _uof.produtoRepository.Update(produto);
        _uof.Commit();
        return Ok();
    }

    [HttpDelete("{id:int}")]
    public ActionResult<ProdutoDTO> Delete(int id)
    {
        var produto = _uof.produtoRepository.GetById(p => p.ProdutoId == id);
        if (produto is null)
            return NotFound();

        _uof.produtoRepository.Delete(produto);
        _uof.Commit();
        var produtoDto = _mapper.Map<ProdutoDTO>(produto);
        return Ok(produtoDto);

    }

}
