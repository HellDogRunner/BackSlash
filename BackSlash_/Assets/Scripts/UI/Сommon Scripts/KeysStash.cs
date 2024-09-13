using System.Collections.Generic;
using UnityEngine;

public class KeysStash : MonoBehaviour
{

    public Dictionary<string, List<string>> KeysInputs = new Dictionary<string, List<string>> 
    {
        { "Mouse", new List<string> { "LMB", "RMB" } },
        { "Keyboard", new List<string> { "Space" } }
    };

    public Dictionary<string, int> MouseButtons = new Dictionary<string, int>
    {
        { "LMB", 0 },
        { "MMB", 1 },
        { "RMB", 2 },
    };

    public Dictionary<string, string> KeysText = new Dictionary<string, string>
    {
        { "Space", "SPACE" },
    };

    public List<Vector2> Frames = new List<Vector2>
    {
        { new Vector2(40, 40) },
        { new Vector2(54, 40) },
        { new Vector2(69, 40) },
        { new Vector2(83, 40) },
        { new Vector2(98, 40) },
        { new Vector2(112, 40) },
    };
}