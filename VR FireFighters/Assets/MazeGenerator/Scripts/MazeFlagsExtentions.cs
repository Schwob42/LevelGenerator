/**
 * In this class we have a lot of bit wise operations like AND, OR, etc to add, subtract and check the masks.
 * 
 */

public static class MazeFlagsExtensions
{
	public static bool Has(this MazeFlags flags, MazeFlags mask) =>
		(flags & mask) == mask;

	public static bool HasAny(this MazeFlags flags, MazeFlags mask) =>
		(flags & mask) != 0;

	public static bool HasNot(this MazeFlags flags, MazeFlags mask) =>
		(flags & mask) != mask;

	public static bool HasExactlyOne(this MazeFlags flags) =>
		flags != 0 && (flags & (flags - 1)) == 0;

	/**
	 * Adds to masks
	 * m1 = b10001
	 * m2 = b10010
	 * 
	 * => = b10011
	 */
	public static MazeFlags With(this MazeFlags flags, MazeFlags mask) =>
		flags | mask;

	/**
	 * Subtracts from masks
	 * m1 = b10011
	 * m2 = b10010
	 * 
	 * => = b10001
	 */
	public static MazeFlags Without(this MazeFlags flags, MazeFlags mask) =>
		flags & ~mask;
}