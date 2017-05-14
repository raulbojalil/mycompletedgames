﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyCompletedGames.Models
{
    public class Platform
    {
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        public int Type { get; set; } 

        public string Image { get; set; }

    }
}