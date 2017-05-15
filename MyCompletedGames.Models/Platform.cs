using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCompletedGames.Models
{
    /// <summary>
    /// Represents a Game Platform, it can either be a Console, Handheld console or Computer/OS. 
    /// A platform can have 0..n completed games.
    /// Examples of Platforms include MS-DOS, Commodore 64, etc.
    /// Examples of Games include Doom, Pong, etc.
    /// </summary>
    public class Platform
    {
        /// <summary>
        /// The ID of the Platform
        /// </summary>
        public int ID { get; set;  }

        /// <summary>
        /// The name of the Platform.
        /// </summary>
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        /// <summary>
        /// The type of the Platform (0 = Console, 1 = Handheld, 2 = Computer/OS)
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// The URL of the Image (this URL retrieves the image data from the DB, this is NOT a local path, please see PlatformsController/GetImage)
        /// </summary>
        public string Image { get; set; }

    }
}
