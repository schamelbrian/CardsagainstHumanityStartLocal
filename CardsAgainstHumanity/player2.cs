using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CardsAgainstHumanity
{
	public partial class player2 : Form
	{
		
		private int mincardlength;
		private int spacebetweencards;
		private Button[] hand;

		private int buttonheight;
		private CardsAgainstHumanityGame theGame;

		static int IDmaker;

		public CardsAgainstHumanityGame thegame 
		{ 
			set 
			{ 
				if(theGame == null)
					theGame = value; 
			} 
		}
		private int playerID;
		public const int MAXHANDSIZE = 9;

		public player2()
		{
			InitializeComponent();
			playerID = IDmaker++;
			hand = new Button[9];

			spacebetweencards = 3;
			mincardlength = 150;
			buttonheight = 150;
			initbuttons();

			this.BackColor = Color.White;
		}

		public player2(CardsAgainstHumanityGame param)
		{


		}
		private void player2_Load(object sender, EventArgs e)
		{

			this.Focus();
			

		}
		public void showhand(List<int> cards, Queue<int> deck, List<int> discard, string[] decklist)
		{
			int i = 0;
			foreach (int card in cards)
			{
				hand[i].Text = decklist[card];
				hand[i].Visible = true;
                hand[i].Tag = card;
				i++;
			}
			while (i < 9)
		    {
				hand[i].Visible = false;
				i++;
			}

			listBox1.Items.Clear(); listBox2.Items.Clear();
			foreach (int card in deck)
				listBox1.Items.Add(decklist[card]);

			foreach (int card in discard)
				listBox2.Items.Add(decklist[card]);


		}
		private void initbuttons()
		{
			
			int count = 0;
			foreach (Control x in this.Controls)
			{

				if (x is Button)
				{

					if ((((Button)x).Tag).ToString() == "Hand")
					{
						hand[count] = ((Button)x);
						count++;
						((Button)x).BackColor = Color.White;
						
					}
				}
			}
		}
		public void spreadHandButtonsAcrossScreen(int height, int length)
		{
			Point currpos = new Point(0, 0);
			int count = 0;
			foreach (Button b in hand)
			{
				if (count < theGame.Handsize)
				{

					currpos.X += spacebetweencards;
					b.Height = height;
					b.Width = length;
					b.Location = currpos;
					currpos.X += length;

					if (currpos.X + length > this.Width)
					{
						currpos.X = 0;
						currpos.Y += height + spacebetweencards;
					}
				}
				else
				{
					b.Enabled = false;
					b.Visible = false;
				}
				count++;//just bookeeping

			}
			count = 0;

			refreshHand();
		}
		public void refreshHand()
		{
			showhand(theGame.getHand(playerID), theGame.getdeckleft(), theGame.getdiscard(), theGame.whtdeck.decklist);
		}
		private void clickedhandbutton(object sender)
		{
			int s = (int)((sender as Button).Tag);
			theGame.playcard(playerID, s);

			showhand(theGame.getHand(playerID), theGame.getdeckleft(), theGame.getdiscard(), theGame.whtdeck.decklist);
		}


		private void button1_Click(object sender, EventArgs e) { clickedhandbutton(sender); }
		private void button2_Click(object sender, EventArgs e) { clickedhandbutton(sender); }
		private void button3_Click(object sender, EventArgs e) { clickedhandbutton(sender); }
		private void button4_Click(object sender, EventArgs e) { clickedhandbutton(sender); }
		private void button5_Click(object sender, EventArgs e) { clickedhandbutton(sender); }
		private void button6_Click(object sender, EventArgs e) { clickedhandbutton(sender); }
		private void button7_Click(object sender, EventArgs e) { clickedhandbutton(sender); }
		private void button8_Click(object sender, EventArgs e) { clickedhandbutton(sender); }
		private void button9_Click(object sender, EventArgs e) { clickedhandbutton(sender); }


		public void spreadHandButtonsAcrossScreen()
		{
			spreadHandButtonsAcrossScreen(buttonheight, mincardlength);
		}
	}
}
