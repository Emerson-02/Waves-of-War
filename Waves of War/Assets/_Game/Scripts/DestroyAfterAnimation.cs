using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterAnimation : MonoBehaviour
{

    void Start()
    {
        Animator animator = GetComponent<Animator>();
        AnimatorStateInfo animatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
        float animationDuration = animatorStateInfo.length;

        Destroy(gameObject, animationDuration);
    }
}
