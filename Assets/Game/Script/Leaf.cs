using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaf : MonoBehaviour
{
    [HideInInspector]
    public Vector2Int posIndex;
    [HideInInspector]
    public Board board;

    private Vector2 firstTouchPosition;
    private Vector2 finalTouchPosition;

    private bool mousePressed;
    private float swipeAngle = 0;

    private Leaf otherLeaf;

    public enum LeafType {green, yellow, red, brown, pink, spider, woodBlock}
    public LeafType type;

    public bool isMatched;
    
    private Vector2Int previousPos;

    public GameObject destroyEffect;

    public int blastSize = 2;

    public int scoreValue;

  
  

    void Start()
    {
        
    }

   
    void Update()
    {
        if (Vector2.Distance(transform.position, posIndex) > .01f)
        {
            transform.position = Vector2.Lerp(transform.position, posIndex, board.leafSpeed * Time.deltaTime);
        }
        else
        {
            transform.position = new Vector3(posIndex.x, posIndex.y, 0f);
            board.allLeafs[posIndex.x, posIndex.y] = this;
        }
        

        if (mousePressed && Input.GetMouseButtonUp(0))
        {
            mousePressed = false;

            if (board.currentState == Board.BoardState.move && board.roundMan.roundTime > 0)
            {
                finalTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                CalculateAngle();
            }

        }
    }

    public void SetupLeaf(Vector2Int pos, Board theBoard) 
    {

        posIndex = pos;
        board = theBoard;
    
    }

    private void OnMouseDown()
    {
        if (board.currentState == Board.BoardState.move && board.roundMan.roundTime > 0)
        {
            firstTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePressed = true;
        }

    }

    private void CalculateAngle() 
    {
        swipeAngle = Mathf.Atan2(finalTouchPosition.y - firstTouchPosition.y, finalTouchPosition.x - firstTouchPosition.x);
        swipeAngle = swipeAngle * 180 / Mathf.PI;
        Debug.Log(swipeAngle);

        if (Vector3.Distance(firstTouchPosition,finalTouchPosition) > 0.5f)
        {
            MovePieces();
        }
    
    }

    private void MovePieces() 
    {
        previousPos = posIndex;

        // swipe Right
        if (swipeAngle < 45 && swipeAngle > -45 && posIndex.x < board.width - 1)
        {
            otherLeaf = board.allLeafs[posIndex.x + 1, posIndex.y];
            otherLeaf.posIndex.x--;
            posIndex.x++;

        } // swipe Up
        else if (swipeAngle > 45 && swipeAngle <= 135 && posIndex.y < board.height - 1)
        {
            otherLeaf = board.allLeafs[posIndex.x, posIndex.y + 1];
            otherLeaf.posIndex.y--;
            posIndex.y++;

        } // swipe Down
        else if (swipeAngle < -45 && swipeAngle >= -135 && posIndex.y > 0)
        {
            otherLeaf = board.allLeafs[posIndex.x, posIndex.y - 1];
            otherLeaf.posIndex.y++;
            posIndex.y--;
        } // swipe Left
        else if ((swipeAngle > 135 || swipeAngle < -135) && posIndex.x > 0)
        {
            otherLeaf = board.allLeafs[posIndex.x - 1, posIndex.y];
            otherLeaf.posIndex.x++;
            posIndex.x--;
        }

        board.allLeafs[posIndex.x, posIndex.y] = this;
        board.allLeafs[otherLeaf.posIndex.x, otherLeaf.posIndex.y] = otherLeaf;


        StartCoroutine(CheckMoveCo());
    }

    public IEnumerator CheckMoveCo() 
    {
        board.currentState = Board.BoardState.wait;

        yield return new WaitForSeconds(.5f);

        board.matchFind.FindAllMatches();

        if (otherLeaf != null)
        {
            if (!isMatched && !otherLeaf.isMatched)
            {
                otherLeaf.posIndex = posIndex;
                posIndex = previousPos;

                board.allLeafs[posIndex.x, posIndex.y] = this;
                board.allLeafs[otherLeaf.posIndex.x, otherLeaf.posIndex.y] = otherLeaf;

                yield return new WaitForSeconds(.5f);

                board.currentState = Board.BoardState.move;

            }
            else
            {
                board.DestroyMatches();
            }
        }

    }

}
