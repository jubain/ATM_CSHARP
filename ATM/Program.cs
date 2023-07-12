using MySql.Data.MySqlClient;

public class CardHolder
{
    private string CardNum;
    private int Pin;
    private string FirstName;
    private string LastName;
    private double Balance;

    public CardHolder(string CardNum, int Pin, string FirstName, string LastName, double Balance)
    {
        this.CardNum = CardNum;
        this.Pin = Pin;
        this.FirstName = FirstName;
        this.LastName = LastName;
        this.Balance = Balance;
    }

    public string getCardNum()
    {
        return CardNum;
    }

    public int getPin()
    {
        return Pin;
    }

    public string getFirstName()
    {
        return FirstName;
    }

    public string getLastName()
    {
        return LastName;
    }

    public double getBalance()
    {
        return Balance;
    }

    // Setters
    public void serCardNum(string CardNumber)
    {
        this.CardNum = CardNumber;
    }

    public void setPin(int Pin)
    {
        this.Pin = Pin;
    }

    public void setFirstName(string FirstName)
    {
        this.FirstName = FirstName;
    }

    public void setLastName(string LastName)
    {
        this.LastName = LastName;
    }

    public void setBalance(double Balance)
    {
        this.Balance = Balance;
    }

    public static void Main(String[] args)
    {
        // Database connection
        try
        {
            string connectionString = "Server=127.0.0.1;Port=3307;Database=ATM;User Id=root;password=Password;";
            MySqlConnection con = new MySqlConnection();
            con.ConnectionString = connectionString;
            con.Open();


            void printOptions()
            {
                Console.WriteLine("Please choose from...");
                Console.WriteLine("1. Diposit");
                Console.WriteLine("2. Withdraw");
                Console.WriteLine("3. Show Balance");
                Console.WriteLine("4. Exit");
            }

            void deposit(CardHolder currentUser)
            {
                try
                {
                    Console.WriteLine("How much would you like to deposit?");
                    // Convert string to double
                    double deposit = Double.Parse(Console.ReadLine());
                    currentUser.setBalance(currentUser.getBalance() + deposit);
                    Console.WriteLine("Thank you for your deposit. Your current balance is: " + currentUser.getBalance());
                }
                catch (Exception err)
                {
                    Console.WriteLine(err.Message);
                }

            }

            void withDraw(CardHolder currentUser)
            {
                try
                {
                    Console.WriteLine("How much would you like to withdraw?");
                    // Convert string to double
                    double withDraw = Double.Parse(Console.ReadLine());
                    // Check if user has enough money
                    if (currentUser.getBalance() < withDraw)
                    {
                        throw new Exception("Insuffcient balance :( !");
                    }
                    else
                    {
                        double newBalance = currentUser.getBalance() - withDraw;
                        currentUser.setBalance(newBalance);
                        Console.WriteLine("Thank you for your withdraw. Your current balance is: " + currentUser.getBalance());
                    }
                }
                catch (Exception err)
                {
                    Console.WriteLine(err.Message);
                }

            }

            void balance(CardHolder currentUser)
            {

                Console.WriteLine("Your current balance is: " + currentUser.getBalance());

            }
            string sql = "select * from CardHolders;";
            MySqlCommand command = new MySqlCommand(sql, con);
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                List<CardHolder> cardHolders = new List<CardHolder>();
                cardHolders.Add(new CardHolder("76715263190", 5678, "Randy", "Orton", 350));
                cardHolders.Add(new CardHolder(reader["CardNum"].ToString(), int.Parse(reader["Pin"].ToString()), reader["FirstName"].ToString(), reader["LastName"].ToString(), double.Parse(reader["Balance"].ToString())));

                Console.WriteLine("Welcome to simple ATM.");
                Console.WriteLine("Please insert your debit card");
                string debitCardNumber = "";
                CardHolder currentUser;

                while (true)
                {
                    try
                    {
                        debitCardNumber = Console.ReadLine();
                        // check num from the List
                        // store the current user detail

                        currentUser = cardHolders.FirstOrDefault(a => a.CardNum == debitCardNumber);

                        if (currentUser != null)
                        {
                            break;
                        }
                        Console.WriteLine("Card not recognised!");

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Card not recognised!");
                    }
                }

                Console.WriteLine("Please enter your pin: ");
                int pin;
                while (true)
                {
                    try
                    {
                        debitCardNumber = Console.ReadLine();
                        pin = int.Parse(debitCardNumber);
                        // check num from the List
                        if (currentUser.getPin() == pin)
                        {
                            break;
                        }
                        Console.WriteLine("Incorrent Pin!");

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Incorrent Pin!");
                    }
                }

                Console.WriteLine("Welcome " + currentUser.getFirstName());
                int option = 0;
                do
                {
                    printOptions();
                    try
                    {
                        option = int.Parse(Console.ReadLine());
                    }
                    catch
                    {

                    }

                    if (option == 1) { deposit(currentUser); }
                    else if (option == 2) { withDraw(currentUser); }
                    else if (option == 3) { balance(currentUser); }
                    else if (option == 4) { break; }
                    else { option = 0; }
                } while (option != 4);
                Console.WriteLine("Thank you, have a great day :) !");

            }
        }
        catch (MySqlException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}