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
        GenerateRoom(-10, -10, 15, 7, 1000);
    }

    private void GenerateRoom(int startX, int startY, int sizeX, int sizeY, int doorCount)
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
        List<(float, float)> badDoorPositions = new List<(float, float)>();
        for (int i = 0; i < doorCount; i++)
            CreateRoomDoor(ref Walls, ref badDoorPositions, "top");
    }

    private void CreateRoomDoor(ref Dictionary<string, List<GameObject>> Walls, ref List<(float, float)> badDoorPositions, string doorSide = null)
    {
        if (doorSide == null)
            doorSide = Walls.ElementAt(rnd.Next(0, Walls.Count)).Key;
        int RoomWallIndex = rnd.Next(Walls[doorSide].Count);
        GameObject RoomWall = Walls[doorSide][RoomWallIndex];
        AddBadDoorPositions(ref RoomWall, ref badDoorPositions);
        if (badDoorPositions.Any(i => i == (RoomWall.transform.position.x, RoomWall.transform.position.y)))
            return;
        Walls[doorSide].RemoveAt(RoomWallIndex);
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
