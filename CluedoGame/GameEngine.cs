using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CluedoGame //Cluedo game by Alexander Keidel 22397868 last edited 21/12/2014
{
    class GameEngine //this class runs the main parts of logic behind the rule set of cluedo. It constructs all the essential parts like the clues, players, dice and the game board.
    {
        GameBoard gameBoard = new GameBoard();
        Clues clues = new Clues();
        Player[] player;
        String[] suggestion;
        List<ArrayList> cluePiles;
        static int currentPlayer;
        static int playerAmount;
        Die die = new Die();
        bool gameOver;

        public GameEngine() { } //empty constructor

        private void setPlayerAmount(int numberOfPlayers) //setting and constructing players
        {
            player = new Player[numberOfPlayers];
            for (int i = 0; i < numberOfPlayers; i++)
            {
                player[i] = new Player();
                player[i].setName("Player " + (i + 1)); //player names start at 1 instead of 0
                player[i].setPosition(0);
                player[i].moveToField(gameBoard.gameFields[0]); //moving to the first field
            }
        }

        private int askForPlayerAmount() //this method asks for user input and will return a value between 3 and 6.
        {
            Console.WriteLine("Please select how many players are playing.");
            while (true) //It will loop and ask the user again if they do not choose a correct value.
            {
                Console.WriteLine("Choose from 3 to 6 players.");
                String players = Console.ReadLine(); //reading from console
                if (players.Equals("3") || players.Equals("4") || players.Equals("5") || players.Equals("6"))
                {
                    return playerAmount = Int32.Parse(players); //returning an integer value parsed from a string
                }
                else
                {
                    continue; //the user gave a wrong input - start loop again
                }
            }
        }

        public void declareWinner(Player winner) //declared the winner.
        {
            Console.WriteLine("After checking the results it is clear that we have a winner:");
            Console.WriteLine(winner.getName() + " is our winner! Congratulations!");
            Console.ReadLine();
            Environment.Exit(0); //exiting program
        }

        public void runGame() //main method that will run the game
        {

            gameOver = false; //initalising that the game is not over
            currentPlayer = 0; //the first move will be done by the first player (index position 0)

            setPlayerAmount(askForPlayerAmount()); //asking and setting for player amount
            gameBoard.setUp(); //setting up game board
            clues.setMurderScene(); //setting murder scene (selecting a room, weapon and suspect which are required to win the game)
            cluePiles = new List<ArrayList>(); //creating an arraylist containing all the card piles for the players
            cluePiles = clues.giveOutRestOfClues(playerAmount);

            for (int i = 0; i < cluePiles.Count(); i++) //for each pile (based on player amount)
            {
                for (int g = 0; g < cluePiles[i].Count; g++) //for each clue within a pile: determine the clue type and add it to that players list of clues
                {
                    player[i].updateClues(player[i].determineClueType(cluePiles[i][g].ToString()), cluePiles[i][g].ToString());
                }
            }

            while (!gameOver)//main while loop that runs as long as the game is not over
            {
                if (currentPlayer >= playerAmount) //going back to the first player after the "last" player had their move e.g out of 6 players, player 6 just had their move, player 7 does not exist so we go back to the first player
                {
                    currentPlayer = 0;
                }
                Console.WriteLine("It is " + player[currentPlayer].getName() + "'s go.");
                if (player[currentPlayer].checkForDefinitiveClues()) //the player is sure he has got the right answer
                {
                    if (player[currentPlayer].makeDefinitiveGuess().Equals(clues.getCorrectAnswer())) //the answer is correct!
                    {
                        declareWinner(player[currentPlayer]);
                    }
                }
                die.rollD6();   //rolling a d6 die (1 to 6)
                Console.ReadLine(); //waiting for user input
                Console.WriteLine("The die rolled a " + die.getResult());
                for (int i = 0; i < die.getResult(); i++) //moving the player step by step checking for special rooms
                {
                    player[currentPlayer].takeStep(); //moving the player for one field
                    player[currentPlayer].moveToField(gameBoard.gameFields[player[currentPlayer].getPosition()]); //setting the current field of the player
                    Console.WriteLine("You are now on " + player[currentPlayer].currentRoom.getName());
                    if (gameBoard.gameFields[player[currentPlayer].getPosition()].SpecialRoom != null && (die.getResult() - i) >= 1) //checking if the game field the player is currently on has a special room and the player is allowed to take another step
                    {
                        Console.WriteLine(player[currentPlayer].getName() + " may enter the " + gameBoard.gameFields[player[currentPlayer].getPosition()].SpecialRoom.getName());
                        Console.WriteLine("Do you want to enter?");
                        String answer = "y"; //put Readline here if you actually want to type yes each time. There is no point to a player not going into a special room
                        Console.ReadLine();
                        if (answer.Contains("yes") || answer.Equals("y"))
                        {
                            player[currentPlayer].moveToField(gameBoard.gameFields[player[currentPlayer].getPosition()].SpecialRoom); //moving to said special room
                            i++; //incrementing i as we have taken a step.
                            suggestion = new String[3]; // new suggestion holder string array
                            suggestion = player[currentPlayer].makeSuggestion(player[currentPlayer].currentRoom.getName());
                            Console.WriteLine(player[currentPlayer].getName() + ", you may make a suggestion:");
                            Console.WriteLine("It was " + suggestion[2] + " in the " + suggestion[0] + " with " + suggestion[1] + ".");
                            int tempNextPlayer = currentPlayer;
                            while (true) //while we have not made a full circle to the player who's round it is: the player's suggestion will be disproven by the next player, if he can not the next player etc.
                            {
                                tempNextPlayer++;
                                if (tempNextPlayer >= playerAmount) //making sure we go to the first player after we have gone past the last
                                {
                                    tempNextPlayer = 0;
                                }

                                if (tempNextPlayer == currentPlayer) //we made a full round and noone could disprove us. We must have guessed right.
                                {
                                    if (player[currentPlayer].makeDefinitiveGuess(suggestion).Equals(clues.getCorrectAnswer())) //the answer is correct!
                                    {
                                        declareWinner(player[currentPlayer]);
                                    }
                                    break;
                                }

                                String disprovedSuggestion = player[tempNextPlayer].disproveSuggestion(suggestion);
                                if (disprovedSuggestion.Equals("I can not disprove this suggestion.")) //the next player can not disprove the suggestion, moving to next player
                                {
                                    Console.WriteLine(player[tempNextPlayer].getName() + " can not disprove this suggestion. Moving to next player.");
                                    continue;
                                }
                                else
                                {
                                    Console.WriteLine(player[tempNextPlayer].getName() + " disproved your answer.");
                                    Console.WriteLine("It was not: " + disprovedSuggestion);
                                    //the player will update their list of clues with the new one
                                    player[currentPlayer].updateClues(player[currentPlayer].determineClueType(disprovedSuggestion), disprovedSuggestion);
                                    player[currentPlayer].checkForDefinitiveClues();
                                    break; //break the loop since we have found our disproven answer and added it to our list
                                }
                            }
                            player[currentPlayer].moveToField(gameBoard.gameFields[player[currentPlayer].getPosition()].PreviousRoom); //moving back to the previous room
                        }
                    }
                }
                currentPlayer++; //incrementing at the end of while loop 
                Console.WriteLine(); //empty line for spacing
            }
        }
    }
}
