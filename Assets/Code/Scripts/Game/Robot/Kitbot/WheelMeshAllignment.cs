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

        transform.Rotate(rotationAngle, 0, 0);
    }
}
