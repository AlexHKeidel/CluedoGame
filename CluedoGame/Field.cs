using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CluedoGame    //Cluedo game by Alexander Keidel 22397868 last edited 21/12/2014
{
    class Field //field class holding all relevant information to a field
    {
        String name = null;
        public Field SpecialRoom = null;
        public Field PreviousRoom = null;

        public Field()//empty constructor, the following constructers overload this one
        {
        }

        public Field(String name)//constructor with passed name
        {
            this.name = name;
        }

        public void setSpecialRoom(Field SpecialRoom) //setter for the special room (possible murder room)
        {
            this.SpecialRoom = SpecialRoom;
        }

        public Field getSpecialRoom() //getter for special room
        {
            return SpecialRoom;
        }

        public void setPreviousRoom(Field PreviousRoom) //setter for previous room, used only for special rooms to link back to the room the player has come from
        {
            this.PreviousRoom = PreviousRoom;
        }

        public Field getPreviousRoom() //getter for the room linking back to where you have come from
        {
            return PreviousRoom;
        }

        public void setName(String name) //setter for name
        {
            this.name = name;
        }

        public String getName() //getter for name
        {
            return name;
        }
    }

}
