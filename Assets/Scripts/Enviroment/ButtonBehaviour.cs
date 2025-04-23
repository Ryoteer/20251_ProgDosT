using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonBehaviour : MonoBehaviour, IInteractable
{
    private Animation _animation;

    private void Start()
    {
        _animation = GetComponentInParent<Animation>();
    }

    public void OnInteract()
    {
        Debug.Log($"A.");

        if (_animation.isPlaying) return;

        Debug.Log($"B.");

        _animation.Play();
    }
}
