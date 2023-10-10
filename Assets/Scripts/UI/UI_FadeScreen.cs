using UnityEngine;

public class UI_FadeScreen : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void FadeOut() => animator.SetTrigger("fadeOut");
    public void FadeIn() => animator.SetTrigger("fadeIn");
}
