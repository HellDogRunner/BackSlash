using UnityEngine;
using Zenject;

public class Ragdoll : MonoBehaviour
{
    private Rigidbody[] _rigidbodies;

    [Inject]
    private void Construct()
    {
        _rigidbodies = GetComponentsInChildren<Rigidbody>();

        DeactivateRagdoll();
    }

    public void DeactivateRagdoll()
    {
        foreach (var rigidBody in _rigidbodies)
        {
            rigidBody.isKinematic = true;
        }
    }

    public void ActivateRagdoll()
    {
        foreach (var rigidBody in _rigidbodies)
        {
            rigidBody.isKinematic = false;
        }
    }
}
