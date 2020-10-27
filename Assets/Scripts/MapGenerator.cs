using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using PathFind;


public class MapGenerator : MonoBehaviour
{

    public GameObject Wall;
    public GameObject WallNoCollision;
    public GameObject Floor;
    public GameObject Door;
    public GameObject Player;
    private System.Random rnd = new System.Random();
    private Dictionary<string, List<Point>> levelDoors = new Dictionary<string, List<Point>>();
    private bool[,] levelGrid = new bool[100, 100];
    private Point pathFindStart = new Point(50, 50);
    private Point playerSpawn = new Point(50, 44);

    void Start()
    {
        for (int x = 0; x < levelGrid.GetLength(0); x++)
            for (int y = 0; y < levelGrid.GetLength(1); y++)
                levelGrid[x, y] = true;
        levelGrid[pathFindStart.x, pathFindStart.y] = false;
        GenerateRoom(48, 43, rnd.Next(4, 7), rnd.Next(3, 6), 1);
        int roomsCount = rnd.Next(15, 26);
        for (int i = 0; i < roomsCount; i++)
        {
            int startX = rnd.Next(3, 76);
            int startY = rnd.Next(3, 76);
            int sizeX  = rnd.Next(3, 21);
            int sizeY  = rnd.Next(3, 21);
            if (!CheckGrid(startX, startY, sizeX, sizeY))
            {
                i--;
                continue;
            }
            int doorCount = 1;
            if (sizeX >= 5 && sizeY >= 5)
                doorCount = rnd.Next(1, 3);
            GenerateRoom(startX, startY, sizeX, sizeY, doorCount);
        }
        List<Point> pathFloor = new List<Point>();
        PathFind.Grid grid = new PathFind.Grid(levelGrid);
        InstTile(ref Floor, pathFindStart.x, pathFindStart.y);
        pathFloor.Add(new Point(pathFindStart.x, pathFindStart.y));
        foreach (var pair in levelDoors)
        {
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
                List<Point> path = PathFind.Pathfinding.FindPath(grid, from, to, PathFind.Pathfinding.DistanceType.Manhattan, true);
                foreach (var point in path)
                {
                    InstTile(ref Floor, point.x, point.y);
                    pathFloor.Add(new Point(point.x, point.y));
                }
            }
        }
        foreach (var point in pathFloor)
        {
            levelGrid[point.x, point.y] = false;
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
            {
                if (levelGrid[pos.x, pos.y])
                {
                    InstTile(ref Wall, pos.x, pos.y);
                    levelGrid[pos.x, pos.y] = false;
                }
            }
        }
        for (int x = 0; x < levelGrid.GetLength(0); x++)
            for (int y = 0; y < levelGrid.GetLength(1); y++)
                if (levelGrid[x, y])
                    InstTile(ref WallNoCollision, x, y);
        Player.transform.position = new Vector3(playerSpawn.x, playerSpawn.y, 0);
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
            InstTile(ref Wall, point.x, point.y);
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
            Walls["bottom"].Add(InstTile(ref Wall, startX, nextY - 1));
            levelGrid[startX, nextY - 1] = false;
            for (int y = 0; y < sizeY; y++)
            {
                if (x == 0)
                {
                    Walls["left"].Add(InstTile(ref Wall, startX - 1, nextY));
                    levelGrid[startX - 1, nextY] = false;
                }
                else if (x == sizeX - 1)
                {
                    Walls["right"].Add(InstTile(ref Wall, startX + 1, nextY));
                    levelGrid[startX + 1, nextY] = false;
                }
                InstTile(ref Floor, startX, nextY);
                levelGrid[startX, nextY] = false;
                if (y == sizeY - 1)
                {
                    Walls["top"].Add(InstTile(ref Wall, startX, nextY + 1));
                    levelGrid[startX, nextY + 1] = false;
                } 
                nextY++;
            }
            startX++;
        }
        var badDoorPositions = new Dictionary<string, List<Point>>
        {
            {"top",    new List<Point>()},
            {"right",  new List<Point>()},
            {"bottom", new List<Point>()},
            {"left",   new List<Point>()}
        };
        for (int i = 0; i < doorCount; i++)
            CreateRoomDoor(ref Walls, ref badDoorPositions, doorSide);
    }

    private void CreateRoomDoor(ref Dictionary<string, List<GameObject>> Walls, ref Dictionary<string, List<Point>> badDoorPositions, string doorSide=null)
    {
        if (doorSide == null)
        {
            doorSide = Walls.ElementAt(rnd.Next(0, Walls.Count)).Key;
        }
        int RoomWallIndex = rnd.Next(Walls[doorSide].Count);
        GameObject RoomWall = Walls[doorSide][RoomWallIndex];
        int doorX = (int)RoomWall.transform.position.x;
        int doorY = (int)RoomWall.transform.position.y;
        if (doorSide == "top" || doorSide == "bottom")
        {
            badDoorPositions[doorSide].Add(new Point(doorX - 1, doorY));
            badDoorPositions[doorSide].Add(new Point(doorX + 1, doorY));
        }
        else if (doorSide == "left" || doorSide == "right")
        {
            badDoorPositions[doorSide].Add(new Point(doorX, doorY - 1));
            badDoorPositions[doorSide].Add(new Point(doorX, doorY + 1));
        }
        Point doorPosition = new Point(doorX, doorY);
        if (badDoorPositions[doorSide].Any(i => i == doorPosition))
        {
            return;
        }
        Walls[doorSide].RemoveAt(RoomWallIndex);
        if (!levelDoors.ContainsKey(doorSide))
            levelDoors.Add(doorSide, new List<Point>());
        levelDoors[doorSide].Add(new Point(doorX, doorY));
        InstTile(ref Door, doorX, doorY);
        levelGrid[doorX, doorY] = false;
        Destroy(RoomWall);
    }

    private bool CheckGrid(int startX, int startY, int sizeX, int sizeY)
    {
        for (int x = 0; x < sizeX; x++)
        {
            int nextY = startY;
            for (int y = 0; y < sizeY; y++)
            {
                List<Point> sides = new List<Point>{
                    new Point(startX + 2, nextY),
                    new Point(startX - 2, nextY),
                    new Point(startX, nextY + 2),
                    new Point(startX, nextY - 2),
                };
                foreach (var pos in sides)
                {
                    if (!levelGrid[pos.x, pos.y])
                    {
                        return false;
                    }
                }
                nextY++;
            }
            startX++;
        }
        return true;
    }

    private GameObject InstTile(ref GameObject tile, int Vec3X, int Vec3Y)
    {
        return Instantiate(tile, new Vector3(Vec3X, Vec3Y, 0), Quaternion.identity);
    }
}
