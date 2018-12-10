using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WorldScreen.MapReceiver
{
    public class MapRequestAlias
    {
        private MapRequestAlias() { }
        public const string LOAD_MAP = "map/loadMap";
        public const string ACCOUNT_CREATED = "account/accountCreated";
    }
}