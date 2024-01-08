using System.Diagnostics;
using System.Text;

namespace EvoType;

public class Keyboard
{
	public const int keyboardSize = 33;
	public static readonly char[] allKeys = ['q', 'w', 'e', 'r', 't', 'y', 'u', 'i', 'o', 'p', '[', ']', 'a', 's', 'd', 'f', 'g', 'h', 'j', 'k', 'l', ';', '\'', 'z', 'x', 'c', 'v', 'b', 'n', 'm', ',', '.', '?'];

	public char[] KeySet { get; private set; } = new char[keyboardSize];

	private double[] keyCostTable = new double[keyboardSize];
	public static readonly double keySize = 19.05;
	public static readonly int[] rowSize = [12, 11, 10];
	public static readonly double[] rowOffset = [0, 0.25, 0.75];

	public static readonly Dictionary<int, int> HomeRowKeys = new()
	{
        // left hand
        { 0, 12},
		{12, 12},
		{23, 12},

		{ 1, 13},
		{13, 13},
		{24, 13},

		{ 2, 14},
		{14, 14},
		{25, 14},

		{ 3, 15},
		{15, 15},
		{26, 15},

		{ 4, 15},
		{16, 15},
		{27, 15},


        // right hand
		{ 5, 18},
		{17, 18},
		{28, 18},

		{ 6, 18},
		{18, 18},
		{29, 18},

		{ 7, 19},
		{19, 19},
		{30, 19},

		{ 8, 20},
		{20, 20},
		{31, 20},

		{ 9, 21},
		{21, 21},
		{32, 21},

		{10, 21},
		{11, 21},
		{22, 21},
	};

	public Keyboard()
	{
		for (int i = 0; i < keyboardSize; i++) keyCostTable[i] = -1;
		Array.Copy(allKeys, KeySet, keyboardSize);
	}

	public void RandomizeKeySet()
	{
		KeySet = RandomKeySet();
	}

	public static char[] RandomKeySet()
	{
		char[] randomKeySet = new char[keyboardSize];
		for (int i = 0; i < randomKeySet.Length; i++)
		{
			randomKeySet[i] = allKeys[i];
		}

		Random random = new Random();

		for (int i = 1; i < randomKeySet.Length; i++)
		{
			int randomIndex = random.Next(0, i);
			(randomKeySet[randomIndex], randomKeySet[i]) = (randomKeySet[i], randomKeySet[randomIndex]);
		}
		// Debug.Assert(ValidKeySet());
		return randomKeySet;
	}

	public double EvaluatePenalty(char[] chars)
	{
		double penalty = 0;
		foreach (char c in chars)
		{
			penalty += GetCostOfKey(c);
		}
		return penalty;
	}

	public double GetCostOfKey(char key)
	{
		key = Scanner.Normalize(key);
		if (HomeRowKeys.TryGetValue(FindHomeKeyIndex(key), out var HomeRowKeyIndex))
		{
			int keyIndex = FindKeyIndex(key);
			if (keyCostTable[keyIndex] != -1)
			{
				return keyCostTable[keyIndex];
			}
			return keyCostTable[keyIndex] = CalcDifference(CalcPos(keyIndex), CalcPos(HomeRowKeyIndex));
		}
		throw new Exception($"Unknown Character: {key}.");
	}

	public void Mutate()
	{
		Random random = new();
		for (int i = 0; i < random.Next(4); i++)
		{
			int left = random.Next(Keyboard.keyboardSize);
			int right = random.Next(Keyboard.keyboardSize);
			(KeySet[left], KeySet[right]) = (KeySet[right], KeySet[left]);
		}
	}

	public override string ToString()
	{
		string[] keyString = new string[3];
		int rowSizeSum = 0;
		for (int i = 0; i < 3; i++)
		{
			StringBuilder row = new();
			for (int j = 0; j < rowSize[i]; j++)
			{
				row.Append(KeySet[rowSizeSum + j]);
			}
			rowSizeSum += rowSize[i];

			string sep = "";
			if (i == 1) sep = " ";
			if (i == 2) sep = "  ";
			keyString[i] = sep + string.Join(" ", row.ToString().ToCharArray());
		}
		return string.Join('\n', keyString);
	}

	public bool ValidKeySet()
	{
		foreach (char c in allKeys)
		{
			if (!KeySet.Contains(c))
			{
				Console.WriteLine($"char '{c}' does not exist.");
				return false;
			}
		}
		return true;
	}

	private (int x, int y) CalcPos(int index)
	{
		int keyCount = 0, x = 0;
		for (; x < 3; x++)
		{
			if (keyCount + rowSize[x] <= index)
				keyCount += rowSize[x];
			else
				break;
		}
		int y = index - keyCount;
		return (x, y);
	}

	private int FindKeyIndex(char key)
	{
		for (int i = 0; i < KeySet.Length; i++)
		{
			if (key == KeySet[i]) return i;
		}
		throw new Exception($"Unknown Charachter: {key}.");
	}

	private static int FindHomeKeyIndex(char key)
	{
		for (int i = 0; i < allKeys.Length; i++)
		{
			if (key == allKeys[i]) return i;
		}
		throw new Exception($"Unknown Charachter: {key}.");
	}

	private static double CalcDifference((int x, int y) first, (int x, int y) second)
	{
		(double x, double y) firstPos = first, secondPos = second;
		firstPos.x *= keySize;
		firstPos.y *= keySize;
		secondPos.x *= keySize;
		secondPos.y *= keySize;

		firstPos.x += rowOffset[first.x];
		secondPos.x += rowOffset[second.x];

		double diff = Math.Sqrt((firstPos.x - secondPos.x) * (firstPos.x - secondPos.x) +
								(firstPos.y - secondPos.y) * (firstPos.y - secondPos.y));
		return diff;
	}
}


