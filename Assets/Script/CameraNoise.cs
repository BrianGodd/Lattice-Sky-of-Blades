using UnityEngine;

public class CameraNoise : MonoBehaviour
{
    [Header("Position Noise")]
    public float amplitude = 0.15f;   // 晃動幅度（小就好）
    public float frequency = 0.1f;    // 晃動速度（越小越慢）

    [Header("Rotation Noise (optional)")]
    public float rotationAmplitude = 0.5f;

    Vector3 basePos;
    Quaternion baseRot;
    float seedX;
    float seedY;

    void Start()
    {
        basePos = transform.localPosition;
        baseRot = transform.localRotation;

        seedX = Random.Range(0f, 1000f);
        seedY = Random.Range(0f, 1000f);
    }

    void LateUpdate()
    {
        float t = Time.time * frequency;

        float nx = Mathf.PerlinNoise(seedX, t) - 0.5f;
        float ny = Mathf.PerlinNoise(seedY, t) - 0.5f;

        // 位置微移
        transform.localPosition = basePos + new Vector3(
            nx * amplitude,
            ny * amplitude,
            0f
        );

        // 非必要，但加了會更「活」
        transform.localRotation = baseRot * Quaternion.Euler(
            ny * rotationAmplitude,
            nx * rotationAmplitude,
            0f
        );
    }
}
