﻿using System;
using System.Collections.Generic;

namespace Bobs_Racing.Models
{
    public class Race
    {
        public int RaceId { get; set; }
        public List<RaceAnimal> RaceAnimals { get; set; } = new List<RaceAnimal>();
        public DateTime Date { get; set; }
        public List<int> Rankings { get; set; } = new List<int>(); // Assuming Rankings is a list of Animal IDs
    }
}
