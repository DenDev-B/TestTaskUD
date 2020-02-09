using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace test_task
{
    public class UpPanel : MonoBehaviour
    {
        [SerializeField] private GameObject viewBttn = null;
        [SerializeField] private Color32 chackColor = Color.black;
         private int coun_Actions = 5;
        private GameObject[] upButtons;

        private void Awake()
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
        public void Reset()
        {
            for(int i=0;i<coun_Actions; i++)
            {
                upButtons[i].GetComponent<UpBttn>().img.color = Color.white;
            }
        }

        public void Check(int id)
        {
            upButtons[id].GetComponent<UpBttn>().img.color = chackColor;
        }

    }
}