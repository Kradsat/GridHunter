using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GachaSystem : MonoBehaviour
{
    //mode切り替え用state
    private enum STATE
    {
        STANDBYMODE,
        STARTFADEMODE,
        STARTDIRECTINGMODE,
        DIRECTINGMODE,
        OUTFADEMODE,
        STARTDISPLAYMODE,
        DISPLAYMODE,
        ENDFADEMODE,
    }

    private STATE state;
    private Action[] updatefanc;

    [SerializeField]
    private GameObject MainCamera;

    [SerializeField]
    private GameObject standbyPanel;//ガチャ待機画面
    [SerializeField]
    private GameObject DirectingPanel;//ガチャ演出画面
    [SerializeField]
    private GameObject DisplayPanel;//ガチャ結果画面
    [SerializeField]
    private GameObject LayoutPanel;//ガチャ結果を実際に並べるパネル
    [SerializeField]
    private GameObject WarningPanel;

    [SerializeField]
    private GameObject ItemTray;//ガチャ結果を乗せるやつ


    [SerializeField]
    private Animator hammerAnimator;//ガチャ演出用のアニメーション
    [SerializeField]
    private GameObject[] Stones;//演出に使用する石オブジェクト

    [SerializeField]
    private GameObject[] fadeItems;//Fadeで使用するImage
    [SerializeField]
    private CanvasGroup[] fadeItemsCanvas;//Fadeで使用するImageのCanvasGroup


    [SerializeField]
    private int sceneNum;//遷移先シーン数値
    //string excelPath;
    //Excel xls;

    private itemData _itemData;//ガチャアイテムのデータ
    private string[] rarity = { "Nomal", "Rare", "SRare", "SSRare" };//全Rarityを保持
    private System.Random random;//乱数

    private bool fadeBool = true;//Fadeで使用する

    private bool LayoutOneTime = true;//ガチャ結果を一度だけ並べる用bool

    private int directingFlag = 0;//ガチャ演出でのFadeのFlag

    const int value = 100;// ガチャの値段
    private int countPerPack;//ガチャ回数

    List<string> resultList;//ガチャ結果保持
    private int RarityNum = 0;//ガチャの最大レアリティ保持

    static public int DirectingClickCount = 0;//ガチャ演出クリック数取得
    // Start is called before the first frame update

    private void Awake()
    {
        if (!PlayerPrefs.HasKey("paid") && !PlayerPrefs.HasKey("free"))
        {
            PlayerPrefs.SetInt("paid", 1000);
            PlayerPrefs.SetInt("free", 50);
        }
    }
    void Start()
    {
        updatefanc = new Action[]
        {
            StandbyMode,
            StartFadeMode,
            StartDirectingMode,
            DirectingMode,
            OutFadeMode,
            StartDispayMode,
            DispayMode,
            EndFade
        };
        _itemData = Resources.Load<itemData>("Excel/itemData");
        //excelPath = Application.streamingAssetsPath + "/playerData.xlsx";
        random = new System.Random((int)DateTime.Now.Ticks);
    }

    // Update is called once per frame
    void Update()
    {
        var fanc = updatefanc[(int)state];
        fanc.Invoke();
    }

    private void StandbyMode()//ガチャ待機モード
    {
        standbyPanel.SetActive(true);
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            PlayerPrefs.SetInt("paid", 1000);
            PlayerPrefs.SetInt("free", 50);
        }
    }

    public void StartGacha(int num)//ボタンで押されたら起動
    {
        DirectingClickCount = 0;
        countPerPack = num;
        GetGacha();
        MainCamera.transform.position = new Vector3(-1.5f, 6.98f, 2.18f);
        MainCamera.transform.rotation = Quaternion.Euler(49.53f, 139.3f, 0.0f);
        //BlackFade
        fadeItems[3].SetActive(true);
        state = STATE.STARTFADEMODE;


        //xls = ExcelHelper.LoadExcel(excelPath);
        // string paidStone = (string)xls.Tables[0].GetValue(3, 1);
        //int paidStoneNum = PlayerPrefs.GetInt("paid");
        ////string freeStone = (string)xls.Tables[0].GetValue(3, 2);
        //int freeStoneNum = PlayerPrefs.GetInt("free");
        //if (num == 1)
        //{
        //    if ((paidStoneNum + freeStoneNum) >= 1)
        //    {
        //        if (freeStoneNum >= 1)
        //        {
        //            freeStoneNum -= 1;
        //            PlayerPrefs.SetInt("free", freeStoneNum);
        //        }
        //        else
        //        {
        //            int paidCost = 1 - freeStoneNum;
        //            paidStoneNum -= paidCost;
        //            freeStoneNum = 0;
        //            PlayerPrefs.SetInt("paid", paidStoneNum);
        //            PlayerPrefs.SetInt("free", freeStoneNum);
        //        }

        //        DirectingClickCount = 0;
        //        countPerPack = num;
        //        GetGacha();
        //        //BlackFade
        //        fadeItems[3].SetActive(true);
        //        state = STATE.STARTFADEMODE;

        //    }
        //    else
        //    {
        //        WarningPanel.SetActive(true);
        //    }
        //}
        //else if (num == 10)
        //{
        //    if ((paidStoneNum + freeStoneNum) >= 10)
        //    {
        //        if (freeStoneNum >= 10)
        //        {
        //            freeStoneNum -= 10;
        //            PlayerPrefs.SetInt("free", freeStoneNum);
        //            //string freeStoneSt = freeStoneNum.ToString();
        //            //xls.Tables[0].SetValue(3, 2, freeStoneSt);
        //            //ExcelHelper.SaveExcel(xls, excelPath);
        //        }
        //        else
        //        {
        //            int paidCost = 10 - freeStoneNum;
        //            paidStoneNum -= paidCost;
        //            freeStoneNum = 0;
        //            PlayerPrefs.SetInt("paid", paidStoneNum);
        //            PlayerPrefs.SetInt("free", freeStoneNum);
        //            //string paidStoneSt = paidStoneNum.ToString();
        //            //xls.Tables[0].SetValue(3, 1, paidStoneSt);
        //            //string freeStoneSt = freeStoneNum.ToString();
        //            //xls.Tables[0].SetValue(3, 2, freeStoneSt);
        //            //ExcelHelper.SaveExcel(xls, excelPath);
        //        }

        //        DirectingClickCount = 0;
        //        countPerPack = num;
        //        GetGacha();
        //        //BlackFade
        //        fadeItems[3].SetActive(true);
        //        state = STATE.STARTFADEMODE;

        //    }
        //    else
        //    {
        //        WarningPanel.SetActive(true);
        //    }
        //}

    }
    public void WarningPanelOut()
    {
        WarningPanel.SetActive(false);
    }

    private void StartFadeMode()//BlackFade
    {
        if (fadeItemsCanvas[3].alpha >= 1.0f)
        {
            fadeBool = false;
        }

        if (fadeBool)
        {
            //フェードイン
            fadeItemsCanvas[3].alpha += 0.02f;
            
        }
        if(!fadeBool)
        {
            //フェードアウト
            standbyPanel.SetActive(false);
            fadeItemsCanvas[3].alpha -= 0.02f;
            if (fadeItemsCanvas[3].alpha <= 0.0f)
            {
                //フェードオブジェクトを消して次のstateへ
                fadeBool = true;
                fadeItems[3].SetActive(false);
                state = STATE.STARTDIRECTINGMODE;
            }
        }

        

    }

    private void StartDirectingMode()//ガチャ演出モード開始時の処理
    {
        int randNum = getRamdomNom(100 + 1);
        switch (RarityNum)
        {
            case 0:
                directingFlag = 1;
                break;
            case 1:
                directingFlag = 1;
                break;
            case 2:
                
                if (randNum <= 80)
                {
                    directingFlag = 2;
                }
                else
                {
                    directingFlag = 1;
                }
                break;
            case 3:
                if (randNum <= 80)
                {
                    directingFlag = 3;
                }
                else if(randNum <= 95)
                {
                    directingFlag = 2;
                }
                else
                {
                    directingFlag = 1;
                }
                break;
        }
        Debug.Log(directingFlag);
        DirectingPanel.SetActive(true);
        state = STATE.DIRECTINGMODE;
    }


    private void DirectingMode()//ガチャ演出モード
    {
        
        if (DirectingClickCount >= directingFlag)
        {
            DirectingPanel.SetActive(false);
            state = STATE.OUTFADEMODE;
        }
    }

    public void DirectingClick()//ガチャ演出のアニメーションを再生する
    {
        hammerAnimator.SetTrigger("Hammer_hit");
        DirectingPanel.SetActive(false);

    }


    private void OutFadeMode()//ガチャ演出画面からガチャ結果画面への移動時のFade
    {
        DirectingPanel.SetActive(false);
        if(RarityNum <= 0)
        {
            fadeItems[RarityNum].SetActive(true);
            //フェードイン
            fadeItemsCanvas[RarityNum].alpha += 0.02f;

            if (fadeItemsCanvas[RarityNum].alpha >= 1.0f)
            {
                MainCamera.transform.position = new Vector3(0.0f, 0.0f, -80.0f);
                MainCamera.transform.rotation = Quaternion.Euler(30.0f, 180.0f, 0.0f);
                state = STATE.STARTDISPLAYMODE;
            }
        }
        else
        {
            fadeItems[RarityNum - 1].SetActive(true);
            //フェードイン
            fadeItemsCanvas[RarityNum - 1].alpha += 0.02f;

            if (fadeItemsCanvas[RarityNum - 1].alpha >= 1.0f)
            {
                MainCamera.transform.position = new Vector3(0.0f, 0.0f, -80.0f);
                MainCamera.transform.rotation = Quaternion.Euler(30.0f, 180.0f, 0.0f);
                state = STATE.STARTDISPLAYMODE;
            }
        } 
    }

    private void StartDispayMode()//ガチャ結果表示モード開始時の処理
    {
        Hammer.StoneNam = 0;
        Stones[0].SetActive(true);
        Stones[1].SetActive(false);
        Stones[2].SetActive(false);
        Stones[3].SetActive(false);
        DisplayPanel.SetActive(true);
        state = STATE.DISPLAYMODE;
    }

    private void DispayMode()//ガチャ結果表示モード
    {
        if(RarityNum <= 0)
        {
            if (fadeItemsCanvas[RarityNum].alpha != 0.0f)
            {
                fadeItemsCanvas[RarityNum].alpha -= 0.02f;
            }
            else if (fadeItemsCanvas[RarityNum].alpha <= 0.0f)
            {
                fadeItems[RarityNum].SetActive(false);
            }
        }
        else
        {
            if (fadeItemsCanvas[RarityNum - 1].alpha != 0.0f)
            {
                fadeItemsCanvas[RarityNum - 1].alpha -= 0.02f;
            }
            else if (fadeItemsCanvas[RarityNum - 1].alpha <= 0.0f)
            {
                fadeItems[RarityNum - 1].SetActive(false);
            }
        }

        if (LayoutOneTime)
        {
            for (int i = 0; i < countPerPack; i++)
            {
                LayoutItem(i);
            }
            LayoutOneTime = false;
        }
        
    }


    private void LayoutItem(int num)//入手したアイテムを並べる
    {
        Sprite itemSprite = null;
        GameObject itemobject = null;

        itemSprite = Resources.Load<Sprite>("Item/Text/" + resultList[num] + "_Text");
        itemobject = Resources.Load<GameObject>("Item/Object/" + resultList[num]+ "_Object");
        
        GameObject InstantItemTray = Instantiate(ItemTray, LayoutPanel.transform);
        Instantiate(itemobject, new Vector3(0.0f, 0.0f, -83.7f), Quaternion.identity);
        InstantItemTray.GetComponent<Image>().sprite = itemSprite;
        return;
    }

    public void DispayModeOut()
    {
        
        fadeItems[3].SetActive(true);
        LayoutOneTime = true;
        state = STATE.ENDFADEMODE;
    }


    private void EndFade()//BlackFade
    {
        if (fadeItemsCanvas[3].alpha >= 1.0f)
        {
            fadeBool = false;
        }

        if (fadeBool)
        {
            //フェードイン
            fadeItemsCanvas[3].alpha += 0.02f;

        }
        if (!fadeBool)
        {
            SceneManager.LoadScene(sceneNum);
            //フェードアウト
            //standbyPanel.SetActive(true);
            //DisplayPanel.SetActive(false);
            //fadeItemsCanvas[3].alpha -= 0.01f;
            //resultList.Clear();
            //if (fadeItemsCanvas[3].alpha == 0.0f)
            //{
            //    //フェードオブジェクトを消して次のstateへ
            //    fadeBool = true;

            //    for (int index = 0; index < LayoutPanel.transform.childCount; index++)
            //    {
            //        Destroy(LayoutPanel.transform.GetChild(index).gameObject);
            //    }

            //    fadeItems[3].SetActive(false);
            //    state = STATE.STANDBYMODE;
            //}
        }


    }
    /****************************************************************************************/
    private void GetGacha()//ガチャシステム本体
    {
        resultList = new List<string>();
        int totalProbability = 0;
        RarityNum = 0;
        for (int i = 0; i < rarity.Length; i++)
        {
            // レアリティの確率を足し合わせる
            totalProbability += _itemData.probability[i].num;
        }

        resultList = new List<string>(); // 抽選結果格納用変数
        for (int i = 0; i < countPerPack; i++)
        {
            // 抽選を行う
            string itmes = getItems(totalProbability);
            resultList.Add(itmes);
        }
        for (int i = 0; i < resultList.Count; i++)
        {
            Debug.Log(resultList[i]);
            //saveItem(resultList[i], 3);
        }
        Debug.Log(RarityNum);
    }

    private string getItems(int _probability)//アイテムを取得
    {
        int randomValue = getRamdomNom(_probability);
        int totalprobability = 0;

        for (int i = 0; i < rarity.Length; i++)
        {
            totalprobability += _itemData.probability[i].num;
            if (totalprobability >= randomValue)
            {
                string id = postItemName(i);
                return id;
            }
        }
        return null;
    }

    private string postItemName(int num)
    {
        switch (num)
        {
            case 0:
                return _itemData.nomalItemData[random.Next(0, _itemData.nomalItemData.Count)].name;//入手アイテムを取得
            case 1:
                if (RarityNum == 0)
                {
                    RarityNum = 1;//入手アイテムの最高レアリティの数値を取得
                }
                return _itemData.rareItemData[random.Next(0, _itemData.rareItemData.Count)].name;//入手アイテムを取得
            case 2:
                if (RarityNum <= 1)
                {
                    RarityNum = 2;//入手アイテムの最高レアリティの数値を取得
                }
                return _itemData.SRareItemData[random.Next(0, _itemData.SRareItemData.Count)].name;//入手アイテムを取得
            case 3:
                if(RarityNum <= 2)
                {
                    RarityNum = 3;//入手アイテムの最高レアリティの数値を取得
                }
                return _itemData.SSRareItemData[random.Next(0, _itemData.SSRareItemData.Count)].name;//入手アイテムを取得
        }
        return null;
    }

    //private void saveItem(string item, int steps)
    //{

    //    string handItem = (string)xls.Tables[1].GetValue(steps, 1);
    //    if (handItem == item)
    //    {
    //        string stNumber = (string)xls.Tables[1].GetValue(steps, 2);
    //        int Number = int.Parse(stNumber);
    //        Number++;
    //        stNumber = Number.ToString();
    //        xls.Tables[1].SetValue(steps, 2, stNumber);
    //        ExcelHelper.SaveExcel(xls, excelPath);
    //    }
    //    else if (handItem == "notItem")
    //    {
    //        xls.Tables[1].SetValue(steps, 1, item);
    //        xls.Tables[1].SetValue(steps + 1, 1, "notItem");
    //        xls.Tables[1].SetValue(steps, 2, "1");
    //        ExcelHelper.SaveExcel(xls, excelPath);
    //    }
    //    else
    //    {
    //        saveItem(item, steps + 1);
    //    }
    //}

    private int getRamdomNom(int _max)
    {
        return random.Next(0, _max);
    }

    private int getRamdomListnum(List<string> _list)
    {
        return random.Next(0, _list.Count);
    }

}
