using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Prova.Data; 
using Prova.Models;

namespace Prova.Controllers
{
    [Route("prova/funcionario")]
    [ApiController]
    public class FuncionarioController : ControllerBase
    {
        private readonly AppDataContext _context;

        public FuncionarioController(AppDataContext context)
        {
            _context = context;
        }

        
        [HttpGet]
        [Route("listar")]
        public ActionResult<IEnumerable<Funcionario>> GetFuncionarios()
        {
            return _context.Funcionarios.ToList();
        }

        
        [HttpGet]
        [Route("listar/{id}")]
       public ActionResult<Funcionario> GetFuncionario(int id)
{
    var funcionario = _context.Funcionarios.Find(id);

    if (funcionario == null)
    {
        return NotFound();
    }

    return Ok(funcionario);
}

        
        [HttpPost]
        [Route("cadastrar")]
        public IActionResult CadastrarFuncionario([FromBody] Funcionario funcionario)
        {
            try
            {
                _context.Funcionarios.Add(funcionario);
                _context.SaveChanges();

                return CreatedAtAction(nameof(GetFuncionario), new { id = funcionario.Id }, funcionario);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        
        [HttpPut]
[Route("editar/{id}")]
public IActionResult UpdateFuncionario(int id, [FromBody] Funcionario funcionario)
{
    if (id != funcionario.Id)
    {
        return BadRequest();
    }

    _context.Entry(funcionario).State = EntityState.Modified;

    try
    {
        _context.SaveChanges();
        return Ok(new { message = "Funcionário atualizado com sucesso." });
    }
    catch (DbUpdateConcurrencyException)
    {
        if (!FuncionarioExists(id))
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
        [Route("deletar/{id}")]
        public IActionResult DeleteFuncionario(int id)
{
    var funcionario = _context.Funcionarios.Find(id);
    if (funcionario == null)
    {
        return NotFound();
    }

    _context.Funcionarios.Remove(funcionario);
    _context.SaveChanges();

    return Ok(new { message = "Funcionário deletado com sucesso." });
}

private bool FuncionarioExists(int id)
{
    return _context.Funcionarios.Any(f => f.Id == id);
}
    }
}
