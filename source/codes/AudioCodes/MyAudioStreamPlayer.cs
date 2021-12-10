using Godot;
using System;

public class MyAudioStreamPlayer : AudioStreamPlayer
{
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
