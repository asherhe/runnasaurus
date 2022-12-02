using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace runnasaurus
{
  internal class InputManager
  {
    public Dictionary<String, ConsoleKey[]> keyBindings;
    public InputManager()
    {
      keyBindings = new Dictionary<String, ConsoleKey[]>();
    }

    public InputManager(Dictionary<String, ConsoleKey[]> keyBindings)
    {
      this.keyBindings = keyBindings;
    }

    public bool getInput(String key)
    {
      foreach (ConsoleKey k in keyBindings[key])
        if (Keyboard.GetKey(k)) return true;
      return false;
    }
  }
}
