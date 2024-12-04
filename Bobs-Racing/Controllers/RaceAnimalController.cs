﻿using Bobs_Racing.Interface;
using Bobs_Racing.Models;
using Bobs_Racing.Models.Input;
using Bobs_Racing.Services;
using Microsoft.AspNetCore.Mvc;

namespace Bobs_Racing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RaceAnimalController : ControllerBase
    {
        private readonly IRaceAnimalRepository _raceAnimalRepository;
        private readonly IRaceService _raceService;
        private readonly IAnimalRepository _animalRepository;

        public RaceAnimalController(IRaceAnimalRepository raceAnimalRepository)
        {
            _raceAnimalRepository = raceAnimalRepository;
        }
        public RaceAnimalController(IRaceService raceService)
        {
            _raceService = raceService;
        }

        [HttpPost("process-race")]
        public async Task<IActionResult> ProcessRace([FromBody] RaceAnimalInputModel raceInput)
        {
            if (raceInput.AnimalIds == null || !raceInput.AnimalIds.Any())
                return BadRequest("Animal IDs cannot be empty.");

            // Fetch animals by IDs
            var animals = await _animalRepository.GetAnimalsByIdsAsync(raceInput.AnimalIds);

            if (!animals.Any())
                return NotFound("No animals found for the given IDs.");

            // Process the race
            var raceResults = await _raceService.ProcessRaceAsync(raceInput.RaceId, animals);
            return Ok(raceResults);
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
        [HttpGet("{animalId}/{raceId}")]
        public async Task<ActionResult<RaceAnimal>> GetBetById(int animalId, int raceId)
        {
            var raceAnimal = await _raceAnimalRepository.GetBetByIdAsync(animalId, raceId);
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
                return BadRequest($"Animal with ID {raceAnimal.AnimalId} is not valid.");
            }

            if (!isValidRace)
            {
                return BadRequest($"Race with ID {raceAnimal.RaceId} is not valid.");
            }

            await _raceAnimalRepository.AddRaceAnimalAsync(raceAnimal);
            return CreatedAtAction(nameof(GetBetById), new { animalId = raceAnimal.AnimalId, raceId = raceAnimal.RaceId }, raceAnimal);
        }

        // PUT: api/RaceAnimal
        [HttpPut]
        public async Task<ActionResult> UpdateRaceAnimal([FromBody] RaceAnimal raceAnimal)
        {
            if (raceAnimal == null)
            {
                return BadRequest("Invalid data.");
            }

            var isValidAnimal = await _raceAnimalRepository.ValidateAnimalAsync(raceAnimal.AnimalId);
            var isValidRace = await _raceAnimalRepository.ValidateRaceAsync(raceAnimal.RaceId);

            if (!isValidAnimal)
            {
                return BadRequest($"Animal with ID {raceAnimal.AnimalId} is not valid.");
            }

            if (!isValidRace)
            {
                return BadRequest($"Race with ID {raceAnimal.RaceId} is not valid.");
            }

            await _raceAnimalRepository.UpdateRaceAnimalAsync(raceAnimal);
            return NoContent();
        }

        // DELETE: api/RaceAnimal/{animalId}/{raceId}
        [HttpDelete("{animalId}/{raceId}")]
        public async Task<ActionResult> DeleteRaceAnimal(int animalId, int raceId)
        {
            var isValidAnimal = await _raceAnimalRepository.ValidateAnimalAsync(animalId);
            var isValidRace = await _raceAnimalRepository.ValidateRaceAsync(raceId);

            if (!isValidAnimal)
            {
                return BadRequest($"Animal with ID {animalId} is not valid.");
            }

            if (!isValidRace)
            {
                return BadRequest($"Race with ID {raceId} is not valid.");
            }

            await _raceAnimalRepository.DeleteRaceAnimalAsync(animalId, raceId);
            return NoContent();
        }
    }
}
