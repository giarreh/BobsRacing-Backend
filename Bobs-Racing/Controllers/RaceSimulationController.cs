﻿using Bobs_Racing.Hubs;
using Bobs_Racing.Interface;
using Bobs_Racing.Services;
using Microsoft.AspNetCore.Mvc;
using Bobs_Racing.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Bobs_Racing.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RaceSimulationController : ControllerBase
    {
        private readonly RaceSimulationService _simulationService;
        private readonly RaceSimulationHub _raceSimulationHub;
        private readonly IAthleteRepository _athleteRepository;
        private readonly List<Runner> _runners;

        public RaceSimulationController(RaceSimulationService simulationService, IAthleteRepository athleteRepository,
            RaceSimulationHub raceSimulationHub)
        {
            _simulationService = simulationService;
            _athleteRepository = athleteRepository;
            _raceSimulationHub = raceSimulationHub;
         /*   _runners = new List<Runner>
        {
            new Runner { Name = "Runner 1", Speed = 0, Position = 0, FastestTime = 9.58, LowestTime = 11.0},
            new Runner { Name = "Runner 2", Speed = 0, Position = 0, FastestTime = 10.00, LowestTime = 10.07}
            // Add more runners if needed
        };*/
        }

        [HttpPost("start")]
        public async Task<IActionResult> StartRace([FromBody] List<int> athleteIds, CancellationToken cancellationToken)
        {

            if (athleteIds == null)
            {
                return NotFound("Athlete not found");
            }
            var athletes = await _athleteRepository.GetAthletesByIdsAsync(athleteIds);
            
            var runners = athletes.Select(a => new Runner
            {
                Name = a.Name,
                Speed = 0.0,
                Position = 0.0,
                SlowestTime = a.SlowestTime,
                FastestTime = a.FastestTime
            }).ToList();

            _simulationService.SetRunners(runners);

            await _simulationService.StartRace(cancellationToken);

            
                //var cts = new CancellationTokenSource();
                //var task = _simulationService.StartRace(cts.Token);

                return Ok(new { message = "Race started!" });
            
        }
        }

}