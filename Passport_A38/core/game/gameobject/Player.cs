using System.Numerics;

namespace Passport_A38.core.game.gameobject;

public class Player : GameObject
{
    private const char character= '§';
    public override char Character => character;
    
    public int Score { get; set; }  //number of collected forms
    public string Counter { get; set; } = "0:c";    //current counter
    public Form Needed { get; set; }  //the form the player needs next

    public Player(Vector2 pos, Form needed) : base(pos)
    {
        Needed = needed;
    }
}