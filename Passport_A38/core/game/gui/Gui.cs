using System.Diagnostics.Tracing;
using System.Globalization;
using System.Numerics;
using System.Runtime.InteropServices;
using Passport_A38.core.game.controller;
using Passport_A38.core.game.gameobject;
using Passport_A38.core.game.map;
using Passport_A38.core.game.utility;

namespace Passport_A38.core.game.gui;

public static class Gui //TODO: Optimize (less if statements and individual char placement)
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
        if (stats is not Stats playerStats) { return; }


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
                
            while (x.Length<GameMap.Width-1) {x += " ";}
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
        Console.SetCursorPosition(0,0);
        
        var lines = File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + "\\resources\\screens\\start.guiscreen");
        foreach (var line in lines)
        {
            Console.WriteLine(line);
        }
    }

    public static void DrawCounterScreen(string counter,Player player)
    {
        Console.Clear();
        Console.SetCursorPosition(0,0);
        
        var lines = File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + "\\resources\\screens\\0-a.guiscreen");
        
        Console.WriteLine("Score: "+player.Stats.Score+", InteractionState: "+player.InteractionState+", Counter: "+player.Counter+", Next: "+player.Next.Counter+", Needed: "+player.Needed.Counter);  //debugging
        Console.WriteLine();

        var first = true;
        foreach (var line in lines)
        {
            switch (player.InteractionState)
            {
                case 1:
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
                            while (x.Length < GameMap.Width)
                            {
                                x = x.Insert(x.Length - 2, " ");
                            }
                        }
                        else if (x.Contains('*'))
                        {
                            x = x.Replace("*", player.Next.Counter);
                            while (x.Length < GameMap.Width)
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
                    break;
                }
                //start of conversation
                case 0:
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
                            x = "# ask: Could you please hand me a #\n# copy of permit " + player.Needed.Colour + "-" + Utility.IntToRoman(player.Needed.Number) + " (f)";
                            var temp = x[x.IndexOf("\n", StringComparison.Ordinal)..];
                            for (var i=0;i<line.Length-(temp.Length);i++) { x +=" "; }
                            x += "#";
                        }
                    }
                    Console.WriteLine(x);
                    break;
                }
                default:
                {
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
                    break;
                }
            }
        }
        Console.SetCursorPosition(0,0); //reset cursor
    }

    public static void UpdatePlayerPosition(Player p, Vector2 oldPos, char replacement)
    {
        Console.SetCursorPosition((int)oldPos.X,(int)oldPos.Y);
        Console.Write(replacement);
        Console.SetCursorPosition((int)p.Pos.X,(int)p.Pos.Y);
        Console.Write(p.Character);
    }
    public static void UpdateAt(Vector2 after, Vector2 position, char replacement)
    {

        Console.SetCursorPosition((int)position.X>0?(int)position.X-1:0,(int)position.Y);
        Console.Write(replacement);
        Console.SetCursorPosition((int)after.X+1,(int)after.Y);
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
            Console.Write(i < player.Stats.Score ? "+" : "0");
        }
        //info:
        Console.WriteLine();
        Console.WriteLine("move: (arrow keys), interact: (e)  ");
        
        Console.SetCursorPosition(0,0); //reset cursor
    }
}
