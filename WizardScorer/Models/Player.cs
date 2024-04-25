using System;
using System.Collections.Generic;

namespace WizardScorer.Models;

public partial class Player
{
    public int PlayerId { get; set; }

    public string PlayerName { get; set; } = null!;

    public virtual ICollection<Score> Scores { get; set; } = new List<Score>();
}
