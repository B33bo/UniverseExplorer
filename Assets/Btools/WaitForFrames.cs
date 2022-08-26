//Shouldn't need a namespace because it's so useful
using UnityEngine;

public class WaitForFrames : CustomYieldInstruction
{
    private int targetFrames;
    public WaitForFrames(int frames)
    {
        targetFrames = Time.frameCount + frames;
    }

    public override bool keepWaiting => targetFrames > Time.frameCount;
}
