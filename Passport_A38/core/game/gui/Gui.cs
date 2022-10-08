using Passport_A38.core.game.gameobject;
using Passport_A38.core.game.map;

namespace Passport_A38.core.game.gui;

public static class Gui
{

    public static Screen Screen { get; set; } = Screen.Game;   //TODO: Should be start

    public static void DrawEndScreen(bool win)
    {
        Console.Clear();
        if (win)
        {
            Console.WriteLine("Won");
            return;
        }
        
        Console.WriteLine("Lost");
        
    }

    public static void Draw(GameMap map, List<GameObject> inputObjects)
    {
        List<GameObject> objects = new List<GameObject>(inputObjects);

        Console.Clear();
        Player? player=null;
        foreach (var x in objects)
        {
            if (x is not Player p)
                break;
            player = p;
        }
        

        for (var i = 0; i < map.Tiles.GetLength(0); i++)
        {
            for (var j = 0; j < map.Tiles.GetLength(1); j++)
            {
                if (objects.Count>0 && (int)objects.First().Pos.Y==i && (int)objects.First().Pos.X==j && objects.First().Character!='_')
                {
                    Console.Write(objects.First().Character);
                    objects.Remove(objects.First());
                }
                else
                {
                    Console.Write(map.Tiles[i,j]);
                }
            }
            Console.WriteLine();
        }
        
        //score:
        if (player is null)
            return;
        for (var i = 0; i < map.Tiles.GetLength(1); i++)
        {
            if (i < player.Score)
            {
                Console.Write("+");
            }
            else
            {
                Console.Write("0");
            }
        }
        Console.WriteLine();
        
        //info:
        Console.WriteLine("move: arrow keys, interact: e      ");

    }

    /*
     * Parses an integer value to its roman counterpart.
     * I : 1
     * V : 5
     * X : 10
     * L : 50
     * C : 100
     * D : 500
     * M : 1000
     */
    /*public static string IntToRoman(int x)
    {
        var roman = "";
        while (x>=1000) //that's the easy part
        {
            roman += "M";
            x -= 1000;
        }
            
        var temp="";    
        while (x>=100)  //that's nuts
        {
            temp += "";
        }
        if(temp){
            
        }

    }*/
}
