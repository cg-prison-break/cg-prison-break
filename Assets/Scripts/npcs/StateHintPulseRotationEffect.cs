using UnityEngine;

public class StateHintPulseRotationEffect : MonoBehaviour
{
    public float rotateSpeed = 90f;
    public float pulseSpeed = 2f;
    public float pulseAmount = 0.3f;

    private Vector3 baseScale;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        baseScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        // rotate
        transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);

        // pulse
        float scale = 1f + Mathf.Sin(Time.time * pulseSpeed) * pulseAmount;
        transform.localScale = baseScale * scale;
    }
}