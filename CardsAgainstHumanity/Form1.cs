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
	public enum gamestate { NONE, PLAY, CZAR, WAIT }
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
		public player2 playerform;

		public List<int> hand;
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
			hand = new List<int>();
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
		public List<int> discard;
		public Queue<int> cardsleft;

        public string[] decklist
        {
            get { return allcards; }
        }
		public Deck() { ;}
		public Deck(string deckfile)
		{
			rng = new Random();
			discard = new List<int>();
			cardsleft = new Queue<int>();
			allcards = System.IO.File.ReadAllLines(deckfile);
            for (int card = 0; card < allcards.Count(); card++ )
            {
                discard.Add(card);
            }
			shuffle();

		}
        
        public string this[int index]
        {
            get { return allcards[index]; }
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
		public int drawFrom()
		{
			if (cardsleft.Count == 0)
				shuffle();
			return cardsleft.Dequeue();
		}

	}
	public partial class CardsAgainstHumanityGame
	{

		public Deck whtdeck;
		public Deck blkdeck;


		private Random rng;
		private int handsize;
		public List<player> players;
		public List<int> playedthisturn;//cards played this turn
		private const int HOSTPLAYER = 0;

        public int MAXPLAYERS = 15;

		//private Thread[] threads;

		private int blkcard; public int black { get { return blkcard; } }
		private int czarplayerid; public int czarid { get { return czarplayerid; } }

        public gamestate gstate;

		public List<int> getdiscard()
		{
			return whtdeck.discard;
		}
		public Queue<int> getdeckleft()
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
			playedthisturn = new List<int>();


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
			
		}

        public int addPlayer(string name)
        {
            if (players.Count >= MAXPLAYERS)
            {
                players.Add(new player(name));
                return players.Count - 1;
            }
            return -1;
        }
		public List<int> getHand(int player)
		{
			return players[player].hand;
		}
		
       
		public void playcard(int player, int card)
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
			foreach (int card in playedthisturn)
				whtdeck.discard.Add(card);

			playedthisturn.Clear();
			foreach (player player in players)
			{
				player.unplay();
			}
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
