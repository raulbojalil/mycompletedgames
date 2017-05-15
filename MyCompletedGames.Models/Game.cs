using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCompletedGames.Models
{
    /// <summary>
    /// Represents a completed game. A platform can have 0..n completed games.
    /// </summary>
    public class Game
    {
        /// <summary>
        /// The ID of the Game
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// The ID of the Platform
        /// </summary>
        [Required]
        public int PlatformId { get; set; }

        /// <summary>
        /// The name of the game, for example Doom, Pong, etc.
        /// </summary>
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        /// <summary>
        /// Completion date
        /// </summary>
        [Required]
        [MaxLength(255)]
        public string Completed { get; set; }

    }
}
