using System;
using System.Collections.Generic;

namespace WizardScorer.Models;

public partial class Game
{
    public int GameId { get; set; }

    public int Date { get; set; }

    public virtual ICollection<Score> Scores { get; set; } = new List<Score>();
}
