using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CluedoGame //Cluedo game by Alexander Keidel 22397868 last edited 21/12/2014
{
    class Die //die class used to generate random values for the game
    {
        int result;

        public Die() // default constructor
        {
        }

        public int rollDie(int upperBound)//rolls a number between 1 (inclusive) and a given upper bound (exclusive)
        {
            Random random = new Random();
            result = random.Next(1, upperBound);
            return result;
        }

        public int rollD6() //rolling a d6 die: random number between 1 and 6
        {
            return rollDie(7);
        }

        public int rollD12() //rolling a d12 die: random number between 1 and 12
        {
            return rollDie(13);
        }

        public int getResult() //get current result without rerolling it
        {
            return result;
        }
    }
}
