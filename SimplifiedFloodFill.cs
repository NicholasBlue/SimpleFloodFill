using UnityEngine;
using System.Collections.Generic;
/// <summary>
/// Simplified flood fill Algorithm.
/// Nick Blue
/// 5/27/16
/// </summary>
public class SimplifiedFloodFill {
	/// <summary>
	/// Simple tile is our data structure used to refer to all tiles on the game field.
	/// We have given it the MonoBehavior inheritance so it can be attached to game objects.
	/// </summary>
	public class SimpleTile : MonoBehaviour{
		/// <summary>
		/// This tells the Flood Fill Algorithm if it has already been checked.
		/// </summary>
		private bool floodFill; public bool FloodFill{get{return this.floodFill;}set{this.floodFill = value;}}
		/// <summary>
		/// This is the location of the tile, it is used relationally to the other tiles.
		/// For our purposes, all tiles are 1 distance from each other. This can be modified by keeping track
		/// of neighbors through references and then checking neighbors, but that is outside the scope of this example script.
		/// </summary>
		private Vector2 tileLocation; public Vector2 Location{get{return this.tileLocation;}set{this.tileLocation = value;}}
		/// <summary>
		/// Impassable is included in the logic of this script only because it is an oft-required feature in games that use flood-filling,
		/// since it is not usually the intended effect to floodfill everything on the map.
		/// </summary>
		private bool impassable; public bool IsImpassable{get{return this.impassable;}set{this.impassable = value;}}
		/// <summary>
		/// Tiles can have variable costs that make them more or less difficult to traverse. This is an oft-required feature in games
		/// that want to implement tile effects that slow down entity movement as they pass over particular tiles.
		/// </summary>
		private int cost; public int MovementCost{get{return this.cost;}set{this.cost = value;}}
	}
	/// <summary>
	/// Floods the fill possible moves.
	/// </summary>
	/// <returns>The fill possible moves.</returns>
	/// <param name="map">Map.</param>
	/// <param name="start">Start.</param>
	/// <param name="distance">Distance.</param>
	public static List<Vector2> FloodFillPossibleMoves(Dictionary<Vector2, SimpleTile> map, Vector2 start, int distance){
		foreach (SimpleTile tile in map.Values) 
			tile.FloodFill = false;//Reset Prior Value
		List<Vector2> floodList = new List<Vector2> ();
		floodList.Add (start);
		FloodFillSearch(map, start, distance, ref floodList);
		return floodList;
	}
	/// <summary>
	/// Begins Searching the neighbors of the starting tile.
	/// </summary>
	/// <param name="map">Map.</param>
	/// <param name="start">Start.</param>
	/// <param name="distance">Distance.</param>
	/// <param name="list">List.</param>
	private static void FloodFillSearch(Dictionary<Vector2, SimpleTile> map, Vector2 start, int distance, ref List<Vector2> list){
		if (distance > 0) {//Search the neighboring tiles
			CalculateFloodFill (map, map[new Vector2(start.x,start.y+1)], distance, ref list);
			CalculateFloodFill (map, map[new Vector2(start.x+1,start.y)], distance, ref list);
			CalculateFloodFill (map, map[new Vector2(start.x,start.y-1)], distance, ref list);
			CalculateFloodFill (map, map[new Vector2(start.x-1,start.y)], distance, ref list);
		} else {
			if (!map[start].FloodFill) {
				if(!list.Contains(start))
					list.Add (start);
				map[start].FloodFill = true;
			}
		}
	}
	/// <summary>
	/// Calculates whether or not to flood fill at the specified neighboring tile.
	/// </summary>
	/// <param name="map">Map.</param>
	/// <param name="neighborTile">Neighbor tile.</param>
	/// <param name="distance">Distance.</param>
	/// <param name="list">List.</param>
	private static void CalculateFloodFill(Dictionary<Vector2, SimpleTile> map, SimpleTile neighborTile, int distance, ref List<Vector2> list){
		if (neighborTile && !neighborTile.FloodFill) {
			if (neighborTile.IsImpassable) 
				neighborTile.FloodFill = true;
			else {
				neighborTile.FloodFill = true;
				list.Add (neighborTile.Location);
				FloodFillSearch (map, neighborTile.Location, Mathf.Max(distance - neighborTile.MovementCost, 0), ref list);
			}
		} else if(neighborTile && neighborTile.FloodFill) 
			FloodFillSearch (map, neighborTile.Location, Mathf.Max(distance - neighborTile.MovementCost, 0), ref list);
	}
}