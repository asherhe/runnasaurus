using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace runnasaurus
{
  internal class Keyboard
  {
    public static Dictionary<ConsoleKey, bool> keyState { get => _keyState; }
    private static Dictionary<ConsoleKey, bool> _keyState;

    static Keyboard()
    {
      _keyState = new Dictionary<ConsoleKey, bool>();

      Thread keyListener = new Thread(new ThreadStart(_waitKey));
      keyListener.Start();
    }

    private static void _waitKey()
    {
      while (true)
      {
        while (!Console.KeyAvailable)
          System.Threading.Thread.Sleep(5);
        _keyState[Console.ReadKey(true).Key] = true;
      }
    }

    /**
     * Gets whether a key has been pressed since the last call to getKey();
     */
    public static bool GetKey(ConsoleKey key)
    {
      if (_keyState.ContainsKey(key) && _keyState[key])
      {
        _keyState[key] = false; // reset state
        return true;
      }
      return false;
    }
  }
}
