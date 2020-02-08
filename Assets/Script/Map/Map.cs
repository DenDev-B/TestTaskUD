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

        void Start()
        {
            downPanel = GameObject.Find("DownPanel");
            upPanel = GameObject.Find("UpPanel");

            close_sell = cells * rows - Mathf.RoundToInt(cells * rows * .8f);
            // Debug.Log(close_sell);
            units = new GameObject[cells, rows];
            close_sell_array = new Point[close_sell];
            map = GameObject.FindGameObjectWithTag("Map");

            CreateGrid();
          //  addSmile(3, 3);
        }
        public void OnNextScene()
        {
            this.transform.rotation = Quaternion.Euler(0, 0, 0);
            Camera.main.GetComponent<Camera>().fieldOfView = 70.0f;
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, 4f, Camera.main.transform.position.z);
            downPanel.SetActive(true);
            CreateGrid();
        }
        public void OnApply()
        {
            this.transform.rotation = Quaternion.Euler(45, 0, 0);
            Camera.main.GetComponent<Camera>().fieldOfView=50.0f;
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x,3f, Camera.main.transform.position.z);
            downPanel.SetActive(false);
        }

        private void addSmile(int v1, int v2)
        {
            GameObject nn = GameObject.Instantiate(pul_units[1],  Vector3.zero, Quaternion.identity);
            nn.transform.SetParent(units[v1, v2].transform);
            nn.transform.position =  units[v1, v2].transform.position;

        }
       
        private void CreateGrid()
        {
            clearRandom();
            for (int i = 0; i < close_sell_array.Length; i++)
            {
                close_sell_array[i] = randomCloseCell(i);
                Debug.Log("close sells x " + close_sell_array[i].x + " y " + close_sell_array[i].y);
            }

            for (int i = 0; i < cells; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    GameObject cell = GameObject.Instantiate(tile, new Vector3(i * w_h_tile, j * w_h_tile, 0f), Quaternion.identity);
                    cell.transform.SetParent(map.transform);
                    cell.name = "Cell_" + i + "_" + j;

                    ; for (int c = 0; c < close_sell_array.Length; c++)
                    {
                        if (close_sell_array[c].x == i && close_sell_array[c].y == j)
                        {
                            cell.GetComponent<SpriteRenderer>().color = close_cell;
                            break;
                        }
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