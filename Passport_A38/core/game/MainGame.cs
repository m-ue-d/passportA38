using System.Numerics;
using Passport_A38.core.game.controller;
using Passport_A38.core.game.gameobject;
using Passport_A38.core.game.gui;
using Passport_A38.core.game.map;

namespace Passport_A38.core.game;

public class MainGame
{

    private GameMap _gameMap;
    private List<GameObject> objects= new();

    public static void Main(string[] args)
    {
        const string map =
              "        ____________________       "
            + " ______|        -<>-        |_____ "
            + "|                                 |"
            + "|                                 |"
            + "[ (°: ]________________v____[ :°) ]"
            + "|#####################zZT#########|"
            + "[ (°: ]_v______________^____[ :°) ]"
            + "|######zZT########################|"
            + "[ (°: ]_X______________^____[ :°) ]"
            + "|######zZT############zZT#########|"
            + "[ (°: ]_X__________|___X____[ :°) ]"
            + "|######zZT############zZT#########|"
            + "[ (°: ]_X__________|___^____[ :°) ]"
            + "|######zZT########################|"
            + "[ (°: ]_X__________|___v____[ :°) ]"
            + "|######zZT############zZT#########|"
            + "[ (°: ]_X______________^____[ :°) ]"
            + "|######zZT########################|"
            + "[ (°: ]_^_________________________>";

        var main = new MainGame();
        
        main.Start(map);
    }

    /*
     * Inputs run on a separate Thread to everything else (needs to be fast and precise)
     * Gui is drawn every time something changes (inputs, moving gameObjects, interactions)
     */
    private void Start(string map)
    {
        //TODO: make re/start screen {
        
        _gameMap = new GameMap(map);
        
        var player = new Player(new Vector2(33,18));

        objects.Add(player);
        
        //initialize keyhandler
        PlayerInputHandler playerHandler = new PlayerInputHandler(player,_gameMap);
        Thread keyHandler = new Thread(new ThreadStart(playerHandler.ReadKeyData));
        keyHandler.Start();
        
        while (Updater.Active)
        {
            //draw screen
            if (Updater.Update)
            {
                Gui.Draw(_gameMap, objects);
                Updater.Update = false;
            }
        }
        Gui.DrawEndScreen(Updater.Win);
        
        //TODO: make re/start screen }
    }

    public List<GameObject> Objects { get; }
}
