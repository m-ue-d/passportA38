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
            
            switch (key)
            {
                case ConsoleKey.LeftArrow:
                {
                    if ()
                    {
                        
                    }
                    else{
                        player.Pos = Vector2.Add(player.Pos, new Vector2(-1, 0));
                    }
                    Updater.Update = true;
                    break;
                }
                case ConsoleKey.RightArrow:
                {
                    player.Pos = Vector2.Add(player.Pos, new Vector2(1, 0));
                    Updater.Update = true;
                    break;
                }
                case ConsoleKey.UpArrow:
                {
                    player.Pos = Vector2.Add(player.Pos, new Vector2(0, -1));
                    Updater.Update = true;
                    break;
                }
                case ConsoleKey.DownArrow:
                {
                    player.Pos = Vector2.Add(player.Pos, new Vector2(0, 1));
                    Updater.Update = true;
                    break;
                }
                case ConsoleKey.E:
                {
                    var counter = map.GetCounter(player);
                    if (Gui.Screen.Equals(Screen.Game) && counter!=null)
                    {
                        Gui.DrawCounterScreen(counter);
                        Updater.Update = true;
                    }
                    break;
                }
            }
            Thread.Sleep(100);
        }while (key != ConsoleKey.X || Gui.Screen.Equals(Screen.Counter));
        
        Updater.Active = false;
    }
}