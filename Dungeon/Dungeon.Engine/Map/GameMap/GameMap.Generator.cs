using Dungeon.Settings;
using Dungeon.Types;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;

namespace Dungeon.Map
{
    public partial class GameMap
    {
        public Action OnGeneration;

        private const char Wall = '#';
        private const char Floor = '.';
        private const char Corridor = '.';
        private const char Door = '/';
        private const char Exit = '>';
        private const char Player = '@';
        private const char Enemy = '*';

        private bool generation = false;

        public void Generate()
        {
            if (generation)
                return;

            generation = true;

            var removeDeadEnds = true;
            var extraCon = 4 * Level;
            var numberroom =  Level;
            var windingperc =  50;
            int enemies = (int)Math.Round(Level * .2);

            var gen = new MazeGenerator(41, 21, removeDeadEnds: removeDeadEnds, extraConnectorChance: extraCon, numberRoomTries: numberroom, windingPercent: windingperc, numberOfEnemies:enemies);
            try
            {
                gen.Generate();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            var genned = //TRYGROW(
                gen.PrintDebug();

            Console.WriteLine(genned);

            int x = 0;
            int y = 0;

            var player = this.Map.Nodes.FirstOrDefault(nod => !typeof(GameMapContainerObject).IsAssignableFrom(nod.GetType()));

            this.MapOld = new List<List<List<MapObject>>>();
            this.Map = new GameMapObject();
            
            foreach (var line in genned.Split('\n', StringSplitOptions.RemoveEmptyEntries))
            {
                var listLine = new List<List<Map.MapObject>>();

                x = 0;

                foreach (var @char in line)
                {
                    MapObject mapObj = null;

                    if (@char == '@')
                    {
                        mapObj = player;
                    }
                    else
                    {
                        mapObj = MapObject.Create(@char.ToString());
                    }

                    if (@char != '#' && @char != '.')
                    {
                        AddMapObject(x, y, MapObject.Create("."));
                    }

                    AddMapObject(x, y, mapObj);

                    listLine.Add(new List<MapObject>() { mapObj });
                    x++;
                }

                y++;

                this.MapOld.Add(listLine);
            }

            this.Map.Add(player);

            needReloadCache = true;
            generation = false;
            OnGeneration?.Invoke();
        }

        private string TRYGROW(string generated)
        {
            var list = generated.Split('\n', StringSplitOptions.RemoveEmptyEntries).ToList();
            
            var lines = generated.Split('\n', StringSplitOptions.RemoveEmptyEntries);

            var shift = 0;
            
            for (int y = 0; y < lines.Length; y++)
            {
                StringBuilder topLine = new StringBuilder(new String(Enumerable.Range(0, 39).Select(x => '.').ToArray()));

                var line = lines[y];
                for (int x = 0; x < line.Length; x++)
                {
                    var @char = line[x];
                    var topChar = y == 0
                        ? '.'
                        : lines[y - 1][x];

                    if (@char == '#' && topChar != '#')
                    {
                        topLine[x] = '#';
                    }
                    else
                    {
                        topLine[x] = @char == '#'
                            ? '#'
                            : '.';
                    }
                }

                list.Insert(y+shift, topLine.ToString());
                shift++;
            }

            return string.Join('\n', list);
        }

        private void AddMapObject(int x, int y, MapObject mapObj)
        {
            mapObj.Location = new Point(x, y);
            mapObj.Region = new Rectangle
            {
                Height = 32,
                Width = 32,
                Pos = mapObj.Location
            };

            if (mapObj.Obstruction)
            {
                this.Map.Add(mapObj);
            }
        }

        /// <summary>
        /// The random dungeon generator.
        ///
        /// Starting with a stage of solid walls, it works like so:
        ///
        /// 1. Place a number of randomly sized and positioned rooms. If a room
        ///    overlaps an existing room, it is discarded. Any remaining rooms are
        ///    carved out.
        /// 2. Any remaining solid areas are filled in with mazes. The maze generator
        ///    will grow and fill in even odd-shaped areas, but will not touch any
        ///    rooms.
        /// 3. The result of the previous two steps is a series of unconnected rooms
        ///    and mazes. We add connectors for each room.
        /// 4. The mazes will have a lot of dead ends. Finally, we remove those by
        ///    repeatedly filling in any open tile that's closed on three sides. When
        ///    this is done, every corridor in a maze actually leads somewhere.
        ///
        /// The end result of this is a multiply-connected dungeon with rooms and lots
        /// of winding corridors.
        /// </summary>
        public class MazeGenerator
        {
            private bool ExitExists = false;
            public char[,] Maze { get; private set; }

            public int StageWidth { get; set; }
            public int StageHeight { get; set; }

            public int NumberRoomTries { get; set; }

            public int NumberOfEnemies { get; set; }

            public bool RemoveDeadEnds { get; set; }

            /// <summary>
            /// The inverse chence of adding a connector between two regions that have
            /// already been joined. Increasing this leads to more loosely connected dungeons
            /// </summary>
            public int ExtraConnectorChance { get; set; }

            /// <summary>
            /// Increasing this allows rooms to be larger
            /// </summary>
            public int RoomExtraSize { get; set; }

            public int WindingPercent { get; set; }

            private List<Rectangle> _rooms;

            public MazeGenerator(int stageWidth = 51, int stageHeight = 51, int numberRoomTries = 30, int roomExtraSize = 0, int extraConnectorChance = 20, int windingPercent = 0, bool removeDeadEnds = true, int numberOfEnemies = 10)
            {
                StageHeight = stageHeight;
                StageWidth = stageWidth;
                NumberRoomTries = numberRoomTries;
                RoomExtraSize = roomExtraSize;
                RemoveDeadEnds = removeDeadEnds;
                WindingPercent = windingPercent;
                NumberOfEnemies = numberOfEnemies;
                ExtraConnectorChance = extraConnectorChance;
                Maze = new char[stageWidth, stageHeight];
                _rooms = new List<Rectangle>();
            }

            public char[,] Generate()
            {
                if (StageWidth % 2 == 0 || StageHeight % 2 == 0)
                {
                    throw new System.Exception("The stage must be odd-sized.");
                }

                _fill(Wall);

                //this.PrintDebug("fill wall");

                // Add the rooms
                _addRooms();

                //this.PrintDebug("add rooms");

                // Fill in all of the empty space with mazes.
                for (int y = 1; y < StageHeight; y += 2)
                {
                    for (int x = 1; x < StageWidth; x += 2)
                    {
                        _growMaze(x, y);
                    }
                }

                //this.PrintDebug("growed");

                _connectRegions();
                //this.PrintDebug("connecting");

                if (RemoveDeadEnds)
                {
                    _removeDeadEnds();
                    //this.PrintDebug("remove deadends");
                }

                _addPlayer();
                _addExit();
                //this.PrintDebug("added player");


                _addEnemies();
                //this.PrintDebug("added enemies");

                return Maze;
            }

            private void _addRooms()
            {
                for (int i = 0; i < NumberRoomTries; i++)
                {
                    // Pick a random room size. The funny math here does two things:
                    // - It makes sure rooms are odd-sized to line up with maze.
                    // - It avoids creating rooms that are too rectangular: too tall and
                    //   narrow or too wide and flat.
                    // TODO: This isn't very flexible or tunable. Do something better here.
                    int size = (int)RandomRogue.Range(1, 3 + RoomExtraSize) * 2 + 1;
                    int rectangularity = (int)RandomRogue.Range(0, 1 + size / 2) * 2;
                    var width = size;
                    var height = size;
                    if (RandomRogue.Range(0, 1) > 0.5f)
                    {
                        width += rectangularity;
                    }
                    else
                    {
                        height += rectangularity;
                    }

                    int x = (int)RandomRogue.Range(0, (StageWidth - width) / 2) * 2 + 1;
                    int y = (int)RandomRogue.Range(0, (StageHeight - height) / 2) * 2 + 1;

                    var room = new Rectangle(x, y, width, height);

                    //ебучий подорожник что бы комнаты не генерировались на краю
                    if(room.xMax>=StageWidth || room.yMax>=StageHeight)
                    {
                        continue;
                    }

                    bool overlaps = false;
                    for (int j = 0; j < _rooms.Count; j++)
                    {
                        if (room.Overlaps(_rooms[j]))
                        {
                            overlaps = true;
                            break;
                        }
                    }
                    if (overlaps) continue;

                    _rooms.Add(room);

                    for (int pos = 0; pos < width * height; pos++)
                    {
                        _carve((pos % width) + x, (int)(pos / width) + y);
                    }
                }
            }

            /// <summary>
            /// Implementation of the "growing tree" algorithm from here:
            /// http://www.astrolog.org/labyrnth/algrithm.htm.
            /// </summary>
            private void _growMaze(int start_x, int start_y)
            {
                LinkedList<Vector2> cells = new LinkedList<Vector2>();
                Vector2 lastDirection = new Vector2();

                _carve(start_x, start_y, Corridor);

                cells.AddLast(new Vector2(start_x, start_y));

                int loopAvoidance = 0;
                while (cells.Count != 0 && loopAvoidance < 10000)
                {
                    loopAvoidance++;
                    Vector2 cell = cells.Last.Value;

                    // See which adjacent cells are open.
                    List<Vector2> unmadeCells = new List<Vector2>();

                    for (int i = 0; i < Directions.Cardinals.Length; i++)
                    {
                        Vector2 dir = Directions.Cardinals[i];
                        if (_canCarve(cell + dir) && _canCarve(cell + dir * 2)) unmadeCells.Add(dir);
                    }

                    if (unmadeCells.Count != 0)
                    {
                        // Based on how "windy" passages are, try to prefer carving in the
                        // same direction.
                        Vector2 dir;
                        if (unmadeCells.Contains(lastDirection) && RandomRogue.Range(0, 100) < 100 - WindingPercent)
                        {
                            dir = lastDirection;
                        }
                        else
                        {
                            int randomIndex = RandomRogue.Range(0, unmadeCells.Count - 1);
                            dir = unmadeCells[randomIndex];
                        }

                        _carve((int)(cell + dir).X, (int)(cell + dir).Y, Corridor);
                        _carve((int)(cell + dir * 2).X, (int)(cell + dir * 2).Y, Corridor);

                        cells.AddLast(cell + dir * 2);
                        lastDirection = dir;
                    }
                    else
                    {
                        // No adjacent uncarved cells.
                        cells.RemoveLast();

                        // This path has ended.
                        lastDirection = new Vector2();
                    }
                }
            }

            private bool _canCarve(Vector2 nextCell)
            {
                return ((int)nextCell.X > 0 && (int)nextCell.Y > 0 && (int)nextCell.X < StageWidth && (int)nextCell.Y < StageHeight && Maze[(int)nextCell.X, (int)nextCell.Y] == Wall);
            }

            private void _carve(int position_x, int position_y, char type = '0')
            {
                if (type == '0') type = Floor;
                try
                {
                    _setTile(position_x, position_y, type);
                }
                catch (System.Exception e)
                {
                    Console.WriteLine(position_x + "/" + Maze.GetLength(0));
                    Console.WriteLine(position_y + "/" + Maze.GetLength(1));
                }
            }

            private void _setTile(int x, int y, char type)
            {
                Maze[x, y] = type;
            }

            private void _setTile(Vector2 pos, char type)
            {
                _setTile((int)pos.X, (int)pos.Y, type);
            }

            private void _fill(char type)
            {
                for (int i = 0; i < StageWidth; i++)
                    for (int j = 0; j < StageHeight; j++)
                        Maze[i, j] = type;
            }

            private void _connectRegions()
            {
                // For each rectangle find walls that has floor behind
                for (int i = 0; i < _rooms.Count; i++)
                {
                    int connections = 0;
                    int lastSide = -1;
                    // until we have at least 2 connections for that rectangle
                    int doorTries = 0;
                    int moreDoors = RandomRogue.Range(0, 100) > 100 - ExtraConnectorChance ? 1 : 0;
                    while (connections < 2 + moreDoors && doorTries < 100)
                    {
                        doorTries++;
                        // Pick a random side
                        int side = RandomRogue.Range(0, 15) % 4;
                        if (side == lastSide) side = (side + RandomRogue.Range(0, 3)) % 4; // Priority to different side then last one
                        Line line;
                        Vector2 direction;
                        if (side == 0)
                        {
                            line = new Line(_rooms[i].Xf + 1, _rooms[i].Yf, _rooms[i].xMax - 1, _rooms[i].Yf);
                            direction = Directions.up;
                        }
                        else if (side == 1)
                        {
                            line = new Line(_rooms[i].xMax + 1, _rooms[i].Yf + 2, _rooms[i].xMax + 1, _rooms[i].yMax);
                            direction = Directions.right;
                        }
                        else if (side == 2)
                        {
                            line = new Line(_rooms[i].Xf + 2, _rooms[i].yMax + 1, _rooms[i].xMax, _rooms[i].yMax + 1);
                            direction = Directions.down;
                        }
                        else // if (side == 3)
                        {
                            line = new Line(_rooms[i].Xf, _rooms[i].Yf + 1, _rooms[i].Xf, _rooms[i].yMax - 1);
                            direction = Directions.left;
                        }

                        // Pick a random point in the line perpendicular to direction
                        int randomX = RandomRogue.Range((int)line.Start.X, (int)line.End.X) - 1;
                        int randomY = RandomRogue.Range((int)line.Start.Y, (int)line.End.Y) - 1;

                        // Debug
                        // Maze[randomX, randomY] = side.ToString()[0];

                        // see if point + direction is floor, if it is, place the door and increment connections
                        if (_canPlaceDoor(randomX, randomY, direction))
                        {
                            Maze[randomX, randomY] = Door;
                            connections++;
                            if (connections >= 2) lastSide = -1;
                        }

                    }
                }
            }

            private bool _canPlaceDoor(int randomX, int randomY, Vector2 direction)
            {
                return (randomY + direction.Y >= 0 && randomX + direction.X >= 0 &&
                            randomY + direction.Y < Maze.GetLength(1) && randomX + direction.X < Maze.GetLength(0) &&
                            _noDoorsAround(randomX, randomY) &&
                            Maze[randomX + (int)direction.X, randomY + (int)direction.Y] == Floor);
            }

            private void _addPlayer()
            {
                try
                {
                    // Pick a random room
                    Rectangle randomRoom = _rooms[RandomRogue.Range(0, _rooms.Count - 1)];
                    // Pick a random floor tile in the room, 2 tiles from the wall
                    Vector2 pos = new Vector2(RandomRogue.Range((int)randomRoom.X + 2, (int)randomRoom.xMax - 2), RandomRogue.Range((int)randomRoom.Y + 2, (int)randomRoom.yMax - 2));

                    // set player position
                    if (_getTile(pos) == Floor)
                        _setTile(pos, Player);
                }
                catch
                {
                    _addPlayer();
                }
            }

            private void _addExit()
            {
                try
                {
                    // Pick a random room
                    Rectangle randomRoom = _rooms[RandomRogue.Range(0, _rooms.Count - 1)];
                    // Pick a random floor tile in the room, 2 tiles from the wall
                    Vector2 pos = new Vector2(RandomRogue.Range((int)randomRoom.X + 1, (int)randomRoom.xMax - 1), RandomRogue.Range((int)randomRoom.Y + 1, (int)randomRoom.yMax - 1));

                    // set player position
                    if (_getTile(pos) == Floor)
                        _setTile(pos, Exit);
                }
                catch
                {
                    _addExit();
                }
            }

            private void _addEnemies()
            {
                for (int i = 0; NumberOfEnemies > 0 && i < 1000; i++)
                {
                    try
                    {
                        // Pick a random room
                        Rectangle randomRoom = _rooms[RandomRogue.Range(0, _rooms.Count)];
                        // Pick a random floor tile in the room, 2 tiles from the wall
                        Vector2 pos = new Vector2(RandomRogue.Range((int)randomRoom.X + 2, (int)randomRoom.xMax - 2), RandomRogue.Range((int)randomRoom.Y + 2, (int)randomRoom.yMax - 2));

                        // set player position
                        if (_getTile(pos) == Floor)
                        {
                            _setTile(pos, Enemy);
                            NumberOfEnemies--;
                        }
                    }
                    catch
                    {                        
                    }
                }
            }

            private bool _noDoorsAround(int x, int y)
            {
                if (Maze[x, y] == Door)
                {
                    return false;
                }
                for (int i = 0; i < Directions.Cardinals.Length; i++)
                {
                    if (x + (int)Directions.Cardinals[i].X >= 0 && y + (int)Directions.Cardinals[i].Y >= 0 &&
                       x + (int)Directions.Cardinals[i].X < Maze.GetLength(0) && y + (int)Directions.Cardinals[i].Y < Maze.GetLength(1) &&
                       Maze[x + (int)Directions.Cardinals[i].X, y + (int)Directions.Cardinals[i].Y] == Door)
                    {
                        return false;
                    }
                }
                return true;
            }

            private char _getTile(Vector2 pos)
            {
                return Maze[(int)pos.X, (int)pos.Y];
            }

            private void _removeDeadEnds()
            {
                var done = false;

                while (!done)
                {
                    done = true;

                    for (int i = 0; i < StageHeight * StageWidth; i++)
                    {
                        Vector2 pos = new Vector2(i % StageWidth, (int)(i / StageWidth));
                        if (_getTile(pos) == Wall) continue;

                        // If it only has one exit, it's a dead end.
                        var exits = 0;
                        for (int j = 0; j < Directions.Cardinals.Length; j++)
                        {
                            Vector2 dir = Directions.Cardinals[j];
                            try
                            {
                                var vect = pos + dir;

                                if (vect.X == StageWidth)
                                {
                                    vect.X -= 1;
                                }
                                if (vect.Y==StageHeight)
                                {
                                    vect.Y -= 1;
                                }

                                if (_getTile(vect) != Wall)
                                {
                                    exits++;
                                }
                            }
                            catch (Exception ex)
                            {
                                throw ex;
                            }
                        }

                        if (exits != 1) continue;

                        done = false;
                        _setTile(pos, Wall);
                    }
                }
            }

            public string PrintDebug(string stage = null)
            {
                if (stage != null)
                {
                    Console.WriteLine(stage);
                }


                var matrix = Maze;
                string print = "";

                for (int y = 0; y < matrix.GetLength(1); ++y)
                {
                    for (int x = 0; x < matrix.GetLength(0); ++x)
                    {
                        print += matrix[x, y];
                    }
                    print += "\n";
                }

                Console.WriteLine(print);
                return print;
            }

            private static class Directions
            {
                public static readonly Vector2[] Cardinals = new Vector2[4]{
                    new Vector2(0,-1),  // up
                    new Vector2(0,1),   // down
                    new Vector2(-1,0),  // left
                    new Vector2(1,0)    // right
                };
                public static readonly Vector2 up = new Vector2(0, -1);
                public static readonly Vector2 down = new Vector2(0, 1);
                public static readonly Vector2 left = new Vector2(-1, 0);
                public static readonly Vector2 right = new Vector2(1, 0);
            }

            private class Line
            {
                public Vector2 Start { get; set; }
                public Vector2 End { get; set; }

                public Line(float x1, float y1, float x2, float y2)
                {
                    Start = new Vector2(x1, y1);
                    End = new Vector2(x2, y2);
                }
            }
        }
    }
}