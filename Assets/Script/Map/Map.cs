using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace test_task
{
    public class Map : MonoBehaviour
    {
        private enum cell_color
        {
            black,
            green,
            red
        }

        private class Point
        {
            public int x;
            public int y;
        }

        [Header("Grid")]
        [SerializeField] private int cells = 0;
        [SerializeField] private int rows = 0;
        [SerializeField] private float w_h_tile = 1.5f;
        [SerializeField] private GameObject tile = null;

        [Header("Color Cells")]
        [SerializeField] private Color32 free_cell = Color.black;// new Color(96, 92, 92, 255);
        [SerializeField] private Color32 close_cell = Color.black; // new Color(85, 60, 60, 255);
        [SerializeField] private Color32 reset_cell = Color.black; // new Color(130, 0, 0, 255);
        [SerializeField] private Color32 add_cell = Color.black;  //new Color(90, 140, 84, 255);
        [SerializeField] private Color32 dell_cell = Color.black;  //new Color(228, 14, 24, 255);

        [Header("Units")]
        [SerializeField] private GameObject[] pul_units;
        private GameObject[,] units;

        private GameObject map;
        private int close_sell = 0;
        private Point[] close_sell_array;

        private GameObject downPanel = null;
        private GameObject upPanel = null;
        //  private float TempAngles = 0f;
        private int inStep = 0;
        private bool[] stepFinish;
        private bool isApply = false;

        void Start()
        {
            stepFinish = new bool[5];
            downPanel = GameObject.Find("DownPanel");
            upPanel = GameObject.Find("UpPanel");

            close_sell = cells * rows - Mathf.RoundToInt(cells * rows * .8f);
            // Debug.Log(close_sell);
            units = new GameObject[cells, rows];
            close_sell_array = new Point[close_sell];
            map = GameObject.FindGameObjectWithTag("Map");

            ClearStep();
            CreateGrid();
        }

        private void ClearStep()
        {
            for (int i = 0; i < stepFinish.Length; i++)
            {
                stepFinish[i] = false;
            }
        }
        public void NextStep(){

            if (!isApply)
                return;    
            bool find = false;
            int _step =0;
            while (!find)
            {
                _step = UnityEngine.Random.Range(0, stepFinish.Length);

                if (stepFinish[_step] == false)
                {
                    find = true;
                }
                else
                {
                    bool ch = false;
                    for (int i = 0; i < stepFinish.Length; i++)
                    {
                        if (stepFinish[i] == false)
                        {
                            ch = true;
                            break;
                        }
                    }
                    if (!ch)
                        return;
                }
              
            }
         
               inStep = _step;
               onStep();
            
        }

        private void onStep()
        {
            this.transform.rotation = Quaternion.Euler(UnityEngine.Random.Range(0, 80), 0, 0);
            clearColor();
            Debug.Log("Next Step");
            upPanel.GetComponent<UpPanel>().Check(inStep);
            stepFinish[inStep] = true;

            int[] actions=new int [0]; // 0 -"dell" 1 -"add" 2 -"res" 
            switch (inStep)
            {
                case 0:
                    {
                        actions = new int[] { 1, 1, 1, 1, 1,0,0,0 };
                        break;
                    }
                case 1:
                    {
                        actions = new int[] { 1, 2,2, 2 };
                        break;
                    }
                case 2:
                    {
                        actions = new int[] { 0, 0,0, 0, 2, 2, 2 };
                        break;
                    }
                case 3:
                    {
                        actions = new int[] { 2, 2, 2, 2 };
                        break;
                    }
                case 4:
                    {
                        actions = new int[] { 0, 0, 0, 2, 2 };
                        break;
                    }
            }
            for (int i = 0; i < actions.Length; i++)
            {
                if (actions[i] == 0)
                {
                    Debug.Log("Dell");
                    addUnit(RandomCell(true));
                }else
                if (actions[i] == 1)
                {
                    Debug.Log("add");
                    dellUnit(RandomCell(false));
                }else
                if(actions[i] == 2)
                {
                    Debug.Log("Reset");
                    Point nPoint = RandomCell(false);
                    if (nPoint.x != -1 && nPoint.y != -1)
                    {
                        dellUnit(nPoint);
                        addUnit(nPoint);
                        units[nPoint.x, nPoint.y].GetComponent<SpriteRenderer>().color = reset_cell;
                    }
                }
            }
        }

        private void clearColor()
        {
            for (int i = 0; i < cells; i++)
                for (int j = 0; j < rows; j++)
                    if(units[i, j].GetComponent<Cell>().type != "close")
                         units[i, j].GetComponent<SpriteRenderer>().color = free_cell;
        }

        private void dellAllUnit()
        {
            for (int i = 0; i < cells; i++)
                for (int j = 0; j < rows; j++)
                    if (units[i, j].GetComponent<Cell>().type == "unit")
                        GameObject.Destroy(units[i, j].GetComponent<Cell>().go);
        }

        private void dellUnit(Point point)
        {
            if (point.x == -1 && point.y == -1)
                return;

            GameObject.Destroy(units[point.x, point.y].GetComponent<Cell>().go);
            units[point.x, point.y].GetComponent<Cell>().type = "free";
            units[point.x, point.y].GetComponent<SpriteRenderer>().color = dell_cell;
        }

        private Point RandomCell(bool type)
        {
            Point point = new Point();
            bool find = false;
            while (!find)
            {
                point.x = UnityEngine.Random.Range(0, cells);
                point.y = UnityEngine.Random.Range(0, rows);
                if(type)
                {
                    if (units[point.x, point.y].GetComponent<Cell>().type == "free")
                    {
                       find = true;
                    }
                }else
                {
                    if (units[point.x, point.y].GetComponent<Cell>().type == "unit")
                    {
                        find = true;
                    }
                }

                if (!find)
                {
                    bool ch = false;
                    for (int i = 0; i < cells; i++)
                    {
                        for (int j = 0; j < rows; j++)
                        {
                            if (type && units[i, j].GetComponent<Cell>().type == "free")
                            {
                                ch = true;
                                break;
                            }
                            if (!type && units[i, j].GetComponent<Cell>().type == "unit")
                            {
                                ch = true;
                                break;
                            }
                        }
                        if (ch)
                            break;
                    }
                    if (!ch)
                    {
                        point.x = point.y = -1;
                        return point;
                    }
                }
            }
            return point;
        }

        public void OnNextScene()
        {
            isApply = false;
            inStep = 0;
            this.transform.rotation = Quaternion.Euler(0, 0, 0);
           // TempAngles = 0f;
            Camera.main.GetComponent<Camera>().fieldOfView = 70.0f;
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, 4f, Camera.main.transform.position.z);
            downPanel.SetActive(true);
            dellAllUnit();
            CreateGrid();
        }

        public void OnApply()
        {
           // this.transform.Rotate(45 * Time.deltaTime, 0f,0f);
              this.transform.rotation = Quaternion.Euler(45, 0, 0);
            // TempAngles= 45f;
            Camera.main.GetComponent<Camera>().fieldOfView=50.0f;
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x,3f, Camera.main.transform.position.z);
            downPanel.SetActive(false);
            isApply = true;
        }

        private void addUnit(Point point)
        {
            if (point.x == -1 && point.y == -1)
                return;

              int i  = UnityEngine.Random.Range(0, pul_units.Length);
            GameObject nn = GameObject.Instantiate(pul_units[i],new Vector3(0f, 0f, -.1f), Quaternion.identity, units[point.x, point.y].transform);
             nn.transform.position =  units[point.x, point.y].transform.position;
          //  units[point.x, point.y].GetComponent<Cell>().view.transform
            units[point.x, point.y].GetComponent<Cell>().go = nn;
            units[point.x, point.y].GetComponent<Cell>().type = "unit";
            units[point.x, point.y].GetComponent<SpriteRenderer>().color = add_cell;
            nn.transform.position = units[point.x, point.y].GetComponent<Cell>().view.transform.position;
        }
       
        private void CreateGrid()
        {
            clearRandom();
            for (int i = 0; i < close_sell_array.Length; i++)
            {
                close_sell_array[i] = randomCloseCell(i);
               // Debug.Log("close sells x " + close_sell_array[i].x + " y " + close_sell_array[i].y);
            }

            for (int i = 0; i < cells; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    GameObject cell = GameObject.Instantiate(tile, new Vector3(i * w_h_tile, j * w_h_tile, 0f), Quaternion.identity);
                    cell.transform.SetParent(map.transform);
                    cell.name = "Cell_" + i + "_" + j;

                    for (int c = 0; c < close_sell_array.Length; c++)
                    {
                        if (close_sell_array[c].x == i && close_sell_array[c].y == j)
                        {
                            cell.GetComponent<SpriteRenderer>().color = close_cell;
                            cell.GetComponent<Cell>().type = "close";
                            break;
                        }
                        else
                            cell.GetComponent<Cell>().type = "free";
                    }
                    units[i, j] = cell;
                }
            }
        }


        private void clearRandom()
        {
            for (int i = 0; i < close_sell_array.Length; i++)
            {
                close_sell_array[i] = new Point();
                close_sell_array[i].x = -1;
                close_sell_array[i].y = -1;
            }
        }

        private Point randomCloseCell(int n)
        {
            Point point = new Point();
            bool find = false;
            while (!find)
            {
                point.x = UnityEngine.Random.Range(0, cells);
                point.y = UnityEngine.Random.Range(0, rows);
                bool ch = true;
                for (int i = 0; i < n; i++)
                {
                    if (close_sell_array[i].x == point.x && close_sell_array[i].y == point.y)
                    {
                        ch = false;
                        break;
                    }
                }
                if (ch)
                    find = true;
            }
            return point;
        }


    }

}