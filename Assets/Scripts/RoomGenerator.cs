using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator : MonoBehaviour
{

    public GameObject Wall;
    public GameObject Floor;
    // Start is called before the first frame update
    void Start()
    {
        GenerateRoom(-10, -10, 10, 10);
    }

    private void GenerateRoom(int startX, int startY, int sizeX, int sizeY)
    {
        Instantiate(Wall, new Vector3(startX - 3, startY - 3, 0), Quaternion.identity); // bottom left corner
        for (int x = 0;  x < sizeX; x++)
        {
            int nextY = startY;
            Instantiate(Wall, new Vector3(startX, nextY - 3, 0), Quaternion.identity); // bottom wall
            for (int y = 0; y < sizeY; y++)
            {
                if (x == 0) // left wall
                {
                    Instantiate(Wall, new Vector3(startX - 3, nextY, 0), Quaternion.identity);
                }
                else if (x == sizeX - 1) // right wall
                {
                    Instantiate(Wall, new Vector3(startX + 3, nextY, 0), Quaternion.identity);
                }
                Instantiate(Floor, new Vector3(startX, nextY, 0), Quaternion.identity);
                if (y == sizeY - 1) // top wall
                {
                    if (x == 0)
                       Instantiate(Wall, new Vector3(startX - 3, nextY + 3, 0), Quaternion.identity); // top left corner
                    if (x == sizeX - 1)
                        Instantiate(Wall, new Vector3(startX + 3, nextY + 3, 0), Quaternion.identity); // top right corner
                    Instantiate(Wall, new Vector3(startX, nextY + 3, 0), Quaternion.identity);
                }
                nextY += 3;
            }
            if (x == sizeX - 1)
                Instantiate(Wall, new Vector3(startX + 3, startY - 3, 0), Quaternion.identity); // bottom right corner
            startX += 3;
        }
    }
}
