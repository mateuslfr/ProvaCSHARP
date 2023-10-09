using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Prova.Data; 
using Prova.Models;

namespace Prova.Controllers
{
    [Route("prova/folha")]
    [ApiController]
    public class FolhaController : ControllerBase
    {
        private readonly AppDataContext _context;

        public FolhaController(AppDataContext context)
        {
            _context = context;
        }

        
        [HttpGet]
        [Route("listar")]
        public ActionResult<IEnumerable<Folha>> Listar()
        {
            return _context.Folhas.Include(f => f.Funcionario).ToList();
        }

        
   [HttpGet]
[Route("listar/{cpf}/{mes}/{ano}")]
public ActionResult<IEnumerable<Folha>> Listar([FromRoute] string cpf, [FromRoute] int mes, [FromRoute] int ano)
{
    var folhas = _context.Folhas.Include(f => f.Funcionario)
                                .Where(f => f.Funcionario.Cpf == cpf && f.Mes == mes && f.Ano == ano)
                                .ToList();

    if (folhas == null || folhas.Count == 0)
    {
        return NotFound();
    }

    return folhas;
}


        
[HttpPost]
[Route("cadastrar")]
public IActionResult CadastrarFolha([FromBody] Folha folha)
{
    try
    {
        Funcionario funcionario = _context.Funcionarios.Find(folha.FuncionarioId);
        if (funcionario == null)
        {
            return NotFound("Funcionário não encontrado");
        }

        folha.Funcionario = funcionario;

    
        folha.SalarioBruto = folha.Valor * folha.Quantidade;
        
        
        if (folha.SalarioBruto <= 1903.98)
        {
            folha.ImpostoIrrf = 0;
        }
        else if (folha.SalarioBruto <= 2826.65)
        {
            folha.ImpostoIrrf = (folha.SalarioBruto * 0.075) - 142.80;
        }
        else if (folha.SalarioBruto <= 3751.05)
        {
            folha.ImpostoIrrf = (folha.SalarioBruto * 0.15) - 354.80;
        }
        else if (folha.SalarioBruto <= 4664.68)
        {
            folha.ImpostoIrrf = (folha.SalarioBruto * 0.225) - 636.13;
        }
        else
        {
            folha.ImpostoIrrf = (folha.SalarioBruto * 0.275) - 869.36;
        }


        if (folha.SalarioBruto <= 1693.72)
        {
            folha.ImpostoInss = folha.SalarioBruto * 0.08;
        }
        else if (folha.SalarioBruto <= 2822.90)
        {
            folha.ImpostoInss = folha.SalarioBruto * 0.09;
        }
        else if (folha.SalarioBruto <= 5645.80)
        {
            folha.ImpostoInss = folha.SalarioBruto * 0.11;
        }
        else
        {
            folha.ImpostoInss = 621.03; 
        }

        folha.ImpostoFgts = folha.SalarioBruto * 0.08;

        folha.SalarioLiquido = folha.SalarioBruto - folha.ImpostoIrrf - folha.ImpostoInss;

        _context.Folhas.Add(folha);
        _context.SaveChanges();

        return CreatedAtAction(nameof(Listar), new { id = folha.Id }, folha);
    }
    catch (Exception e)
    {
        return BadRequest(e.Message);
    }
}


        
        [HttpPut]
        [Route("editar/{id}")]
        public IActionResult Editar(int id, [FromBody] Folha folha)
{
    if (id != folha.Id)
    {
        return BadRequest();
    }

    _context.Entry(folha).State = EntityState.Modified;

    try
    {
        _context.SaveChanges();
        return Ok(new { message = "Folha editada com sucesso." });
    }
    catch (DbUpdateConcurrencyException)
    {
        if (!FolhaExists(id))
        {
            return NotFound();
        }
        else
        {
            throw;
        }
    }
}

        
        [HttpDelete]
        [Route("deletar{id}/")]
        public IActionResult Deletar(int id)
{
    var folha = _context.Folhas.Find(id);
    if (folha == null)
    {
        return NotFound();
    }

    _context.Folhas.Remove(folha);
    _context.SaveChanges();

    return Ok(new { message = "Folha deletada com sucesso." });
}

private bool FolhaExists(int id)
{
    return _context.Folhas.Any(f => f.Id == id);
}
}
}
