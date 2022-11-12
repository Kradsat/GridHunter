using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UnitWorldUI : MonoBehaviour
{
   [SerializeField] private TextMeshProUGUI actionPointsText;
   [SerializeField] private UnitBase unit;
   [SerializeField] public Image healthBarImage;
   [SerializeField] private HealthSystem healthSystem;
    GameObject BossBar;
    public bool isBoss;

    private void Start()
    {
        unit = GetComponentInParent<UnitBase>();
        UnitBase.OnAnyActionPointsChanged += Unit_OnAnyActionPointsChanged;
        UnitBase.OnDamage += UnitBase_OnDamage;
        if( isBoss ) {
            BossBar = GameObject.Find( "BossBar" );
            healthBarImage = BossBar.GetComponent<Image>();
        }
        UpdateActionPointsText();
        UpdateHealthBar();
    }

    private void UnitBase_OnDamage( object sender, EventArgs e)
    {
        UpdateHealthBar();
    }

    private void Unit_OnAnyActionPointsChanged(object sender, EventArgs e)
    {
        UpdateActionPointsText();
    }

    private void UpdateActionPointsText()
    {
        actionPointsText.text = unit.GetActionPoints().ToString();
    }

    private void UpdateHealthBar()
    {
        healthBarImage.fillAmount = ( float )healthSystem.GetHealthNormalized();
    }
}
