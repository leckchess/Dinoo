using DG.Tweening;
using System.Collections;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public Color color;
    private Animator _anim;

    public void SetTransform(Transform parent, Vector3 position, Vector3 scale)
    {
        transform.SetParent(parent);
        transform.localPosition = position;
        transform.localScale = scale;
    }

    public void PlayAnimation(int animationnumber)
    {
        if (_anim == null)
            _anim = GetComponent<Animator>();

        _anim.SetInteger("animation", animationnumber);
    }
}
