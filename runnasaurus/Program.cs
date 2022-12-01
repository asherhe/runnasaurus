using Pastel;
using System.Runtime.Serialization.Json;
using static System.Formats.Asn1.AsnWriter;

namespace runnasaurus
{
  internal class Program
  {
    static int dinoPos,
               dinoVel,
               dinoAcc;
    static float tickSpeed, score;

    static Scroll groundTiles, cacti;

    static string frameColor = "#5b606e",
                  dinoColor = "#767372",
                  cactusColor = "#65e22f",
                  groundColor = "#edc370",
                  textColor = "#aea9a0";

    static void render()
    {
      Console.Clear();
      Console.SetCursorPosition(0, 0);
      Console.Write("+--------------------------------+".Pastel(frameColor));
      for (int i = 1; i < 8; i++)
      {
        Console.SetCursorPosition(0, i);
        Console.Write("|".Pastel(frameColor));
        Console.SetCursorPosition(33, i);
        Console.Write("|".Pastel(frameColor));
      }
      Console.SetCursorPosition(0, 8);
      Console.Write("+--------------------------------+".Pastel(frameColor));
      Console.SetCursorPosition(0, 9);
      Console.Write("W, UP, or SPACE to jump".Pastel(textColor));

      if (dinoPos / 3 < 6)
      {
        Console.SetCursorPosition(2, 6 - (dinoPos / 3));
        Console.Write("D".Pastel(dinoColor));
      }

      for (int i = 0; i < 32; ++i)
      {
        Console.SetCursorPosition(i + 1, 6);
        if (cacti.get(31 - i)) Console.Write("C".Pastel(cactusColor));

        Console.SetCursorPosition(i + 1, 7);
        Console.Write((groundTiles.get(31 - i) ? "-" : "=").Pastel(groundColor));
      }

      int digits = 1;
      for (int i = (int)score; i >= 10; i /= 10, digits++)
        ;
      Console.SetCursorPosition(32 - digits, 1);
      Console.Write(((int)score).ToString().Pastel((score >= 100 && score % 100 < 2) ? "#f3a303" : textColor));
    }

    static void Main(string[] args)
    {
      Dictionary<string, Keyboard.KeyCode[]> keyBindings = new Dictionary<string, Keyboard.KeyCode[]>();
      keyBindings["jump"] = new Keyboard.KeyCode[] { Keyboard.KeyCode.SPACE, Keyboard.KeyCode.W, Keyboard.KeyCode.UP };

      InputManager inputManager = new InputManager(keyBindings);

      Random random = new Random();

      Console.CursorVisible = false;

      // row and column lengths are not the same
      int frameColDrawDelay = 5,
          frameRowDrawDelay = 25;
      Console.Clear();
      Console.SetCursorPosition(0, 0);
      Console.Write("+".Pastel(frameColor));
      for (int i = 1; i < 33; i++)
      {
        System.Threading.Thread.Sleep(frameColDrawDelay);
        Console.Write("-".Pastel(frameColor));
      }
      System.Threading.Thread.Sleep(frameColDrawDelay);
      Console.Write("+".Pastel(frameColor));
      for (int i = 1; i < 8; i++)
      {
        System.Threading.Thread.Sleep(frameRowDrawDelay);
        Console.SetCursorPosition(33, i);
        Console.Write("|".Pastel(frameColor));
      }
      Console.SetCursorPosition(33, 8);
      Console.Write("+".Pastel(frameColor));
      for (int i = 32; i > 0; i--)
      {
        System.Threading.Thread.Sleep(frameColDrawDelay);
        Console.SetCursorPosition(i, 8);
        Console.Write("-".Pastel(frameColor));
      }
      Console.SetCursorPosition(0, 8);
      System.Threading.Thread.Sleep(frameColDrawDelay);
      Console.Write("+".Pastel(frameColor));
      for (int i = 7; i > 0; i--)
      {
        System.Threading.Thread.Sleep(frameRowDrawDelay);
        Console.SetCursorPosition(0, i);
        Console.Write("|".Pastel(frameColor));
      }
      while (true)
      {

        dinoPos = 0;
        dinoVel = 0;
        dinoAcc = -2;
        score = 0;
        tickSpeed = 50;

        groundTiles = new Scroll();
        for (int i = 0; i < 32; i++)
          groundTiles.set(i, random.Next(2));
        cacti = new Scroll();
        int nextCactus = random.Next(1, 4);

        while (true)
        {
          // get input
          bool jump = inputManager.getInput("jump");
          if (jump && dinoPos == 0) dinoVel = 4;

          // render phase
          render();

          // update phase
          dinoPos += dinoVel;
          dinoVel += dinoAcc;
          if (dinoPos < 0) dinoPos = 0;
          groundTiles.advance();
          groundTiles.set(0, random.Next(2));
          cacti.advance();
          if (dinoPos == 0 && cacti.get(30)) break;
          if (--nextCactus == 0)
          {
            cacti.set(0, 1);
            nextCactus = random.Next(8, 16);
          }

          score += tickSpeed / 100; // 10 points per second
          tickSpeed = 10000 / (score + 125) + 20;
          
          // wait
          System.Threading.Thread.Sleep((int)Math.Round(tickSpeed));
        }

        render();
        Console.SetCursorPosition(11, 1);
        Console.Write("game over :(".Pastel("#d436eb"));
        System.Threading.Thread.Sleep(1000);
        Console.SetCursorPosition(12, 2);
        Console.Write("press jump".Pastel("#3667eb"));
        while (!inputManager.getInput("jump"))
          ; // wait for key
      }
    }
  }
}