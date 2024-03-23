using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using GameArchitecture;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour
{
    public static GameMaster instance;

    public GameObject gameGround;
    public GameObject blockPrefab;
    public GameObject linePrefab;
    public ActionPanelScript actionMenu;
    public GameObject finishMenu;
    public GameObject gameOverMenu;
    public GameObject abilityPanel;
    public Text scoreText;
    public Text bestScoreText;
    public TextMeshProUGUI levelText;
    public Button buttonEnd;
    public Toggle toggle;

    public BlockController targetBlock;

    public int loadLevelInstead = 0;

    private int level;

    //private int choosenNumbersCount;
    private string abilityStr;
    public List<int> choosenNumbers = new();
    public int targetNumberIndex = 0;
    public List<int> targetSlotIndexes = new List<int>();
    public List<int> activeSlotIndexes = new List<int>();

    public GameObject[] block;
    public GameObject[] line;
    public GameObject tmp;
    public GameObject sss;
    private Vector3 mousePos;
    public bool pressed = false;
    public bool usingAbility = false;
    public int[] st = new int[40];
    public int n = 0;
    public int b = 0;
    System.Random rand = new System.Random();
    public static int score = 0;
    int[] s1 = { -1, 0, 1, 1, 1, 0, -1, -1 };
    int[] s2 = { 1, 1, 1, 0, -1, -1, -1, 0 };
    int cnt;
    float w;
    int matchWidthOrHeight = 0;
    int sh = 0;
    int toggleIsOn = 0;
    public int[] md = new int[40];

    public float downValue = 125;

    public List<string> numbers = new();

    private string[] notes = { "do", "re", "mi", "fa", "sol", "la", "si", "do2", };

    //2,0,1,5,3|3,1,0,0,1|1,0,0,0,0|0,0,0,0,0|0,0,0,0,0|0,0,0,0,0|0,0,0,0,0

    private string[] initialNumbers =
    {
        "2",
        "4",
        "8",
        "16",
        "32", //1
        "64", //2
        "128", //3
        "256",
        "512",
        "1024",
        "2048",
        "4096",
        "8192",
        "16K",
        "32K",
        "64K",
        "128K",
        "256K",
        "512K",
        "1M",
        "2M",
        "4M",
        "8M",
        "16M",
        "32M",
        "64M",
        "128M",
        "256M",
        "512M",
        "1B",
        "2B",
        "4B",
        "8B",
        "16B",
        "32B",
        "64B",
        "128B",
        "256B",
        "512B",
        "1T",
        "2T",
        "4T",
        "8T",
        "16T",
        "32T",
        "64T",
        "128T",
        "256T",
        "512T",
        "1Q",
        "2Q",
        "4Q",
        "8Q",
        "16Q",
        "32Q",
        "64Q", //56
        "128Q", //57
        "256Q", //58
        "512Q", //59
        "1S", //60
        "2S", //61
        "4S", //62
        "8S", //63
        "16S", //64
        "32S", //65
        "64S", //66
        "128S", //67
        "256S", //68
        "512S", //69
        "1O", //70
    };

    string grid = "";
    string targets = "";

    void SaveGrid()
    {
        string gridNew = "";
        for (int i = 6; i >= 0; --i)
        {
            for (int j = 0; j < 5; ++j)
            {
                gridNew += GetNumIndex(i + j * 7).ToString() + ",";
            }
            gridNew = gridNew.Substring(0, gridNew.Length - 1);
            gridNew += "|";
        }
        gridNew = gridNew.Substring(0, gridNew.Length - 1);
        print(gridNew);
        PlayerPrefs.SetString("Level" + level.ToString(), gridNew);
    }

    void SaveTargets()
    {
        string targetsNew = "";
        foreach (var targetSlotIndex in targetSlotIndexes)
        {
            targetsNew += targetSlotIndex.ToString() + "-";
            if (activeSlotIndexes.Contains(targetSlotIndex))
            {
                targetsNew += "1";
            }
            else
            {
                targetsNew += "0";
            }
            targetsNew += "|";
        }
        targetsNew = targetsNew.Substring(0, targetsNew.Length - 1);
        print(targetsNew);
        PlayerPrefs.SetString("LevelTarget" + level.ToString(), targetsNew);
    }

    private void Awake() { }

    void Start()
    {
        foreach (var num in initialNumbers)
        {
            numbers.Add(num);
        }
        instance = this;

        level = PlayerPrefs.GetInt("currentLevel", 1);
        if (loadLevelInstead > 0)
        {
            level = loadLevelInstead;
        }

        levelText.text = "Level " + level.ToString();

        targets = PlayerPrefs.GetString("LevelTarget" + level.ToString()); //0-1|3-0       it means there is 2 slots active (0 and 3) and 0th one is active and third one is not active

        if (targets == "")
        {
            if (level <= 60)
            {
                int randIndex = rand.Next(0, 4);
                targetSlotIndexes.Add(randIndex);
                actionMenu.OpenTarget(randIndex);
            }
            else
            {
                int randIndex = rand.Next(0, 4);
                int randIndex2 = rand.Next(0, 4);
                while (randIndex == randIndex2)
                {
                    randIndex2 = rand.Next(0, 4);
                }
                targetSlotIndexes.Add(randIndex);
                targetSlotIndexes.Add(randIndex2);
                actionMenu.OpenTarget(randIndex);
                actionMenu.OpenTarget(randIndex2);
            }
            SaveTargets();
        }
        else
        {
            foreach (var target in targets.Split("|"))
            {
                var targetIndex = Int16.Parse(target.Split("-")[0]);
                var targetActive = Int16.Parse(target.Split("-")[1]);
                print(targetIndex);
                print(targetActive);
                actionMenu.OpenTarget(targetIndex);
                if (targetActive == 1)
                {
                    activeSlotIndexes.Add(targetIndex);
                    actionMenu.ActivateTarget(targetIndex);
                }
                targetSlotIndexes.Add(targetIndex);
            }
        }

        grid = PlayerPrefs.GetString("Level" + level.ToString());

        if (level <= 60)
        {
            targetNumberIndex = level + 3;
        }
        else
        {
            targetNumberIndex = level - 55;
        }

        targetBlock.SetNumIndex(targetNumberIndex);

        List<int[]> gridList = new List<int[]>();
        if (grid != "")
        {
            foreach (var row in grid.Split("|"))
            {
                List<int> rowList = new();
                foreach (var num in row.Split(","))
                {
                    print(num);
                    rowList.Add(Int16.Parse(num));
                }
                gridList.Add(rowList.ToArray());
            }
        }

        score = 0;
        block = new GameObject[35];
        line = new GameObject[70];

        for (int i = 0; i < 5; ++i)
        for (int j = 0; j < 7; ++j)
        {
            int x = i * 7 + j;
            block[x] = Instantiate(blockPrefab) as GameObject;
            block[x].transform.SetParent(actionMenu.gameObject.transform, false);

            if (grid == "")
            {
                SetNumIndex(x, RandIndex());
            }
            else
            {
                SetNumIndex(x, gridList[6 - j][i]);
                //SetNumIndex(x, x+10);
            }
            SetPosition(x, i, j + 4);
            MoveDown(x, 4);
        }
        StartCoroutine(Up());
        StartCoroutine(UpCheck());
        //bestScoreText.text = "Best Score: " + MenuController.bestScore.ToString();
    }

    void SetNumIndex(int x, int t)
    {
        block[x].GetComponent<BlockController>().SetNumIndex(t);
    }

    int GetNumIndex(int x)
    {
        return block[x].GetComponent<BlockController>().GetNumIndex();
    }

    void SetPosition(int x, int i, int j)
    {
        //StartCoroutine(block[x].GetComponent<BlockController>().SetPosition(i, j));
        block[x].GetComponent<BlockController>().SetPosition(i, j);
    }

    void MovePosition(int x, Vector3 pos, float time)
    {
        Vector2 v = new Vector2(pos.x, pos.y);
        block[x].GetComponent<BlockController>().MovePosition(v, time);
    }

    void MoveDown(int x, int cnt)
    {
        Vector3 pos = block[x].GetComponent<RectTransform>().anchoredPosition;
        Vector2 v = new Vector2(pos.x, pos.y - cnt * downValue);
        block[x].GetComponent<BlockController>().MovePosition(v, 0.4f);
    }

    void Choose(int x)
    {
        block[x].GetComponent<BlockController>().Choose();
    }

    void Unchoose(int x)
    {
        block[x].GetComponent<BlockController>().Unchoose();
    }

    void Click(int x)
    {
        print("click: " + x);
        block[x].GetComponent<BlockController>().Click();
    }

    void Unclick(int x)
    {
        print("unclick: " + x);
        block[x].GetComponent<BlockController>().Unclick();
    }

    public void AbilityOpen(string ability)
    {
        if (DiamondManager.instance.GetDiamond() < 200)
        {
            return;
        }
        abilityStr = ability;
        abilityPanel.SetActive(true);
        usingAbility = true;
    }

    public void AbilityClose()
    {
        abilityPanel.SetActive(false);
        usingAbility = false;

        actionMenu.CloseSlots();
        foreach (var choosenNumber in choosenNumbers)
        {
            block[choosenNumber].GetComponent<BlockController>().Unchoose();
        }
        choosenNumbers.Clear();
    }

    public IEnumerator CoAbilityOpen()
    {
        yield return new WaitForSeconds(0.1F);
    }

    public void Swap(int x, int y)
    {
        Vector2 xPos = block[x].GetComponent<RectTransform>().anchoredPosition;
        Vector2 yPos = block[y].GetComponent<RectTransform>().anchoredPosition;

        MovePosition(x, yPos, 0.2f);
        MovePosition(y, xPos, 0.2f);

        GameObject _tmp = block[x];
        block[x] = block[y];
        block[y] = _tmp;

        SaveGrid();
    }

    bool Getclicked(int x)
    {
        return block[x].GetComponent<BlockController>().GetClicked();
    }

    bool GetChoosen(int x)
    {
        return block[x].GetComponent<BlockController>().GetChoosen();
    }

    bool MouseInBlock(float x, float y)
    {
        if (mousePos.x < x - w || mousePos.x > x + w)
            return false;
        if (mousePos.y < y - w || mousePos.y > y + w)
            return false;
        return true;
    }

    bool CheckAdject(int x, int y)
    { //CheckAdject(st[n-1], b)
        Vector2 xx = new Vector2(x / 7, x % 7);
        Vector2 yy = new Vector2(y / 7, y % 7);
        if (
            (Math.Abs(xx.x - yy.x) + Math.Abs(xx.y - yy.y) == 1)
            || Mathf.Abs(x - y) == 8
            || Mathf.Abs(x - y) == 6
        )
            return true;
        return false;
    }

    IEnumerator Scoring(int sum)
    {
        int oldscore = score;
        score += sum;

        if (score - oldscore < 10)
        {
            for (int i = oldscore + 1; i <= score; ++i)
            {
                //scoreText.text = i.ToString();
                yield return new WaitForSeconds(0.07f);
            }
        }
        else
        {
            float xxx = (float)(score - oldscore) / 10;
            float yyy = oldscore;
            for (int i = 0; i < 10; ++i)
            {
                yyy += xxx;
                int zzz = (int)yyy;
                scoreText.text = zzz.ToString();
                yield return new WaitForSeconds(0.07f);
            }
        }
        scoreText.text = score.ToString();
    }

    int RandIndex()
    {
        int x;
        if (toggleIsOn == 0)
            x = rand.Next(1, 5);
        else
            x = rand.Next(58, 63);
        return x - 1;
    }

    bool CheckEndgame()
    {
        for (int i = 0; i < 5; ++i)
        for (int j = 0; j < 7; ++j)
        {
            int b = i * 7 + j;
            for (int k = 1; k < 8; k += 2)
            {
                int x = i + s1[k];
                int y = j + s2[k];
                int z = x * 7 + y;
                if (x < 0 || x >= 5)
                    continue;
                if (y < 0 || y >= 7)
                    continue;
                if (GetNumIndex(b) == GetNumIndex(z))
                    return false;
            }
        }
        return true;
    }

    bool CheckFinished(){
        return activeSlotIndexes.Count == targetSlotIndexes.Count;
    }

    IEnumerator waiter(float ss)
    {
        yield return new WaitForSeconds(20f);
    }

    void Match(int n)
    {
        if (n <= 1)
            return;
        line[n - 1] = Instantiate(linePrefab) as GameObject;
        line[n - 1].transform.SetParent(actionMenu.gameObject.transform, false);

        Vector3 p1 = block[st[n - 1]].GetComponent<RectTransform>().anchoredPosition;
        Vector3 p2 = block[st[n - 2]].GetComponent<RectTransform>().anchoredPosition;
        line[n - 1].GetComponent<RectTransform>().localRotation = Quaternion.FromToRotation(
            Vector3.left,
            p2 - p1
        );
        Vector2 p3 = new Vector2();
        Vector3 p4 = new Vector3();
        p3.x = (p1.x + p2.x) / 2;
        p3.y = (p1.y + p2.y) / 2;

        p4.x = (p3.x + p2.x) / 2;
        p4.y = (p3.y + p2.y) / 2;
        //Debug.Log(p3.x + " " + p3.y + " / " + p4.x + " " + p4.y);

        line[n - 1].GetComponent<RectTransform>().anchoredPosition = p4;
        line[n - 1].GetComponent<RectTransform>().DOAnchorPos(p3, 0.1f);
        PlayNote();
    }

    void Unmatch(int n)
    {
        if (n <= 1)
            return;
        Vector3 p1 = block[st[n - 1]].GetComponent<RectTransform>().anchoredPosition;
        Vector3 p2 = block[st[n - 2]].GetComponent<RectTransform>().anchoredPosition;
        Vector2 p4 = new Vector2();

        p4.x = (p1.x + 3 * p2.x) / 4;
        p4.y = (p1.y + 3 * p2.y) / 4;
        //Debug.Log(p4.x + " " + p4.y);
        line[n - 1].GetComponent<RectTransform>().DOAnchorPos(p4, 0.2f);
        Destroy(line[n - 1], 0.1f);
        PlayNote();
    }

    void PlayNote()
    {
        int audioIndex = 0;
        if (n > notes.Length)
        {
            audioIndex = notes.Length - 1;
        }
        else
        {
            audioIndex = n - 1;
        }
        AudioManager.Instance.PlayClipOneShot(notes[audioIndex]);
    }

    int clickedBlock()
    {
        Vector3 p1 = sss.GetComponent<Transform>().position;
        float xx = p1.x;
        float yy = p1.y;

        w = xx * 50f / 720f;

        b = -1;

        for (int i = 0; i < 35; ++i)
        {
            Vector3 p = block[i].GetComponent<Transform>().position;
            if (MouseInBlock(p.x, p.y))
            {
                b = i;
                break;
            }
        }
        return b;
    }

    IEnumerator CoDisappear(int b)
    {
        block[b]
            .GetComponent<Transform>()
            .DOShakePosition(0.6f, new Vector3(6f, 3f, 3f), 15, 30f, false, true);

        yield return new WaitForSeconds(0.6f);
        block[b].GetComponent<CanvasGroup>().DOFade(0, 0.5f);
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < 5; ++i)
        {
            int p = 0;
            int cnt = 0;
            for (int j = 0; j < 7; ++j)
            {
                int x = i * 7 + j;
                int y = i * 7 + p;

                if (GetChoosen(x) == false)
                {
                    tmp = block[x];
                    block[x] = block[y];
                    block[y] = tmp;
                    md[y] = cnt;
                    p++;
                }
                else
                {
                    Unchoose(x);
                    cnt++;
                }
            }

            for (int j = p; j < 7; ++j)
            {
                int x = i * 7 + j;
                SetNumIndex(x, RandIndex());
                SetPosition(x, i, j + cnt);
                md[x] = cnt;
            }
        }

        for (int i = 0; i < 35; ++i)
        {
            block[i].GetComponent<CanvasGroup>().alpha = 1;
            MoveDown(i, md[i]);
        }
    }

    IEnumerator UpCheck()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.01f);

            foreach (var item in targetSlotIndexes)
            {
                int index = item * 7;
                if (
                    block[index].GetComponent<BlockController>().GetNumIndex() == targetNumberIndex
                    && activeSlotIndexes.Contains(item) == false
                )
                {
                    choosenNumbers.Add(index);
                    block[index].GetComponent<BlockController>().Choose();
                    StartCoroutine(CoDisappear(index));
                    activeSlotIndexes.Add(item);
                    actionMenu.ActivateTarget(item);
                    SaveTargets();
                    yield return new WaitForSeconds(2);
                    
                    SaveGrid();
                    print(index + " BROOOOOOOOOOOO");
                }
            }
        }
    }

    IEnumerator Up()
    {
        yield return new WaitForSeconds(1);
        while (true)
        {
            yield return new WaitForSeconds(0.01f);

            //if (pressed) Debug.Log(Time.time + ".........................");
            //  else Debug.Log(Time.time + "**");

            if (usingAbility && pressed)
            {
                int b = clickedBlock();
                if (b == -1)
                {
                    continue;
                }
                BlockController blockController = block[b].GetComponent<BlockController>();

                switch (abilityStr)
                {
                    case "swap":
                        int slotIndex = 6 - (b % 7);
                        if (
                            actionMenu.activeSlotIndex != -1
                            && slotIndex != actionMenu.activeSlotIndex
                        )
                        {
                            continue;
                        }

                        if (blockController.choosen)
                        {
                            choosenNumbers.Remove(b);
                            if (choosenNumbers.Count == 0)
                            {
                                actionMenu.CloseSlots();
                            }
                            AudioManager.Instance.PlayClip("unchoose");
                            blockController.Unchoose();
                        }
                        else
                        {
                            choosenNumbers.Add(b);
                            actionMenu.OpenSlot(slotIndex);
                            AudioManager.Instance.PlayClip("choose");
                            blockController.Choose();
                        }

                        if (choosenNumbers.Count == 2)
                        {
                            Swap(choosenNumbers[0], choosenNumbers[1]);
                            yield return new WaitForSeconds(0.5f);
                            AbilityClose();
                            DiamondManager.instance.RemoveDiamond(200);
                            //continue;
                        }
                        break;
                    case "delete":
                        choosenNumbers.Add(b);
                        blockController.Choose();
                        AudioManager.Instance.PlayClip("choose");

                        StartCoroutine(CoDisappear(b));

                        yield return new WaitForSeconds(2);

                        SaveGrid();
                        AbilityClose();

                        DiamondManager.instance.RemoveDiamond(200);

                        break;
                }

                yield return new WaitForSeconds(0.2F);
                continue;
            }

            if (pressed)
            {
                int b = clickedBlock();
                if (b == -1)
                {
                    continue;
                }

                if (n > 1 && st[n - 2] == b)
                {
                    Unmatch(n);

                    Unclick(st[n - 1]);
                    n--;
                    continue;
                }

                if (Getclicked(b) == false)
                    if (n == 0 || CheckAdject(st[n - 1], b)) // KONUMUN DOGRULUGU
                        if (
                            n == 0
                            || GetNumIndex(b) == GetNumIndex(st[n - 1])
                            || GetNumIndex(b) == GetNumIndex(st[n - 1]) + 1
                        ) //SAYININ DOGRULUGU
                            if (n != 1 || GetNumIndex(b) == GetNumIndex(st[0]))
                            {
                                Click(b);
                                //FindObjectOfType<AudioManager>().Play("Turn");
                                st[n] = b;
                                n++;
                                Match(n);
                            }
            }
            else
            {
                if (usingAbility)
                {
                    continue;
                }
                if (n == 1)
                {
                    Unclick(st[0]);
                }

                if (n > 1)
                {
                    //FindObjectOfType<AudioManager>().Play("Delete");
                    //for (int i = 1; i < n; ++i) Destroy(line[i]);
                    //for (int i = 0; i < n-1; ++i) MovePosition(st[i], block[st[n-1]].GetComponent<RectTransform>().anchoredPosition);
                    //yield return new WaitForSeconds(0.45f);
                    //cnt1++; Debug.Log(cnt1);
                    for (int i = 1; i < n; ++i)
                    {
                        Destroy(line[i]);
                        for (int j = 0; j < i; ++j)
                            MovePosition(
                                st[j],
                                block[st[i]].GetComponent<RectTransform>().anchoredPosition,
                                0.08f
                            );
                        yield return new WaitForSeconds(0.08f);
                    }

                    ulong sum = 0;
                    int k = 0;
                    for (int i = 0; i < n; ++i)
                        sum += (ulong)(Mathf.Pow(2, GetNumIndex(st[i]) + 1));

                    print("Sum: " + sum.ToString());
                    AudioManager.Instance.PlayClipWithRandomPitch("merge", 1.1f, 1.2f);
                    //StartCoroutine(Scoring(sum));
                    ulong s = 2;
                    int sindex = 0;
                    while (s <= sum)
                    {
                        sindex++;
                        s *= 2;
                    }
                    sindex--;
                    s /= 2;
                    print("S: " + s.ToString());
                    print("Sindex: " + sindex.ToString());

                    Unclick(st[n - 1]);

                    //AŞA İNDİRİYOR YENİ OLUŞANI GÖNDERİYOR
                    for (int i = 0; i < 5; ++i)
                    {
                        int p = 0;
                        int cnt = 0;
                        for (int j = 0; j < 7; ++j)
                        {
                            int x = i * 7 + j;
                            int y = i * 7 + p;
                            //print(x + " " + y);

                            if (Getclicked(x) == false)
                            {
                                tmp = block[x];
                                block[x] = block[y];
                                block[y] = tmp;
                                md[y] = cnt;
                                p++;
                                if (x == st[n - 1])
                                    k = y;
                            }
                            else
                            {
                                Unclick(x);
                                cnt++;
                            }
                        }

                        for (int j = p; j < 7; ++j)
                        {
                            int x = i * 7 + j;
                            SetNumIndex(x, RandIndex());
                            SetPosition(x, i, j + cnt);
                            print(x + " " + i + " " + (j + cnt));
                            md[x] = cnt;
                        }
                    }

                    //Debug.Log(k);

                    //yield return new WaitForSeconds(1f);
                    block[k].GetComponent<BlockController>().SetNumIndex(sindex);
                    for (int i = 0; i < 35; ++i)
                        MoveDown(i, md[i]);

                    SaveGrid();

                    //yield return new WaitForSeconds(0.2f);
                    //cnt1++; Debug.Log(cnt1);
                }
                n = 0;

                if (CheckEndgame())
                {
                    yield return new WaitForSeconds(2f);
                    for (int i = 0; i < 35; ++i)
                        block[i]
                            .GetComponent<Transform>()
                            .DOShakePosition(2f, new Vector3(6f, 3f, 3f), 5, 20f, false, true);
                    yield return new WaitForSeconds(1.8f);
                    gameOverMenu.SetActive(true);
                }
            }

            if (CheckFinished())
            {
                yield return new WaitForSeconds(2f);
                    for (int i = 0; i < 35; ++i)
                        block[i]
                            .GetComponent<Transform>()
                            .DOShakePosition(2f, new Vector3(6f, 3f, 3f), 5, 20f, false, true);
                    yield return new WaitForSeconds(1.8f);
                finishMenu.SetActive(true);
                break;
            }
        }
        Debug.Log("ENDED");
    }

    void Update()
    {
        /*if (Camera.main.aspect > 16f / 9f) sh = 1;
            else sh = 0;
        if (matchWidthOrHeight != sh) {
            matchWidthOrHeight = sh;
            actionMenu.GetComponent<CanvasScaler>().matchWidthOrHeight = sh;
        }*/

        //if (Input.GetMouseButtonDown(0)) pressed = true;
        //if (Input.GetMouseButtonUp(0)) pressed = false;
        //mousePos = Input.mousePosition;
        //Debug.Log(mousePos.x + " " + mousePos.y);



        if (Input.touchCount > 0)
        {
            pressed = true;
            Touch _touch = Input.GetTouch(0);
            mousePos = _touch.position;
        }
        else
            pressed = false;
    }
}
