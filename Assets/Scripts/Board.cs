using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Board : MonoBehaviour {

    [Tooltip("Must be the same dimensions and sprite type as the board; used for computing the next iteration")]
    public Texture2D buffer;

    [Tooltip("If true, pixels off the edges of the board are treated as alive when counting neighbors")]
    public bool OutOfBoundsIsAlive = false;

    [Tooltip("This many seconds between generations")]
    public float GenerationInterval = 1f;

    public Texture2D board;
    private float lastUpdateTime = 0;

    private int generation = 0;

    public bool paused = false;

    private Message message;

    private bool ready;

    private void Update() {
        if (ready) {
            ShowNextGeneration();
            ready = false;
        }
        if (!paused) {           
            if (Time.time - lastUpdateTime >= GenerationInterval) {
                ready = false;
                StartCoroutine("ComputeNextGeneration");
                generation++;
                Debug.Log("Generation: " + generation);
                lastUpdateTime = Time.time;
            }
        }
        float v = CrossPlatformInputManager.GetAxis("Mouse ScrollWheel");
        if (v > 0 && transform.localScale.x < 256) {
            transform.localScale *= 2;
        } else if (v < 0 && transform.localScale.x > 1f / 64f) {
            transform.localScale /= 2f;
        }

        if (CrossPlatformInputManager.GetButtonDown("Cancel")) {
            if (paused) {
                paused = false;
                message.text.text = "Resuming";
                message.timeToLive = 1f;
            } else {
                paused = true;
                message.text.text = "Paused";
                message.timeToLive = 1f;
            }
        }

    }

    private void Start() {
        board = GetComponent<SpriteRenderer>().sprite.texture;
        paused = false;
        message = FindObjectOfType<Message>();
    }

    /// <summary>
    /// If the current generation is in the board sprite, fill the buffer with the next generation
    /// </summary>
    public IEnumerator ComputeNextGeneration() {

        for (int i = 0; i < buffer.width; i++) {
            if (i % (buffer.width / 25) == 0) {
                yield return new WaitForEndOfFrame();
            }
            for (int j = 0; j < buffer.height; j++) {
                int n = CountNeighbors(board, i, j);
                if (n < 2 || n > 3) {
                    Kill(buffer, i, j);
                } else if (n == 3) {
                    CreateCell(buffer, i, j);
                }else if(IsAlive(board, i, j)) {
                    CreateCell(buffer, i, j);
                }
            }
        }
        buffer.Apply();
        ready = true;
    }

    public void ShowNextGeneration() {
        for (int i = 0; i < buffer.width; i++) {
            for (int j = 0; j < buffer.height; j++) {
                board.SetPixel(i, j, buffer.GetPixel(i, j));
            }
        }
        board.Apply();
    }

    public void Kill(Texture2D t, int i, int j) {
        t.SetPixel(i, j, Color.black);
    }

    public void CreateCell(Texture2D t, int i, int j) {
        t.SetPixel(i, j, Color.white);
    }

    public int CountNeighbors(Texture2D t, int i, int j) {
        int count = 0;
        if (IsAlive(t, i - 1, j - 1)) {
            count++;
        }
        if (IsAlive(t, i, j - 1)) {
            count++;
        }
        if (IsAlive(t, i + 1, j - 1)) {
            count++;
        }
        if (IsAlive(t, i - 1, j)) {
            count++;
        }
        if (IsAlive(t, i + 1, j)) {
            count++;
        }
        if (IsAlive(t, i - 1, j + 1)) {
            count++;
        }
        if (IsAlive(t, i, j + 1)) {
            count++;
        }
        if (IsAlive(t, i + 1, j + 1)) {
            count++;
        }

        return count;
    }

    public bool IsAlive(Texture2D t, int i, int j) {
        if (i < 0 || i >= t.width || j < 0 || j >= t.height) {
            return OutOfBoundsIsAlive;
        }
        Color c = t.GetPixel(i, j);
        return c.r >= .5f && c.g >= .5f && c.b >= .5f && c.a >= .5f;
    }


}
