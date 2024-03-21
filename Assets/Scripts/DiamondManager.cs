using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiamondManager : MonoBehaviour
{
    public static DiamondManager instance;
    public bool stop = false;
    private int diamond;
    [SerializeField]private int startDiamond = 5;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

        GetDiamond();
    }


    public int GetDiamond(){
        diamond = PlayerPrefs.GetInt("diamond", startDiamond);
        return diamond;
    }

    public void AddDiamond(int by)
    {
        GetDiamond();
        PlayerPrefs.SetInt("diamond", diamond + by);
    }

    public void RemoveDiamond(int by)
    {
        GetDiamond();
        PlayerPrefs.SetInt("diamond", diamond - by);
    }
}
