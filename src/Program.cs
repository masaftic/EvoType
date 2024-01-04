using EvoType;
using System;


Keyboard bestKeyboard = new Keyboard();


Scanner scanner = new Scanner("E:\\dev\\EvoType\\input.txt");

double bestPenalty = 1e9;

for (int i = 0; i < 1000; i++)
{

	double penalty = 0;

	Keyboard keyboard = new Keyboard();
	keyboard.RandomizeKeySet();

	foreach (char c in scanner.GetKeys())
	{
		penalty += keyboard.GetCostOfKey(c);
	}

	if (penalty < bestPenalty)
	{
		bestPenalty = penalty;
		bestKeyboard = keyboard;
	}
}


Console.WriteLine($"{bestPenalty}");
Console.WriteLine(bestKeyboard.PrintKeySet()); 

