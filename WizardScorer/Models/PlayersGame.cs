using System;
using System.Collections.Generic;

namespace WizardScorer.Models;

public partial class PlayersGame
{
    public int GamePlayerId { get; set; }

    public int PlayerId { get; set; }

    public int GameId { get; set; }

    public virtual Game Game { get; set; } = null!;

    public virtual Player Player { get; set; } = null!;

    public virtual Score? Score { get; set; }
}
