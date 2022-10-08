namespace Passport_A38.core.game.gameobject;

public class Form
{
    public int Number { get; set; } = 0;
    public string Colour { get; set; } = "unset";
    public string Counter { get; set; } = "x:x";


    public Form() { }
    public Form(int num,string colour, string counter)
    {
        Number = num;
        Colour = colour;
        Counter = counter;
    }

    public override string ToString()
    {
        return "Form: "+Number+" - Counter: " +Counter+" - Colour: "+Colour;
    }
}