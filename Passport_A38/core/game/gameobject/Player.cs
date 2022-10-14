﻿using System.Numerics;

namespace Passport_A38.core.game.gameobject;

public class Player : GameObject
{
    private const char character= '§';
    public override char Character => character;
    
    public int Score { get; set; }  //number of collected forms
    public string Counter { get; set; } = "0:c";    //current counter
    public Form Needed { get; set; }  //the form the player needs next

    public Form Next { get; set; }  //form after needed

    public byte InteractionState { get; set; } = 0;   //0 or 1: tells you if you started the conversation or if you are able to end it

    public Player(Vector2 pos, Form needed, Form next) : base(pos)
    {
        Needed = needed;
        Next = next;
    }
}