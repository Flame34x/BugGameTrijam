using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class NotButton : MonoBehaviour
{
    public GameObject selector;

    public GameObject HTP;

    [SerializeField] bool isStartButton;

    private void OnMouseOver()
    {
        selector.SetActive(true);
    }
    private void OnMouseExit()
    {
        selector.SetActive(false);
    }

    private void OnMouseDown()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (isStartButton)
            {
                SceneManager.LoadScene("Main");
            }
            else
            {
                HTP.SetActive(true);
            }
        }
    }
}
