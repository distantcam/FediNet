using System.Security.Cryptography;
using Asn1;

namespace FediNet.Infrastructure;

public class ASN1
{
    public static RSA ToRSA(byte[] data)
    {
        var node = Asn1Node.ReadNode(data);

        var rsaSequence = Asn1Node.ReadNode(((Asn1BitString)node.Nodes[1]).Data);

        var modulus = ((Asn1Integer)rsaSequence.Nodes[0]).Value;
        var exponent = ((Asn1Integer)rsaSequence.Nodes[1]).Value;
        var prms = new RSAParameters { Modulus = modulus, Exponent = exponent };
        var rsa = RSA.Create();
        rsa.ImportParameters(prms);
        return rsa;
    }

    public static byte[] FromRSA(RSA rsa)
    {
        var prms = rsa.ExportParameters(false);

        var modulus = new Asn1Integer(new byte[] { 0x00 }.Concat(prms.Modulus!).ToArray());
        var exponent = new Asn1Integer(prms.Exponent);

        var oidheader = new Asn1Sequence();
        oidheader.Nodes.Add(new Asn1ObjectIdentifier("1.2.840.113549.1.1.1"));
        oidheader.Nodes.Add(new Asn1Null());

        var rsaSequence = new Asn1Sequence();
        rsaSequence.Nodes.Add(modulus);
        rsaSequence.Nodes.Add(exponent);

        var bitString = new Asn1BitString(rsaSequence.GetBytes());

        var result = new Asn1Sequence();
        result.Nodes.Add(oidheader);
        result.Nodes.Add(bitString);

        return result.GetBytes();
    }
}
