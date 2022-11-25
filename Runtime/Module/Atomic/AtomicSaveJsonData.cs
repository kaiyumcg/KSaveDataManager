using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KSaveDataMan
{
    [System.Serializable]
    internal class Ray_Wrapper
    {
        [SerializeField] internal Vector3 rayOrigin, rayDirection;
    }

    [System.Serializable]
    internal class Ray2D_Wrapper
    {
        [SerializeField] internal Vector2 rayOrigin, rayDirection;
    }

    [System.Serializable]
    internal class Bounds_Wrapper
    {
        [SerializeField] internal Bounds data;
    }

    [System.Serializable]
    internal class BoundsInt_Wrapper
    {
        [SerializeField] internal BoundsInt data;
    }

    [System.Serializable]
    internal class LayerMask_Wrapper
    {
        [SerializeField] internal LayerMask data;
    }

    [System.Serializable]
    internal class Rect_Wrapper
    {
        [SerializeField] internal Rect data;
    }

    [System.Serializable]
    internal class RectInt_Wrapper
    {
        [SerializeField] internal RectInt data;
    }

    [System.Serializable]
    internal class Vector2Int_Wrapper
    {
        [SerializeField] internal Vector2Int data;
    }

    [System.Serializable]
    internal class Vector3Int_Wrapper
    {
        [SerializeField] internal Vector3Int data;
    }
}