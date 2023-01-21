using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : MonoBehaviour
{
    [SerializeField]
    private Animator hammerAnimator;
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip hitSound;

    [SerializeField]
    private GameObject DirectingDisplay;

    [SerializeField]
    private GameObject[] Stones;

    public static int StoneNam = 0;
    // Start is called before the first frame update

    public void Hammer_Sound()
    {
        audioSource.PlayOneShot(hitSound);
    }
    public void Hammer_Hit()
    {
        
        Stones[StoneNam].SetActive(false);
        Stones[StoneNam + 1].SetActive(true);
        StoneNam++;
    }
    public void Hammer_Store()
    {
        GachaSystem.DirectingClickCount += 1;
        DirectingDisplay.SetActive(true);  
    }
}
