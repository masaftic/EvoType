using EvoType;
using EvoType.src;
using System;


Keyboard Keyboard = new Keyboard();

Scanner scanner = new Scanner("E:\\dev\\EvoType\\input.txt");

System.Console.WriteLine(Keyboard.EvaluatePenalty(scanner.GetKeys()));
System.Console.WriteLine(Keyboard);

System.Console.WriteLine();
System.Console.WriteLine(string.Join(" ", Keyboard.allKeys));

Population population = new Population(2, scanner.GetKeys());

Array.Reverse(population.Keyboards[1].KeySet);


(Keyboard k1, Keyboard k2) = Population.Crossover(population.Keyboards[0], population.Keyboards[1]);


System.Console.WriteLine();
System.Console.WriteLine(string.Join(" ", k1.KeySet));
System.Console.WriteLine();
System.Console.WriteLine(string.Join(" ", k2.KeySet));
