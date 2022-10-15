using System.Numerics;
using Passport_A38.core.game.gameobject;
using Passport_A38.core.game.gui;
using Passport_A38.core.game.map;

namespace Passport_A38.core.game.controller;

public class PlayerInputHandler
{

    private readonly Player _player;
    private readonly GameMap _map;
    
    public PlayerInputHandler(Player player, GameMap map)
    {
        _player = player;
        _map = map;
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

            switch (Gui.Screen)
            {
                case Screen.Game: 
                {
                    switch (key)
                    {
                        case ConsoleKey.LeftArrow:
                        {
                            if (_map.Tiles[(int) _player.Pos.Y, (int) _player.Pos.X - 1] is not '_' and not '^' and not 'v'
                                and not 'X')
                                break;
                            _player.Pos = Vector2.Add(_player.Pos, new Vector2(-1, 0));
                            Updater.Update = true;
                            break;
                        }
                        case ConsoleKey.RightArrow:
                        {
                            if (_map.Tiles[(int) _player.Pos.Y, (int) _player.Pos.X + 1] is not '_' and not '^' and not 'v'
                                and not 'X')
                                break;
                            _player.Pos = Vector2.Add(_player.Pos, new Vector2(1, 0));
                            Updater.Update = true;
                            break;
                        }
                        case ConsoleKey.UpArrow:
                        {
                            if (_map.Tiles[(int) _player.Pos.Y, (int) _player.Pos.X] is not '^' and not 'X')
                                break;
                            _player.Pos = Vector2.Add(_player.Pos, new Vector2(0, -2));
                            Updater.Update = true;
                            break;
                        }
                        case ConsoleKey.DownArrow:
                        {
                            if (_map.Tiles[(int) _player.Pos.Y, (int) _player.Pos.X] is not 'v' and not 'X')
                                break;
                            _player.Pos = Vector2.Add(_player.Pos, new Vector2(0, 2));
                            Updater.Update = true;
                            break;
                        }
                        case ConsoleKey.E:
                        {
                            var counter = _map.GetCounter(_player);
                            if (counter != null)
                            {
                                SoundController.StartBackgroundMusic(Screen.Counter);
                                Gui.Screen = Screen.Counter;
                                _player.Counter = counter;
                                Updater.Update = true;
                            }

                            break;
                        }
                    }

                    break;
                }
                case Screen.Counter:
                {
                    switch (key)
                    {
                        case ConsoleKey.F:
                        {

                            _player.InteractionState += 1; //change interaction state
                            if (_player.Needed.Counter.Equals(_player.Counter))
                            {


                                if (_player.InteractionState == 2) //interaction done
                                {
                                    if (_player.Next.Number == -1) //end of round
                                    {
                                        Updater.Win = true;
                                        Updater.Active = false;
                                        break;
                                    }

                                    _player.Score += 1; //change score

                                    if (_player.Score == _map.Forms.Count) //player is at the last needed counter
                                    {
                                        _player.Next = new Form(-1, _map.Forms[1].Colour, _map.Forms[1].Counter);
                                        _player.Needed = _map.Forms[0];
                                    }
                                    else if (_player.Score < _map.Forms.Count)
                                    {
                                        _player.Needed = _map.Forms[_player.Score];
                                        if (_player.Score + 1 < _map.Forms.Count)
                                        {
                                            _player.Next = _map.Forms[_player.Score + 1];
                                        }
                                        else
                                        {
                                            _player.Next = new Form(_map.Forms.Count, "black", "0:a");
                                        }
                                    }
                                }
                            }

                            Updater.Update = true;
                            break;
                        }
                        case ConsoleKey.E:
                        {
                            _player.InteractionState = 0; //reset InteractionState
                            SoundController.StartBackgroundMusic(Screen.Game);
                            Gui.Screen = Screen.Game;
                            Updater.Update = true;
                            break;
                        }
                    }

                    break;
                }
                case Screen.Start:
                {
                    switch (key)
                    {
                        case ConsoleKey.E:
                        {
                            SoundController.StartBackgroundMusic(Screen.Game);
                            Gui.Screen = Screen.Game;
                            Updater.Update = true;
                            break;
                        }
                    }

                    break;
                }
            }

            Thread.Sleep(100);
        }while (key != ConsoleKey.X);
        
        Updater.Active = false;
    }
}
