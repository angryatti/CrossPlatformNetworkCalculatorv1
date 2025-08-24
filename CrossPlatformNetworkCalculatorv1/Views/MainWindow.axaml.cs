using System;
using System.Globalization;
using System.Net;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace CrossPlatformNetworkCalculatorv1.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }


    private void Grid_Loaded(object sender, RoutedEventArgs e)
    {
        for (int i = 0; i <= 32; i++)
        {
            SubnetMask.Items.Add("/" + i);
        }

        SubnetMask.SelectedIndex = 24;
    }

   private string GetSubnetAddressFromIpNetMask(string netMask)
    {
        string subNetMask = string.Empty;
        double result = 0;
        if (!string.IsNullOrEmpty(netMask))
        {
            int calSubNet = 32 - Convert.ToInt32(netMask);
            if (calSubNet is >= 0 and <= 8)
            {
                for (int ipower = 0; ipower < calSubNet; ipower++)
                {
                    result += Math.Pow(2, ipower);
                }

                double finalSubnet = 255 - result;
                subNetMask = "255.255.255." + Convert.ToString(finalSubnet, CultureInfo.InvariantCulture);
            }
            else if (calSubNet is > 8 and <= 16)
            {
                int secOctet = 16 - calSubNet;

                secOctet = 8 - secOctet;

                for (int ipower = 0; ipower < secOctet; ipower++)
                {
                    result += Math.Pow(2, ipower);
                }

                double finalSubnet = 255 - result;
                subNetMask = "255.255." + Convert.ToString(finalSubnet, CultureInfo.InvariantCulture) + ".0";
            }
            else if (calSubNet is > 16 and <= 24)
            {
                int thirdOctet = 24 - calSubNet;

                thirdOctet = 8 - thirdOctet;

                for (int ipower = 0; ipower < thirdOctet; ipower++)
                {
                    result += Math.Pow(2, ipower);
                }

                double finalSubnet = 255 - result;
                subNetMask = "255." + Convert.ToString(finalSubnet, CultureInfo.InvariantCulture) + ".0.0";
            }
            else if (calSubNet is > 24 and <= 32)
            {
                int fourthOctet = 32 - calSubNet;

                fourthOctet = 8 - fourthOctet;

                for (int ipower = 0; ipower < fourthOctet; ipower++)
                {
                    result += Math.Pow(2, ipower);
                }

                double finalSubnet = 255 - result;
                subNetMask = Convert.ToString(finalSubnet, CultureInfo.InvariantCulture) + ".0.0.0";
            }
        }

        return subNetMask;
    }

   private IPAddress InvertArray(byte[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            array[i] = (byte)~array[i];
        }

        IPAddress ipAddress = new IPAddress(array);

        return ipAddress;
    }

    private void subnetShow_Click(object sender, RoutedEventArgs e)
    {
        string? selectedValue = SubnetMask.SelectedValue?.ToString();
        string tempMask = GetSubnetAddressFromIpNetMask(selectedValue?.Substring(1) ?? "24");

        SubnetCalced.Content = $"Subnet Mask: {tempMask}";
        IPAddress address = IPAddress.Parse(tempMask);
        Byte[] bytes = address.GetAddressBytes();

        WildsubnetCalced.Content = $"Wild Subnet Mask: {InvertArray(bytes)}";
    }
}