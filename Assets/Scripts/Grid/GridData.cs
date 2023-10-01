using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents the data of a grid that contains placed objects.
/// </summary>
public class GridData
{
	public Dictionary<Vector3Int, PlacementData> placedObjects = new();

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
				throw new Exception($"Trying to place an object at an occupied position {position}");
			}
			placedObjects.Add(position, data);
		}
	}

	/// <summary>
	/// Returns the PlacementData object at the specified grid position, if it exists.
	/// </summary>
	/// <param name="gridPosition">The grid position to check for a PlacementData object.</param>
	/// <returns>The PlacementData object at the specified grid position, or null if no object exists at that position.</returns>
	public PlacementData GetObjectAt(Vector3Int gridPosition)
	{
		if (placedObjects.ContainsKey(gridPosition))
		{
			return placedObjects[gridPosition];
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
			if (placedObjects.ContainsKey(position))
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
		return placedObjects[gridPosition].PlacedObjectIndex;
	}

	/// <summary>
	/// Removes the object at the specified grid position and all other positions it occupies.
	/// </summary>
	/// <param name="gridPosition">The grid position of the object to remove.</param>
	public void RemoveObjectAt(Vector3Int gridPosition)
	{
		foreach (var pos in placedObjects[gridPosition].occupiedPositions)
		{
			placedObjects.Remove(pos);
		}
	}
}

/// <summary>
/// Represents the data required to place a building on the grid.
/// </summary>
public class PlacementData
{
	public List<Vector3Int> occupiedPositions;
	public BoardObjectSO BoardObjectSO { get; private set; }
	public Vector3Int GridPosition { get; private set; }
	public int PlacedObjectIndex { get; private set; }

	public PlacementData(Vector3Int _gridPosition, List<Vector3Int> _occupiedPositions, BoardObjectSO _boardObjectSO, int _placeObjectIndex)
	{
		GridPosition = _gridPosition;
		occupiedPositions = _occupiedPositions;
		BoardObjectSO = _boardObjectSO;
		PlacedObjectIndex = _placeObjectIndex;
	}
}