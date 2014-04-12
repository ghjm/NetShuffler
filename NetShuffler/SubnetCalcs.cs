using System;
using System.Net;

// This file provides some useful extension methods for IP addresses.

public static class IPAddressExtensions
{

    // Given a subnet mask, get the broadcast address related to this IP address.
    public static IPAddress BroadcastAddress(this IPAddress address, IPAddress subnetMask)
    {
        byte[] ipAdressBytes = address.GetAddressBytes();
        byte[] subnetMaskBytes = subnetMask.GetAddressBytes();

        if (ipAdressBytes.Length != subnetMaskBytes.Length)
            throw new ArgumentException("Lengths of IP address and subnet mask do not match.");

        byte[] broadcastAddress = new byte[ipAdressBytes.Length];
        for (int i = 0; i < broadcastAddress.Length; i++)
        {
            broadcastAddress[i] = (byte)(ipAdressBytes[i] | (subnetMaskBytes[i] ^ 255));
        }
        return new IPAddress(broadcastAddress);
    }

    // Given a subnet mask, get the network address related to this IP address.
    public static IPAddress NetworkAddress(this IPAddress address, IPAddress subnetMask)
    {
        byte[] ipAdressBytes = address.GetAddressBytes();
        byte[] subnetMaskBytes = subnetMask.GetAddressBytes();

        if (ipAdressBytes.Length != subnetMaskBytes.Length)
            throw new ArgumentException("Lengths of IP address and subnet mask do not match.");

        byte[] broadcastAddress = new byte[ipAdressBytes.Length];
        for (int i = 0; i < broadcastAddress.Length; i++)
        {
            broadcastAddress[i] = (byte)(ipAdressBytes[i] & (subnetMaskBytes[i]));
        }
        return new IPAddress(broadcastAddress);
    }

    // Given another IP address and a subnet mask, determine whether the two addresses are in the same subnet.
    public static bool IsInSameSubnet(this IPAddress address2, IPAddress address, IPAddress subnetMask)
    {
        IPAddress network1 = address.NetworkAddress(subnetMask);
        IPAddress network2 = address2.NetworkAddress(subnetMask);

        return network1.Equals(network2);
    }

    // Convert an address-format subnet mask (like 255.255.255.0) to a CIDR-style bit count (like 24).
    public static UInt32 NetmaskToBits(this IPAddress subnetMask)
    {
        byte[] ipParts = subnetMask.GetAddressBytes();
        UInt32 subnet = 16777216 * Convert.ToUInt32(ipParts[0]) + 65536 * Convert.ToUInt32(ipParts[1]) + 256 * Convert.ToUInt32(ipParts[2]) + Convert.ToUInt32(ipParts[3]);
        UInt32 mask = 0x80000000;
        UInt32 subnetConsecutiveOnes = 0;
        for (int i = 0; i < 32; i++)
        {
            if (!(mask & subnet).Equals(mask)) break;

            subnetConsecutiveOnes++;
            mask = mask >> 1;
        }
        return subnetConsecutiveOnes;
    }

    // Convert a CIDR-style bit count (like 24) to an address-format subnet mask (like 255.255.255.0).
    public static IPAddress BitsToNetmask(this IPAddress subnetMask, int Bits)
    {
        UInt32 mask = ~(((UInt32)1 << (32 - Bits)) - 1);
        mask = (UInt32)IPAddress.NetworkToHostOrder((int)mask);
        return new IPAddress(mask);
    }
}