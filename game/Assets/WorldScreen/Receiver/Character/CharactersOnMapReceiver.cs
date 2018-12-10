using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WorldScreen.CharacterReceiver
{
    public class CharactersOnMapReceiver : IReceiver
    {
        public HashSet<Character> characters;
    }
}