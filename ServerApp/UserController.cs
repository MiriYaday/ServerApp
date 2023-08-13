using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace ServerApp
{
    public class UserController
    {

        private readonly string connectionString = "server=localhost;database=MyDatabase;user=root;password=m686868!;";
        // add user - action=add : http://localhost:8080/user?action=add&name=Moshe&email=miri47470@gmail.com&password=123456
        public void AddUser(User user)
        {
            MySqlConnection connection = new MySqlConnection(connectionString);

            connection.Open();

            string query = "INSERT INTO Users (Name, Email, Password) VALUES (@Name, @Email, @Password)";

            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@Name", user.Name);
            command.Parameters.AddWithValue("@Email", user.Email);
            command.Parameters.AddWithValue("@Password", user.Password);

            command.ExecuteNonQuery();
        }

        // get user details according to id - action = userById: http://localhost:8080/user?action=userById&id=3
        public User GetUserByID(int id)
        {

            User user = null;

            using MySqlConnection connection = new MySqlConnection(connectionString);

            connection.Open();

            string query = "SELECT ID, Name, Email, Password FROM Users WHERE ID = @ID";

            using MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@ID", id);

            using MySqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                user = new User
                {
                    ID = reader.GetInt32("ID"),
                    Name = reader.GetString("Name"),
                    Email = reader.GetString("Email"),
                    Password = reader.GetString("Password")
                };
            }
            return user;
        }
        //update user - action = update : http://localhost:8080/user?action=update&id=9&name=MiriamDror&email=Miri47470@gmail.com&password=123
        public void UpdateUser(User user)
        {
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();

            string query = "UPDATE Users SET Name = @Name, Email = @Email, Password = @Password WHERE ID = @ID";

            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@ID", user.ID);
            command.Parameters.AddWithValue("@Name", user.Name);
            command.Parameters.AddWithValue("@Email", user.Email);
            command.Parameters.AddWithValue("@Password", user.Password);

            command.ExecuteNonQuery();
        }
        // delete user according to id - action = delete : http://localhost:8080/user?action=delete&id=9
        public void DeleteUser(int id)
        {
            MySqlConnection connection = new MySqlConnection(connectionString);

            connection.Open();

            string query = "DELETE FROM Users WHERE ID = @ID";

            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@ID", id);

            command.ExecuteNonQuery();
        }
    }
}
