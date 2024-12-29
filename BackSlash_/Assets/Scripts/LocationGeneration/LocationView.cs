using System.Collections.Generic;
using UnityEngine;

public class LocationView : MonoBehaviour
{
    [SerializeField] private GameObject platformPrefab;
    [SerializeField] private GameObject bridgePrefab;

    private List<GameObject> generatedObjects = new List<GameObject>();

    // ����������� ��������� ���������, ����� �� ����� ������������ ��� ������
    public Collider PlatformPrefabCollider => platformPrefab.GetComponent<Collider>();

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
        bridge.transform.localScale = new Vector3(1, bridge.transform.localScale.y, Vector3.Distance(startPosition, endPosition));
        generatedObjects.Add(bridge);
    }

    public Collider GetPlatformColliderAtPosition(Vector3 position)
    {
        // ������� ��������� � ������ �������, ����� ����� �� ���������
        var platform = Instantiate(platformPrefab, position, Quaternion.identity);
        Collider platformCollider = platform.GetComponent<Collider>();

        // ������� ���������, ��� ��� ��� ����� ������ �� ���������
        Destroy(platform);

        return platformCollider;
    }
}
