using System.Diagnostics.Tracing;
using System.Globalization;
using Passport_A38.core.game.controller;
using Passport_A38.core.game.gameobject;
using Passport_A38.core.game.map;
using Passport_A38.core.game.utility;

namespace Passport_A38.core.game.gui;

public static class Gui
{

    public static Screen Screen { get; set; } = Screen.Start;

    public static void DrawEndScreen(Player player, Thread creditThread)
    {
        Console.Clear();
        switch (creditThread.ThreadState)
        {
            case ThreadState.Unstarted:
            {

                creditThread.Start(player.Stats);
                break;

            }
            case ThreadState.Stopped:
            {

                creditThread = new Thread(Credits);

                creditThread.Start(player.Stats);

                break;

            }
        }
    }

    public static void Credits(object? stats)
    {
        if (stats is not Stats) { return; }
        var playerStats = (Stats)stats;
        
        
        var lines = File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory +"\\resources\\credits.txt");
        foreach (var line in lines)
        {
            if (!Screen.Equals(Screen.End) || !Updater.Active)
                return;
            
            var i = (int)(line.Contains('%') ? char.GetNumericValue(line[line.IndexOf("%", StringComparison.Ordinal) + 1]) : -1);
            WriteStat(line,i,playerStats.GetAt[i]);

            Thread.Sleep(200);
        }
    }

    private static void WriteStat(string line, int i, object s)
    {
        if (line.Contains("%"+i))
        {
            var temp = line.Split("%"+i);
            var x = temp[0]+s;
                
            while (x.Length<GameMap.width-1) {x += " ";}
            x += "#";

            Console.WriteLine(x);
        }
        else
        {
            Console.WriteLine(line);
        }
    }

    public static void DrawStartScreen()
    {
        Console.Clear();
        
        var lines = File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + "\\resources\\screens\\start.guiscreen");
        foreach (var line in lines)
        {
            Console.WriteLine(line);
        }
    }

    public static void DrawCounterScreen(string counter,Player player)
    {
        Console.Clear();
        
        var lines = File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + "\\resources\\screens\\0-a.guiscreen");
        
        Console.WriteLine("Score: "+player.Stats.Score+", InteractionState: "+player.InteractionState+", Counter: "+player.Counter+", Next: "+player.Next.Counter+", Needed: "+player.Needed.Counter);
        Console.WriteLine();

        var first = true;
        foreach (var line in lines)
        {
            if (player.InteractionState == 1)
            {
                var x = line;
                
                if (player.Next.Number == -1 && player.Counter.Equals("0:a"))   //last counter
                {
                    if (x.Contains('~'))
                    {
                        if (first)
                        {
                            x = "# [counter:] Hold on, i need to   #\n# ask the others first...         #";
                            first = false;
                        }
                        else
                        {
                            x = "#                                 #";
                        }
                    }
                    else if (x.Contains('%'))
                    {
                        x = "# done: (f)                       #";
                    }
                }
                else if (counter.Equals(player.Needed.Counter)) //needed counter
                {
                    if (x.Contains('°'))
                    {

                        x = x.Replace("°", player.Next.Colour);
                        while (x.Length < GameMap.width)
                        {
                            x = x.Insert(x.Length - 2, " ");
                        }
                    }
                    else if (x.Contains('*'))
                    {
                        x = x.Replace("*", player.Next.Counter);
                        while (x.Length < GameMap.width)
                        {
                            x = x.Insert(x.Length - 2, " ");
                        }
                    }

                    if (x.Contains('~'))
                    {
                        x = x.Replace("~", " ");
                    }
                    else if (x.Contains('%'))
                    {
                        x = x.Replace("%", " ");
                    }
                }
                else
                {
                    if (x.Contains('~'))
                    {
                        if (first)
                        {
                            x = "# [counter:] Sorry, wrong counter!#";
                            first = false;
                        }
                        else
                        {
                            x = "#                                 #";
                        }
                    }
                    else if (x.Contains('%'))
                    {
                        x = "#                                 #";
                    }
                }
                Console.WriteLine(x);
            }
            else if(player.InteractionState==0) //start of conversation
            {
                var x = line;
                if (x.Contains('~'))
                {
                    x = (first? "# Hello, how can I help you Sir?  #":"#                                 #");
                    first = false;
                }
                else if (x.Contains('%'))
                {
                    if (player.Next.Number == -1 && player.Counter.Equals("0:a")) //end of search (21 is the line, where the text will be displayed)
                    {
                        x = "# ask: Could you please hand me a #\n#      copy of permit 39? (f)     #";
                    }
                    else if(player.Needed.Number == 0 && player.Counter.Equals("0:a"))
                    {
                        x = "# ask: Could you please hand me a #\n#      copy of permit 38? (f)     #";
                    }
                    else
                    {
                        x = "# ask: Could you please hand me a #\n# copy of permit " + player.Needed.Colour + "-" + player.Needed.Number+" (f)";
                        var temp = x[x.IndexOf("\n", StringComparison.Ordinal)..];
                        for (var i=0;i<line.Length-(temp.Length);i++) { x +=" "; }
                        x += "#";
                    }
                }
                Console.WriteLine(x);
            }
            else{
                var x = line;
                if (x.Contains('~'))
                {
                    if (first)
                    {
                        x = "# [counter:] Sorry, wrong counter!#";
                        first = false;
                    }
                    else
                    {
                        x = "#                                 #";
                    }
                }
                else if (x.Contains('%'))
                {
                    x = "#                                 #";
                }

                Console.WriteLine(x);
            }
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
            if (i < player.Stats.Score)
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
        Console.WriteLine("move: (arrow keys), interact: (e)  ");

    }

    /*TODO: Parse form numbers to roman when asking for form x
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
