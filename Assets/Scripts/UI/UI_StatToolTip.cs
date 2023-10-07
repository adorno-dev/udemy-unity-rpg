using TMPro;
using UnityEngine;

public class UI_StatToolTip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI description;

    public void ShowStatToolTip(string text)
    {
        description.text = text;
        gameObject.SetActive(true);
    }

    public void HideStatToolTip()
    {
        description.text = string.Empty;
        gameObject.SetActive(false);
    }
}
