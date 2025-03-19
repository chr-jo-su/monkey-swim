using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoveInvisibleBox : MonoBehaviour
{
    private float[] xList = new float[5];
    private float[] yList = new float[5];
    private string[] wrds = new string[5];
    private int pos = 0;
    public Camera cam;
    public SceneChanger sc;
    // public TextMeshProUGUI txt;
    // Start is called before the first frame update
    // x = [-4.4, 5, -4.4, 5, 2]
    // y = [2, 2, -8, -8, -3]
    // camera size should become 11 at end
    void Start()
    {

        // txt = GameObject.Find("txt").GetComponent<Text>();
        xList[0] = (float)-4.4;
        xList[1] = 5;
        xList[2] = (float)-4.4;
        xList[3] = 5;
        xList[4] = 2;
        yList[0] = 2;
        yList[1] = 2;
        yList[2] = -8;
        yList[3] = -8;
        yList[4] = -3;
        wrds[0] = "Gorilla tha Grilla: Hey Monke!";
        wrds[1] = "Gorilla tha Grilla: I want to make a fireee dish out of the Kraken - Fried Kraken served with mash potatoes.";
        wrds[2] = "Gorilla tha Grilla: Swim to the bottom of the puddle to get to the Kraken, but watch out for the other fish out to get you.";
        wrds[3] = "Gorilla tha Grilla: Good Luck Monke";
        wrds[4] = "Off to Monkey Buisness";
    }

    // Update is called once per frame
    public void SecretServiceAnnouncementToThePrimeMinister()
    {
        if (pos < 5)
        {
            transform.position = new Vector3(xList[pos], yList[pos], 0);
            // txt.text = wrds[pos];
            if (pos == 4)
            {
                cam.orthographicSize = 11;
            }
            pos++;
        }
        else
        {
            sc.ChangeScene("Level1");
        }
    }
}
