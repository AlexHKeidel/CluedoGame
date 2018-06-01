using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CluedoGame    //Cluedo game by Alexander Keidel 22397868 last edited 21/12/2014
{
    class Program   //main class that will run the program
    {
        static void Main(string[] args)
        {
            GameEngine engine = new GameEngine(); //constructing the games engine, which will in turn construct everything else that is needed to run the game  
            engine.runGame(); //main loop that will run the game
        }
    }
}
