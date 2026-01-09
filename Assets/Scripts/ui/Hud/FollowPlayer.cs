using UnityEngine;

public class MinimapFollow : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float lowHeight = 3;
    [SerializeField] float highHeight = 6.3f;
    [SerializeField] float switchThresholdY = 3f;
    [SerializeField] bool lockRotation = true;

    void LateUpdate()
    {
        if (!target) return;

        // Follow player on X/Z; keep Y fixed to one of two heights based on player's Y.
        float chosenHeight = target.position.y >= switchThresholdY ? highHeight : lowHeight;

        Vector3 pos = transform.position;
        pos.x = target.position.x;
        pos.z = target.position.z;
        pos.y = chosenHeight;
        transform.position = pos;

        if (lockRotation)
            transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        else
            transform.rotation = Quaternion.Euler(90f, target.eulerAngles.y, 0f);
    }
}
