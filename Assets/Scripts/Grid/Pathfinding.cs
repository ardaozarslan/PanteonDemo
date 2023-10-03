using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Pathfinding
{

	public static List<Tile> FindPath(Tile startNode, Tile targetNode)
	{
		var toSearch = new List<Tile>() { startNode };
		var processed = new List<Tile>();

		while (toSearch.Any())
		{
			var current = toSearch[0];
			foreach (var t in toSearch)
				if (t.F < current.F || t.F == current.F && t.H < current.H) current = t;

			processed.Add(current);
			toSearch.Remove(current);


			if (current == targetNode)
			{
				var currentPathTile = targetNode;
				var path = new List<Tile>();
				var count = 100;
				while (currentPathTile != startNode)
				{
					path.Add(currentPathTile);
					currentPathTile = currentPathTile.Connection;
					count--;
					if (count < 0) throw new Exception();
				}

				// Debug.Log(path.Count);
				return path.Reverse<Tile>().ToList();
			}

			foreach (var neighbor in current.Neighbors.Where(t => t.Walkable && !processed.Contains(t)))
			{
				var inSearch = toSearch.Contains(neighbor);

				var costToNeighbor = current.G + current.GetDistance(neighbor);

				if (!inSearch || costToNeighbor < neighbor.G)
				{
					neighbor.SetG(costToNeighbor);
					neighbor.SetConnection(current);

					if (!inSearch)
					{
						neighbor.SetH(neighbor.GetDistance(targetNode));
						toSearch.Add(neighbor);
					}
				}
			}
		}
		Tile closestTile = FindClosestEmptyTile(startNode, new List<Vector3Int>() { targetNode.GridPos });
		if (closestTile != null)
		{
			return FindPath(startNode, closestTile);
		}
		return null;
	}

	// Returns the closest empty tile to the startNode that is a neighbor of one of the tiles positioned at the occupied positions
	public static Tile FindClosestEmptyTile(Tile startNode, List<Vector3Int> occupiedPositions)
	{
		List<Tile> toSearch = new();
		foreach (Vector3Int _pos in occupiedPositions)
		{
			GridManager.Instance.GetTileAtPosition(_pos).Neighbors.ForEach(x => {
				if ((x == startNode || x.Walkable) && !toSearch.Contains(x))
				{
					toSearch.Add(x);
				}
			});
		}
		Tile closestTile = null;
		float closestDistance = Mathf.Infinity;
		foreach (Tile tile in toSearch)
		{
			List<Tile> path = FindPath(startNode, tile);
			if (path != null)
			{
				float distance = path.Count;
				if (distance < closestDistance)
				{
					closestDistance = distance;
					closestTile = tile;
				}
			}
		}
		return closestTile;
	}
}