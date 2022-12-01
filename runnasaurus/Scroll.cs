using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace runnasaurus
{
  internal class Scroll
  {
    UInt32 contents = 0; // 32-bit bit vector

    public Scroll()
    {

    }

    public bool get(int index) => ((contents >> index) & 1) == 1;
    public void set(int index, int val)
    {
      contents &= (UInt32)~(1 << index);
      contents |= (UInt32)val << index;
    }

    public void advance(int n) => contents >>= n;

    public void advance() => advance(1);
  }
}
