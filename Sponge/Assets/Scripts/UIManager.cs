using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    public Image water;
    public float waterPercent
    {
        set
        {
            water.fillAmount = value;
        }
    }

    public int score = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);

        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnDestroy()
    {
        if (this == Instance)
            Instance = null;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddScore(int value = 1)
    {
        score += value;
    }
}
