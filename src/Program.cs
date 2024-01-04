using EvoType;
using System;


Keyboard bestKeyboard = new Keyboard();

Scanner scanner = new Scanner("E:\\dev\\EvoType\\input.txt");

double bestPenalty = 1e9;

for (int i = 0; i < 10000; i++)
{
	Keyboard keyboard = new Keyboard();
	keyboard.RandomizeKeySet();

	double penalty = keyboard.EvaluatePenalty(scanner.GetKeys());

	if (penalty < bestPenalty)
	{
		bestPenalty = penalty;
		bestKeyboard = keyboard;
	}
}

Console.WriteLine($"{bestPenalty}");
Console.WriteLine(bestKeyboard); 

