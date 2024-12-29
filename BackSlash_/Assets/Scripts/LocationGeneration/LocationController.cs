using System.Collections.Generic;
using UnityEngine;

public class LocationController
{
    private readonly LocationGenerationModel model;
    private readonly LocationView view;

    private List<Vector3> platformPositions = new List<Vector3>();

    public LocationController(LocationGenerationModel model, LocationView view)
    {
        this.model = model;
        this.view = view;
    }

    public void GenerateLocation()
    {
        view.ClearLocation();
        Random.InitState(model.Seed);
        platformPositions.Clear();

        GeneratePlatforms();
        ConnectPlatforms();
    }

    private void GeneratePlatforms()
    {
        for (int i = 0; i < model.PlatformCount; i++)
        {
            Vector3 position;
            do
            {
                position = new Vector3(
                    Random.Range(-model.AreaSize / 2, model.AreaSize / 2),
                    Random.Range(model.MinHeight, model.MaxHeight),
                    Random.Range(-model.AreaSize / 2, model.AreaSize / 2)
                );
            }
            while (IsTooCloseToOtherPlatforms(position));

            platformPositions.Add(position);
            view.GeneratePlatform(position);
        }
    }

    private bool IsTooCloseToOtherPlatforms(Vector3 position)
    {
        foreach (var other in platformPositions)
        {
            // �������� ������� ���������, �������� ����� � ���������
            Vector3 platformSize = view.PlatformPrefabCollider.bounds.size;

            // ���� ���������� ����� ����������� ������, ��� ����� �� ��������, �� ��� ������� ������
            if (Vector3.Distance(position, other) < Mathf.Max(platformSize.x, platformSize.z) + 10f)
                return true;
        }
        return false;
    }

    private void ConnectPlatforms()
    {
        var connected = new HashSet<Vector3>();
        var queue = new Queue<Vector3>();

        if (platformPositions.Count > 0)
        {
            queue.Enqueue(platformPositions[0]);
            connected.Add(platformPositions[0]);
        }

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();

            foreach (var target in platformPositions)
            {
                if (connected.Contains(target)) continue;

                if (Vector3.Distance(current, target) <= 15f)
                {
                    queue.Enqueue(target);
                    connected.Add(target);

                    // �������� ������� ����� �������� ��� �������� �����
                    Vector3 startEdge = GetPlatformEdgePosition(current, target);
                    Vector3 endEdge = GetPlatformEdgePosition(target, current);

                    view.GenerateBridge(startEdge, endEdge);
                }
            }
        }

        foreach (var platform in platformPositions)
        {
            if (!connected.Contains(platform))
            {
                var nearest = FindNearestPlatform(platform, connected);
                if (nearest != null)
                {
                    connected.Add(platform);

                    // �������� ������� ����� �������� ��� �������� �����
                    Vector3 startEdge = GetPlatformEdgePosition(platform, nearest.Value);
                    Vector3 endEdge = GetPlatformEdgePosition(nearest.Value, platform);

                    view.GenerateBridge(startEdge, endEdge);
                }
            }
        }
    }

    private Vector3 GetPlatformEdgePosition(Vector3 from, Vector3 to)
    {
        // �������� ��������� ���������, ����� ��������� �� ������
        Collider fromCollider = view.GetPlatformColliderAtPosition(from);
        Collider toCollider = view.GetPlatformColliderAtPosition(to);

        // ��������� �������� �� ������ ��������� �� �� ���� �� ���� X � Z
        Vector3 fromEdge = from + (to - from).normalized * (Mathf.Max(fromCollider.bounds.extents.x, fromCollider.bounds.extents.z));
        fromEdge.y = from.y; // ��������� �� �� ������

        return fromEdge;
    }

    private Vector3? FindNearestPlatform(Vector3 from, HashSet<Vector3> connected)
    {
        Vector3? nearest = null;
        float minDistance = float.MaxValue;

        foreach (var platform in connected)
        {
            float distance = Vector3.Distance(from, platform);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearest = platform;
            }
        }

        return nearest;
    }

    // ����� ��� ��������� ��������� ���������
    public Vector3 GetRandomPlatformPosition()
    {
        if (platformPositions.Count > 0)
        {
            return platformPositions[Random.Range(0, platformPositions.Count)];
        }
        return Vector3.zero; // ���� ��������� �� ���� �������
    }
}
