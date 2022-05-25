﻿namespace NotesApp.Services.Email
{
    public class EmailSettings
    {
        public string From { get; set; }
        public string SmtpServer { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ResetPasswordRoute { get; set; }
    }
}
