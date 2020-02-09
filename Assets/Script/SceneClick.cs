using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace test_task {
    public class SceneClick : MonoBehaviour
    {
        private Map map = null;
        private void Start()
        {
            map = GameObject.FindGameObjectWithTag("Map").GetComponent<Map>();
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0)) {
                map.NextStep();
            }
        }
    }
}