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
    void Update()
    {
        if (Script == null)
        {
            Script = GameObject.FindGameObjectWithTag("Player").GetComponent<WeaponManager>();
        }

        Count = Script.BananarangCount;

        One.SetActive(false);
        Two.SetActive(false);
        Three.SetActive(false);

        if (Count == 3)
        {
            Three.SetActive(true);
        }
        else if (Count == 2)
        {
            Two.SetActive(true);
        }
        else if (Count == 1)
        {
            One.SetActive(true);
        }
    }
}
