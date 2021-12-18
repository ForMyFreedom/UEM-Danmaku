using Godot;
using System;

public abstract class BaseTransition : MyControl
{
    public abstract void SetMinorText(String text);

    public abstract void SetMajorText(String text);

    public abstract void SetTexture(Texture texture);

    protected override ScenesDataCross GetDataCrossType()
    {
        return new BlankDataCross();
    }
}
