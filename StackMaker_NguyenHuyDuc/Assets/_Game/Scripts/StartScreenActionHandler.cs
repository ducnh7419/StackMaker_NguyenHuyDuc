using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StartScreenActionHandler : MonoBehaviour
{
    [SerializeField]private TextMeshProUGUI text;

    private void Start()
    {
        
        StartBlinking();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0)){
            GameManager.Ins.ChangeState(GameManager.State.MainMenu);
        }
    }


    IEnumerator Blink()
    {
        while (true)
        {
            switch (text.color.a.ToString())
            {
                case "0":
                    text.color = new Color(text.color.r, text.color.g, text.color.b, 1);
                    yield return new WaitForSeconds(0.5f);
                    break;
                case "1":
                    text.color = new Color(text.color.r, text.color.g, text.color.b, 0);
                    yield return new WaitForSeconds(0.5f);
                    break;
            }
        }
    }

    void StartBlinking()
    {
        StopCoroutine("Blink");
        StartCoroutine("Blink");
    }

    void StopBlinking()
    {
        StopCoroutine("Blink");
    }
}
