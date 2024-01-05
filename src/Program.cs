using EvoType;
using EvoType.src;
using System;


Keyboard Keyboard = new Keyboard();

Scanner scanner = new Scanner("E:\\dev\\EvoType\\input.txt");

System.Console.WriteLine(Keyboard.EvaluatePenalty(scanner.GetKeys()));
System.Console.WriteLine(Keyboard);

Population population = new Population(4, scanner.GetKeys());
population.RandomizePopulation();
population.Evaluate();