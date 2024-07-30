using UnityEngine;

public class WheelMeshAllignment : MonoBehaviour
{
    [SerializeField]
    private WheelCollider WheelCollider;

    private float rpm;
    private float rotationAngle;

    private void FixedUpdate()
    {
        rpm = WheelCollider.rpm;
        rotationAngle = rpm / 60 * 360 * Time.deltaTime;

        transform.Rotate(0, 0, rotationAngle);
    }
}
