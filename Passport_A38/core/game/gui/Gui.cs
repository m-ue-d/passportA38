using System.Reflection;
using Microsoft.Win32.SafeHandles;
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
        
        //TODO: Credits
        
    }

    public static void DrawStartScreen()
    {
        //TODO: implement    
    }

    public static void DrawCounterScreen(string counter,Player player, GameMap map)
    {
        Console.Clear();
        
        var lines = File.ReadAllLines("screens/0-a.guiscreen");

        var first = false;
        int i = 0;
        foreach (var line in lines) //TODO: # Counter: What do you want? # text message before player presses f (use c for asking)
        {
            var x = line;
            if (player.Next.Number == -1 && player.Counter.Equals("0:a") && i==21) //end of search (21 is the line, where the text will be displayed)
            {
                Console.WriteLine("# ask for passport 39: q          #");
            }
            else if (counter.Equals(player.Needed.Counter))
            {
                if (x.Contains('°'))
                {

                    x = x.Replace("°", player.Next.Colour);
                    while (x.Length<35)
                    {
                        x= x.Insert(x.Length-2, " ");
                    }
                }
                else if (x.Contains('*'))
                {
                    x= x.Replace("*",player.Next.Counter);
                    while (x.Length<35)
                    {
                        x= x.Insert(x.Length-2, " ");
                    }
                }
                if (x.Contains('~'))
                {
                    x= x.Replace("~"," ");
                }
            }
            else
            {
                if(x.Contains('~'))
                {
                    if (!first)
                    {
                        x = "# [counter:] Sorry, wrong counter!#";
                        first = true;
                    }
                    else
                    {
                        x = "#                                 #";
                    }
                }
            }

            Console.WriteLine(x);
            i++;
        }
    }

    public static void DrawGameScreen(GameMap map, List<GameObject> inputObjects)
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
        for (var i = 0; i < map.Forms.Count; i++)
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
        //info:
        Console.WriteLine();
        //Console.WriteLine("Next form: "+player.Needed.Counter);
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
