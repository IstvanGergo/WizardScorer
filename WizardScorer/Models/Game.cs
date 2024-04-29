using System;
using System.Collections.Generic;

namespace WizardScorer.Models;

public partial class Game
{
    public int GameId { get; set; }

    public string Date { get; set; } = null!;

    public virtual ICollection<PlayersGame> PlayersGames { get; set; } = new List<PlayersGame>();
}
