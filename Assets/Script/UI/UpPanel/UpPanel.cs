using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace test_task
{
    public class UpPanel : MonoBehaviour
    {
        [SerializeField] private GameObject viewBttn = null;
        private int coun_Actions = 5;
        private GameObject[] upButtons;

        void Start()
        {
            upButtons = new GameObject[coun_Actions];
            CreatePanel();
        }

        private void CreatePanel()
        {
            float d = Screen.width / (coun_Actions + 1); //step
            float dx = -d * 2; //step position

            for (int i = 0; i < coun_Actions; i++)
            {
                GameObject bttn = GameObject.Instantiate(viewBttn, Vector3.zero, Quaternion.identity);
                bttn.transform.SetParent(this.transform);
             //   bttn.transform.position = this.transform.position;
                bttn.transform.localPosition = new Vector3(dx, 0f, 0f);
                dx += d;
                upButtons[i] = bttn;
            }
        }

    }
}