using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomGenerator : MonoBehaviour
{

    public GameObject Wall;
    public GameObject Floor;
    public GameObject Door;
    private System.Random rnd = new System.Random();

    void Start()
    {
        GenerateRoom(-10, -10, 15, 7, 2);
        GenerateRoom(50, -10, 15, 7, 5);
        GenerateRoom(120, -10, 15, 7, 10);
    }

    private void GenerateRoom(int startX, int startY, int sizeX, int sizeY, int doorCount=0, string doorSide=null)
    {
        int topX = (sizeX * 3) + startX;
        int topY = (sizeY * 3) + startY;
        Instantiate(Wall, new Vector3(startX - 3, topY, 0), Quaternion.identity);       // top left corner
        Instantiate(Wall, new Vector3(topX, topY, 0), Quaternion.identity);             // top right corner
        Instantiate(Wall, new Vector3(startX - 3, startY - 3, 0), Quaternion.identity); // bottom left corner
        Instantiate(Wall, new Vector3(topX, startY - 3, 0), Quaternion.identity);       // bottom right corner
        var Walls = new Dictionary<string, List<GameObject>>
        {
            {"top",    new List<GameObject>()},
            {"right",  new List<GameObject>()},
            {"bottom", new List<GameObject>()},
            {"left",   new List<GameObject>()}
        };
        for (int x = 0;  x < sizeX; x++)
        {
            int nextY = startY;
            Walls["bottom"].Add(Instantiate(Wall, new Vector3(startX, nextY - 3, 0), Quaternion.identity));
            for (int y = 0; y < sizeY; y++)
            {
                if (x == 0)
                    Walls["left"].Add(Instantiate(Wall, new Vector3(startX - 3, nextY, 0), Quaternion.identity));
                else if (x == sizeX - 1)
                    Walls["right"].Add(Instantiate(Wall, new Vector3(startX + 3, nextY, 0), Quaternion.identity));
                Instantiate(Floor, new Vector3(startX, nextY, 0), Quaternion.identity);
                if (y == sizeY - 1)
                    Walls["top"].Add(Instantiate(Wall, new Vector3(startX, nextY + 3, 0), Quaternion.identity));
                nextY += 3;
            }
            startX += 3;
        }
        var badDoorPositions = new Dictionary<string, List<(float, float)>>
        {
            {"top",    new List<(float, float)>()},
            {"right",  new List<(float, float)>()},
            {"bottom", new List<(float, float)>()},
            {"left",   new List<(float, float)>()}
        };
        for (int i = 0; i < doorCount; i++)
            CreateRoomDoor(ref Walls, ref badDoorPositions, doorSide);
    }

    private (float, float) CreateRoomDoor(ref Dictionary<string, List<GameObject>> Walls, ref Dictionary<string, List<(float, float)>> badDoorPositions, string doorSide=null)
    {
        if (doorSide == null)
            doorSide = Walls.ElementAt(rnd.Next(0, Walls.Count)).Key;
        int RoomWallIndex = rnd.Next(Walls[doorSide].Count);
        GameObject RoomWall = Walls[doorSide][RoomWallIndex];
        float doorX = RoomWall.transform.position.x;
        float doorY = RoomWall.transform.position.y;
        if (doorSide == "top" || doorSide == "bottom")
        {
            badDoorPositions[doorSide].Add((doorX - 3, doorY));
            badDoorPositions[doorSide].Add((doorX + 3, doorY));
        }
        else if (doorSide == "left" || doorSide == "right")
        {
            badDoorPositions[doorSide].Add((doorX, doorY - 3));
            badDoorPositions[doorSide].Add((doorX, doorY + 3));
        }
        if (badDoorPositions[doorSide].Any(i => i == (doorX, doorY)))
            return (0f, 0f);
        Walls[doorSide].RemoveAt(RoomWallIndex);
        Instantiate(Door, new Vector3(doorX, doorY, 0), Quaternion.identity);
        Destroy(RoomWall);
        return (doorX, doorY);
    }
}
