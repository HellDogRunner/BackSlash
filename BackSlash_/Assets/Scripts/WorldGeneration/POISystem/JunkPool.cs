using System.Collections.Generic;
using UnityEngine;

public class JunkPool
{
    readonly Transform _poolRoot;
    readonly Dictionary<GameObject, Stack<GameObject>> _stacks = new();

    public JunkPool(Transform poolRoot)
    {
        _poolRoot = poolRoot;
    }

    public GameObject Acquire(GameObject prefab, Transform parent)
    {
        if (prefab == null) return null;

        if (!_stacks.TryGetValue(prefab, out var stack))
        {
            stack = new Stack<GameObject>();
            _stacks[prefab] = stack;
        }

        GameObject go;
        if (stack.Count > 0)
        {
            go = stack.Pop();
            go.SetActive(true);
        }
        else
        {
            go = Object.Instantiate(prefab);
        }

        go.transform.SetParent(parent, false);
        return go;
    }

    public void Release(GameObject prefab, GameObject instance)
    {
        if (prefab == null || instance == null) return;

        if (!_stacks.TryGetValue(prefab, out var stack))
        {
            stack = new Stack<GameObject>();
            _stacks[prefab] = stack;
        }

        instance.SetActive(false);
        instance.transform.SetParent(_poolRoot, false);
        stack.Push(instance);
    }

    public void ClearAll()
    {
        foreach (var kv in _stacks)
        {
            var stack = kv.Value;
            while (stack.Count > 0)
            {
                var go = stack.Pop();
                if (go != null) Object.Destroy(go);
            }
        }
        _stacks.Clear();
    }
}
