using EvoType;
using EvoType.src;
using System;



Scanner scanner = new Scanner("E:\\dev\\EvoType\\input.txt");

int size = 50;

Population population = new Population(size, scanner.GetKeys());
population.Randomize();

for (int i = 0; i < size; i++)
{
    population.Evaluate();

    population.SelectNextGen();

    Random random = new Random();
    for (int j = 0; j < (int)(Population.crossOverRate * size); j++)
    {
        int ind1 = random.Next(population.Size);
        int ind2 = random.Next(population.Size);
        (population.Individuals[ind2].Keyboard, population.Individuals[ind1].Keyboard) = Population.Crossover(
         population.Individuals[ind1].Keyboard, population.Individuals[ind2].Keyboard);
    }

    for (int j = 0; j < size; j++)
    {
        population.Individuals[i].Keyboard.Mutate();
    }
}


population.Individuals.OrderBy(x => x.Fitness);

Console.WriteLine($"{population.Individuals[size - 1].Keyboard} \n {population.Individuals[size - 1].Penalty}");
