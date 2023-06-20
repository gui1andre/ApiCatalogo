using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiCatalogo.Context;
using ApiCatalogo.DTOs;
using ApiCatalogo.Models;
using ApiCatalogo.Pagination;
using ApiCatalogo.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
    public async Task<ActionResult<IEnumerable<ProdutoDTO>>> GetProdutosPreco()
    {
        var produtos = await _uof.produtoRepository.GetProdutosPorPreco();
        var produtoDTO = _mapper.Map<List<ProdutoDTO>>(produtos);
        return Ok(produtoDTO);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProdutoDTO>>> Get([FromQuery] ProdutosParameters produtosParameters)
    {
        var produtos = await _uof.produtoRepository.GetProdutos(produtosParameters);
        if (produtos is null)
        {
            return NotFound("Não foram encontrados produtos");
        }

        var metadata = new
        {
            produtos.TotalCount,
            produtos.PageSize,
            produtos.Currentpage,
            produtos.TotalPages,
            produtos.HasNext,
            produtos.HasPrevious
        };

        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
        var produtosDTO = _mapper.Map<List<ProdutoDTO>>(produtos);
        return Ok(produtosDTO);
    }

    [HttpGet("{id:int}", Name = "ObterProdutoId")]
    public async Task<ActionResult<ProdutoDTO>> Get(int id)
    {
        var produto = await _uof.produtoRepository.GetById(p => p.ProdutoId == id);
        if (produto is null)
        {
            return NotFound("Produto não encontrado");
        }

        var produtoDTO = _mapper.Map<ProdutoDTO>(produto);
        return Ok(produtoDTO);
    }

    
    [HttpPost]
    public async Task<ActionResult> Post(ProdutoDTO produtoDto)
    {
        var produto = _mapper.Map<Produto>(produtoDto);
        _uof.produtoRepository.Add(produto);
        await _uof.Commit();

        var produtoDTO = _mapper.Map<ProdutoDTO>(produto);
        return new CreatedAtRouteResult("ObterProdutoId", new { id = produto.ProdutoId }, produtoDTO);

    }


    [HttpPut("{id:int}")]
    public async Task<ActionResult> Put(int id, ProdutoDTO produtoDto)
    {
        if (id != produtoDto.ProdutoId)
        {
            return BadRequest();
        }
        var produto = _mapper.Map<Produto>(produtoDto);

        _uof.produtoRepository.Update(produto);
        await _uof.Commit();
        return Ok();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ProdutoDTO>> Delete(int id)
    {
        var produto = await _uof.produtoRepository.GetById(p => p.ProdutoId == id);
        if (produto is null)
            return NotFound();

        _uof.produtoRepository.Delete(produto);
        await _uof.Commit();
        var produtoDto = _mapper.Map<ProdutoDTO>(produto);
        return Ok(produtoDto);

    }

}
