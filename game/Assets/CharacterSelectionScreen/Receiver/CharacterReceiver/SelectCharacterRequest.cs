using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CharacterSelectionScreen.CharacterReceiver
{
    public class SelectCharacterRequest : IRequest
    {
        public string name;
    }
}