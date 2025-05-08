using Microsoft.Data.SqlClient;
using System.Data;

namespace ADO.net
{
    internal class Program
    {
        const string connectionString = "Data Source=DESKTOP-LED85E0;Initial Catalog=Sakila;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False";
        static void Main(string[] args)
        {
            /* Show Menu */
            Menu();


            /* Return to menu */
            ReturnMenu();
        }
        static void Menu()
        {
            Console.WriteLine("Menu");
            Console.WriteLine("1. Search by First name actor");
            Console.WriteLine("2. Search by Last name actor");
            Console.WriteLine("3. Search by Full name actor");
            int choice = int.Parse(Console.ReadLine());
            MenuChoice(choice);
        }

        static void MenuChoice(int choice)
        {
            switch (choice)
            {
                case 1:
                    SearchBySingleName("uspSearchActorsFirstName", "FirstName");
                    break;
                case 2:
                    SearchBySingleName("uspSearchActorsLastName", "LastName");
                    break;
                case 3:
                    SearchByFull();
                    break;
                default:
                    Console.WriteLine("Please choose a number between 1-3");
                    ReturnMenu();
                    break;
            }
        }
        static void ReturnMenu()
        {
            Console.WriteLine("Press any key to return to menu");
            Console.ReadKey();
            Console.Clear();
            Menu();
        }
        static void SearchBySingleName(string procedureName, string parameterName)
        {
            Console.WriteLine("Please enter the First name of your chosen actor:");
            string inputName = Console.ReadLine();

            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand(procedureName, connection)) {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@{parameterName}", inputName);

                connection.Open();
                using (SqlDataReader reader  = command.ExecuteReader()) {
                    if (!reader.HasRows) {
                    Console.WriteLine("No actors in this list by that name");
                    connection.Close();
                    ReturnMenu();
                    }
                    else
                    {
                        DisplayActors(reader);
                        SearchById();
                    }
                }
                connection.Close();
            }
        }
        static void SearchByFull()
        {
            Console.WriteLine("Please enter the First name of your chosen actor:");
            string firstName = Console.ReadLine();
            Console.WriteLine("Please enter the Last name of your chosen actor:");
            string lastName = Console.ReadLine();

            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand("uspSearchActorsFullName", connection)) {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@FirstName", firstName);
                command.Parameters.AddWithValue("@LastName", lastName);

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (!reader.HasRows)
                    {
                        Console.WriteLine("No actors found that name.");
                        ReturnMenu();
                    }
                    else
                    {
                        DisplayActors(reader);
                        SearchById();
                    }
                }
                connection.Close();
            }
        }
        static void SearchById()
        {
            Console.WriteLine("Please enter the ID of the correct actor:");
            int chosenId = int.Parse(Console.ReadLine());

            using(var connection = new SqlConnection(connectionString))
            using(var command = new SqlCommand("uspSearchActorsActorId", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ActorId", chosenId);

                connection.Open();
                using(SqlDataReader reader = command.ExecuteReader())
                {
                    if(!reader.HasRows)
                    {
                        Console.WriteLine("No actor found with that ID");
                        ReturnMenu();
                    }
                    else
                    {
                        Console.WriteLine("This actor has been in the following films:");
                        while (reader.Read())
                        {
                            Console.WriteLine($"Title: {reader["Title"]}, Released in: {reader["Released"]}");
                        }
                    }

                    connection.Close();
                    Console.WriteLine("Would you like to choose another actor?");
                    Console.WriteLine("1. Yes");
                    Console.WriteLine("2. No");
                    int choice = int.Parse(Console.ReadLine());
                    if (choice == 1)
                    {
                        SearchById();
                    } else
                    {
                        ReturnMenu();
                    }
                }    
            }
        }
        static void DisplayActors(SqlDataReader reader)
        {
            while (reader.Read())
            {
                Console.WriteLine($"ID: {reader["Actor_Id"]}, Actor Name: {reader["FirstName"]} {reader["LastName"]}");
            }
        }
    }
}
