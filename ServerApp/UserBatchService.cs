using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;

namespace ServerApp
{
    class UserBatchService
    {
        private string connectionString = "server=localhost;database=MyDatabase;user=root;password=m686868!;";
        private  Timer _batchTimer;

        //This method is go through each user, send an email and update the date in LastUpdate column  
        public void RunBatchService()
        {
            using MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();

            string query = "SELECT * FROM Users";
            MySqlCommand command = new MySqlCommand(query, connection);

            using MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                User user = null;
                    user = new User
                    {
                        ID = reader.GetInt32("ID"),
                        Name = reader.GetString("Name"),
                        Email = reader.GetString("Email"),
                        Password = reader.GetString("Password")
                    };

                //Send the email
                //sendEmail(user.Email);
                updateDate(user.ID);
                Console.WriteLine($"Send email to: Id: {user.ID}, Username: {user.Name}, Email: {user.Email}, Password: {user.Password} ");
                //Console.WriteLine($"Update the LastUpdate to user {user.Email} (ID: {user.ID})");

            }
        }
        
        public void StartBatchService()
        {

            //// Calculate the time until the next upcoming Sunday at 4:00 PM
            //DateTime nextSunday = DateTime.Today.AddDays(((int)DayOfWeek.Sunday - (int)DateTime.Today.DayOfWeek + 7) % 7).AddHours(16);
            //TimeSpan timeUntilNextSunday = nextSunday - DateTime.Now;
            //_batchTimer = new Timer(ExecuteBatchService, null, timeUntilNextSunday, TimeSpan.FromDays(7)); // Run every week

            _batchTimer = new Timer(ExecuteBatchService, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));
        }

        // This method will be executed at the specified interval
        private void ExecuteBatchService(object state)
        {
            RunBatchService();
            Console.WriteLine($"Executing batch service at {DateTime.Now}");
        }
        // update the date in LastUpdate column 
        public void updateDate(int userId) {
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();
            DateTime currentDate = DateTime.Now;
            string query = "UPDATE Users SET LastUpdate = @LastUpdate WHERE ID = @ID";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@ID", userId);
            command.Parameters.AddWithValue("@LastUpdate", currentDate);
            command.ExecuteNonQuery();
        }

        // This method is sending email to each use..
        public void sendEmail(string toEmail)
        {
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("miri47470@gmail.com");
            mailMessage.To.Add(toEmail);
            mailMessage.Subject = "Newsletter";
            mailMessage.Body = $"Hello {toEmail},\n\nHere's your personalized newsletter content for this week!";

            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Host = "smtp.gmail.com";
            smtpClient.Port = 587;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential("miri47470@gmail.com", "******");
            smtpClient.EnableSsl = true;
            try
            {
                smtpClient.Send(mailMessage);
                Console.WriteLine("Email Sent Successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

    }
}

