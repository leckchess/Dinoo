using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public Color color;
    public void SetTransform(Transform parent, Vector3 position, Vector3 scale)
    {
        transform.SetParent(parent);
        transform.localPosition = position;
        transform.localScale = scale;
    }
}
