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
        if (!string.IsNullOrEmpty(netMask))
        {
            int calSubNet = 32 - Convert.ToInt32(netMask);
    
            if (calSubNet is >= 0 and <= 32)
            {
                double result = 0;
                int octetIndex = calSubNet / 8;
                int bitsInOctet = calSubNet % 8;
        
                if (bitsInOctet == 0 && calSubNet > 0)
                {
                    octetIndex--;
                    bitsInOctet = 8;
                }
        
                for (int i = 0; i < bitsInOctet; i++)
                {
                    result += Math.Pow(2, i);
                }
        
                double finalSubnet = 255 - result;
        
                string[] octets = new string[4];
        
                for (int i = 0; i < 4; i++)
                {
                    octets[i] = "255";
                }
        
                for (int i = 0; i < 3 - octetIndex; i++)
                {
                    octets[i] = "255";
                }
        
                octets[3 - octetIndex] = Convert.ToString(finalSubnet, CultureInfo.InvariantCulture);
        
                for (int i = 3 - octetIndex + 1; i < 4; i++)
                {
                    octets[i] = "0";
                }
        
                subNetMask = string.Join(".", octets);
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

    private void decimalToBinary_Click(object? sender, RoutedEventArgs e)
    {
      //  throw new NotImplementedException();
    }
}