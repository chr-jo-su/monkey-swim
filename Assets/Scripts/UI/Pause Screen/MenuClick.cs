using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MenuClick : MonoBehaviour
{
    public void OnMouseDown()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
