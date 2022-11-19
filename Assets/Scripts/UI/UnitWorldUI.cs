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

   [SerializeField] private TextMeshProUGUI BossHPText;
   [SerializeField] private TextMeshProUGUI HPText;
    [SerializeField] private HealthSystem healthSystem;

    GameObject BossBar;
    public bool isBoss;
    private double bossMaxHeath;
    private double MaxHeath;

    private void Start()
    {
        unit = GetComponentInParent<UnitBase>();
        UnitBase.OnAnyActionPointsChanged += Unit_OnAnyActionPointsChanged;
        UnitBase.OnHPChange += UnitBase_OnDamage;
        if( isBoss ) {
            BossBar = GameObject.Find( "BossBar" );
            healthBarImage = BossBar.GetComponent<Image>();
            BossBar = GameObject.Find( "BossHPText" );
            bossMaxHeath = unit.HP;
            BossHPText = BossBar.GetComponent<TextMeshProUGUI>( );
            BossHPText.text = unit.HP.ToString( ) + "/" + bossMaxHeath;
        } else if( unit.IsEnemy ) {
            MaxHeath = unit.HP;
            UpdateHealthText( );
        }
        UpdateActionPointsText();
        UpdateHealthBar();
    }

    private void UnitBase_OnDamage( object sender, EventArgs e)
    {
        UpdateHealthBar();
        if( isBoss ) {
            UpdateBossHealthText( );
        } else if( unit.IsEnemy ) {
            UpdateHealthText( );
        }
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
        healthBarImage.fillAmount = (float)(unit.HP / unit.MAX_HP);
    }

    private void UpdateBossHealthText(  ) {
        BossHPText.text = unit.HP.ToString( ) + "/" + bossMaxHeath;
    }

    private void UpdateHealthText( ) {
        HPText.text = unit.HP.ToString( ) + "/" + MaxHeath;
    }
}
