using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Represents the data of a grid that contains placed objects.
/// </summary>
public class GridData
{
	public Dictionary<Vector3Int, List<PlacementData>> placedObjects = new();

	/// <summary>
	/// Adds a building object to the grid at the specified position and size.
	/// </summary>
	/// <param name="gridPosition">The position on the grid to place the object.</param>
	/// <param name="objectSize">The size of the object to be placed.</param>
	/// <param name="boardObjectSO">The BoardObjectSO object to be placed on the grid.</param>
	/// <param name="index">The index of the object to be placed.</param>
	/// <exception cref="Exception">Thrown when trying to place an object at an occupied position.</exception>
	public void AddObjectAt(Vector3Int gridPosition, BoardObjectSO boardObjectSO, int index)
	{
		List<Vector3Int> positionsToOccupy = CalculatePositions(gridPosition, boardObjectSO.Size);
		PlacementData data = new(gridPosition, positionsToOccupy, boardObjectSO, index);
		foreach (Vector3Int position in positionsToOccupy)
		{
			if (placedObjects.ContainsKey(position))
			{
				// throw new Exception($"Trying to place an object at an occupied position {position}");
				placedObjects[position].Add(data);
			}
			else
			{
				placedObjects.Add(position, new List<PlacementData>() { data });
			}
		}
	}

	/// <summary>
	/// Adds a new occupation position to the list of positions occupied by a board object.
	/// </summary>
	/// <param name="boardObjectIndex">The index of the board object.</param>
	/// <param name="occupationPositions">The list of positions currently occupied by the board object.</param>
	/// <param name="newOccupationPosition">The new position to be added to the list of occupied positions.</param>
	public void AddToOccupationList(int boardObjectIndex, List<Vector3Int> occupationPositions, Vector3Int newOccupationPosition)
	{
		List<Vector3Int> positionsToOccupy = occupationPositions.Concat(new List<Vector3Int>() { newOccupationPosition }).ToList();
		PlacementData data = null;
		foreach (Vector3Int position in occupationPositions)
		{
			if (placedObjects.ContainsKey(position))
			{
				PlacementData _data = placedObjects[position].Where(x => x.PlacedObjectIndex == boardObjectIndex).Last();
				_data.OccupiedPositions = positionsToOccupy;
				_data.GridPosition = newOccupationPosition;
				data = _data;
			}
		}
		if (placedObjects.ContainsKey(newOccupationPosition))
		{
			placedObjects[newOccupationPosition].Add(data);
		}
		else
		{
			placedObjects.Add(newOccupationPosition, new List<PlacementData>() { data });
		}
	}

	/// <summary>
	/// Removes the given occupation position from the list of occupied positions for the board object with the given index.
	/// </summary>
	/// <param name="boardObjectIndex">The index of the board object to remove the occupation position from.</param>
	/// <param name="occupationPositions">The list of occupation positions for the board object.</param>
	/// <param name="occupationPositionToRemove">The occupation position to remove from the list.</param>
	/// <exception cref="Exception">Thrown when trying to remove an object at an empty position.</exception>
	public void RemoveFromOccupationList(int boardObjectIndex, List<Vector3Int> occupationPositions, Vector3Int occupationPositionToRemove)
	{
		List<Vector3Int> positionsToOccupy = occupationPositions.Where(x => x != occupationPositionToRemove).ToList();
		PlacementData data = null;
		foreach (Vector3Int position in occupationPositions)
		{
			if (placedObjects.ContainsKey(position))
			{
				PlacementData _data = placedObjects[position].Where(x => x.PlacedObjectIndex == boardObjectIndex).Last();
				_data.OccupiedPositions = positionsToOccupy;
				data = _data;
			}
		}
		if (!placedObjects.ContainsKey(occupationPositionToRemove))
		{
			throw new Exception($"Trying to remove an object at an empty position {occupationPositionToRemove}");
		}
		else
		{
			placedObjects[occupationPositionToRemove].Remove(data);
		}
	}

	/// <summary>
	/// Returns the PlacementData object at the specified grid position, if it exists.
	/// </summary>
	/// <param name="gridPosition">The grid position to check for a PlacementData object.</param>
	/// <returns>The PlacementData object at the specified grid position, or null if no object exists at that position.</returns>
	public PlacementData GetObjectAt(Vector3Int gridPosition, int boardObjectIndex = -1)
	{
		if (placedObjects.ContainsKey(gridPosition))
		{
			if (boardObjectIndex >= 0)
			{
				return placedObjects[gridPosition].Where(x => x.PlacedObjectIndex == boardObjectIndex).ToList().Count > 0 ? placedObjects[gridPosition].Where(x => x.PlacedObjectIndex == boardObjectIndex).Last() : null;
			}
			else
			{
				return placedObjects[gridPosition].Count > 0 ? placedObjects[gridPosition].Last() : null;
			}
		}
		return null;
	}

	/// <summary>
	/// Calculates the positions that an object with the given size would occupy on the grid, starting from the given grid position.
	/// </summary>
	/// <param name="gridPosition">The starting grid position of the object.</param>
	/// <param name="objectSize">The size of the object in grid units.</param>
	/// <returns>A list of grid positions that the object would occupy.</returns>
	public List<Vector3Int> CalculatePositions(Vector3Int gridPosition, Vector2Int objectSize)
	{
		List<Vector3Int> positionsToOccupy = new();
		for (int x = 0; x < objectSize.x; x++)
		{
			for (int y = 0; y < objectSize.y; y++)
			{
				Vector3Int positionToOccupy = new(gridPosition.x + x, gridPosition.y + y);
				positionsToOccupy.Add(positionToOccupy);
			}
		}
		return positionsToOccupy;
	}

	/// <summary>
	/// Determines whether an object can be placed at the given grid position and size.
	/// </summary>
	/// <param name="gridPosition">The position on the grid to check.</param>
	/// <param name="objectSize">The size of the object to check.</param>
	/// <returns>True if the object can be placed at the given position and size, false otherwise.</returns>
	public bool CanPlaceObjectAt(Vector3Int gridPosition, Vector2Int objectSize)
	{
		List<Vector3Int> positionsToOccupy = CalculatePositions(gridPosition, objectSize);
		foreach (Vector3Int position in positionsToOccupy)
		{
			if (placedObjects.ContainsKey(position) && placedObjects[position].Count > 0)
			{
				return false;
			}
		}
		return true;
	}

	/// <summary>
	/// Returns the index of the placed object at the given grid position.
	/// If there is no object at the given position, returns -1.
	/// </summary>
	/// <param name="gridPosition">The position of the object in the grid.</param>
	/// <returns>The index of the placed object at the given grid position, or -1 if there is no object at the given position.</returns>
	public int GetRepresentationIndex(Vector3Int gridPosition)
	{
		if (placedObjects.ContainsKey(gridPosition) == false)
		{
			return -1;

		}
		return placedObjects[gridPosition].Count > 0 ? placedObjects[gridPosition].Last().PlacedObjectIndex : -1;
	}

	/// <summary>
	/// Removes the object at the specified grid position and all other positions it occupies.
	/// </summary>
	/// <param name="gridPosition">The grid position of the object to remove.</param>
	public void RemoveObjectAt(int boardObjectIndex, Vector3Int gridPosition)
	{
		if (!placedObjects.ContainsKey(gridPosition) || placedObjects[gridPosition].Count == 0)
		{
			return;
		}
		List<Vector3Int> positionsToRemove = placedObjects[gridPosition].Where(x => x.PlacedObjectIndex == boardObjectIndex).Last().OccupiedPositions;
		foreach (Vector3Int _position in positionsToRemove)
		{
			if (!placedObjects.ContainsKey(_position) || placedObjects[_position].Count == 0)
			{
				continue;
			}
			PlacementData dataToRemove = placedObjects[_position].Where(x => x.PlacedObjectIndex == boardObjectIndex).Last();
			if (dataToRemove == null)
			{
				continue;
			}
			placedObjects[_position].Remove(dataToRemove);
		}
	}

	/// <summary>
	/// Checks if the given grid position is walkable.
	/// </summary>
	/// <param name="gridPosition">The grid position to check.</param>
	/// <returns>True if the position is walkable, false otherwise.</returns>
	public bool IsWalkable(Vector3Int gridPosition)
	{
		if (placedObjects.ContainsKey(gridPosition) && placedObjects[gridPosition].Count > 0)
		{
			return false;
		}
		return true;
	}
}

/// <summary>
/// Represents the data required to place a building on the grid.
/// </summary>
public class PlacementData
{
	public List<Vector3Int> OccupiedPositions { get; set; }
	public BoardObjectSO BoardObjectSO { get; private set; }
	public Vector3Int GridPosition { get; set; }
	public int PlacedObjectIndex { get; private set; }

	public PlacementData(Vector3Int _gridPosition, List<Vector3Int> _occupiedPositions, BoardObjectSO _boardObjectSO, int _placeObjectIndex)
	{
		GridPosition = _gridPosition;
		OccupiedPositions = _occupiedPositions;
		BoardObjectSO = _boardObjectSO;
		PlacedObjectIndex = _placeObjectIndex;
	}
}