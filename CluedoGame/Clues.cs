using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CluedoGame    //Cluedo game by Alexander Keidel 22397868 last edited 21/12/2014
{
    class Clues
    {
        //read-only String arrays which contain all the possible clues in the three different categories
        public readonly String[] Rooms = { "Foyer", "Lecture Theatre", "Research Lab", "Laptop Lab", "Network Lab", "School Office", "CE 011" };
        ArrayList remainingRooms = new ArrayList(); //arraylist to hold values of already dealt clue cards

        public readonly String[] Weapons = { "Pen Drive", "Power Cable", "Keyboard", "Stapler", "Soldering Iron", "Scissors", "Powerpoint Slides" };
        ArrayList remainingWeapons = new ArrayList(); //arraylist to hold values of already dealt clue cards

        public readonly String[] Suspects = { "Mark", "Collette", "Chris", "Dan", "Darryl", "Sally", "Peter" };
        ArrayList remainingSuspects = new ArrayList(); //arraylist to hold values of already dealt clue cards

        String MurderRoom;
        String MurderWeapon;
        String Murderer;

        public readonly int ROOMTYPE = 1;
        public readonly int WEAPONTYPE = 2;
        public readonly int SUSPECTTYPE = 3;

        public Clues()
        {
        }

        public void setMurderScene() // setting the murder room, weapon and suspect
        {
            //populating the arraylists of remaining rooms, weapons and suspects
            for (int i = 0; i < Rooms.Length; i++)
            {
                remainingRooms.Add(Rooms[i]);
            }

            for (int i = 0; i < Weapons.Length; i++)
            {
                remainingWeapons.Add(Weapons[i]);
            }

            for (int i = 0; i < Suspects.Length; i++)
            {
                remainingSuspects.Add(Suspects[i]);
            }

            //rolling numbers between 0 and 6 and setting our murder room, weapon and suspect
            int temp;

            temp = selectInt(Rooms.Length); //rolling new number between 0 and the amount of rooms
            MurderRoom = Rooms[temp]; //setting MurderRoom to the name containted at the rolled position in the array
            remainingRooms.Remove(Rooms[temp]); //removing the selected room from the remaining room clues

            //rerolling new number and setting the murder weapon, as well as removing it from the remaining weapon clue pool
            temp = selectInt(Weapons.Length);
            MurderWeapon = Weapons[temp];
            remainingWeapons.Remove(Weapons[temp]);

            //same as above, but for suspects
            temp = selectInt(Suspects.Length);
            Murderer = Suspects[temp];
            remainingSuspects.Remove(Suspects[temp]);
            Console.WriteLine("The murder scene has been set:");
            Console.WriteLine("It was " + Murderer + " with " + MurderWeapon + " in " + MurderRoom);
            Console.WriteLine("The players do not know this answer.");
            Console.ReadLine();
        }

        public List<ArrayList> giveOutRestOfClues(int amountOfPlayers) //give each player clues for rooms, weapons and suspects and add them to the respective arraylists containing given out clues until all clues have been given out
        {
            List<ArrayList> cardPiles = new List<ArrayList>(); //creating a list of arrays; each will hold an array of Strings containing all the clues for a single player
            for (int i = 0; i < amountOfPlayers; i++)
            {
                cardPiles.Add(new ArrayList());
            }

            int currentPlayer = 0;
            String temp;

            while (true) //dealing out ROOMTYPE cards
            {
                if (currentPlayer >= amountOfPlayers) //going back to the first player
                {
                    currentPlayer = 0;
                }

                temp = selectNewClue(ROOMTYPE);
                if (temp.Equals("EMPTY"))
                {
                    break; // all room clues have been dealt
                }

                cardPiles[currentPlayer].Add(temp); //adding randomly selected clue to this players arraylist of Strings
                currentPlayer++;
            }

            while (true) //dealing out WEAPONTYPE cards
            {
                if (currentPlayer >= amountOfPlayers) //going back to the first player
                {
                    currentPlayer = 0;
                }

                temp = selectNewClue(WEAPONTYPE);
                if (temp.Equals("EMPTY"))
                {
                    break; // all weapon clues have been dealt
                }

                cardPiles[currentPlayer].Add(temp); //adding randomly selected clue to this players arraylist of Strings
                currentPlayer++;
            }

            while (true) //dealing out SUSPECTTYPE cards
            {
                if (currentPlayer >= amountOfPlayers) //going back to the first player
                {
                    currentPlayer = 0;
                }

                temp = selectNewClue(SUSPECTTYPE);
                if (temp.Equals("EMPTY"))
                {
                    break; // all suspect clues have been dealt
                }

                cardPiles[currentPlayer].Add(temp); //adding randomly selected clue to this players arraylist of Strings
                currentPlayer++;
            }
            return cardPiles;
        }

        public String selectNewClue(int type) //selecting a clue that has not yet been given out from a specific type: 1 for room, 2 for weapon and 3 for suspect and REMOVING it from the remaining clues list
        {
            Random rd = new Random();
            String newClue;
            switch (type)
            {
                case 1: //room type
                    if (remainingRooms.Count == 0) //checking for empty array list
                    {
                        return "EMPTY";
                    }
                    newClue = remainingRooms[rd.Next(0, (remainingRooms.Count))].ToString(); //rolling a new number from 0 to the amount of remaining rooms (exclusive, hence + 1) and setting it for our new clue
                    remainingRooms.Remove(newClue); //removing selected clue from our list
                    return newClue;

                case 2: //weapon type
                    if (remainingWeapons.Count == 0) //checking for empty array list
                    {
                        return "EMPTY";
                    }
                    newClue = remainingWeapons[rd.Next(0, (remainingWeapons.Count))].ToString(); //rolling a new number from 0 to the amount of remaining rooms (exclusive, hence + 1) and setting it for our new clue
                    remainingWeapons.Remove(newClue); //removing selected clue from our list
                    return newClue;

                case 3: //suspect type
                    if (remainingSuspects.Count == 0) //checking for empty array list
                    {
                        return "EMPTY";
                    }
                    newClue = remainingSuspects[rd.Next(0, (remainingSuspects.Count))].ToString(); //rolling a new number from 0 to the amount of remaining rooms (exclusive, hence + 1) and setting it for our new clue
                    remainingSuspects.Remove(newClue); //removing selected clue from our list
                    return newClue;
            }
            return "ERROR"; //this only happens when the type is false
        }

        public Boolean checkIfAvailable(int type, String name) //this method will return true if the name (clue card) has already been handed out
        // type = 1 is room, 2 is weapon and 3 is suspect
        {
            if (type > 3 || type < 1) //making sure you pass it the right type
            {
                throw new IndexOutOfRangeException("Wrong type selected, please select from 1, 2 or 3. Refer to Clues.checkIfTaken() method for further instructions.");
            }

            switch (type)
            {
                case 1: //checking if this room is already a previously dealt clue card
                    if (remainingRooms.Contains(name))
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }

                case 2: //checking if this weapon is already a previously dealt clue card
                    if (remainingWeapons.Contains(name))
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }

                case 3: //checking if this victim is already a previously dealt clue card
                    if (remainingSuspects.Contains(name))
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
            }
            Console.WriteLine("Something went wrong in the Clues.checkIfTaken method!");
            return true;
        }

        private int selectInt(int bound) //rolling a random number between 0 and given bound (inclusive, since it will add 1 in this method)
        {
            Random rd = new Random();
            return rd.Next(0, (bound));
        }

        public String[] getAllRooms() //getter for all rooms
        {
            return Rooms;
        }

        public String[] getAllWeapons() //getter for all weapons
        {
            return Weapons;
        }

        public String[] getAllSuspects() //getter for all suspects
        {
            return Suspects;
        }

        public String getMurderRoom() //getter for murder room
        {
            return MurderRoom;
        }

        public String getMurderWeapon() //getter for murder weapon
        {
            return MurderWeapon;
        }

        public String getMurderer() //getter for murder victim
        {
            return Murderer;
        }

        public String getCorrectAnswer() //getter for the correct answer, used to evaluate whether a player has won
        {
            return MurderRoom + " " + MurderWeapon + " " + Murderer;
        }
    }
}
