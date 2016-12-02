using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Message : MonoBehaviour {
    public Text text;

    [Tooltip("Set to a positive value to show the message")]
    public float timeToLive = 0f;

    float initialTimeToLive = 0;

    // Use this for initialization
    void Start() {
        initialTimeToLive = 0;
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0);
    }

    // Update is called once per frame
    void Update() {
        if (timeToLive > 0) {
            if (initialTimeToLive == 0) {
                initialTimeToLive = timeToLive;
            }
            float alpha = timeToLive / initialTimeToLive;
            text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);
            timeToLive -= Time.deltaTime;
        }else {
            initialTimeToLive = 0;
            text.color = new Color(text.color.r, text.color.g, text.color.b, 0);
        }
    }
}
