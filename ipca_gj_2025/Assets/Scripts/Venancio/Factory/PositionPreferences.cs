using UnityEngine;

/// <summary>
/// Represents the position preferences for an object, including its container, position, and whether the position is local.
/// </summary>
public struct PositionPreferences
{
	/// <summary>
	/// The container transform for the object.
	/// </summary>
	public Transform Container;

	/// <summary>
	/// The position of the object.
	/// </summary>
	public Vector3 Position;

	/// <summary>
	/// Indicates whether the position is local to the container.
	/// </summary>
	public bool IsLocalPosition;

	/// <summary>
	/// Initializes a new instance of the <see cref="PositionPreferences"/> struct with the specified container, position, and local position flag.
	/// </summary>
	/// <param name="container">The container transform for the object.</param>
	/// <param name="position">The position of the object.</param>
	/// <param name="isLocalPosition">Indicates whether the position is local to the container.</param>
	public PositionPreferences(Transform container, Vector3 position, bool isLocalPosition)
	{
		Container = container;
		Position = position;
		IsLocalPosition = isLocalPosition;
	}
}