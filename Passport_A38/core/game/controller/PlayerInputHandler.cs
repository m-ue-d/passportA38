using System.Numerics;
using Passport_A38.core.game.gameobject;
using Passport_A38.core.game.gui;
using Passport_A38.core.game.map;
using Passport_A38.core.game.utility;

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
        do
        {
            while (Console.KeyAvailable)
            {
                var temp = Console.ReadKey(false).Key;
            }
            var key = Console.ReadKey(false).Key;

            switch (Gui.Screen)
            {
                case Screen.Game: 
                {
                    var oldPos = new Vector2(_player.Pos.X,_player.Pos.Y);
                    Gui.UpdateAt(_player.Pos, new Vector2(Console.CursorLeft, Console.CursorTop), _map.Tiles[(int)_player.Pos.Y, (int)_player.Pos.X+1]); //get rid of input chars at Cursor
                    Gui.UpdateAt(new Vector2(-1,0), new Vector2(0,0), ' '); //get rid of input chars at 0,0
                    switch (key)
                    {
                        case ConsoleKey.LeftArrow:
                        {
                            if (_map.Tiles[(int) _player.Pos.Y, (int) _player.Pos.X - 1] is not '_' and not '^'
                                and not 'v'
                                and not 'X')
                            {
                                Gui.UpdatePlayerPosition(_player, oldPos, _map.Tiles[(int) oldPos.Y, (int) oldPos.X]);
                                break;
                            }

                            _player.Pos = Vector2.Add(_player.Pos, new Vector2(-1, 0));
                            Gui.UpdatePlayerPosition(_player, oldPos, _map.Tiles[(int) oldPos.Y, (int) oldPos.X]);
                            break;
                        }
                        case ConsoleKey.RightArrow:
                        {
                            if (_map.Tiles[(int) _player.Pos.Y, (int) _player.Pos.X + 1] is not '_' and not '^'
                                and not 'v'
                                and not 'X')
                            {
                                Gui.UpdatePlayerPosition(_player, oldPos, _map.Tiles[(int) oldPos.Y, (int) oldPos.X]);
                                break;
                            }
                            
                            _player.Pos = Vector2.Add(_player.Pos, new Vector2(1, 0));
                            Gui.UpdatePlayerPosition(_player, oldPos, _map.Tiles[(int) oldPos.Y, (int) oldPos.X]);
                            break;
                        }
                        case ConsoleKey.UpArrow:
                        {
                            if (_map.Tiles[(int) _player.Pos.Y, (int) _player.Pos.X] is not '^' and not 'X')
                            {
                                Gui.UpdatePlayerPosition(_player, oldPos, _map.Tiles[(int) oldPos.Y, (int) oldPos.X]);
                                break;
                            }

                            _player.Pos = Vector2.Add(_player.Pos, new Vector2(0, -2));
                            Gui.UpdatePlayerPosition(_player, oldPos, _map.Tiles[(int) oldPos.Y, (int) oldPos.X]);
                            break;
                        }
                        case ConsoleKey.DownArrow:
                        {
                            if (_map.Tiles[(int) _player.Pos.Y, (int) _player.Pos.X] is not 'v' and not 'X')
                            {
                                Gui.UpdatePlayerPosition(_player, oldPos, _map.Tiles[(int) oldPos.Y, (int) oldPos.X]);
                                break;
                            }

                            _player.Pos = Vector2.Add(_player.Pos, new Vector2(0, 2));
                            Gui.UpdatePlayerPosition(_player, oldPos, _map.Tiles[(int) oldPos.Y, (int) oldPos.X]);
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
                        case ConsoleKey.X:
                        {
                            Gui.Screen = Screen.End;
                            Updater.Update = true;
                            break;
                        }
                    }

                    break;
                }
                case Screen.Counter:
                {
                    Gui.UpdateAt(new Vector2(-1,0), new Vector2(0,0), ' '); //get rid of input chars at 0,0
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
                                        Gui.Screen = Screen.End;
                                        Updater.Update = true;
                                        break;
                                    }

                                    _player.Stats.Score += 1; //change score

                                    if (_player.Stats.Score == _map.Forms.Count) //player is at the last needed counter
                                    {
                                        _player.Next = new Form(-1, _map.Forms[1].Colour, _map.Forms[1].Counter);
                                        _player.Needed = _map.Forms[0];
                                    }
                                    else if (_player.Stats.Score < _map.Forms.Count)
                                    {
                                        _player.Needed = _map.Forms[_player.Stats.Score];
                                        if (_player.Stats.Score + 1 < _map.Forms.Count)
                                        {
                                            _player.Next = _map.Forms[_player.Stats.Score + 1];
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
                        case ConsoleKey.X:
                        {
                            Gui.Screen = Screen.End;
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
                        case ConsoleKey.X:
                        {
                            Gui.Screen = Screen.End;
                            Updater.Update = true;
                            break;
                        }
                    }
                    break;
                }
                case Screen.End:
                {
                    switch (key)
                    {
                        case ConsoleKey.X:
                        {
                            Updater.End = true;
                            Updater.Active = false;
                            break;
                        }
                        case ConsoleKey.E:
                        {
                            //Restart (map and player)
                            Properties prop = new Properties(AppDomain.CurrentDomain.BaseDirectory + "\\resources\\startup.properties");
                            _map.Restart( int.Parse(prop.get("seed")),GameMap.DifficultyDictionary[prop.get("difficulty")],
                                "        ____________________       "
                                + " ______|        -<>-        |_____ "
                                + "|                                 |"
                                + "|                                 |"
                                + "[ (°: ]________________v____[ :°) ]"
                                + "|#####################zZT#########|"
                                + "[ (°: ]_v______________^____[ :°) ]"
                                + "|######zZT########################|"
                                + "[ (°: ]_X______________v____[ :°) ]"
                                + "|######zZT############zZT#########|"
                                + "[ (°: ]_X__________|___X____[ :°) ]"
                                + "|######zZT############zZT#########|"
                                + "[ (°: ]_X__________|___^____[ :°) ]"
                                + "|######zZT########################|"
                                + "[ (°: ]_X__________|___v____[ :°) ]"
                                + "|######zZT############zZT#########|"
                                + "[ (°: ]_X______________^____[ :°) ]"
                                + "|######zZT########################|"
                                + "[ (°: ]_^_________________________>");   //TODO: make level generator (used with difficulty)
                            _player.Restart(new Vector2(33,18),_map.Forms[0], _map.Forms[1]);
                            
                            Gui.Screen = Screen.Start;
                            Updater.Update = true;
                            break;
                        }
                    }
                    break;
                }
            }
        }while (!Updater.End);
    }
}
