using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SnakeGame
{
    class Input
    {
        private static Hashtable keyTable = new Hashtable();
        //new instance of Hashtable and this class is used to optimize the keys inserted in it

        public static bool KeyPress(Keys key)
        {
            //this function will return a key back to the class
            if(keyTable[key]==null)
            {
                //if hash table is empty than we return false;
                return false; 
            }
            //if hashtable is not empty than we return true;
            return (bool)keyTable[key];
        }
        public static void changeState(Keys key,bool state)
        {
            //this function will change the state of the keys and the player with it
            keyTable[key] = state;
        }
    }
}
