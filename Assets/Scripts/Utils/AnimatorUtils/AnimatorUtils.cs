using UnityEngine;
using System.Collections;

public static class AnimatorUtils
{
    public static IEnumerator WaitForAnimationEnd(Animator anim, string stateName, int layer)
    {
        // ждём пока анимация реально войдёт в state
        while (!anim.GetCurrentAnimatorStateInfo(layer).IsName(stateName))
            yield return null;

        // ждём пока анимация не проиграется до конца
        while (anim.GetCurrentAnimatorStateInfo(layer).normalizedTime < 1f)
            yield return null;
    }
}
