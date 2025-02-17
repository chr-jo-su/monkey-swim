using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveInvisibleBox : MonoBehaviour
{
    private float[] xList = new float[5];
    private float[] yList = new float[5];
    private int pos = 0;
    public Camera cam;
    public SceneChanger sc;
    // Start is called before the first frame update
    // x = [-4.4, 5, -4.4, 5, 2]
    // y = [2, 2, -8, -8, -3]
    // camera size should become 11 at end
    void Start()
    {
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
    }

    // Update is called once per frame
    public void SecretServiceAnnouncementToThePrimeMinister()
    {
        if (pos < 5)
        {
            transform.position = new Vector3(xList[pos], yList[pos], 0);
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
