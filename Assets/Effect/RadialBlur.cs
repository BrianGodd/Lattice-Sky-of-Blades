using System;
using UnityEngine;
using UnityEngine.Rendering;

[Serializable, VolumeComponentMenu("Custom/Radial Blur")]
public class RadialBlur : VolumeComponent
{
    public ClampedFloatParameter intensity = new ClampedFloatParameter(0f, 0f, 1f);
    public Vector2Parameter center = new Vector2Parameter(new Vector2(0.5f, 0.5f));
    public ClampedFloatParameter radius = new ClampedFloatParameter(1f, 0f, 2f);
    public IntParameter sampleCount = new IntParameter(8);

    public bool IsActive() => intensity.value > 0f;
}
