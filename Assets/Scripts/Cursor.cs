using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;


public class Cursor : MonoBehaviour {
    public GameObject cursorSprite;

    private Board board;

    float minX, minY, maxX, maxY;

    bool dragging = false;

    float dragX, dragY;

    // Use this for initialization
    void Start() {
        board = FindObjectOfType<Board>();
        dragging = false;
    }


    // Update is called once per frame
    void Update() {
        Vector3 v = Camera.main.ScreenToWorldPoint(CrossPlatformInputManager.mousePosition);
        if (CrossPlatformInputManager.GetButton("Fire2")) {
            if (dragging) {
                board.transform.position = new Vector3(dragX + v.x, dragY + v.y, board.transform.position.z);
            } else {
                dragging = true;
                dragX = board.transform.position.x-v.x;
                dragY = board.transform.position.y-v.y;
            }
        } else {
            dragging = false;

        }

        if (FindObjectOfType<Board>().paused) {
            minX = board.transform.position.x - board.board.width / 2f * board.transform.localScale.x;
            maxX = board.transform.position.x + board.board.width / 2f * board.transform.localScale.x;
            minY = board.transform.position.y - board.board.height / 2f * board.transform.localScale.y;
            maxY = board.transform.position.y + board.board.height / 2f * board.transform.localScale.y; cursorSprite.transform.position = new Vector3(v.x, v.y, cursorSprite.transform.position.z);

            if (CrossPlatformInputManager.GetButtonDown("Fire1")) {
                float x = (v.x - minX) / (maxX - minX);
                float y = (v.y - minY) / (maxY - minY);
                int i = (int)Mathf.Floor(x * board.board.width);
                int j = (int)Mathf.Floor(y * board.board.height);
                board.CreateCell(board.board, i, j);
                board.board.Apply();
                Debug.Log("Created cell at (" + i + ", " + j + ")");
            }
        } else {
            //put it off screen
            cursorSprite.transform.position = new Vector3(5000, 5000, cursorSprite.transform.position.z);
        }

    }
}
