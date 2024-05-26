using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

namespace Pratique
{
    public class Pathfinding
    {
        public class PathNode
        {
            public Vector2 position;
            public int type; //Type is refering to the roadQuality property of a Tile, -1 means not passable
            public List<PathNode> neighbours = new List<PathNode>();
        }

        class AscendingAndDuplicateComparer<TKey> : IComparer<float>
        {
            public int Compare(float x, float y)
            {
                int result = -(y.CompareTo(x));
                if (result == 0)
                    return 1;
                return result;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pathfindingMap"></param>
        /// <param name="start">Vector representing the start node of the path, with x and y representing the indices of the node within the map</param>
        /// <param name="end">Vector representing the end node of the path, with x and y representing the indices of the node within the map</param>
        /// <param name="callback">Callback lambda function to pass when the computing is finished</param>
        /// <returns></returns>
        public static IEnumerator FindPath(List<PathNode> nodes, Vector2 start, Vector2 end, System.Action<List<PathNode>> callback)
        {
            int threadID = Random.Range(0, 100000);
            Stopwatch sw = Stopwatch.StartNew();

            Dictionary<PathNode, PathNode> parentMap = new Dictionary<PathNode, PathNode>();
            Dictionary<PathNode, float> fScoreMap = new Dictionary<PathNode, float>();
            Dictionary<PathNode, float> gScoreMap = new Dictionary<PathNode, float>();

            PathNode startNode = Pathfinding.getNodeAt(nodes, start);
            startNode.type = -2; //computation and memory  quiteefficient way to mark a node as the start or target
            gScoreMap.Add(startNode, 0);

            PathNode endNode = Pathfinding.getNodeAt(nodes, end);
            endNode.type = -3;

            PathNode current = startNode;

            SortedList<float, PathNode> openNodes = new SortedList<float, PathNode>(new AscendingAndDuplicateComparer<float>());
            openNodes.DefaultIfEmpty(new KeyValuePair<float, PathNode>(0, null));
            openNodes.Add(0f, startNode);

            while (openNodes.Count > 0)
            {
                //UnityEngine.Debug.Log("#" + threadID + ": open list size: " + openNodes.Count);
                openNodes.RemoveAt(openNodes.IndexOfValue(current));

                foreach (PathNode neighbour in current.neighbours)
                {
                    float gScore = gScoreMap[current] + ((neighbour.position - current.position).magnitude <= Tile.SIZE ? 10 : 14);
                    if (!gScoreMap.ContainsKey(neighbour) || gScore < gScoreMap[neighbour])
                    {
                        if (parentMap.ContainsKey(neighbour))
                            parentMap[neighbour] = current;
                        else
                            parentMap.Add(neighbour, current);

                        if (gScoreMap.ContainsKey(neighbour))
                            gScoreMap[neighbour] = gScore;
                        else
                            gScoreMap.Add(neighbour, gScore);

                        float fScore = gScore + Pathfinding.getNodeHeuristicValue(neighbour, current, endNode);
                        if (fScoreMap.ContainsKey(neighbour))
                            fScoreMap[neighbour] = fScore;
                        else
                            fScoreMap.Add(neighbour, fScore);

                        if (!openNodes.ContainsValue(neighbour))
                        {
                            openNodes.Add(fScoreMap[neighbour], neighbour);
                        }
                    }
                }

                current = openNodes.FirstOrDefault().Value;
                if (current == endNode)
                {
                    //UnityEngine.Debug.Log("#" + threadID + ": path found. " + current.x + ", " + current.y + " == " + endNode.x + "," + endNode.y);
                    break;
                }

                //UnityEngine.Debug.Log("#" + threadID + ": "+openNodes.Count);
                if ((sw.ElapsedMilliseconds / 1000f) > 60f) //60FPS
                {
                    //UnityEngine.Debug.Log("#" + threadID + ": wait for the next frame...");
                    sw.Restart();
                    yield return null;
                }
            }

            sw.Stop();
            //Rebuilding path from last checked node
            if (current != null)
            {
                List<PathNode> path = new List<PathNode>();

                while (true)
                {
                    if (parentMap.ContainsKey(current))
                        current = parentMap[current];
                    else
                        break;
                    path.Add(current);
                    yield return null;
                }

                //UnityEngine.Debug.Log("#" + threadID + ": Path size: " + path.Count);

                callback(path);
            }
            //Or sending no path when no path has been found
            else
            {
                //UnityEngine.Debug.Log("#" + threadID + ": path not found.");
                callback(null);
            }
        }

        //The higher the heuristic value, the better this node is fit for being the next choice in path
        public static float getNodeHeuristicValue(PathNode node, PathNode previous, PathNode endNode)
        {
            float nodeHeuristicValue = 1f;
            if (previous.type >= 0) //If passable, use roadQuality/type to lower heuristic value and avoid division by zero
                nodeHeuristicValue = 1f / (previous.type + 1f);
            if (previous.type == -3) //If end node, set as super important = very low heuristic value
                nodeHeuristicValue = float.MinValue;

            float distanceToTarget = (endNode.position - node.position).magnitude;
            float nodeDistanceHeuristicValue = distanceToTarget; //The higher the distance, the higher this heuristic value
            return nodeDistanceHeuristicValue * nodeHeuristicValue; //Final heuristic value of this node is the combination of distance and our custom characteristics
                                                                    //UnityEngine.Debug.Log("Heuristic result: " + node.H);
        }

        public static PathNode getNodeAt(List<PathNode> nodes, Vector2 position)
        {
            PathNode result = null;
            float dist = 0f;

            foreach (PathNode node in nodes)
            {
                Vector2 diff = position - node.position;
                if (result == null || diff.magnitude < dist)
                {
                    result = node;
                    dist = diff.magnitude;
                }
            }

            return result;
        }

        public static bool isNodeUseable(PathNode node)
        {
            return !(node.type == -1);
        }
    }
}