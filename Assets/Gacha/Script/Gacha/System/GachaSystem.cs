using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GachaSystem : MonoBehaviour
{
    //mode�؂�ւ��pstate
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
    private GameObject standbyPanel;//�K�`���ҋ@���
    [SerializeField]
    private GameObject DirectingPanel;//�K�`�����o���
    [SerializeField]
    private GameObject DisplayPanel;//�K�`�����ʉ��
    [SerializeField]
    private GameObject LayoutPanel;//�K�`�����ʂ����ۂɕ��ׂ�p�l��
    [SerializeField]
    private GameObject WarningPanel;

    [SerializeField]
    private GameObject ItemTray;//�K�`�����ʂ��悹����


    [SerializeField]
    private Animator hammerAnimator;//�K�`�����o�p�̃A�j���[�V����
    [SerializeField]
    private GameObject[] Stones;//���o�Ɏg�p����΃I�u�W�F�N�g

    [SerializeField]
    private GameObject[] fadeItems;//Fade�Ŏg�p����Image
    [SerializeField]
    private CanvasGroup[] fadeItemsCanvas;//Fade�Ŏg�p����Image��CanvasGroup


    [SerializeField]
    private int sceneNum;//�J�ڐ�V�[�����l
    //string excelPath;
    //Excel xls;

    private itemData _itemData;//�K�`���A�C�e���̃f�[�^
    private string[] rarity = { "Nomal", "Rare", "SRare", "SSRare" };//�SRarity��ێ�
    private System.Random random;//����

    private bool fadeBool = true;//Fade�Ŏg�p����

    private bool LayoutOneTime = true;//�K�`�����ʂ���x�������ׂ�pbool

    private int directingFlag = 0;//�K�`�����o�ł�Fade��Flag

    const int value = 100;// �K�`���̒l�i
    private int countPerPack;//�K�`����

    List<string> resultList;//�K�`�����ʕێ�
    private int RarityNum = 0;//�K�`���̍ő僌�A���e�B�ێ�

    static public int DirectingClickCount = 0;//�K�`�����o�N���b�N���擾
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

    private void StandbyMode()//�K�`���ҋ@���[�h
    {
        standbyPanel.SetActive(true);
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            PlayerPrefs.SetInt("paid", 1000);
            PlayerPrefs.SetInt("free", 50);
        }
    }

    public void StartGacha(int num)//�{�^���ŉ����ꂽ��N��
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
            //�t�F�[�h�C��
            fadeItemsCanvas[3].alpha += 0.02f;
            
        }
        if(!fadeBool)
        {
            //�t�F�[�h�A�E�g
            standbyPanel.SetActive(false);
            fadeItemsCanvas[3].alpha -= 0.02f;
            if (fadeItemsCanvas[3].alpha <= 0.0f)
            {
                //�t�F�[�h�I�u�W�F�N�g�������Ď���state��
                fadeBool = true;
                fadeItems[3].SetActive(false);
                state = STATE.STARTDIRECTINGMODE;
            }
        }

        

    }

    private void StartDirectingMode()//�K�`�����o���[�h�J�n���̏���
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


    private void DirectingMode()//�K�`�����o���[�h
    {
        
        if (DirectingClickCount >= directingFlag)
        {
            DirectingPanel.SetActive(false);
            state = STATE.OUTFADEMODE;
        }
    }

    public void DirectingClick()//�K�`�����o�̃A�j���[�V�������Đ�����
    {
        hammerAnimator.SetTrigger("Hammer_hit");
        DirectingPanel.SetActive(false);

    }


    private void OutFadeMode()//�K�`�����o��ʂ���K�`�����ʉ�ʂւ̈ړ�����Fade
    {
        DirectingPanel.SetActive(false);
        if(RarityNum <= 0)
        {
            fadeItems[RarityNum].SetActive(true);
            //�t�F�[�h�C��
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
            //�t�F�[�h�C��
            fadeItemsCanvas[RarityNum - 1].alpha += 0.02f;

            if (fadeItemsCanvas[RarityNum - 1].alpha >= 1.0f)
            {
                MainCamera.transform.position = new Vector3(0.0f, 0.0f, -80.0f);
                MainCamera.transform.rotation = Quaternion.Euler(30.0f, 180.0f, 0.0f);
                state = STATE.STARTDISPLAYMODE;
            }
        } 
    }

    private void StartDispayMode()//�K�`�����ʕ\�����[�h�J�n���̏���
    {
        Hammer.StoneNam = 0;
        Stones[0].SetActive(true);
        Stones[1].SetActive(false);
        Stones[2].SetActive(false);
        Stones[3].SetActive(false);
        DisplayPanel.SetActive(true);
        state = STATE.DISPLAYMODE;
    }

    private void DispayMode()//�K�`�����ʕ\�����[�h
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


    private void LayoutItem(int num)//���肵���A�C�e������ׂ�
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
            //�t�F�[�h�C��
            fadeItemsCanvas[3].alpha += 0.02f;

        }
        if (!fadeBool)
        {
            SceneManager.LoadScene(sceneNum);
            //�t�F�[�h�A�E�g
            //standbyPanel.SetActive(true);
            //DisplayPanel.SetActive(false);
            //fadeItemsCanvas[3].alpha -= 0.01f;
            //resultList.Clear();
            //if (fadeItemsCanvas[3].alpha == 0.0f)
            //{
            //    //�t�F�[�h�I�u�W�F�N�g�������Ď���state��
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
    private void GetGacha()//�K�`���V�X�e���{��
    {
        resultList = new List<string>();
        int totalProbability = 0;
        RarityNum = 0;
        for (int i = 0; i < rarity.Length; i++)
        {
            // ���A���e�B�̊m���𑫂����킹��
            totalProbability += _itemData.probability[i].num;
        }

        resultList = new List<string>(); // ���I���ʊi�[�p�ϐ�
        for (int i = 0; i < countPerPack; i++)
        {
            // ���I���s��
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

    private string getItems(int _probability)//�A�C�e�����擾
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
                return _itemData.nomalItemData[random.Next(0, _itemData.nomalItemData.Count)].name;//����A�C�e�����擾
            case 1:
                if (RarityNum == 0)
                {
                    RarityNum = 1;//����A�C�e���̍ō����A���e�B�̐��l���擾
                }
                return _itemData.rareItemData[random.Next(0, _itemData.rareItemData.Count)].name;//����A�C�e�����擾
            case 2:
                if (RarityNum <= 1)
                {
                    RarityNum = 2;//����A�C�e���̍ō����A���e�B�̐��l���擾
                }
                return _itemData.SRareItemData[random.Next(0, _itemData.SRareItemData.Count)].name;//����A�C�e�����擾
            case 3:
                if(RarityNum <= 2)
                {
                    RarityNum = 3;//����A�C�e���̍ō����A���e�B�̐��l���擾
                }
                return _itemData.SSRareItemData[random.Next(0, _itemData.SSRareItemData.Count)].name;//����A�C�e�����擾
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
