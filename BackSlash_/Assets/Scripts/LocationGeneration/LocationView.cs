using System.Collections.Generic;
using UnityEngine;

public class LocationView : MonoBehaviour
{
    [SerializeField] private GameObject platformPrefab;
    [SerializeField] private GameObject bridgePrefab;

    private List<GameObject> generatedObjects = new List<GameObject>();

    public void ClearLocation()
    {
        foreach (var obj in generatedObjects)
        {
            Destroy(obj);
        }
        generatedObjects.Clear();
    }

    public void GeneratePlatform(Vector3 position)
    {
        var platform = Instantiate(platformPrefab, position, Quaternion.identity, transform);
        generatedObjects.Add(platform);
    }

    public void GenerateBridge(Vector3 startPosition, Vector3 endPosition)
    {
        var bridge = Instantiate(bridgePrefab, (startPosition + endPosition) / 2, Quaternion.identity, transform);
        bridge.transform.LookAt(endPosition);
        bridge.transform.localScale = new Vector3(1, 1, Vector3.Distance(startPosition, endPosition));
        generatedObjects.Add(bridge);
    }
}
