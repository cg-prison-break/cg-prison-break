using UnityEngine;

public class MinimapFollow : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float height = 20f;
    [SerializeField] bool lockRotation = true;

    void LateUpdate()
    {
        if (!target) return;
        Vector3 pos = target.position;
        pos.y += height;
        transform.position = pos;

        if (lockRotation)
            transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        else
            transform.rotation = Quaternion.Euler(90f, target.eulerAngles.y, 0f);
    }
}
