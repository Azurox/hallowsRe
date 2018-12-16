using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WorldScreen.MapReceiver
{
    public class MapReceiverAlias
    {
        private MapReceiverAlias() { }
        public const string LOAD_MAP = "map/loadMap";
        public const string ILLEGAL_MOVEMENT = "map/illegalMovement";
        public const string LEGAL_MOVEMENT = "map/legalMovement";
    }
}
