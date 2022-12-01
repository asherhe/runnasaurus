using Pastel;
using System.Runtime.Serialization.Json;
using static System.Formats.Asn1.AsnWriter;

namespace runnasaurus
{
  internal class Program
  {
    static int drawX,
               drawY;

    static float dinoPos,
                 dinoVel,
                 dinoAcc,
                 tickSpeed,
                 score;

    static Scroll groundTiles, cacti, pterodactyls;

    static string frameColor = "#5b606e",
                  dinoColor = "#767372",
                  cactusColor = "#65e22f",
                  groundColor = "#edc370",
                  textColor = "#aea9a0";

    static void render()
    {
      Console.Clear();
      Console.SetCursorPosition(drawX, drawY);
      Console.Write("+--------------------------------+".Pastel(frameColor));
      for (int i = 1; i < 8; i++)
      {
        Console.SetCursorPosition(drawX, drawY + i);
        Console.Write("|".Pastel(frameColor));
        Console.SetCursorPosition(drawX + 33, drawY + i);
        Console.Write("|".Pastel(frameColor));
      }
      Console.SetCursorPosition(drawX, drawY + 8);
      Console.Write("+--------------------------------+".Pastel(frameColor));
      Console.SetCursorPosition(drawX, drawY + 9);
      Console.Write("W, UP, or SPACE to jump".Pastel(textColor));

      if (dinoPos / 3 < 6)
      {
        Console.SetCursorPosition(drawX + 2, drawY + (int)(6 - dinoPos / 3));
        Console.Write("D".Pastel(dinoColor));
      }

      for (int i = 0; i < 32; ++i)
      {
        if (pterodactyls.get(i))
        {
          int pos = (int)(1.5f * (i - 1));
          if (0 <= pos && pos < 32)
          {
            Console.SetCursorPosition(drawX + pos + 2, drawY + 4);
            Console.Write("<".Pastel(dinoColor));
          }
        }
        if (cacti.get(i))
        {
          Console.SetCursorPosition(drawX + i + 1, drawY + 6);
          Console.Write("C".Pastel(cactusColor));
        }

        Console.SetCursorPosition(drawX + i + 1, drawY + 7);
        Console.Write((groundTiles.get(i) ? "-" : "=").Pastel(groundColor));
      }

      int digits = 1;
      for (int i = (int)score; i >= 10; i /= 10, digits++)
        ;
      Console.SetCursorPosition(drawX + 32 - digits, drawY + 1);
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
      drawX = (Console.WindowWidth - 34) / 2;
      drawY = (Console.WindowHeight - 10) / 2;
      int frameColDrawDelay = 5,
          frameRowDrawDelay = 25;
      Console.Clear();
      Console.SetCursorPosition(drawX, drawY);
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
        Console.SetCursorPosition(drawX + 33, drawY + i);
        Console.Write("|".Pastel(frameColor));
      }
      Console.SetCursorPosition(drawX + 33, drawY + 8);
      Console.Write("+".Pastel(frameColor));
      for (int i = 32; i > 0; i--)
      {
        System.Threading.Thread.Sleep(frameColDrawDelay);
        Console.SetCursorPosition(drawX + i, drawY + 8);
        Console.Write("-".Pastel(frameColor));
      }
      Console.SetCursorPosition(drawX, drawY + 8);
      System.Threading.Thread.Sleep(frameColDrawDelay);
      Console.Write("+".Pastel(frameColor));
      for (int i = 7; i > 0; i--)
      {
        System.Threading.Thread.Sleep(frameRowDrawDelay);
        Console.SetCursorPosition(drawX, drawY + i);
        Console.Write("|".Pastel(frameColor));
      }
      while (true)
      {
        dinoPos = 0;
        dinoVel = 0;
        dinoAcc = -1.5f;
        score = 0;
        tickSpeed = 50;

        groundTiles = new Scroll();
        for (int i = 0; i < 32; i++)
          groundTiles.set(i, random.Next(2));
        cacti = new Scroll();
        pterodactyls = new Scroll();
        int nextObstacle = random.Next(1, 4);

        while (true)
        {
          // get input
          bool jump = inputManager.getInput("jump");
          if (jump && dinoPos == 0) dinoVel = 3;

          // render phase
          drawX = (Console.WindowWidth - 34) / 2;
          drawY = (Console.WindowHeight - 10) / 2;
          render();

          // update phase
          if ((dinoPos == 0 && cacti.get(1)) || (3 <= dinoPos && dinoPos <= 5 && pterodactyls.get(1))) break;
          dinoPos += dinoVel * tickSpeed / 100;
          dinoVel += dinoAcc * tickSpeed / 100;
          if (dinoPos < 0) dinoPos = 0;
          groundTiles.advance();
          groundTiles.set(31, random.Next(2));
          cacti.advance();
          pterodactyls.advance();
          if (--nextObstacle == 0)
          {
            if (score >= 300 && random.Next(4) == 0)
              pterodactyls.set(31, 1);
            else
            cacti.set(31, 1);
            nextObstacle = random.Next(12, 16);
          }

          score += tickSpeed / 100; // 10 points per second
          tickSpeed = 10000 / (score + 125) + 20;

          // wait
          System.Threading.Thread.Sleep((int)Math.Round(tickSpeed));
        }

        render();
        Console.SetCursorPosition(drawX + 11, drawY + 1);
        Console.Write("game over :(".Pastel("#d436eb"));
        System.Threading.Thread.Sleep(500);
        Console.SetCursorPosition(drawX + 12, drawY + 2);
        Console.Write("press jump".Pastel("#3667eb"));
        System.Threading.Thread.Sleep(500);
        while (!inputManager.getInput("jump"))
          ; // wait for key
      }
    }
  }
}