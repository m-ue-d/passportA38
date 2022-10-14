namespace Passport_A38.core.game.controller;

using System;
using System.Media;

public class SoundController
{
    private SoundPlayer _player;

    public SoundController()
    {
        _player = new SoundPlayer();
    }

    public void StartBackgroundMusic()
    {
        _player.SoundLocation = AppDomain.CurrentDomain.BaseDirectory + "\\resources\\music\\HotelMain.wav";
        _player.PlayLooping();
    }
    public void StopBackgroundMusic()
    {
        _player.Stop();
    }
}