using Passport_A38.core.game.controller;
using Passport_A38.core.game.gameobject;

namespace Passport_A38.core.game.map;

public class GameMap
{
    private char[,] _tiles = new char[19, 35];

    public GameMap(string inputTiles)
    {
        for (int k = 0, t = 0; k < _tiles.GetLength(0); k++)
        {
            for (int i = 0; i < _tiles.GetLength(1); i++, t++)
            {
                _tiles[k, i] = inputTiles[t];
            }
        }
    }

    /*
     * Gets the counter the player is standing at.
     * Output looks like 4:a for the y level the player is at and if it's the first or the second counter 
     */
    public string? GetCounter(Player player)
    {
        if (!PlayerAtCounter(player))
            return null;

        var counter = player.Pos.Y+":";
        
        

    }

    /*
     * Returns if the player is standing next to a counter.
     */
    private bool PlayerAtCounter(Player player)
    {
        return _tiles[(int) player.Pos.Y, (int) player.Pos.X - 1] == ']' ||
               _tiles[(int) player.Pos.Y, (int) player.Pos.X + 1] == '[';
    }

    public char[,] Tiles{
        get => _tiles;
        set => _tiles=value;
    }
}