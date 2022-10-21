namespace Passport_A38.core.game.utility;

public class Stats
{
    private readonly DateTime _startTime;
    
    public Dictionary<int, object> GetAt => new()
        {
            {-1,-1},
            {1, TimedScore},
            {2, Difficulty},
            {3, Seed}
        };

    public Stats()
    {
        _startTime = DateTime.UtcNow;
    }

    public int Score { get; set; } = 0; //number of collected forms
    public int TimedScore => Score/(_startTime.Subtract(DateTime.UtcNow).Minutes==0? 1 : _startTime.Subtract(DateTime.UtcNow).Minutes); //collected forms / Minutes passed
    public int Seed { get; set; } = 0;
    public Difficulty Difficulty { get; set; } = Difficulty.easy;
}