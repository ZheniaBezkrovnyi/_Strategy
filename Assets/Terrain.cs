using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terrain : MonoBehaviour
{
    private int[,] setMap;
    [SerializeField] private Vector2Int sizeMap;
    private TakeObjects takeObjects;
    private void Start()
    {
        takeObjects = new TakeObjects();
        setMap = new int[sizeMap.x, sizeMap.y];
    }
    void Update()
    {
        takeObjects.MoveHouse(setMap);
    }
    public void TakeHouse(House house)
    {
        takeObjects.TakeHouse(house);
    }
}



public class TakeObjects : MonoBehaviour
{
    public static House _house;
    private House myHouse;
    private int neParniX;
    private int neParniZ;
    public void TakeHouse(House house)
    {
        _house = Instantiate(house);
        neParniX = _house.sides.x % 2 == 1 ? 1 : 0;
        neParniZ = _house.sides.y % 2 == 1 ? 1 : 0;
        _house.transform.position += new Vector3(0, _house.transform.localScale.y / 2, 0);
        _house.stateHouse = StateHouse.IsActive;
        _house.startMove = true;
        myHouse = _house;
        _house.InitColor();
    }
    int x;
    int z;
    public void MoveHouse(int[,] _setMap)
    {
        if (_house != null && _house.Drag)
        {
            if(myHouse == null)
            {
                myHouse = _house;
            }
            var newplane = new Plane(Vector3.up, Vector3.zero);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (newplane.Raycast(ray, out float position))
            {
                int side = 1;
                Vector3 worldPos = ray.GetPoint(position);
                x = Mathf.RoundToInt(worldPos.x / side) * side;
                z = Mathf.RoundToInt(worldPos.z / side) * side;
                _house.transform.position = new Vector3(x + (float)neParniX / 2f, _house.transform.position.y, z + (float)neParniZ / 2f);
                //Debug.Log(CheckCells(x,z));
            }
            if (_house.startMove)
            {
                _house.startMove = false;
                ZeroCell(x,z,_setMap);
            }
        }
        if (_house == null && myHouse != null && myHouse.endMove)
        {
            Debug.Log("end");
            myHouse.endMove = false;
            End(x, z, _setMap);
            myHouse = null;
        }



        bool CheckCells(int X,int Z)
        {
            for(int x = X - _house.sides.x/2; x < X + _house.sides.x/2 + neParniX; x++)
            {
                for (int z = Z - _house.sides.y/2; z < Z + _house.sides.y/2 + neParniZ; z++)
                {
                    if(x >= 0 && z >= 0 && x < _setMap.GetLength(0) && z < _setMap.GetLength(1))
                    {
                        if (_setMap[x, z] == 1) {
                            return false; 
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
    private void End(int X, int Z, int[,] _setMap)        //поставить туда, где будут учитани граници map, якось обдумать просчет границ самому, бо була хуйня
    {
        myHouse.posOnMap = new int[2, myHouse.sides.x, myHouse.sides.y];
        for (int x = X - myHouse.sides.x / 2, dX = 0; x < X + myHouse.sides.x / 2 + neParniX; x++,dX++)
        {
            for (int z = Z - myHouse.sides.y / 2, dZ = 0; z < Z + myHouse.sides.y / 2 + neParniZ; z++,dZ++)
            {
                Debug.Log(x + " " + z + "end");
                _setMap[x, z] = 1;
                myHouse.posOnMap[0, dX, dZ] = x;
                myHouse.posOnMap[1, dX, dZ] = z;
            }
        }
    }
    private void ZeroCell(int X, int Z, int[,] _setMap)         //поставить туда, где будут учитани граници map
    {
        if (_house.posOnMap != null)
        {
            for (int i = 0; i < _house.posOnMap.GetLength(1); i++)
            {
                for (int j = 0; j < _house.posOnMap.GetLength(2); j++)
                {
                    Debug.Log(_house.posOnMap[0, i, j] + " " + _house.posOnMap[1, i, j] + "zero");
                    _setMap[_house.posOnMap[0, i, j], _house.posOnMap[1, i, j]] = 0;
                }
            }
        }
    }

}