﻿using System;
using System.Linq;
using System.Windows;
using TinyDdd.Interaction;

namespace TinyDdd.Example.Client.Desktop
{
    static class UserInteraction
    {
        internal static void ShowError(string message, Response response)
        {
            MessageBox.Show(string.Format("{1}{0}{2}",
                                Environment.NewLine,
                                message,
                                response.Errors.Aggregate(string.Empty,
                                    (result, error) => string.Format("{1}{2}{0}", Environment.NewLine, result, error.Message))),
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
    }
}
