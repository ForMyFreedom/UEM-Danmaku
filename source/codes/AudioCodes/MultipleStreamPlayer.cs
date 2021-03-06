using Godot;
using Godot.Collections;
using System;

public class MultipleStreamPlayer : AudioStreamPlayer
{
    [Export]
    private AudioStreamSample[] audioStreamList;
    [Export]
    private String[] audioNameList;

    public void PlayAudioByName(String name)
    {
        Stream = audioStreamList[SearchIndexByName(name)];
        Play();
    }

    private int SearchIndexByName(String searchName)
    {
        for(int i = 0; i < audioNameList.Length; i++) 
        {
            if (audioNameList[i] == searchName)
                return i;
        }
        return -1;
    }

    public int GetQuantOfAudios()
    {
        return audioNameList.Length;
    }
}
