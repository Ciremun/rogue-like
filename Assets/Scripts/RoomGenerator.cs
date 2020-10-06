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
        GenerateRoom(20, -10, 30, 30, 10);
    }

    private void GenerateRoom(int startX, int startY, int sizeX, int sizeY, int doorCount)
    {
        int topX = (sizeX * 3) + startX;
        int topY = (sizeY * 3) + startY;
        Instantiate(Wall, new Vector3(startX - 3, topY, 0), Quaternion.identity);       // top left corner
        Instantiate(Wall, new Vector3(topX, topY, 0), Quaternion.identity);             // top right corner
        Instantiate(Wall, new Vector3(startX - 3, startY - 3, 0), Quaternion.identity); // bottom left corner
        Instantiate(Wall, new Vector3(topX, startY - 3, 0), Quaternion.identity);       // bottom right corner
        var Walls = new List<GameObject>();
        for (int x = 0;  x < sizeX; x++)
        {
            int nextY = startY;
            Walls.Add(Instantiate(Wall, new Vector3(startX, nextY - 3, 0), Quaternion.identity)); // bottom wall
            for (int y = 0; y < sizeY; y++)
            {
                if (x == 0)
                    Walls.Add(Instantiate(Wall, new Vector3(startX - 3, nextY, 0), Quaternion.identity)); // left wall
                else if (x == sizeX - 1)
                    Walls.Add(Instantiate(Wall, new Vector3(startX + 3, nextY, 0), Quaternion.identity)); // right wall
                Instantiate(Floor, new Vector3(startX, nextY, 0), Quaternion.identity); // floor
                if (y == sizeY - 1)
                    Walls.Add(Instantiate(Wall, new Vector3(startX, nextY + 3, 0), Quaternion.identity)); // top wall
                nextY += 3;
            }
            startX += 3;
        }
        List<(float, float)> badDoorPositions = new List<(float, float)>{};
        for (int i = 0; i < doorCount; i++)
            CreateRoomDoor(ref Walls, ref badDoorPositions);
    }

    private void CreateRoomDoor(ref List<GameObject> Walls, ref List<(float, float)> badDoorPositions)
    {
        int RoomWallIndex = rnd.Next(Walls.Count);
        GameObject RoomWall = Walls[RoomWallIndex];
        AddBadDoorPositions(ref RoomWall, ref badDoorPositions);
        if (badDoorPositions.Any(i => i == (RoomWall.transform.position.x, RoomWall.transform.position.y)))
            return;
        Walls.RemoveAt(RoomWallIndex);
        Instantiate(Door, new Vector3(RoomWall.transform.position.x, RoomWall.transform.position.y, 0), Quaternion.identity);
        Destroy(RoomWall);
    }

    private void AddBadDoorPositions(ref GameObject RoomWall, ref List<(float, float)> badDoorPositions)
    {
        badDoorPositions.Add((RoomWall.transform.position.x - 3, RoomWall.transform.position.y));
        badDoorPositions.Add((RoomWall.transform.position.x + 3, RoomWall.transform.position.y));
        badDoorPositions.Add((RoomWall.transform.position.x, RoomWall.transform.position.y - 3));
        badDoorPositions.Add((RoomWall.transform.position.x, RoomWall.transform.position.y + 3));
    }
}
