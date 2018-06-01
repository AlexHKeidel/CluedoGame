using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CluedoGame    //Cluedo game by Alexander Keidel 22397868 last edited 21/12/2014
{
    class GameBoard //this class will contain an array of fields to build the gameboard
    {
        private static readonly int maximumFields = 24; //maximum value to check that we have the correct amount of fields (EXCLUDING special rooms, only the base game board), the integer is readonly so it may not be accidentially changed at runtime
        public Field[] gameFields = new Field[maximumFields]; //creating an array of Fields

        private static String[] SpecialRoomNames = { "Foyer", "Lecture Theatre", "Research Lab", "Laptop Lab", "Network Lab", "School Office", "CE 011" }; // array containing the different possible murder rooms
        Field[] specialRooms = new Field[SpecialRoomNames.Length]; //creating array of special rooms

        public GameBoard() //constructor
        {
        }

        public void setUp() //creating the full gameboard, including the 24 spaces and the 7 special rooms linked to them and back
        {
            Console.WriteLine("Constructing Game Board.");
            for (int i = 0; i < maximumFields; i++)
            {
                gameFields[i] = new Field("Field " + (i + 1)); // constructing fields within array
            }

            for (int i = 0; i < specialRooms.Length; i++)
            {
                specialRooms[i] = new Field(SpecialRoomNames[i]); // constructing fields within the array
            }

            //assigning special rooms to their respective fields and the link back as well
            gameFields[3].setSpecialRoom(specialRooms[0]); //field 4 with Foyer
            specialRooms[0].setPreviousRoom(gameFields[3]);

            gameFields[7].setSpecialRoom(specialRooms[1]); //field 8 with Lecture Theatre
            specialRooms[1].setPreviousRoom(gameFields[7]);

            gameFields[9].setSpecialRoom(specialRooms[2]); //field 10 with Research Lab
            specialRooms[2].setPreviousRoom(gameFields[9]);

            gameFields[15].setSpecialRoom(specialRooms[3]); //field 16 with Laptop Lab
            specialRooms[3].setPreviousRoom(gameFields[15]);

            gameFields[19].setSpecialRoom(specialRooms[4]); //field 20 with Network Lab
            specialRooms[4].setPreviousRoom(gameFields[19]);

            gameFields[21].setSpecialRoom(specialRooms[5]); //field 22 with School Office
            specialRooms[5].setPreviousRoom(gameFields[21]);

            gameFields[23].setSpecialRoom(specialRooms[6]); //field 24 with CE 011
            specialRooms[6].setPreviousRoom(gameFields[23]);
        }

    }
}
