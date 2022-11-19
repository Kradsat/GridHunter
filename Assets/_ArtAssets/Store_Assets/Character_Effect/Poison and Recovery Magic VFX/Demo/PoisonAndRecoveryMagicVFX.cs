//====================Copyright statement:AppsTools===================//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace PoisonAndRecoveryMagicVFXSpace
{
    public class PoisonAndRecoveryMagicVFX : MonoBehaviour
    {
        private bool r = true;
        string[] allArray = null;

        public int i = 0;
        public UnityEngine.UI.Text tex;
        public Transform ts;
        private GameObject currObj;
        public Transform hideParent;

        public void Awake()
        {
            allArray = new string[hideParent.childCount];
            for (int i = 0; i < hideParent.childCount; i++)
            {
                allArray[i] = hideParent.GetChild(i).gameObject.name;
            }
            currObj = GameObject.Instantiate(hideParent.transform.Find(allArray[i]).gameObject);
            currObj.transform.SetParent(ts);
            //currObj.transform.localPosition = Vector3.zero;
            tex.text = "Name: " + i + " [" + allArray[i] + "]";
        }

        public void Update()
        {
            if (ts != null && r)
            {
                ts.transform.Rotate(Vector3.up * Time.deltaTime * 90f);
            }
        }

        public void R()
        {
            r = true;
        }

        public void NotR()
        {
            r = false;
        }

        public void RePlay()
        {
            if (currObj != null)
            {
                currObj.SetActive(false);
                currObj.SetActive(true);
            }
        }

        public void CopyName()
        {
            var s = allArray[i].ToLower().Replace(".prefab", "");
            s = s.Substring(s.IndexOf("/") + 1);
            UnityEngine.GUIUtility.systemCopyBuffer = s;
        }

        public void OnLeftBtClick()
        {
            i--;
            if (i < 0)
            {
                i = allArray.Length - 1;
            }
            if (currObj != null)
            {
                GameObject.DestroyImmediate(currObj);
            }
            currObj = GameObject.Instantiate(hideParent.transform.Find(allArray[i]).gameObject);
            currObj.transform.SetParent(ts);
            //currObj.transform.localPosition = Vector3.zero;
            tex.text = "Name: " + i + " [" + allArray[i] + "]";
            if (i == 0)
            {
                r = true;
            }
            else 
            {
                r = false;
            }
        }

        public void OnRightBtClick()
        {
            i++;
            if (i >= allArray.Length)
            {
                i = 0;
            }
            if (currObj != null)
            {
                GameObject.DestroyImmediate(currObj);
            }
            currObj = GameObject.Instantiate(hideParent.transform.Find(allArray[i]).gameObject);
            currObj.transform.SetParent(ts);
            //currObj.transform.localPosition = Vector3.zero;
            tex.text = "Name: " + i + " [" + allArray[i] + "]";
            if (i == 0)
            {
                r = true;
            }
            else
            {
                r = false;
            }
        }
    }
}