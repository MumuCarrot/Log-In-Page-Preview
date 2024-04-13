﻿using System.Windows;
using System.Windows.Controls;

namespace LogInPage
{
    public partial class MessageFrame : UserControl
    {
        public MessageFrame(Message message, bool? isMy)
        {
            InitializeComponent();

            username.Text = message.Username;
            content.Text = message.Content?.Text;
            time.Text = message.Time.ToString("HH:mm");

            if (isMy is not null && (bool)isMy)
            {
                frame.HorizontalAlignment = HorizontalAlignment.Right;
                frame.CornerRadius = new(11, 11, 0, 8);
            }
            else 
            {
                frame.HorizontalAlignment = HorizontalAlignment.Left;
                frame.CornerRadius = new(11, 11, 8, 0);
            }
        }
    }
}