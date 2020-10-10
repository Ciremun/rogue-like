using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using PathFind;


public class RoomGenerator : MonoBehaviour
{

    public GameObject Wall;
    public GameObject WallNoCollision;
    public GameObject Floor;
    public GameObject Door;
    public GameObject Player;
    private System.Random rnd = new System.Random();
    private Dictionary<string, List<Point>> levelDoors = new Dictionary<string, List<Point>>();
    private bool[,] levelGrid = new bool[100, 100];
    private Point pathFindStart = new Point(25, 78);

    void Start()
    {
        for (int x = 0; x < levelGrid.GetLength(0); x++)
            for (int y = 0; y < levelGrid.GetLength(1); y++)
                levelGrid[x, y] = true;
        GenerateRoom(10, 80, rnd.Next(2, 15), rnd.Next(2, 15), 1);
        GenerateRoom(30, 78, rnd.Next(2, 15), rnd.Next(2, 15), 1);
        GenerateRoom(15, 56, rnd.Next(2, 15), rnd.Next(2, 15), 1);
        GenerateRoom(44, 78, rnd.Next(2, 15), rnd.Next(2, 15), 1);
        List<Point> pathFloor = new List<Point>();
        PathFind.Grid grid = new PathFind.Grid(levelGrid);
        Instantiate(Floor, new Vector3(pathFindStart.x, pathFindStart.y, 0), Quaternion.identity);
        levelGrid[pathFindStart.x, pathFindStart.y] = false;
        pathFloor.Add(new Point(pathFindStart.x, pathFindStart.y));
        foreach (var pair in levelDoors)
            foreach (var door in pair.Value)
            {
                int doorX = door.x;
                int doorY = door.y;
                if (pair.Key == "top")
                    doorY += 1;
                else if (pair.Key == "right")
                    doorX += 1;
                else if (pair.Key == "bottom")
                    doorY -= 1;
                else if (pair.Key == "left")
                    doorX -= 1;
                Point from = new Point(pathFindStart.x, pathFindStart.y);
                Point to = new Point(doorX, doorY);
                List<Point> path = PathFind.Pathfinding.FindPath(grid, from, to, PathFind.Pathfinding.DistanceType.Manhattan);
                foreach (var point in path)
                {
                    Instantiate(Floor, new Vector3(point.x, point.y, 0), Quaternion.identity);
                    levelGrid[point.x, point.y] = false;
                    pathFloor.Add(new Point(point.x, point.y));
                }
            }
        foreach (var point in pathFloor)
        {
            List<Point> sides = new List<Point>{
                new Point(point.x + 1, point.y),
                new Point(point.x - 1, point.y),
                new Point(point.x, point.y + 1),
                new Point(point.x, point.y - 1),
            };
            foreach (var pos in sides)
                if (levelGrid[pos.x, pos.y])
                {
                    Instantiate(Wall, new Vector3(pos.x, pos.y, 0), Quaternion.identity);
                    levelGrid[pos.x, pos.y] = false;
                }

        }
        for (int x = 0; x < levelGrid.GetLength(0); x++)
            for (int y = 0; y < levelGrid.GetLength(1); y++)
                if (levelGrid[x, y])
                    Instantiate(WallNoCollision, new Vector3(x, y, 0), Quaternion.identity);
        Player.transform.position = new Vector3(pathFindStart.x, pathFindStart.y, 0);
    }

    private void GenerateRoom(int startX, int startY, int sizeX, int sizeY, int doorCount=0, string doorSide=null)
    {
        int topX = sizeX + startX;
        int topY = sizeY + startY;
        Point[] roomCorners = {
            new Point(startX - 1, topY),       // top left corner
            new Point(topX, topY),             // top right corner
            new Point(startX - 1, startY - 1), // bottom left corner
            new Point(topX, startY - 1),       // bottom right corner
        };
        foreach (var point in roomCorners)
        {
            Instantiate(Wall, new Vector3(point.x, point.y, 0), Quaternion.identity);
            levelGrid[point.x, point.y] = false;
        }
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
            Walls["bottom"].Add(Instantiate(Wall, new Vector3(startX, nextY - 1, 0), Quaternion.identity));
            levelGrid[startX, nextY - 1] = false;
            for (int y = 0; y < sizeY; y++)
            {
                if (x == 0)
                {
                    Walls["left"].Add(Instantiate(Wall, new Vector3(startX - 1, nextY, 0), Quaternion.identity));
                    levelGrid[startX - 1, nextY] = false;
                }
                else if (x == sizeX - 1)
                {
                    Walls["right"].Add(Instantiate(Wall, new Vector3(startX + 1, nextY, 0), Quaternion.identity));
                    levelGrid[startX + 1, nextY] = false;
                }
                Instantiate(Floor, new Vector3(startX, nextY, 0), Quaternion.identity);
                levelGrid[startX, nextY] = false;
                if (y == sizeY - 1)
                {
                    Walls["top"].Add(Instantiate(Wall, new Vector3(startX, nextY + 1, 0), Quaternion.identity));
                    levelGrid[startX, nextY + 1] = false;
                } 
                nextY += 1;
            }
            startX += 1;
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

    private void CreateRoomDoor(ref Dictionary<string, List<GameObject>> Walls, ref Dictionary<string, List<(float, float)>> badDoorPositions, string doorSide=null)
    {
        if (doorSide == null)
            doorSide = Walls.ElementAt(rnd.Next(0, Walls.Count)).Key;
        int RoomWallIndex = rnd.Next(Walls[doorSide].Count);
        GameObject RoomWall = Walls[doorSide][RoomWallIndex];
        int doorX = (int)RoomWall.transform.position.x;
        int doorY = (int)RoomWall.transform.position.y;
        if (doorSide == "top" || doorSide == "bottom")
        {
            badDoorPositions[doorSide].Add((doorX - 1, doorY));
            badDoorPositions[doorSide].Add((doorX + 1, doorY));
        }
        else if (doorSide == "left" || doorSide == "right")
        {
            badDoorPositions[doorSide].Add((doorX, doorY - 1));
            badDoorPositions[doorSide].Add((doorX, doorY + 1));
        }
        if (badDoorPositions[doorSide].Any(i => i == (doorX, doorY)))
            return;
        Walls[doorSide].RemoveAt(RoomWallIndex);
        if (!levelDoors.ContainsKey(doorSide))
            levelDoors.Add(doorSide, new List<Point>());
        levelDoors[doorSide].Add(new Point(doorX, doorY));
        Instantiate(Door, new Vector3(doorX, doorY, 0), Quaternion.identity);
        levelGrid[doorX, doorY] = false;
        Destroy(RoomWall);
    }
}
