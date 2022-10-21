using System.Runtime.InteropServices;
using Passport_A38.core.game.gui;

namespace Passport_A38.core.game.controller;

using System;
using System.Media;

public static class SoundController
{
    private static readonly SoundPlayer? Player;

    static SoundController()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            Player = new SoundPlayer();
    }
    
    public static void StartBackgroundMusic(Screen screen)
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            return;
        
        //TODO: fade song out before other plays (use audio library)

        if (Player == null) return;
        Player.SoundLocation = screen switch
        {
            Screen.Game => AppDomain.CurrentDomain.BaseDirectory + "\\resources\\music\\Building.wav",
            Screen.Start => AppDomain.CurrentDomain.BaseDirectory + "\\resources\\music\\Title.wav",
            Screen.Counter => AppDomain.CurrentDomain.BaseDirectory + "\\resources\\music\\Building.wav",
            _ => AppDomain.CurrentDomain.BaseDirectory + "\\resources\\music\\Title.wav"
        };

        Player.PlayLooping();
    }
    public static void StopBackgroundMusic()
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            return;
        Player?.Stop();
    }
}
