using System;
using System.Collections.Generic;

namespace WizardScorer.Models;

public partial class Score
{
    public int ScoreId { get; set; }

    public int GameId { get; set; }

    public int PlayerId { get; set; }

    public int RoundNumber { get; set; }

    public int Prediction { get; set; }

    public int Tricks { get; set; }

    public int Score1 { get; set; }

    public virtual Game Game { get; set; } = null!;

    public virtual Player Player { get; set; } = null!;
}
