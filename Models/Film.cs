using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Sakila.Models
{
    public class Film
    {
        public int FilmId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string ReleaseYear { get; set; }
        public List<FilmActor> FilmActors { get; set; }

    }
}
