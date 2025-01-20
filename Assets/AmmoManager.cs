using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoManager : MonoBehaviour
{
    public GameObject One;
    public GameObject Two;
    public GameObject Three;
    
    private WeaponManager Script;
    private float Count;

    // Start is called before the first frame update
    void Start()
    {
        Script = GameObject.FindGameObjectWithTag("Player").GetComponent<WeaponManager>();
    }

    // Update is called once per frame
    void Update() {
        Count = Script.BananarangCount;

        if (Count == 3) {
            One.SetActive(false);
            Two.SetActive(false);
            Three.SetActive(true);
        } else if (Count == 2) {
            One.SetActive(false);
            Two.SetActive(true);
            Three.SetActive(false);
        } else if (Count == 1) {
            One.SetActive(true);
            Two.SetActive(false);
            Three.SetActive(false);
        } else {
            One.SetActive(false);
            Two.SetActive(false);
            Three.SetActive(false);
        }
     }
}
