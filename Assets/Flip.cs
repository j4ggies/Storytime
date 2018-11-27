using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flip : MonoBehaviour {

    private GameObject[] currentGroups; //Popup groups of same color
    private GameObject[] newGroups; //Popup groups of new color

    void Start() {
        string parentTag = transform.parent.parent.tag;
        currentGroups = GameObject.FindGameObjectsWithTag(parentTag);
        string switchTag = transform.parent.GetChild(0).tag;
        newGroups = GameObject.FindGameObjectsWithTag(switchTag);
    }

    void OnTriggerEnter2D(Collider2D collision) {
        foreach (GameObject o in currentGroups)
        {
            if (o.transform.parent.tag != "SwitchToken") {
                o.SetActive(false);
            }
        }
        foreach (GameObject o in newGroups)
        {
            o.SetActive(true);
        }
    }
}
