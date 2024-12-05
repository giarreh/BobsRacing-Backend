﻿using Bobs_Racing.Interface;
using Bobs_Racing.Models;
using Bobs_Racing.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Bobs_Racing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RaceAnimalController : ControllerBase
    {
        private readonly IRaceAnimalRepository _raceAnimalRepository;

        public RaceAnimalController(IRaceAnimalRepository raceAnimalRepository)
        {
            _raceAnimalRepository = raceAnimalRepository;
        }

        // GET: api/RaceAnimal
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RaceAnimal>>> GetAllRaceAnimals()
        {
            var raceAnimals = await _raceAnimalRepository.GetAllRaceAnimalAsync();
            if (raceAnimals == null)
            {
                return NotFound();
            }
            return Ok(raceAnimals);
        }

        // GET: api/RaceAnimal/{animalId}/{raceId}
        [HttpGet("{id}")]
        public async Task<ActionResult<RaceAnimal>> GetRaceAnimalById(int id)
        {
            var raceAnimal = await _raceAnimalRepository.GetRaceAnimalByIdAsync(id);
            if (raceAnimal == null)
            {
                return NotFound();
            }
            return Ok(raceAnimal);
        }

        // POST: api/RaceAnimal
        [HttpPost]
        public async Task<ActionResult> AddRaceAnimal([FromBody] RaceAnimal raceAnimal)
        {
            if (raceAnimal == null)
            {
                return BadRequest("Invalid data.");
            }

            var isValidAnimal = await _raceAnimalRepository.ValidateAnimalAsync(raceAnimal.AnimalId);
            var isValidRace = await _raceAnimalRepository.ValidateRaceAsync(raceAnimal.RaceId);

            if (!isValidAnimal)
            {
                return BadRequest($"Animal with ID {raceAnimal.Animal.AnimalId} is not valid.");
            }

            if (!isValidRace)
            {
                return BadRequest($"Race with ID {raceAnimal.Race.RaceId} is not valid.");
            }

            await _raceAnimalRepository.AddRaceAnimalAsync(raceAnimal);
            return CreatedAtAction(nameof(GetRaceAnimalById), new { id = raceAnimal.RaceAnimalId }, raceAnimal);
        }

        // PUT: api/RaceAnimal
        [HttpPut ("{id}")]
        public async Task<ActionResult> UpdateRaceAnimal(int id, [FromBody] RaceAnimal raceAnimal)
        {
            var exisitngRaceAnimal = await _raceAnimalRepository.GetRaceAnimalByIdAsync(id);

            var isValidAnimal = await _raceAnimalRepository.ValidateAnimalAsync(raceAnimal.Animal.AnimalId);
            var isValidRace = await _raceAnimalRepository.ValidateRaceAsync(raceAnimal.Race.RaceId);

            if (!isValidAnimal)
            {
                return BadRequest($"Animal with ID {raceAnimal.Animal.AnimalId} is not valid.");
            }

            if (!isValidRace)
            {
                return BadRequest($"Race with ID {raceAnimal.Race.RaceId} is not valid.");
            }

            exisitngRaceAnimal.Race.RaceId = raceAnimal.Race.RaceId;
            exisitngRaceAnimal.Animal.AnimalId= raceAnimal.Animal.AnimalId;

            await _raceAnimalRepository.UpdateRaceAnimalAsync(exisitngRaceAnimal);
            return NoContent();
        }

        // DELETE: api/RaceAnimal/{animalId}/{raceId}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteRaceAnimal(int id)
        {

            var raceAnimal = await _raceAnimalRepository.GetRaceAnimalByIdAsync(id);
            if (raceAnimal == null)
            {
                return NotFound("RaceAnimal not found");
            }

            await _raceAnimalRepository.DeleteRaceAnimalAsync(id);
            return NoContent();
        }
    }
}