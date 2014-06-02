// TODO-IG: Add Intentionally Bad Code warning!
using System;
using System.Linq;
using System.Windows;
using Pragmatic.Interaction;

namespace Pragmatic.Example.Client.Desktop
{
    static class UserInteraction
    {
        internal static void ShowError(string message, Response response)
        {
            MessageBox.Show(string.Format("{1}{0}{2}",
                                System.Environment.NewLine,
                                message,
                                response.Errors.Aggregate(string.Empty,
                                    (result, error) => string.Format("{1}{2}{0}", System.Environment.NewLine, result, error.Message))),
                             "Error",
                             MessageBoxButton.OK,
                             MessageBoxImage.Error);
        }

        internal static void ShowInformation(string message)
        {
            MessageBox.Show(message,
                            "Information",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information);
        }

        internal static MessageBoxResult ShowQuestion(string message, MessageBoxButton messageBoxButton)
        {
           return  MessageBox.Show(message,
                                   "Question",
                                   messageBoxButton,
                                   MessageBoxImage.Question);
        }

        internal static MessageBoxResult ShowResponse(string message, Response response, MessageBoxButton messageBoxButton)
        {
            MessageBoxImage messageBoxImage = MessageBoxImage.Information;
            string caption = "Information";

            if (response.HasErrors)
            {
                messageBoxImage = MessageBoxImage.Error;
                caption = "Error";
            }
            else if (response.HasWarnings)
            {
                messageBoxImage = MessageBoxImage.Warning;
                caption = "Warning";
            }
            else if (response.HasInformation)
            {
                messageBoxImage = MessageBoxImage.Information;
                caption = "Information";
            }

            string responseMessage = (response.Errors.Aggregate(string.Empty, (result, error) => string.Format("{1}{2}{0}", System.Environment.NewLine, result, error.Message)) + System.Environment.NewLine +
                                      response.Warnings.Aggregate(string.Empty, (result, error) => string.Format("{1}{2}{0}", System.Environment.NewLine, result, error.Message)) + System.Environment.NewLine +
                                      response.Information.Aggregate(string.Empty, (result, error) => string.Format("{1}{2}{0}", System.Environment.NewLine, result, error.Message))).Trim();

            return MessageBox.Show((message + System.Environment.NewLine + responseMessage).Trim(), caption, messageBoxButton, messageBoxImage);
        }
    }
}
