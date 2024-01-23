/**
 * Flags to track the walls in a cell. For that we use the NESW order with the order 0b<W><S><E><N>
 * where "0" means "closed" (there is a wall) and "1" means "open" (there is no wall).
 * 
 * There will be combinations out of these Flags. For example 0b1001 witch means "passage to the south and the west" (so it's a corner)
 * or 0b1110 witch means "passage to the south, west and east" (so it's a T-crossing)
 * usw. 
 */
[System.Flags]
public enum MazeFlags
{
	Empty = 0,

	PassageN = 0b0001, //there is a way to the north
	PassageE = 0b0010, //there is a way to the east
	PassageS = 0b0100, //there is a way to the south
	PassageW = 0b1000, //there is a way to the west

	PassageAll = 0b1111//there is a way in all directions
}
