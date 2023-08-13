using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ServerApp
{
    public class Server
    {
        private const string baseUrl = "http://localhost:8080/";
        private readonly HttpListener httpListener;

        public Server()
        {
            httpListener = new HttpListener();
            httpListener.Prefixes.Add(baseUrl);
        }

        public void Start()
        {
            httpListener.Start();
            Console.WriteLine("Server started. Listening for requests...");

            while (true)
            {
                HttpListenerContext context = httpListener.GetContext();
                HandleRequest(context);
            }
        }

        private void HandleRequest(HttpListenerContext context)
        {
            {
                HttpListenerRequest request = context.Request;
                HttpListenerResponse response = context.Response;

                string responseString = "";
                HttpStatusCode statusCode = HttpStatusCode.OK;

                try
                {
                    if (request.HttpMethod == "GET" && request.RawUrl.StartsWith("/user"))
                    {
                        string queryString = request.Url.Query;
                        var queryParams = HttpUtility.ParseQueryString(queryString);

                        if (queryParams.Count > 0)
                        {
                            UserController userController = new UserController();
                            if (queryParams.AllKeys.Contains("action"))
                            {
                                if (queryParams["action"] == "add")
                                {
                                    // Extract parameters and create a new user
                                    User newUser = new User
                                    {
                                        Name = queryParams["name"],
                                        Email = queryParams["email"],
                                        Password = queryParams["password"]
                                    };

                                    userController.AddUser(newUser);
                                    responseString = "User added successfully.";
                                }
                                else if (queryParams["action"] == "update")
                                {
                                    // Extract parameters and update user information
                                    if (int.TryParse(queryParams["id"], out int userID))
                                    {
                                        User updatedUser = new User
                                        {
                                            ID = userID,
                                            Name = queryParams["name"],
                                            Email = queryParams["email"],
                                            Password = queryParams["password"]
                                        };

                                        userController.UpdateUser(updatedUser);
                                        responseString = "User updated successfully.";
                                    }
                                    else
                                    {
                                        responseString = "Invalid user ID.";
                                        statusCode = HttpStatusCode.BadRequest;
                                    }
                                }
                                else if (queryParams["action"] == "delete")
                                {
                                    if (int.TryParse(queryParams["id"], out int userID))
                                    {

                                       userController.DeleteUser(userID);
                                       responseString = $"User ID : {userID} delete successfully.";
                                    }
                                    else
                                    {
                                        responseString = "Invalid user ID.";
                                        statusCode = HttpStatusCode.BadRequest;
                                    }
                                }
                                else if (queryParams["action"] == "userById")
                                {
                                    if (int.TryParse(queryParams["id"], out int userID))
                                    {

                                        User user = userController.GetUserByID(userID);

                                        if (user != null)
                                        {
                                            responseString = $"User Found - ID: {user.ID}, Name: {user.Name}, Email: {user.Email}, Password: {user.Password}";
                                        }
                                        else
                                        {
                                            responseString = "User not found.";
                                            statusCode = HttpStatusCode.NotFound;
                                        }
                                    }
                                    else
                                    {
                                        responseString = "Invalid user ID.";
                                        statusCode = HttpStatusCode.BadRequest;
                                    }
                                }
                                else
                                {
                                    responseString = "Invalid action or parameters.";
                                    statusCode = HttpStatusCode.BadRequest;
                                }
                            }
                        }
                        else
                        {
                            responseString = "Missing parameters.";
                            statusCode = HttpStatusCode.BadRequest;
                        }
                    }
                    else
                    {
                        statusCode = HttpStatusCode.MethodNotAllowed;
                        responseString = "Invalid request method or URL.";
                    }
                }
                catch (Exception ex)
                {
                    statusCode = HttpStatusCode.InternalServerError;
                    responseString = $"An error occurred: {ex.Message}";
                }

                byte[] buffer = Encoding.UTF8.GetBytes(responseString);
                response.StatusCode = (int)statusCode;
                response.ContentLength64 = buffer.Length;
                response.OutputStream.Write(buffer, 0, buffer.Length);
                response.OutputStream.Close();
            }
        }
    }
}
