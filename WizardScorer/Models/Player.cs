using System;
using System.Collections.Generic;

namespace WizardScorer.Models;

public partial class Player
{
    public int PlayerId { get; set; }

    public string PlayerName { get; set; } = null!;

    public virtual ICollection<PlayersGame> PlayersGames { get; set; } = new List<PlayersGame>();
}
