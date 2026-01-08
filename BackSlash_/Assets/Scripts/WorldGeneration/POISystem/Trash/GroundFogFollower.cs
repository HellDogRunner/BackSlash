using UnityEngine;

public class GroundFogFollower : MonoBehaviour
{
    public Transform target;
    public float heightAboveGround = 1.0f;

    void Reset()
    {
        target = transform.parent;
    }

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 pos = target.position;

        // Корректируем высоту по террейну
        Terrain t = Terrain.activeTerrain;
        if (t != null)
        {
            pos.y = t.SampleHeight(pos) + t.transform.position.y + heightAboveGround;
        }
        else
        {
            pos.y += heightAboveGround;
        }

        transform.position = pos;

        // Туман не должен вращаться
        transform.rotation = Quaternion.identity;
    }
}
