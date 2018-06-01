using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CluedoGame //Cluedo game by Alexander Keidel 22397868 last edited 21/12/2014
{
    class Player //player class that contains all the information a single player needs to hold
    {
        String name;
        int position;
        public Field currentRoom;
        String definiteRoom = null;
        String definiteWeapon = null;
        String definiteSuspect = null;
        protected ArrayList roomClues = new ArrayList(); //rooms that are known to the player for not being the correct answer
        protected ArrayList weaponClues = new ArrayList(); //weapons that are known to the player for not being the correct answer
        protected ArrayList suspectClues = new ArrayList(); //suspects which are known not to be the murderer
        Clues clues = new Clues();

        public Player() //empty constructor, next ones will overload this method
        {

        }

        public Player(String name)//this should be used as the default constructor
        {
            this.name = name; //setting name
            position = 0; // setting default position (first field on the array)
        }

        public String getName() //getter function for the player name
        {
            return name;
        }

        public void setName(String name) //setter function for player name
        {
            this.name = name;
        }

        public int getPosition() //getter function for player position on the game field
        {
            return position;
        }

        public void setPosition(int position) //getter function for player position
        {
            this.position = position;
        }

        public void moveTo(int position) //similar to set position
        {
            setPosition(position);
        }

        public void takeStep() //taking a single step
        {
            if (position + 1 >= 24) //making sure not to go outside the boundaries of the gamefield
            {
                setPosition(0);
            }
            setPosition(position + 1);
        }

        public void moveToField(Field field)
        {
            this.currentRoom = field;
        }

        public void goToSpecialRoom(Field specialRoom) //move to the current fields special room
        {
            if (currentRoom.SpecialRoom != null)
            {
                this.currentRoom = specialRoom;
            }

        }

        public void goToPreviousRoom(Field previousRoom) //move to the current fields previous room
        {
            if (currentRoom.PreviousRoom != null)
            {

            }
        }

        public String getRandomWeapon() //returns a remaining random weapon clue
        {
            ArrayList tempAL = new ArrayList();
            foreach (String s in clues.getAllWeapons()) //creating a new array list containing all the possible murder weapons
            {
                tempAL.Add(s);
            }
            foreach (String s in weaponClues) //removing all known answers from the arraylist
            {
                tempAL.Remove(s);
            }
            //tempAL now contains only potential answers that we are unsure about. We will randomly select one and return it.
            Random random = new Random();
            return tempAL[random.Next(0, tempAL.Count)].ToString(); //returning the string value (clue) contained at a random location between 0 and the length of the arraylist
        }

        public String getRandomSuspect() //returns a random remaining suspect clue
        {
            ArrayList tempAL = new ArrayList();
            foreach (String s in clues.getAllSuspects()) //creating a new array list containing all the possible murderers
            {
                tempAL.Add(s);
            }
            foreach (String s in suspectClues) //removing all known answers from the arraylist
            {
                tempAL.Remove(s);
            }
            //tempAL now contains only potential answers that we are unsure about. We will randomly select one and return it.
            Random random = new Random();
            return tempAL[random.Next(0, tempAL.Count)].ToString(); //returning the string value (clue) contained at a random location between 0 and the length of the arraylist
        }

        public String[] makeSuggestion(String currentRoomName)//making a guess based upon remaining rooms (given by the room you are in), weapons and suspects
        {
            String[] suggestion = new String[3]; //array of strings with the size of 3, adding the room name, a random known weapon and random known suspect to it and returning it
            suggestion[0] = currentRoomName;
            suggestion[1] = getRandomWeapon();
            suggestion[2] = getRandomSuspect();
            return suggestion;
        }

        public String disproveSuggestion(String[] suggestion) //disproving a suggestion by giving back one of the known to be false answers, if possible
        {
            Random random = new Random();
            if (!roomClues.Contains(suggestion[0]) && !weaponClues.Contains(suggestion[1]) && !suspectClues.Contains(suggestion[2])) //suggestion can not be disproven
            {
                return "I can not disprove this suggestion.";
            }
            if (roomClues.Contains(suggestion[0]) && weaponClues.Contains(suggestion[1]) && suspectClues.Contains(suggestion[2]))
            {
                //if you can disprove any part of the suggestion
                switch (random.Next(0, 3)) //randomly choosing which 
                {
                    case 0:
                        return suggestion[0];
                    case 1:
                        return suggestion[1];
                    case 2:
                        return suggestion[2];
                }
            }

            if (roomClues.Contains(suggestion[0]) && weaponClues.Contains(suggestion[1])) //the room and weapon can be disproven, randomly choose one of them
            {
                switch (random.Next(0, 2))
                {
                    case 0:
                        return suggestion[0];
                    case 1:
                        return suggestion[1];
                }
            }

            if (roomClues.Contains(suggestion[0]) && suspectClues.Contains(suggestion[2])) //the room and suspect can be disproven, randomly choose one of them
            {
                switch (random.Next(0, 2))
                {
                    case 0:
                        return suggestion[0];
                    case 1:
                        return suggestion[2];
                }
            }

            if (weaponClues.Contains(suggestion[1]) && suspectClues.Contains(suggestion[2])) //the weapon and suspect can be disproven, choose one at random
            {
                switch (random.Next(0, 2))
                {
                    case 0:
                        return suggestion[1];
                    case 1:
                        return suggestion[2];
                }
            }
            if (!roomClues.Contains(suggestion[0]) && !weaponClues.Contains(suggestion[1])) //if you can disprove the suspect
            {
                return suggestion[2]; //position 2 is the suspect (see makeSuggestion())
            }

            if (!roomClues.Contains(suggestion[0]) && !suspectClues.Contains(suggestion[2])) //if you can disprove the weapon
            {
                return suggestion[1];
            }

            if (!weaponClues.Contains(suggestion[1]) && !suspectClues.Contains(suggestion[2])) //if you can disprove the room
            {
                return suggestion[0];
            }

            return "For some reason I can not disprove this suggestion.";
        }

        public void updateClues(int type, String clue) //this method adds a clue to the selected arraylist (via the type)
        //type = 1 is room, 2 is weapon, 3 is suspect, same logic as in Clues.cs
        {
            if (type > 3 || type < 1)
            {
                throw new IndexOutOfRangeException("Wrong type selected. Type needs to be 1, 2 or 3. Refer to Player.updateClues for further instructions.");
            }
            switch (type)
            {
                case 1:
                    if (roomClues.Contains(clue) || clue.Equals(" ")) //making sure the clue is not already on the list
                    {
                        Console.WriteLine("I already had that clue.");
                        return;
                        //throw new InvalidOperationException("This clue is already in the list.");
                    }
                    roomClues.Add(clue);
                    Console.WriteLine(getName() + "s list of clues has been updated with: " + clue);
                    return;

                case 2:
                    if (weaponClues.Contains(clue) || clue.Equals(" ")) //making sure the clue is not already on the list
                    {
                        Console.WriteLine("I already had that clue.");
                        return;
                        //throw new InvalidOperationException("This clue is already in the list.");
                    }
                    weaponClues.Add(clue);
                    Console.WriteLine(getName() + "s list of clues has been updated with: " + clue);
                    return;

                case 3:
                    if (suspectClues.Contains(clue) || clue.Equals(" ")) //making sure the clue is not already on the list
                    {
                        Console.WriteLine("I already had that clue.");
                        return;
                        //throw new InvalidOperationException("This clue is already in the list.");
                    }
                    suspectClues.Add(clue);
                    Console.WriteLine(getName() + "s list of clues has been updated with: " + clue);
                    return;
            }
        }

        public int determineClueType(String clue)
        {
            if (clues.Rooms.Contains(clue))
            {
                return clues.ROOMTYPE;
            }

            if (clues.Weapons.Contains(clue))
            {
                return clues.WEAPONTYPE;
            }

            if (clues.Suspects.Contains(clue))
            {
                return clues.SUSPECTTYPE;
            }

            throw new InvalidOperationException("This String is not a clue.");
        }

        public bool checkForDefinitiveClues()
        {
            //checking if there has been a definite clue by comparing the list of possible rooms, weapons and suspects with the amount of clues in each category
            if (roomClues.Count == (clues.Rooms.Length - 1) && definiteRoom == null) //if we have all but one clue, we know it must be the answer
            {
                ArrayList tempAL = new ArrayList(); //creating new arraylist containing all possible answers from the clues
                foreach (String s in clues.Rooms)
                {
                    tempAL.Add(s);
                }
                foreach (String s in roomClues) //removing all the collected clues from our arraylist that contains all answers, leaving us with exactly one answer: the definitive room
                {
                    tempAL.Remove(s);
                }
                definiteRoom = tempAL[0].ToString(); //setting our definite room
                Console.WriteLine("Definitive Room clue: " + definiteRoom);
                Console.ReadLine();
            }

            if (weaponClues.Count == (clues.Weapons.Length - 1) && definiteWeapon == null) //if we have all but one clue, we know it must be the answer
            {
                ArrayList tempAL = new ArrayList(); //creating new arraylist containing all possible answers from the clues
                foreach (String s in clues.Weapons)
                {
                    tempAL.Add(s);
                }
                foreach (String s in weaponClues) //removing all the collected clues from our arraylist that contains all answers, leaving us with exactly one answer: the definitive weapon
                {
                    tempAL.Remove(s);
                }
                definiteWeapon = tempAL[0].ToString(); //setting our definite weapon
                Console.WriteLine("Definitive Weapon clue: " + definiteWeapon);
                Console.ReadLine();
            }

            if (suspectClues.Count == (clues.Suspects.Length - 1) && definiteSuspect == null) //if we have all but one clue, we know it must be the answer
            {
                ArrayList tempAL = new ArrayList(); //creating new arraylist containing all possible answers from the clues
                foreach (String s in clues.Suspects)
                {
                    tempAL.Add(s);
                }
                foreach (String s in suspectClues) //removing all the collected clues from our arraylist that contains all answers, leaving us with exactly one answer: the definitive suspect
                {
                    tempAL.Remove(s);
                }
                definiteSuspect = tempAL[0].ToString(); //setting our definite suspect
                Console.WriteLine("Definitive Suspect: " + definiteSuspect);
                Console.ReadLine();
            }

            if (definiteRoom != null && definiteWeapon != null && definiteSuspect != null) //we have found our answer
            {
                return true; //we have found the answer, returning true
            }
            else
            {
                return false; //returning false as we have not yet found the answer.
            }
        }

        public String makeDefinitiveGuess()
        {
            //returning a String with our definite guess
            if (definiteRoom != null && definiteWeapon != null && definiteSuspect != null)
            {
                Console.WriteLine("My definitive guess is:");
                Console.WriteLine("It was " + definiteSuspect + " with " + definiteWeapon + " in " + definiteRoom + ".");
                return definiteRoom + " " + definiteWeapon + " " + definiteSuspect;
            }
            else
            {
                return "I can not make a definitive guess.";
            }
        }

        public String makeDefinitiveGuess(String[] guess)
        {
            return guess[0] + " " + guess[1] + " " + guess[2];
        }
    }
}
