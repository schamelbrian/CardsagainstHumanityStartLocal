using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
/*namespace CardsAgainstHumanity
{
	public class GDcomm
	{
		string outfile, infile;

		static int IDmaker = 0;
		private int ID;
		private int lastoutfileID;
		private int lastinfileID;

		public bool stopcomm;

		GDcomm()
		{
			stopcomm = false;

			ID = IDmaker;
			IDmaker++;

			outfile = "out" + ID + ".dat";
			infile = "in" + ID + ".dat";
			lastoutfileID = 0;
			lastinfileID = -1;

		}
		public void readsendloop(player player)
		{
            while (true)
            {
                List<string> filecontents = new List<string>(System.IO.File.ReadAllLines(infile));
                if (filecontents[0] != lastinfileID.ToString())
                {
                    if (filecontents[0] != "Stub")
                    {
                        filecontents.RemoveRange(0, 2);
                        player.comminqueue.Enqueue(filecontents);
                    }

                    if (player.commoutqueue.Count != 0)
                    {
                        sendmsg(player.commoutqueue.Dequeue());
                    }

                }
                else
                {

                }


            }
		}
		private void sendmsg(List<string> msg)
		{
			StreamWriter s = new StreamWriter(outfile);
			lastoutfileID++;
			s.WriteLine(lastoutfileID);
			s.WriteLine(lastinfileID);


		}



	}
}*/
