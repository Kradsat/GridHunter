using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSystem : MonoBehaviour
{
    [SerializeField] private GameObject[] playerPrefab;

    public static GameObject[] spawnPrefab = new GameObject[4];

    private void Start()
    {
        SpawnPlayer();
    }

    private void SpawnPlayer()
    {
        GridPosition playerPosition1 = new GridPosition(1,0);
        //GridPosition playerPosition2 = new GridPosition(2,0);
       
        playerPrefab[0] = Instantiate(spawnPrefab[0], LevelGrid.Instance.GetWorldPosition(playerPosition1), Quaternion.identity);
        //playerPrefab[1] = Instantiate(playerPrefab[1], LevelGrid.Instance.GetWorldPosition(playerPosition1), Quaternion.identity);

    }
}
