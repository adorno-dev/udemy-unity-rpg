using UnityEngine;

public class UI : MonoBehaviour
{
    public UI_ItemToolTip itemToolTip;
    public UI_StatToolTip statToolTip;

    void Start()
    {
        // itemToolTip = GetComponentInChildren<UI_ItemToolTip>();
    }

    public void SwitchTo(GameObject menu)
    {
        Transform current = null;

        for (int i = 0; i < transform.childCount; i++)
        {
            current = transform.GetChild(i);

            if (current.name == menu.name)
                current.gameObject.SetActive(true);
            else
                current.gameObject.SetActive(false);
        }
    }
}
