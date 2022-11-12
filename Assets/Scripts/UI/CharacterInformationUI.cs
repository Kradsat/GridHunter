using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class CharacterInformationUI : MonoBehaviour {
    [SerializeField] private Unit unit;
    [SerializeField] private HealthSystem healthSystem;
    [SerializeField] private AttackAction Attack;
    [SerializeField] private int order;
    private UnitBase unitBase;
    GameObject character;
    private Image healthBarImage;
    private TextMeshProUGUI HPText;
    private TextMeshProUGUI AttackText;
    // Start is called before the first frame update
    void Start()
    {
        unitBase = GetComponent<UnitBase>();

        character = GameObject.Find("Player" + order + "Bar");
        healthBarImage = character.GetComponent<Image>();
        healthSystem.OnDamage += HealthSystem_OnDamage;

        character = GameObject.Find("Player" + order + "HPText");
        HPText = character.GetComponent<TextMeshProUGUI>();
        HPText.text = unitBase.HP.ToString() + "/" + healthSystem.getMaxHealth().ToString();

        character = GameObject.Find("Player" + order + "AttackText");
        AttackText = character.GetComponent<TextMeshProUGUI>();
        AttackText.text = "Attack: " + unitBase.ATK;
    }

    private void HealthSystem_OnDamage( object sender, EventArgs e ) {
        UpdateHealthBar( );
        UpdateHealthText( );
    }
    private void UpdateHealthBar( ) {
        healthBarImage.fillAmount = healthSystem.GetHealthNormalized( );
    }

    private void UpdateHealthText( ) {
        character = GameObject.Find( "Player" + order + "HPText" );
        HPText = character.GetComponent<TextMeshProUGUI>( );
        HPText.text = healthSystem.getHealth( ).ToString( ) + "/" + healthSystem.getMaxHealth( ).ToString( );
    }
}
