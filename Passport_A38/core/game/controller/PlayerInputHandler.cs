using System.Numerics;
using Passport_A38.core.game.gameobject;
using Passport_A38.core.game.gui;
using Passport_A38.core.game.map;

namespace Passport_A38.core.game.controller;

public class PlayerInputHandler
{

    private Player player;
    private GameMap map;
    
    public PlayerInputHandler(Player player, GameMap map)
    {
        this.player = player;
        this.map = map;
    }

    public void ReadKeyData()
    {
        ConsoleKey key;

        do
        {
            while (Console.KeyAvailable)
            {
                var temp = Console.ReadKey(false).Key;
            }
            key = Console.ReadKey(false).Key;

            if (Gui.Screen.Equals(Screen.Game))
            {
                switch (key)
                {
                    case ConsoleKey.LeftArrow:
                    {
                        if (map.Tiles[(int) player.Pos.Y, (int) player.Pos.X - 1] is not '_' and not '^' and not 'v'
                            and not 'X')
                            break;
                        player.Pos = Vector2.Add(player.Pos, new Vector2(-1, 0));
                        Updater.Update = true;
                        break;
                    }
                    case ConsoleKey.RightArrow:
                    {
                        if (map.Tiles[(int) player.Pos.Y, (int) player.Pos.X + 1] is not '_' and not '^' and not 'v'
                            and not 'X')
                            break;
                        player.Pos = Vector2.Add(player.Pos, new Vector2(1, 0));
                        Updater.Update = true;
                        break;
                    }
                    case ConsoleKey.UpArrow:
                    {
                        if (map.Tiles[(int) player.Pos.Y, (int) player.Pos.X] is not '^' and not 'X')
                            break;
                        player.Pos = Vector2.Add(player.Pos, new Vector2(0, -2));
                        Updater.Update = true;
                        break;
                    }
                    case ConsoleKey.DownArrow:
                    {
                        if (map.Tiles[(int) player.Pos.Y, (int) player.Pos.X] is not 'v' and not 'X')
                            break;
                        player.Pos = Vector2.Add(player.Pos, new Vector2(0, 2));
                        Updater.Update = true;
                        break;
                    }
                    case ConsoleKey.E:
                    {
                        var counter = map.GetCounter(player);
                        if (counter != null)
                        {
                            Gui.Screen = Screen.Counter;
                            player.Counter = counter;
                            Updater.Update = true;
                        }

                        break;
                    }
                }
            }
            else if (Gui.Screen.Equals(Screen.Counter))
            {
                switch (key)
                {
                    case ConsoleKey.F:
                    {
                        if (player.Needed.Counter.Equals(player.Counter))
                        {
                            player.Score += 1;

                            if (player.Score<map.Forms.Count)
                            {
                                player.Needed = map.Forms[player.Score];
                            }
                            else
                            {
                                player.Needed = new Form(-1, "black", "i know what to do now >:]");
                                break;
                            }
                        }
                        break;
                    }
                    case ConsoleKey.Q:
                    {
                        if (player.Needed.Number == -1)
                        {
                            Updater.Win = true;
                            Updater.Active = false;
                        }
                        break;
                    }
                    case ConsoleKey.E:
                    {
                        Gui.Screen = Screen.Game;
                        Updater.Update = true;
                        break;
                    }
                }
            }

            Thread.Sleep(100);
        }while (key != ConsoleKey.X);
        
        Updater.Active = false;
    }
}