﻿using System.Windows;

namespace LogInPage
{
    public partial class Client
    {
        private void GetResponce(string responce)
        {
            int methodIndex = responce.IndexOf(' ');
            if (methodIndex == -1) throw new Exception("Get method was not found.");

            string method = responce[..methodIndex];
            switch (method)
            {
                case "--LOG-IN":
                    this.GetResponceUserCheck(responce);
                    break; // --LOG-IN
                case "--CHAT-HISTORY":
                    this.GetResponceUpdateChat(responce);
                    break; // --CHAT-HISTORY
                case "--USER-LIST":
                    this.GetResponceUserByLogin(responce);
                    break; // --USER-LIST
                case "--CHAT-LIST":
                    this.GetResponceUpdateChatList(responce);
                    break; // --CHAT-LIST
            }
        }

        private void GetResponceUserCheck(string responce)
        {
            CurrentUser = JsonExtractor<User>(responce, "json", right: 2);
            if (!Answer.Contains("FALSE")) 
            { 
                ServerConfirmation = true;
            }
        }

        private void GetResponceUpdateChat(string responce)
        {
            List<Chat>? chatList = null;
            try
            {
                chatList = JsonExtractor<List<Chat>>(responce, "json", right: 4);
            }
            catch
            {
                chatList = JsonExtractor<List<Chat>>(responce, "json", right: 2);
            }

            if (chatList is not null && CurrenWindow is not null && CurrenWindow is ClientWindow cw)
            {
                foreach (var chat in chatList)
                {
                    if (chat.Messages is not null)
                    {
                        foreach (var message in chat.Messages)
                        {
                            cw.UploadMessage(message, message.Username?.Equals(CurrentUser?.Login));
                        }
                    }
                }
            }
        }

        private void GetResponceUserByLogin(string responce)
        {
            try
            {
                UserPackege? userPackege = JsonExtractor<UserPackege>(responce, "json", right: 4);
                if (userPackege is not null && CurrenWindow is not null && CurrenWindow is ClientWindow cw)
                {
                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        cw.userSearchResult = userPackege.users;
                        foreach (var user in userPackege.users)
                        {
                            cw.userList.Add(new ListButton(user));
                        }
                    }));
                }
            }
            catch
            {
                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    if (CurrenWindow is not null && CurrenWindow is ClientWindow cw)
                    {
                        cw.userList = new user_list(cw);
                    }
                }));
            }
        }

        private void GetResponceUpdateChatList(string responce)
        {
            try 
            { 
                UserChatPreload = JsonExtractor<List<Chat>>(responce, "json", right:4);
            }
            catch 
            { 
                UserChatPreload = JsonExtractor<List<Chat>>(responce, "json", right:2);
            }
        }
    }
}
