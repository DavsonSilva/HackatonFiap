﻿using Hackaton.Domain.Requests.Agenda;
using Hackaton.Domain.Responses;
using Hackaton.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace Hackaton.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AgendaController : ControllerBase
    {
        private readonly IAgendaService _agendaService;

        public AgendaController(IAgendaService agendaService)
        {
            _agendaService = agendaService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AgendaResponse>>> GetAll()
        {
            var agendas = await _agendaService.GetAllAsync();
            return Ok(agendas);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AgendaResponse>> GetById(int id)
        {
            var agenda = await _agendaService.GetByIdAsync(id);
            if (agenda == null)
                return NotFound();

            return Ok(agenda);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreateAgendaRequest request)
        {
            await _agendaService.AddAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = request.MedicoId }, request);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] UpdateAgendaRequest request)
        {
            if (id != request.Id)
                return BadRequest();

            await _agendaService.UpdateAsync(request);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _agendaService.DeleteAsync(id);
            return NoContent();
        }
    }
}
