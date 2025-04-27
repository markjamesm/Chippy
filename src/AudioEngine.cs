using Raylib_cs;

namespace Chippy;

public class AudioEngine
{
    private readonly Sound _beep;
    
    public AudioEngine()
    {
        Raylib.InitAudioDevice();
        _beep = Raylib.LoadSound("sounds/beep.wav");
    }

    public void Beep() => Raylib.PlaySound(_beep);
    
    public void StopBeep() => Raylib.StopSound(_beep);
}