using System.Xml;
using Passport_A38.core.game.gameobject;
using Passport_A38.core.game.utility;

namespace Passport_A38.core.game.map;

public class GameMap 
{
    private readonly char[,] _tiles;
    private readonly List<Form> _forms = new();
    private readonly Dictionary<int, string> _colours = new()
    {
        {0,"red"},
        {1,"green"},
        {2,"pink"},
        {3,"blue"},
        {4,"brown"},
        {5,"black"},
        {6,"white"}
    };

    public static Dictionary<string, Difficulty> DifficultyDictionary = new()
    {
        {"easy",Difficulty.easy},
        {"normal",Difficulty.normal},
        { "hard" ,Difficulty.hard},
        { "madhouse" ,Difficulty.madhouse}
    };

    public int Seed { get; set; }
    public static int Width;
    public static int Height;

    public GameMap(string inputTiles, int width)
    {
        Width = width;
        Height = inputTiles.Length / width;
        _tiles = new char[Height, width];
        
        Seed = 0;
        for (int k = 0, t = 0; k < _tiles.GetLength(0); k++)
        {
            for (var i = 0; i < _tiles.GetLength(1); i++, t++)
            {
                _tiles[k, i] = inputTiles[t];
            }
        }

        var random = new Random(Seed);
        var randNum = random.Next(3,21);
        _forms.Add(new Form(0,"red","0:a"));    //first counter is always the first one you need to go to
        for (var i = 1; i < 2; i++)
        {
            Form form = new();
            var temp = random.Next(1,8)+":"+(random.Next(0,2)==0? "a":"b");    //from first to most upper floor left/right
            while (_forms[i - 1].Counter.Equals(temp))
            {
                temp = random.Next(1,8)+":"+(random.Next(0,2)==0? "a":"b");
            }

            form.Counter = temp;
            form.Colour = _colours[random.Next(0,7)];
            form.Number = i;

            _forms.Add(form);
        }
    }

    /*
     * Gets the counter the player is standing at.
     * Output looks like 0:a for the floor of the counter the player is at and if it's the left or the right counter 
     */
    public string? GetCounter(Player player)
    {
        if (!PlayerAtCounter(player))
            return null;

        // ReSharper disable once PossibleLossOfFraction
        var counter = (CounterCount()- 1)/2 - (player.Pos.Y - 4) / 2 + ":";

        if (player.Pos.X <(double)_tiles.GetLength(1)/2)
        {
            return counter + "a";
        }
        return counter + "b";

    }

    /*
     * Returns the number of counters
     */
    private int CounterCount()
    {
        int num = 0;
        foreach (var character in _tiles)
        {
            if (character.Equals('°'))  //"head" of counter. Side doesn't matter
            {
                num++;
            }
        }

        return num;
    }

    /*
     * Returns whether the player is standing next to a counter or not.
     */
    private bool PlayerAtCounter(GameObject player)
    {
        return _tiles[(int) player.Pos.Y, (int) player.Pos.X - 1] == ']' ||
               _tiles[(int) player.Pos.Y, (int) player.Pos.X + 1] == '[';
    }

    public void Restart(int seed, Difficulty difficulty,string inputTiles)  //TODO: Fix
    {
        Seed = seed;
        for (int k = 0, t = 0; k < _tiles.GetLength(0); k++)
        {
            for (var i = 0; i < _tiles.GetLength(1); i++, t++)
            {
                _tiles[k, i] = inputTiles[t];
            }
        }

        var random = new Random(Seed);
        var randNum = random.Next(3,21);
        _forms.Add(new Form(0,"red","0:a"));    //first counter is always the first one you need to go to
        for (var i = 1; i < 2; i++)
        {
            Form form = new();
            var temp = random.Next(1,8)+":"+(random.Next(0,2)==0? "a":"b");    //from first to most upper floor left/right
            while (_forms[i - 1].Counter.Equals(temp))
            {
                temp = random.Next(1,8)+":"+(random.Next(0,2)==0? "a":"b");
            }

            form.Counter = temp;
            form.Colour = _colours[random.Next(0,7)];
            form.Number = i;

            _forms.Add(form);
        }
    }

    public char[,] Tiles => _tiles;

    public List<Form> Forms => _forms;
}