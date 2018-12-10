using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterSelectionScreen.CharacterReceiver
{
    public class CharacterRequestAlias : MonoBehaviour
    {
        private CharacterRequestAlias() { }
        public const string CHARACTERS = "account/characters";
        public const string CREATE_CHARACTER = "account/createCharacter";
        public const string SELECT_CHARACTER = "account/selectCharacter";
    }
}