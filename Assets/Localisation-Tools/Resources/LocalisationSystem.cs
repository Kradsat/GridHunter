using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalisationSystem
{
    public enum Language
    {
        Japanese,
        English 
    }

    public static Language language = Language.Japanese;

    private static Dictionary<string, string> localisedJP;
    private static Dictionary<string, string> localisedEN;

    public static bool isInit = false;

    public static csvLoader CSVLoader;
    public static void Init()
    {
        CSVLoader = new csvLoader();
        CSVLoader.loadCSV();

        UpdateDictionnaries();
       
        isInit = true;
    }

    public static void UpdateDictionnaries()
    {
        localisedJP = CSVLoader.GetDictionaryValues((int)Language.Japanese);
        localisedEN = CSVLoader.GetDictionaryValues((int)Language.English);
    }

    public static Dictionary<string, string> GetDictionaryForEditor()
    {
        if(!isInit) { Init();  }
        return localisedEN;
    }

    public static string GetLocalisedValue(string key)
    {
        if (!isInit) { Init();  }

        string value = key;

     switch(language)
        {
            case Language.Japanese:
                localisedJP.TryGetValue(key, out value);
                break;
            case Language.English:
                localisedEN.TryGetValue(key, out value);
                break;
        }
        return value;
    }

    public static void Add(string key, string jp_value, string en_value)
    {
        if (CSVLoader == null)
        {
            CSVLoader = new csvLoader();
        }

        CSVLoader.loadCSV();
        CSVLoader.Add(key, jp_value, en_value);
        CSVLoader.loadCSV();

        UpdateDictionnaries();
    }

    public static void Replace(string key,string jp_value, string en_value)
    {
        if (CSVLoader == null)
        {
            CSVLoader = new csvLoader();
        }

        CSVLoader.loadCSV();
        CSVLoader.Edit(key, jp_value, en_value);
        CSVLoader.loadCSV();

        UpdateDictionnaries();
    }

    public static void Remove(string key)
    {
      

        if (CSVLoader == null)
        {
            CSVLoader = new csvLoader();
        }

        CSVLoader.loadCSV();
        CSVLoader.Remove(key);
        CSVLoader.loadCSV();

        UpdateDictionnaries();
    }
}
