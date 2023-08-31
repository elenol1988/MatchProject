using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Shared
{
    public class MatchOdds
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int MatchId { get; set; }
        public string Specifier { get; set; }
        public double Odd { get; set; }

        public Match Match { get; set; }
    }
}
