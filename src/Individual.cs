using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EvoType.src;

public class Individual
{
    public Keyboard Keyboard { get; set; }
    public double Penalty { get; set; }
    public double Fitness { get; set; }
    public double Probabilty { get; set; }

    public Individual()
    {
        Keyboard = new();
    }
}
