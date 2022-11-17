using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class CharacterInformationUI : MonoBehaviour {
    [SerializeField] private AttackAction Attack;
    [SerializeField] private int order;
    private UnitBase unitBase;
    GameObject character;
    private Image healthBarImage;
    private TextMeshProUGUI HPText;
    private TextMeshProUGUI AttackText;
    private double maxHealth;

    // Start is called before the first frame update
    void Start()
    {
        unitBase = GetComponent<UnitBase>();
        maxHealth = unitBase.HP;

        character = GameObject.Find("Player" + order + "Bar");
        healthBarImage = character.GetComponent<Image>();
        UnitBase.OnHPChange += UnitBase_OnDamage;

        character = GameObject.Find("Player" + order + "HPText");
        HPText = character.GetComponent<TextMeshProUGUI>();
        HPText.text = unitBase.HP.ToString( ) + "/" + maxHealth.ToString( );

        character = GameObject.Find("Player" + order + "AttackText");
        AttackText = character.GetComponent<TextMeshProUGUI>();
        AttackText.text = "Attack: " + unitBase.ATK;
    }

    private void UnitBase_OnDamage( object sender, EventArgs e ) {
        UpdateHealthBar( );
        UpdateHealthText( );
    }
    private void UpdateHealthBar( ) {
        healthBarImage.fillAmount = ( float )GetHealthNormalized( );
    }

    private void UpdateHealthText( ) {
        character = GameObject.Find( "Player" + order + "HPText" );
        HPText = character.GetComponent<TextMeshProUGUI>( );
        HPText.text = unitBase.HP.ToString( ) + "/" + maxHealth.ToString( );
    }

    public double GetHealthNormalized( ) {
        return unitBase.HP / maxHealth;
    }
}
