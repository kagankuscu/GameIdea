using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetGame : MonoBehaviour
{
    [SerializeField] GameObject[] _enemiesSpawnPoint;
    [SerializeField] GameObject _enemyhPrefab;


    public void ResetTheGame()
    {
        for (int i = 0; i < _enemiesSpawnPoint.Length; i++)
        {
            Instantiate(_enemyhPrefab, _enemiesSpawnPoint[i].transform.position, Quaternion.identity);
        }
    }
}
