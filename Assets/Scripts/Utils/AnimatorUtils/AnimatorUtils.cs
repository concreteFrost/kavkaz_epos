using UnityEngine;
using System.Collections;

public static class AnimatorUtils
{
    public static IEnumerator WaitForAnimationEnd(Animator anim, string stateName, int layer=0)
    {
        // ждём пока анимация реально войдёт в state
        while (!anim.GetCurrentAnimatorStateInfo(layer).IsName(stateName))
            yield return null;

        // ждём пока анимация не проиграется до конца
        while (anim.GetCurrentAnimatorStateInfo(layer).IsName(stateName))
            yield return null;
    }

    public static bool IsAnimationFinished(Animator animator, int stateHash, int layer = 0)
    {
        if (animator.IsInTransition(layer))
            return false;

        var state = animator.GetCurrentAnimatorStateInfo(layer);

        if (state.shortNameHash != stateHash)
            return false;

        return state.normalizedTime >= 1f;
    }

}
