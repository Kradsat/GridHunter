using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


[RequireComponent(typeof(TextMeshProUGUI))]
public class LocaliserUI : MonoBehaviour
{

    TextMeshProUGUI textField;


    //  public string key;
    public LocalisedString localisedString;


    // Start is called before the first frame update
    void Start()
    {
        TextLocalised();


    }

    public void TextLocalised()
        {

        textField = GetComponent<TextMeshProUGUI>();
       
       textField.text = localisedString.value;
    


        }
    }



