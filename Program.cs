using System;
using System.ComponentModel.Design;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;

namespace BlackjackLite
{
    class Program
    {
        
        const double initialMoney = 100.00;

        static string[] cardSuits = { "♥️", "♦️", "♠️", "♣️" };
        static string[] playingCards = { "Ace", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Jack", "Queen", "King" };


        static int firstCardScore, secondCardScore, totalCardScore;
        static int totalGamesPlayed = 0;
        static double playerMoney = initialMoney;
        static string name = ("JOHN/JANE DOE");
        static string playerSkillLevel = "Rookie";

        static int playerTotalCardScore = 0;
        static int dealerTotalCardScore = 0;

        static bool isGameRunning = true;

        static List<int> playerCardScores = new List<int>();
        static List<int> dealerCardScores = new List<int>();



        static int bettingAmount;
        

        


        static void Main(string[] args)

        {
            InitializeConsoleOutputEncoding();
            

            name = getPlayerInfo();   // ask once
            getPlayerSkillLVL();       // gets how many games were played 






            while (isGameRunning) // while loops so that game will be able to be played multiple times
            {




                Console.ForegroundColor = ConsoleColor.Yellow;  //color changes for the the console.
                PrintLogo();                                    // the self made logo for the game 
                PrintGameMenu();                                // this is the games menu and buttons 

                Console.WriteLine("\n Choose your selection and Press <ENTER>");
                string selectMenuOption = Console.ReadLine();
                switch (selectMenuOption)                       // this is what plays out in the menu. this is the switches and cases 
                {
                    case "1":
                        HandleNewRound();                       // this is the method that creates a new game 

                        break;

                    case "2":
                        bool flowControl = resetPlayerStats();    // this gets used when the player want to reset the game 
                        if (!flowControl)
                        {
                            Console.WriteLine("Unchanged Stats");    
                        }
                        break;

                    case "3":
                        Console.WriteLine("Exiting your game goodbye!");     // when the player wants to exit the game 
                        isGameRunning = false;
                        break;
                }
            }








            Console.ReadKey();

            static void InitializeConsoleOutputEncoding()
            {
                Console.OutputEncoding = System.Text.Encoding.UTF8; // this just lets you use the suits symbols in the program 
            }
        }

        private static void HandleNewRound()
        {
            cardServe();
            Console.WriteLine($"Enter your bet amount\nyour starting money is: ${playerMoney:F2}");
            while (true)
            {
                string input = Console.ReadLine();

                // TryParse avoids crashing if input isn't a number
                if (!int.TryParse(input, out bettingAmount))
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("Invalid input. Please enter a number.");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    continue;
                }

                if (bettingAmount > playerMoney)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("You do not have enough money. Please enter a smaller amount.");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                }
                else if (bettingAmount <= 0)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("Bet must be greater than zero. Try again.");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                }
                else
                {
                    
                    break; // this is the way to exit the loop if the bet was vaild 

                }
            }

            Console.WriteLine($"You bet ${bettingAmount:F2}");


        


            // the new game begins
            prepareNewRound();
            
            
          
            HitCard("Dealer"); // this allows the  dealer to have two cards drawn. 
            HitCard("Dealer");
            HitCard();

            bool canHit = true;

            while (canHit)    // this keeps the game running after eacch of the selections 
            {
                HitCard();
                
                canHit = canHitAgain();


            }
            if(playerTotalCardScore > 21)
            {
                printRoundLost();
                playerMoney -= bettingAmount;
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"${bettingAmount} was taken by the house");
                
            }

            while(dealerTotalCardScore < 17 )    // blackjack rules of: the dealer must at least have 17 
            {
                HitCard("Dealer");
            }
            if(dealerTotalCardScore > 21)  // this creates a dealer bust logic if the dealer goes over 21 
            {
                printTotalScore("Dealer");
                printRoundWon();
                playerMoney += bettingAmount;
                Console.WriteLine("\n");
                Console.WriteLine($"${bettingAmount} was added to your total cash");
                return;

                
            }


            printTotalScore();
            printTotalScore("Dealer");

            calculateRoundResult();
        }

        private static bool canHitAgain() 
        {
            if(playerTotalCardScore < 21)     // allows the play to keep hitting cards until they reach 21 
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Do you want to hit again?\n1.Yes\n2.No");
                var hitAgian = Console.ReadLine();
                Console.ForegroundColor= ConsoleColor.Yellow;

                if (hitAgian == "1")
                {
                    return true;
                }
            }
            return false;
            
        }

        private static void calculateRoundResult()   // this just prints out the score at the end of the game if the dealer hasnt gone bust. 
        {
            totalGamesPlayed++;
            if (playerTotalCardScore > 21 || playerTotalCardScore <= dealerTotalCardScore || dealerTotalCardScore == 21)
            {
                playerMoney -= bettingAmount;
                printRoundLost();
            }
            
            else if (playerTotalCardScore > dealerTotalCardScore || playerTotalCardScore == 21)
            {
                playerMoney += bettingAmount;
                printRoundWon();
            }
            Console.WriteLine($"you total money is ${playerMoney:F2}");
            Console.WriteLine($"total amount of games played {totalGamesPlayed}");
         



        }

        private static void printRoundWon()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("YOU WON!...   Congrats!");
            Console.ForegroundColor = ConsoleColor.Yellow;
        }

        private static void printRoundLost()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("YOU LOST... The house gets your $$$");
            Console.ForegroundColor = ConsoleColor.Yellow;
        }

        private static void printTotalScore(string pullerRole = "Player")
        {
            
            if (pullerRole == "Player")
            {
                Console.ForegroundColor = ConsoleColor.Magenta;

                Console.WriteLine($"{pullerRole} Total card score: {playerTotalCardScore}");

                CalculateCardHit("Dealer");
                



            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"{pullerRole} Total card score: {dealerTotalCardScore}");
            }
            Console.ForegroundColor = ConsoleColor.Yellow;
        }

        private static void CalculateCardHit(string pullerRole = "Player")
        {
            if (pullerRole == "Player")
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                playerTotalCardScore = CalculateCurrentTotalCardScore(playerCardScores);


                
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                dealerTotalCardScore = CalculateCurrentTotalCardScore(dealerCardScores);

            }
            Console.ForegroundColor = ConsoleColor.Yellow;
        }



        private static int CalculateCurrentTotalCardScore(List<int> cardScores)
        {
            int total = cardScores.Sum();
            int aceCount = cardScores.Count(cs => cs == 11);

            while (total > 21 && aceCount > 0)
            {
                total -= 10; // treat one Ace as 1 instead of 11
                aceCount--;
            }

            return total;


        }

        private static void prepareNewRound()
        { 
            playerCardScores.Clear();
            dealerCardScores.Clear();
            playerTotalCardScore = 0;
            dealerTotalCardScore = 0;
            
        }

        private static void cardServe()
        {
            Console.ForegroundColor= ConsoleColor.DarkYellow;
            Console.WriteLine("shuffling the deck");
            Console.WriteLine("done shuffling the deck");
            Console.WriteLine("serving the cards...");
            Console.ForegroundColor= ConsoleColor.Yellow;
        }

        private static void HitCard(string pullerRole = "Player")
        {
            var randomGenerator = new Random();
            var cardSuit = cardSuits[randomGenerator.Next(cardSuits.Length)];

            var playingCardIndex = randomGenerator.Next(playingCards.Length);
            var playingCard = playingCards[playingCardIndex];
            int cardScore;
            int totalCardScore;
            List<int> cardScores;


            if (playingCardIndex == 0)
            {
                cardScore = 11;
            }

            else if(playingCardIndex < 9)
            {
                cardScore = playingCardIndex + 1;
            }

            else
            {
                cardScore = 10;
            }
            
                
           

            if (pullerRole == "Player")
            {

                playerCardScores.Add(cardScore);
                CalculateCardHit();
                Console.ForegroundColor = ConsoleColor.Blue;
                totalCardScore = playerTotalCardScore;
                cardScores = playerCardScores;



            }
            else
            {
                dealerCardScores.Add(cardScore);
                CalculateCardHit("Dealer");
                Console.ForegroundColor = ConsoleColor.Red;
                totalCardScore = dealerTotalCardScore;
                cardScores = dealerCardScores;
            }
            Console.WriteLine($"\n{pullerRole} is drawing a card..");

            Console.Write("Current card scores: |");

            foreach (var card in cardScores)
            {
                if (card == 11 && CalculateCurrentTotalCardScore(cardScores) > 21)
                Console.Write(" 1 |");
                else
                    Console.Write($" {card} |");
            
            
            }

            Console.WriteLine($"\n{pullerRole} drew - | {cardSuit}{playingCard}{cardSuit} | ({cardScore}).");
            Console.WriteLine($"[{pullerRole}] -> Current hand score: {totalCardScore}\n");

           
            Console.ForegroundColor = ConsoleColor.Yellow;

            
        }

        private static bool resetPlayerStats()   // this is to reset the player stats for the current player 
        {
            Console.WriteLine("Are you sure you want to reset you stats?\n1.Yes \n2. No");   //are you sure? statement "just in case" 
            string promptAnswer = Console.ReadLine();
            if (promptAnswer == "1")
            {
                totalGamesPlayed = 0;
                playerMoney = initialMoney;
                playerSkillLevel = "Rookie";

                Console.WriteLine("stats were reset");


            }
            if (promptAnswer == "2")
            {
                return false;
            }

            return true;
        }

        private static string getPlayerInfo() 
        {
            string name;
            Console.WriteLine("Please insert your Name and press <ENTER> :");
            name = Console.ReadLine();
            return name;
        }

        private static void getPlayerSkillLVL()      // this will count have many games have been played and determine the skill level of the player 
        {
            Console.WriteLine($"You have played this game {totalGamesPlayed} time(s)");
            if (totalGamesPlayed < 5)
            {
                playerSkillLevel = "Rookie";
            }
            else if (totalGamesPlayed < 10)
            {
                playerSkillLevel = "Intermediate";
            }
            else if (totalGamesPlayed < 20)
            {
                playerSkillLevel = "Advanced";
            }
            else
            {
                playerSkillLevel = "Expert";
            }
        }

        private static void PrintGameMenu()   // this is the menu that will print each time the game is restarted
        {
            Console.WriteLine($"Hello and welcome to the game {name}!");
            Console.WriteLine($"You have played a total of {totalGamesPlayed} games");
            Console.WriteLine($"Player skill level is : {playerSkillLevel}");
            Console.WriteLine($"Current amount of money : {playerMoney:F2}");
            Console.WriteLine(" 1- New Round");
            Console.WriteLine(" 2- Reset Stats");
            Console.WriteLine(" 3- EXIT");
        }

        private static void PrintLogo()  // the logo in ascii art for the console to make it a little more fun looking 
        {
            Console.WriteLine("===============================================");
            Console.WriteLine("Welcome to the Blackjack game!");
            Console.WriteLine("Created by J.Bastille");
            Console.WriteLine(" _____________  ");
            Console.WriteLine("|B  ♣️      ♥️ |");
            Console.WriteLine("| L            |");
            Console.WriteLine("|  A           |");
            Console.WriteLine("|   C          |");
            Console.WriteLine("|    K         |");
            Console.WriteLine("|      J       |");
            Console.WriteLine("|        A     |");
            Console.WriteLine("|          C   |");
            Console.WriteLine("| ♠️    ♦️   k |");
            Console.WriteLine(" --------------");
            Console.WriteLine("================================================");
        }

        
    }
}
