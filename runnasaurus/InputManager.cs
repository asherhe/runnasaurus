using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace runnasaurus
{
  internal class InputManager
  {
    public Dictionary<String, Keyboard.KeyCode[]> keyBindings;
    public InputManager()
    {
      keyBindings = new Dictionary<String, Keyboard.KeyCode[]>();
    }

    public InputManager(Dictionary<String, Keyboard.KeyCode[]> keyBindings)
    {
      this.keyBindings = keyBindings;
    }

    public bool getInput(String key)
    {
      foreach (Keyboard.KeyCode k in keyBindings[key])
        if (Keyboard.getKey(k)) return true;
      return false;
    }
  }
}
