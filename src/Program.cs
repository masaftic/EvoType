using EvoType;
using System;


Keyboard keyboard = new Keyboard();

Scanner scanner = new Scanner("input.txt");

int penalty = 0;

foreach (char c in scanner.GetKeys())
{
	Console.WriteLine(keyboard.GetCostOfKey(c));
}

