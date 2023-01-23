using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ActionButtonUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMeshProUGUI;
    [SerializeField] private Button button;
    [SerializeField] private GameObject selectedGameObject;

    private BaseAction baseAction;

    public void SetBaseAction(BaseAction baseAction)
    {
        this.baseAction = baseAction;
        var text = "";
        switch (baseAction.GetActionName())
        {
            case "Stay":
                text = "ƒXƒeƒC";
                break;
            case "Move":
                text = "ˆÚ“®";
                break;
            case "Attack":
                text = "UŒ‚";
                break;
            case "Heal Ally":
                text = "ƒq[ƒ‹";
                break;
        }
        textMeshProUGUI.text = text;
        button.onClick.AddListener(() =>
        {
            UnitActionSystem.Instance.SetSelectedAction(baseAction);
        });
    }

    public void UpdateSelectedVisual()
    {
        BaseAction selectedBaseAction = UnitActionSystem.Instance.GetSelectedAction();
        selectedGameObject.SetActive(selectedBaseAction == baseAction);
    }

}
