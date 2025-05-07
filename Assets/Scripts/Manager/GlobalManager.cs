using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalManager : MonoBehaviour
{
    public static GlobalManager Instance;

    public int BestWave {  get; private set; }

    private void Awake()
    {
        //if (Instance == null)
        //{
        //    Instance = this;
        //    DontDestroyOnLoad(gameObject);
        //    LoadData();
        //}
        //else
        //{
        //    Destroy(gameObject);
        //}
    }

    public void UpdateBestWave(int newWave)
    {
        if (newWave > BestWave)
        {
            BestWave = newWave;
            SaveData();
        }
    }

    private void SaveData()
    {
        PlayerPrefs.SetInt("BestWave", BestWave);
        PlayerPrefs.Save();
    }

    // 데이터 불러오기
    private void LoadData()
    {
        BestWave = PlayerPrefs.GetInt("BestWave", 0);
    }
}
