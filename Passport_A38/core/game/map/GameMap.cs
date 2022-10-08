﻿using Passport_A38.core.game.controller;
using Passport_A38.core.game.gameobject;

namespace Passport_A38.core.game.map;

public class GameMap
{
    private char[,] _tiles = new char[19, 35];
    private List<Form> _forms = new();
    private Dictionary<int, string> _colours = new()
    {
        {0,"red"},
        {1,"green"},
        {2,"pink"},
        {3,"blue"},
        {4,"brown"},
        {5,"black"},
        {6,"white"}
    };

    public GameMap(string inputTiles)
    {
        for (int k = 0, t = 0; k < _tiles.GetLength(0); k++)
        {
            for (int i = 0; i < _tiles.GetLength(1); i++, t++)
            {
                _tiles[k, i] = inputTiles[t];
            }
        }

        var random = new Random();    //TODO: Add seed
        _forms.Add(new Form(0,"red","0:a"));    //First counter is always the first one you need to go to
        for (int i = 1; i < 3; i++)
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

        var counter = 7-(player.Pos.Y-4)/2+":"; //TODO: de-hard-code this

        if (player.Pos.X <(double)_tiles.GetLength(1)/2)
        {
            return counter + "a";
        }
        return counter + "b";

    }

    /*
     * Returns whether the player is standing next to a counter or not.
     */
    private bool PlayerAtCounter(Player player)
    {
        return _tiles[(int) player.Pos.Y, (int) player.Pos.X - 1] == ']' ||
               _tiles[(int) player.Pos.Y, (int) player.Pos.X + 1] == '[';
    }

    public char[,] Tiles
    {
        get => _tiles;
        set => _tiles=value;
    }

    public List<Form> Forms 
    {
        get => _forms;
    }
}