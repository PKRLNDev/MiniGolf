using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwipeMenuUi : MonoBehaviour
{
    public GameObject ScrollBar;
    float scrollPos = 0;
    float distance = 0;
    float[] pos;

    [SerializeField]
    float LerpSpeed = 0.01f;

    // Start is called before the first frame update
    void Start()
    {
        pos = new float[transform.childCount];

        distance = 1.0f / (pos.Length - 1.0f);

        //for (int i = 0; i < pos.Length; i++)
        //{

        //    pos[i] = distance * i;

        //}
        scrollPos = ScrollBar.GetComponent<Scrollbar>().value = scrollPos;

    }

    // Update is called once per frame
    void Update()
    {


        for (int i = 0; i < pos.Length; i++)
        {

            pos[i] = distance * i;

        }



        if (Input.GetMouseButton(0))
        {
            scrollPos = ScrollBar.GetComponent<Scrollbar>().value;
            
        }
        else
        {

            for (int i = 0; i < pos.Length; i++)
            {
                if (scrollPos < pos[i] + (distance / 2) && scrollPos > pos[i] - (distance / 2))
                {
                    ScrollBar.GetComponent<Scrollbar>().value = Mathf.Lerp(ScrollBar.GetComponent<Scrollbar>().value, pos[i], LerpSpeed);
                }

            }

        }

        for (int i = 0; i < pos.Length; i++)
        {
            
            if (scrollPos < pos[i] + (distance / 2) && scrollPos > pos[i] - (distance / 2))
            {
                transform.GetChild(i).localScale = Vector3.Lerp(transform.GetChild(i).localScale, new Vector3(1.0f, 1.0f,1.0f), LerpSpeed);

                for (int a = 0; a < pos.Length; a++)
                {
                    if (a != i)
                    {
                        transform.GetChild(a).localScale = Vector3.Lerp(transform.GetChild(a).localScale, new Vector3(0.8f, 0.8f, 1.0f), LerpSpeed);
                    }
                }

            }


        }

    }
}
