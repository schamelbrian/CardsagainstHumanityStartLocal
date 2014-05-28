using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace CardsAgainstHumanity
{
   
	[ServiceContract]
	public interface CAHService
	{
		[OperationContract]
		TwoDecks getDecks();
		[OperationContract]
		List<string> gethand(int pid);
	}

	[DataContract]
	public class TwoDecks
	{
		private Deck wdeck;
		private Deck bdeck;

		[DataMember]
		public Deck W
		{
			get { return wdeck; }
			set { wdeck = value; }
		}

		[DataMember]
		public Deck B
		{
			get { return bdeck; }
			set { bdeck = value; }
		}
	}

	public class MyCAHService : CAHService
	{
		CardsAgainstHumanityGame theGame;

		public MyCAHService(CardsAgainstHumanityGame game)
		{
			theGame = game;
		}
		
		public TwoDecks getDecks()
		{
			return theGame.getDecks();
		}

		public List<string> gethand(int pid)
		{
			return theGame.getHand(pid);
		}
	}


}
