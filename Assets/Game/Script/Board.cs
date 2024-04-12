using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public int width;
    public int height;

    public GameObject bgTilePrefab;
    public Leaf[] leafs;

    public Leaf[,] allLeafs;

    public float leafSpeed;

    private UIManager uiMan;

    [HideInInspector]
    public MatchFinder matchFind;

    public enum BoardState {wait, move}
    public BoardState currentState = BoardState.move;

    public Leaf spider;
    public float spiderChance = 2f;

  

    [HideInInspector]
    public RoundManager roundMan;

    private float bonusMulti;
    public float bonusAmount = .5f;

    private BoardLayout boardLayout;
    private Leaf[,] layoutStore;





    private void Awake()
    {
        uiMan = FindObjectOfType<UIManager>();
        matchFind = FindObjectOfType<MatchFinder>();
        roundMan = FindObjectOfType<RoundManager>();
        boardLayout = GetComponent<BoardLayout>();
    }

    void Start()
    {
        allLeafs = new Leaf[width, height];

        layoutStore = new Leaf[width, height];

        Setup();


    }

    private void Update()
    {
        //matchFind.FindAllMatches();

        if (Input.GetKeyDown(KeyCode.S))
        {
            ShuffleBoard();
        }
    }

    private void Setup()
    {

        if (boardLayout != null)
        {
            layoutStore = boardLayout.GetLayout();
        }

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {

                Vector2 pos = new Vector2(x, y);
                GameObject bgTile = Instantiate(bgTilePrefab, pos, Quaternion.identity);
                bgTile.transform.parent = transform;
                bgTile.name = "BG Tile - " + x + " , " + y;

                if (layoutStore[x, y] != null)
                {
                    SpawnLeaf(new Vector2Int(x, y), layoutStore[x, y]);
                }
                else
                {

                    int leafToUse = Random.Range(0, leafs.Length);

                    int iteration = 0;

                    while (MatcheAt(new Vector2Int(x, y), leafs[leafToUse]) && iteration < 100)
                    {
                        leafToUse = Random.Range(0, leafs.Length);
                        iteration++;
                    }

                    SpawnLeaf(new Vector2Int(x, y), leafs[leafToUse]);
                }

            }
        }

    }

    private void SpawnLeaf(Vector2Int pos, Leaf leafToSpawn)
    {
        if (Random.Range(0f,100f) < spiderChance)
        {
            leafToSpawn = spider;
           
        }

        Leaf leaf = Instantiate(leafToSpawn, new Vector3(pos.x, pos.y + height, 0f), Quaternion.identity);
        leaf.transform.parent = transform;
        leaf.name = "Leaf - " + pos.x + " , " + pos.y;
        allLeafs[pos.x, pos.y] = leaf;

        leaf.SetupLeaf(pos, this);
    }

    bool MatcheAt(Vector2Int posToCheck, Leaf leafToCheck)
    {
        if (posToCheck.x > 1)
        {
            if (allLeafs[posToCheck.x - 1, posToCheck.y].type == leafToCheck.type && allLeafs[posToCheck.x - 2, posToCheck.y].type == leafToCheck.type)
            {
                return true;
            }
        }
        if (posToCheck.y > 1)
        {
            if (allLeafs[posToCheck.x, posToCheck.y - 1].type == leafToCheck.type && allLeafs[posToCheck.x, posToCheck.y - 2].type == leafToCheck.type)
            {
                return true;
            }
        }

        return false;
    }

    private void DestroyMatchedLeafAt(Vector2Int pos)
    {

        if (allLeafs[pos.x, pos.y] != null)
        {
            if (allLeafs[pos.x, pos.y].isMatched)
            {
                Instantiate(allLeafs[pos.x, pos.y].destroyEffect, new Vector2(pos.x, pos.y), Quaternion.identity);
                Destroy(allLeafs[pos.x, pos.y].gameObject);
                allLeafs[pos.x, pos.y] = null;
            }
        }
    }

    public void DestroyMatches()
    {
        for (int i = 0; i < matchFind.currentMatches.Count; i++)
        {
            if (matchFind.currentMatches[i] != null)
            {
                ScoreCheck(matchFind.currentMatches[i]);
                DestroyMatchedLeafAt(matchFind.currentMatches[i].posIndex);
            }
        }

        StartCoroutine(DecreaseRowCo());
    }

    private IEnumerator DecreaseRowCo()
    {

        yield return new WaitForSeconds(.2f);

        int nullCounter = 0;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (allLeafs[x, y] == null)
                {
                    nullCounter++;
                }
                else if (nullCounter > 0)
                {
                    allLeafs[x, y].posIndex.y -= nullCounter;
                    allLeafs[x, y - nullCounter] = allLeafs[x, y];
                    allLeafs[x, y] = null;
                }

            }

            nullCounter = 0;
        }

        StartCoroutine(FillBoardCo());
    }

    private IEnumerator FillBoardCo()
    {

        yield return new WaitForSeconds(.5f);
        ReFillBoard();

        yield return new WaitForSeconds(.5f);

        matchFind.FindAllMatches();

        if (matchFind.currentMatches.Count > 0)
        {          
            bonusMulti++;
         
            yield return new WaitForSeconds(.5f);
            DestroyMatches();

        }
        else
        {
      
            yield return new WaitForSeconds(.5f);
            currentState = BoardState.move;

            bonusMulti = 0f;

        }

        if (matchFind.currentMatches.Count > 0)
        {
            uiMan.happyDog.SetActive(true);
        }
        
        if (bonusMulti == 0f)
        {
            uiMan.happyDog.SetActive(false);
        }

  
    }

    private void ReFillBoard()
    {

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (allLeafs[x, y] == null)
                {
                    int leafToUse = Random.Range(0, leafs.Length);

                    SpawnLeaf(new Vector2Int(x, y), leafs[leafToUse]);
                }

            }
        }

        CheckMisplacedLeaf();
    }

    private void CheckMisplacedLeaf()
    {
        List<Leaf> foundLeafs = new List<Leaf>();

        foundLeafs.AddRange(FindObjectsOfType<Leaf>());

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {

                if (foundLeafs.Contains(allLeafs[x,y]))
                {
                    foundLeafs.Remove(allLeafs[x, y]);
                }

            }
        }

        foreach (Leaf g in foundLeafs)
        {
            Destroy(g.gameObject);
        }
    }

    public void ShuffleBoard() 
    {

        if (currentState != BoardState.wait)
        {
            currentState = BoardState.wait;

            List<Leaf> leafsFromBoard = new List<Leaf>();

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    leafsFromBoard.Add(allLeafs[x, y]);
                    allLeafs[x, y] = null;
                }
            }

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    int leafToUse = Random.Range(0, leafsFromBoard.Count);

                    int iteration = 0;

                    while(MatcheAt(new Vector2Int(x,y), leafsFromBoard[leafToUse]) && iteration < 100 && leafsFromBoard.Count > 1 ) 
                    {
                        leafToUse = Random.Range(0, leafsFromBoard.Count);
                        iteration++;

                    }

                    leafsFromBoard[leafToUse].SetupLeaf(new Vector2Int(x, y), this);
                    allLeafs[x, y] = leafsFromBoard[leafToUse];

                    leafsFromBoard.RemoveAt(leafToUse);
                }
            }

            StartCoroutine(FillBoardCo());
        }
    
    }

    public void ScoreCheck(Leaf leafToCheck) 
    {
        
        roundMan.currentScore += leafToCheck.scoreValue;

        if (bonusMulti > 0)
        {
            float bonusToAdd = leafToCheck.scoreValue * bonusMulti * bonusAmount;
            roundMan.currentScore += Mathf.RoundToInt(bonusToAdd);
           
        }

    }

}

