using TMPro;
using UnityEngine;

public class UI_StatToolTip : UI_ToolTip
{
    [SerializeField] private TextMeshProUGUI description;

    public void ShowStatToolTip(string text)
    {
        description.text = text;

        base.AdjustPosition();

        gameObject.SetActive(true);
    }

    public void HideStatToolTip()
    {
        description.text = string.Empty;
        gameObject.SetActive(false);
    }
}
