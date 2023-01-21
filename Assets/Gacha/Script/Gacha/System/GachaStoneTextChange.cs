using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GachaStoneTextChange : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI paidCostText;
    [SerializeField]
    private TextMeshProUGUI freeCostText;

    string excelPath;
    //Excel xls;
    // Start is called before the first frame update
    void Start()
    {
        //excelPath = Application.streamingAssetsPath + "/playerData.xlsx";
        
        
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerPrefs.HasKey("paid"))
        {
            //xls = ExcelHelper.LoadExcel(excelPath);
            string paidStone = PlayerPrefs.GetInt("paid").ToString();
            paidCostText.text = paidStone;
            string freeStone = PlayerPrefs.GetInt("free").ToString();
            freeCostText.text = freeStone;
        }

        
    }
}
