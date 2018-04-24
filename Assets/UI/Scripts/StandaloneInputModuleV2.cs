using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// Custom Standalone Input Module, used to replace the current one formed on every Event System gameObject by default.
// Used to return the gameObject that is under the mouse pointer when hovered over.
// Directly taken from http://answers.unity.com/answers/1433815/view.html
// Used by Peter Liang.
public class StandaloneInputModuleV2 : StandaloneInputModule
{
    // Returns the gameObject under a specified pointer ID, if multiple pointers are present.
    public GameObject GameObjectUnderPointer(int pointerId)
    {
        var lastPointer = GetLastPointerEventData(pointerId);
        if (lastPointer != null)
            return lastPointer.pointerCurrentRaycast.gameObject;
        return null;
    }

    // Returns the gameObject under the pointer.
    public GameObject GameObjectUnderPointer()
    {
        return GameObjectUnderPointer(PointerInputModule.kMouseLeftId);
    }
}