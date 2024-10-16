using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;

/// <summary> Sets a background color for game objects in the Hierarchy tab</summary>
[UnityEditor.InitializeOnLoad]
#endif
public class HierarchyObjectColor
{
    private static Vector2 offset = new Vector2(30, 1);

    private static Color _backgroundColor = new Color(0.15f, 0.15f, 0.15f);
    private static Color _textColor = new Color(0.9f, 0.9f, 0.9f);

    static HierarchyObjectColor()
    {
        EditorApplication.hierarchyWindowItemOnGUI += HandleHierarchyWindowItemOnGUI;
    }

    private static void HandleHierarchyWindowItemOnGUI(int instanceID, Rect selectionRect)
    {

        var obj = EditorUtility.InstanceIDToObject(instanceID);
        if (obj != null)
        {
            Color backgroundColor = Color.white;
            Color textColor = Color.white;
            Texture2D texture = null;

            // Write your object name in the hierarchy.
            //if (obj.name == "Main Camera")
            //{
            //    backgroundColor = new Color(0.2f, 0.6f, 0.1f);
            //    textColor = new Color(0.9f, 0.9f, 0.9f);
            //}


            // Or you can use switch case
            switch (obj.name)
            {
                case "OTHER":
                    backgroundColor = _backgroundColor;
                    textColor = _textColor;
                    break;
                case "LOCATION":
                    backgroundColor = _backgroundColor;
                    textColor = _textColor;
                    break;
                case "USER INTERFACE":
                    backgroundColor = _backgroundColor;
                    textColor = _textColor;
                    break;
                case "CHARACTERS":
                    backgroundColor = _backgroundColor;
                    textColor = _textColor;
                    break;
            }


            if (backgroundColor != Color.white)
            {
                Rect offsetRect = new Rect(selectionRect.position + offset, selectionRect.size);
                Rect bgRect = new Rect(selectionRect.x, selectionRect.y, selectionRect.width + 50, selectionRect.height);

                EditorGUI.DrawRect(bgRect, backgroundColor);
                EditorGUI.LabelField(offsetRect, obj.name, new GUIStyle()
                {
                    normal = new GUIStyleState() { textColor = textColor },
                    fontStyle = FontStyle.Bold
                }
                );

                if (texture != null)
                    EditorGUI.DrawPreviewTexture(new Rect(selectionRect.position, new Vector2(selectionRect.height, selectionRect.height)), texture);
            }
        }
    }
}
