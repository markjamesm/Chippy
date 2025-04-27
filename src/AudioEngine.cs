using Raylib_cs;

namespace Chippy;

public class AudioEngine
{
    private readonly Sound _beep;
    
    public AudioEngine()
    {
        Raylib.InitAudioDevice();
        _beep = Raylib.LoadSound("sounds/beep1.wav");
    }

    public void Beep() => Raylib.PlaySound(_beep);
    
    public void StopBeep() => Raylib.StopSound(_beep);
}