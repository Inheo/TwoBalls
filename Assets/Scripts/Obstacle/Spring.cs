using UnityEngine;

public class Spring : MonoBehaviour
{
    [SerializeField] private float _force = 50;

    private void OnCollisionEnter(Collision other)
    {
        if(other.rigidbody != null)
        {
            other.rigidbody.AddForce(_force * Vector3.up, ForceMode.Impulse);
        }
    }
}
