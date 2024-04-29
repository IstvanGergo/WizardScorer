using System;
using System.Collections.Generic;

namespace WizardScorer.Models;

public partial class Score
{
    public int GamePlayerId { get; set; }

    public int RoundNumber { get; set; }

    public int Prediction { get; set; }

    public int Tricks { get; set; }

    public int Score1 { get; set; }

    public virtual PlayersGame GamePlayer { get; set; } = null!;
}
