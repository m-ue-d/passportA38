using System.Numerics;
using System.Runtime.InteropServices;
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
            + "[ (°: ]_^_________________________>";    //TODO: This is the easy level. Make a hard one and select it by the level selector on the start screen (random level generation?)

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
        
        var player = new Player(new Vector2(33,18),_gameMap.Forms[0], _gameMap.Forms[1]);

        objects.Add(player);
        
        //initialize keyhandler
        PlayerInputHandler playerHandler = new PlayerInputHandler(player,_gameMap);
        Thread keyHandler = new Thread(new ThreadStart(playerHandler.ReadKeyData));
        keyHandler.Start();
        
        //SoundController -> windows only =(
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            SoundController soundController = new SoundController();
            soundController.StartBackgroundMusic();
        }
        
        while (Updater.Active)
        {
            //draw screen
            if (Updater.Update)
            {
                switch (Gui.Screen)
                {
                    case Screen.Game:
                    {
                        Gui.DrawGameScreen(_gameMap, objects);
                        break;
                    }
                    case Screen.Counter:
                    {
                        Gui.DrawCounterScreen(player.Counter, player, _gameMap);
                        break;
                    }
                    case Screen.Start:
                    {
                        
                        break;
                    }
                }
                Updater.Update = false;
            }
        }
        Gui.DrawEndScreen(Updater.Win);
        
        //TODO: make re/start screen }
    }
}
