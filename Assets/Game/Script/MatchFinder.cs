using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class MatchFinder : MonoBehaviour
{
    private UIManager uiMan;
    private Board board;
    public List<Leaf> currentMatches = new List<Leaf>();

    private void Awake()
    {
        board = FindObjectOfType<Board>();
        uiMan = FindObjectOfType<UIManager>();
    }



    public void FindAllMatches()
    {
        currentMatches.Clear();
        for (int x = 0; x < board.width; x++)
        {
            for (int y = 0; y < board.height; y++)
            {
                Leaf currentLeaf = board.allLeafs[x, y];
                if (currentLeaf != null)
                {
                    if (x > 0 && x < board.width - 1)
                    {
                        Leaf leftLeaf = board.allLeafs[x - 1, y];
                        Leaf rightLeaf = board.allLeafs[x + 1, y];
                        if (leftLeaf != null && rightLeaf != null)
                        {
                            if (leftLeaf.type == currentLeaf.type && rightLeaf.type == currentLeaf.type)
                            {
                                currentLeaf.isMatched = true;
                                leftLeaf.isMatched = true;
                                rightLeaf.isMatched = true;

                                currentMatches.Add(currentLeaf);
                                currentMatches.Add(leftLeaf);
                                currentMatches.Add(rightLeaf);

                            }
                        }
                    }

                    if (y > 0 && y < board.height - 1)
                    {
                        Leaf aboveLeaf = board.allLeafs[x, y + 1];
                        Leaf belowLeaf = board.allLeafs[x, y - 1];
                        if (aboveLeaf != null && belowLeaf != null)
                        {
                            if (aboveLeaf.type == currentLeaf.type && belowLeaf.type == currentLeaf.type )
                            {
                                currentLeaf.isMatched = true;
                                aboveLeaf.isMatched = true;
                                belowLeaf.isMatched = true;

                                currentMatches.Add(currentLeaf);
                                currentMatches.Add(aboveLeaf);
                                currentMatches.Add(belowLeaf);


                            }
                        }
                    }

                }
            }

        }
        if (currentMatches.Count > 0)
        {
            currentMatches = currentMatches.Distinct().ToList();

        }

        CheckForSpiders();
    }

    public void CheckForSpiders() 
    {
        for (int i = 0; i < currentMatches.Count; i++)
        {
            Leaf leaf = currentMatches[i];

            int x = leaf.posIndex.x;
            int y = leaf.posIndex.y;

            if (leaf.posIndex.x > 0)
            {
                if (board.allLeafs[x - 1, y] != null)
                {
                    if (board.allLeafs[x-1,y].type == Leaf.LeafType.spider)
                    {
                        MarkSpiderArea(new Vector2Int(x - 1, y), board.allLeafs[x-1,y]);
                        
                    }
                }
            }

            if (leaf.posIndex.x < board.width - 1)
            {
                if (board.allLeafs[x + 1, y] != null)
                {
                    if (board.allLeafs[x + 1, y].type == Leaf.LeafType.spider)
                    {
                        MarkSpiderArea(new Vector2Int(x + 1, y), board.allLeafs[x + 1, y]);
                    }
                }
            }

            if (leaf.posIndex.y > 0)
            {
                if (board.allLeafs[x , y - 1] != null)
                {
                    if (board.allLeafs[x, y - 1 ].type == Leaf.LeafType.spider)
                    {
                        MarkSpiderArea(new Vector2Int(x, y - 1 ), board.allLeafs[x, y - 1]);
                    }
                }
            }

            if (leaf.posIndex.y < board.height - 1)
            {
                if (board.allLeafs[x, y + 1] != null)
                {
                    if (board.allLeafs[x, y + 1].type == Leaf.LeafType.spider)
                    {
                        MarkSpiderArea(new Vector2Int(x, y + 1), board.allLeafs[x, y + 1]);
                        
                    }
                }
            }
        }
    }

    public void MarkSpiderArea(Vector2Int spiderPos, Leaf theSpider ) 
    {
        for (int x = spiderPos.x - theSpider.blastSize; x <= spiderPos.x + theSpider.blastSize; x++ )
        {
            for (int y = spiderPos.y - theSpider.blastSize; y <= spiderPos.y + theSpider.blastSize; y++)
            {
                if (x >= 0 && x < board.width && y >= 0 && y < board.height)
                {
                    if (board.allLeafs[x, y] != null)
                    {
                        board.allLeafs[x, y].isMatched = true;
                        currentMatches.Add(board.allLeafs[x, y]);

                        uiMan.sadDog.SetActive(true);

                        StartCoroutine(SadDog());

                    }

                }
            }
        }
        currentMatches = currentMatches.Distinct().ToList();

        
    }

     public IEnumerator SadDog()
     {
          yield return new WaitForSeconds(2f);
          uiMan.sadDog.SetActive(false);

     }


}
