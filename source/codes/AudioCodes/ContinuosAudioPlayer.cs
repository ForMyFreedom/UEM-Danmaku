using Godot;
using System;

public class ContinuosAudioPlayer : AudioStreamPlayer
{
    public override void _Ready()
    {
        Connect("finished", this, "_OnFinished");
    }

    private void _OnFinished()
    {
        Play();
    }



    public void SetDeturpatedAudioFormat()
    {
        Stream.Set("format", AudioStreamSample.FormatEnum.Format8Bits);
        VolumeDb = -10;
    }

    public void SetRegularAudioFormat()
    {
        Stream.Set("format", AudioStreamSample.FormatEnum.Format16Bits);
        VolumeDb = 0;
    }
}
