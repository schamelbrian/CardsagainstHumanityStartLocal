using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

//NONE - game not playing, PLAY - players choosing card to play, CZAR - czar choosing best, WAIT - period between turns
namespace CardsAgainstHumanity
{
	enum gamestate { NONE, PLAY, CHOOSE, WAIT }
	public partial class MainWindow : Form
	{
		private CardsAgainstHumanityGame theGame;

		public MainWindow()
		{
			InitializeComponent();

		}

		public void errorprompt(string msg, bool end = true)
		{

		}
		private void Form1_Load(object sender, EventArgs e)
		{
			theGame = new CardsAgainstHumanityGame(2);
		}


	}
	public class player
	{
		public Queue<List<string>> commoutqueue;
		public Queue<List<string>> comminqueue;

		public player2 playerform;

		public List<string> hand;
		public int points;
		public string name;
		private bool playedcard;

		static int IDmaker;

		int playerID;
		public void unplay()
		{
			playedcard = false;
		}
		public bool play()
		{
			if (playedcard)
				return true;
			playedcard = true;
			return false;
		}
		public player(string Name)
		{
			playerID = IDmaker;
			IDmaker++;
			hand = new List<string>();
			points = 1;
			name = Name;
			playedcard = false;
			playerform = new player2();
		}

	}
	public partial class Deck
	{
		private Random rng;
		private string[] allcards;
		public List<string> discard;
		public Queue<string> cardsleft;

		public Deck() { ;}
		public Deck(string deckfile)
		{
			rng = new Random();
			discard = new List<string>();
			cardsleft = new Queue<string>();
			allcards = System.IO.File.ReadAllLines(deckfile);
			foreach (string card in allcards)
			{
				discard.Add(card);
			}
			shuffle();

		}

		//puts discard on bottom of deck in random order
		public void shuffle()
		{
			while (discard.Count != 0)
			{
				int index = rng.Next(discard.Count);
				cardsleft.Enqueue(discard[index]);
				discard.RemoveAt(index);
			}
		}
		public string drawFrom()
		{
			if (cardsleft.Count == 0)
				shuffle();
			return cardsleft.Dequeue();
		}

	}
	public partial class CardsAgainstHumanityGame
	{

		private Deck whtdeck;
		private Deck blkdeck;

		private Random rng;
		private int handsize;
		private List<player> players;
		private List<string> playedthisturn;//cards played this turn
		private const int HOSTPLAYER = 0;

		//private Thread[] threads;

		public TwoDecks getDecks()
		{
			TwoDecks ret = new TwoDecks();
			ret.W = whtdeck;
			ret.B = blkdeck;
			return ret;

		}
		private string blkcard; public string black { get { return blkcard; } }
		private int czarplayerid; public int czarid { get { return czarplayerid; } }

		public List<string> getdiscard()
		{
			return whtdeck.discard;
		}
		public Queue<string> getdeckleft()
		{
			return whtdeck.cardsleft;
		}
		public int playersfinished()
		{//eventually change to accomodate more cards
			return playedthisturn.Count();
		}

		public int Handsize
		{
			get { return handsize; }
			set { handsize = Handsize; }
		}
		public CardsAgainstHumanityGame(int numplayers, string whtcardsfile = "wht.txt", string blkcardsfile = "blk.txt")
		{
			getRules();
			loadCardDecks(blkcardsfile, whtcardsfile);
			rng = new Random();
			players = new List<player>();
			
			czarplayerid = rng.Next() % numplayers;
			playedthisturn = new List<string>();


			for (int i = 0; i < numplayers; i++)
				{ players.Add(new player("Brian")); }
			foreach (player p in players)
			{
				p.playerform.thegame = this;
			}
			fillHands();

			foreach (player p in players)
			{
				p.playerform.spreadHandButtonsAcrossScreen();
				p.playerform.Show();
			}

			/*threads = new Thread[numplayers-1];
			
			for (int i = 1; i < numplayers; i++) //set up threads initially
			{
				threads[i] = new Thread(threadcomm);
			}*/
			
		}
		
		public List<string> getHand(int player)
		{
			return players[player].hand;
		}
		
		public void playcard(int player, string card)
		{
			if (players[player].play())
			{
				return;
			}

			players[player].hand.Remove(card);
			playedthisturn.Add(card);

			if (playersfinished() == players.Count)
				newTurn();

		}
		private void newTurn()
		{
			czarplayerid++;
			if (czarplayerid == players.Count())
				czarplayerid = 0;

			fillHands();
			foreach (string card in playedthisturn)
				whtdeck.discard.Add(card);

			playedthisturn.Clear();
			foreach (player player in players)
			{
				player.unplay();
			}
			if (blkcard != null) 
				blkdeck.discard.Add(blkcard);
			blkcard = blkdeck.drawFrom();

			foreach (player p in players)
			{
				p.playerform.refreshHand();
			}
		}
		private void fillHands()
		{
			foreach (player p in players)
				while (p.hand.Count < handsize)
					draw(p);
		}
		private void draw(player theplayer, int num = 1)
		{
			for (int i = 0; i < num; i++)
				theplayer.hand.Add(whtdeck.drawFrom());
		}
		private void getRules()
		{
			handsize = 3;
		}
		private void getRules(string filepath) { loadRulesFile(filepath); }
		private void loadRulesFile(string filepath)
		{

		}//implement
		private void loadCardDecks(string blkcardsfile, string whtcardsfile)
		{
			whtdeck = new Deck(whtcardsfile);
			blkdeck = new Deck(blkcardsfile);
		}
	}
}
