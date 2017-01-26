using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MapGenerator : MonoBehaviour {

    public int width;
    public int height;

    public string seed;
    public bool useRandomSeed;

    [Range(0, 100)] public int randomFillPercent;

    public int mapSmoother = 5;
    public int borderSize = 5;

    int[,] map;

	// Use this for initialization
	void Start () {
        GenerateMap();
	}
	
    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            GenerateMap();
        }
    }

	void GenerateMap() {
        map = new int[width, height];
        RandomFillMap();

        for (int i = 0; i < mapSmoother; i++) {
            SmoothMap();
        }

        ProcessTheMap();

        int[,] borderedMap = new int[width + borderSize * 2, height + borderSize * 2];

        for (int x = 0; x < borderedMap.GetLength(0); x++) {
            for (int y = 0; y < borderedMap.GetLength(1); y++) {
                if (x >= borderSize && x < width+borderSize && y >= borderSize && y < height+borderSize) {
                    borderedMap[x, y] = map[x - borderSize, y - borderSize];
                } else {
                    borderedMap[x,y] = 1;  // this is the wall tile
                }
            }
        }

        MeshGenerator meshGen = GetComponent<MeshGenerator>();
        meshGen.GenerateMesh(borderedMap, 1);
    }

    // if regions of the map considered as "cave walls" are less than the threshold, turn that into the same as the surrounding region.
    void ProcessTheMap() {
        List<List<Coordinates>> wallRegions = GetRegions(1);

        int wallThresholdSize = 50;
        foreach (List<Coordinates> wallRegion in wallRegions) {
            if (wallRegion.Count < wallThresholdSize) {
                foreach (Coordinates tile in wallRegion) {
                    map[tile.tileX, tile.tileY] = 0;
                }
            }
        }

        List<List<Coordinates>> roomRegions = GetRegions(0);

        int roomThresholdSize = 50;
        foreach (List<Coordinates> roomRegion in roomRegions) {
            if (roomRegion.Count < roomThresholdSize) {
                foreach (Coordinates tile in roomRegion) {
                    map[tile.tileX, tile.tileY] = 1;
                }
            }
        }
    }

    List<List<Coordinates>> GetRegions(int tileType) {
        List<List<Coordinates>> regions = new List<List<Coordinates>>();
        int[,] mapFlags = new int[width, height];

        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                if (mapFlags[x, y] == 0 && map[x, y] == tileType) {
                    List<Coordinates> newRegion = GetRegionTiles(x, y);
                    regions.Add(newRegion);

                    foreach(Coordinates tile in newRegion) {
                        mapFlags[tile.tileX, tile.tileY] = 1; // flagged "1", to mean that this tile has already been looked at.
                    }
                }
            }
        }

        return regions;
    }

    List<Coordinates> GetRegionTiles(int startX, int startY) {
        List<Coordinates> tiles = new List<Coordinates>();
        int[,] mapFlags = new int[width, height]; // stores which tile has already been looked at
        int tileType = map[startX, startY]; // stores whether or not this is a wall tile

        Queue<Coordinates> queue = new Queue<Coordinates>(); // stores coordinates
        queue.Enqueue(new Coordinates(startX, startY)); // adds to this queue
        mapFlags[startX, startY] = 1; // sets the map flags to show that we've now looked at this tile.

        while (queue.Count > 0) {
            Coordinates tile = queue.Dequeue();
            tiles.Add(tile);

            // looks at all the adjacent tiles
            for (int x = tile.tileX - 1; x <= tile.tileX + 1; x++) {
                for (int y = tile.tileY -1; y <= tile.tileY + 1; y++) {
                    // check if tile is INSIDE the map, AND tiles that AREN'T diagonal.
                    if (IsInMapRange(x, y) && (y == tile.tileY || x == tile.tileX) ) { // the y == tile.tileY & x == tile.tileX means that this x & y is in the center, therefore not a diagonal.
                        // check map flags to make sure we haven't looked at these tiles yet.
                        if (mapFlags[x, y] == 0 && map[x,y] == tileType) {
                            mapFlags[x, y] = 1;
                            queue.Enqueue(new Coordinates(x, y));

                        }
                    }
                    

                }
            }
        }

        return tiles;
    }

    bool IsInMapRange(int x, int y) {
        return x >= 0 && x < width && y >= 0 && y < height;
    }

    void RandomFillMap() {
        if (useRandomSeed) {
            seed = Time.time.ToString();
        }

        System.Random pseudoRNG = new System.Random(seed.GetHashCode());

        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                if (x == 0 || x == width - 1 || y == 0 || y == height - 1 ) {
                    map[x, y] = 1;
                } else {
                    map[x, y] = (pseudoRNG.Next(0, 100) < randomFillPercent) ? 1 : 0;
                }
            }
        }
    }

    void SmoothMap() {
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                int neighborWallTiles = GetSurroundingWallCount(x, y);

                if (neighborWallTiles > mapSmoother-1) {
                    map[x, y] = 1;
                } else if (neighborWallTiles < mapSmoother-1) {
                    map[x, y] = 0;
                }
            }
        }
    }

    int GetSurroundingWallCount(int gridX, int gridY) {
        int wallCount = 0;

        for (int neighborX = gridX - 1; neighborX <= gridX + 1; neighborX++) {
            for (int neighborY = gridY - 1; neighborY <= gridY + 1; neighborY++) {
                if ( IsInMapRange(neighborX, neighborY) ) {
                    if (neighborX != gridX || neighborY != gridY) {
                        wallCount += map[neighborX, neighborY];
                    }
                } else {
                    wallCount++;
                }
            }
        }

        return wallCount;
    }

    struct Coordinates {
        public int tileX;
        public int tileY;

        public Coordinates(int x, int y) {
            tileX = x;
            tileY = y;
        }
    }

    /*private void OnDrawGizmos() {
        if (map != null) {
            for (int x = 0; x < width; x++) {
                for (int y = 0; y < height; y++) {
                    Gizmos.color = (map[x, y] == 1) ? Color.black : Color.white;
                    Vector3 pos = new Vector3(-width/2 + x + 0.5f, 0, -height/2 + y + 0.5f);
                    Gizmos.DrawCube(pos, Vector3.one);
                }
            }
        }
    }*/
}