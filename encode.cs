using System;

namespace CipherApp
{
    public interface ICode
    {
        string Encode(string sender, string receiver, string message);
        string Decode(string sender, string receiver, string encodedMessage);
    }

    public class SimpleCipher : ICode
    {
        private int GetCharValue(char c)
        {
            if (c >= 'A' && c <= 'Z')
            {
                return c - 'A' + 1; 
            }
            else if (c >= 'a' && c <= 'z')
            {
                return c - 'a' + 27; 
            }
            return -1; 
        }

       
        private char GetCharFromValue(int value)
        {
            if (value >= 1 && value <= 26)
            {
                return (char)('A' + value - 1); 
            }
            else if (value >= 27 && value <= 52)
            {
                return (char)('a' + value - 27); 
            }
            return '\0'; 
        }

     
        private int CalculateK(string sender, string receiver, string method)
        {
            
            int senderSum = 0;
            foreach (var c in sender)
            {
                senderSum += GetCharValue(c);
            }

            int receiverSum = 0;
            foreach (var c in receiver)
            {
                receiverSum += GetCharValue(c);
            }

            senderSum %= 52;
            receiverSum %= 52;

            int K;

           
            if (method == "encode")
            {
                K = (senderSum + receiverSum) % 52;
            }
            else
            {
                K = ((senderSum * receiverSum) / (senderSum + receiverSum)) % 52;
            }

            return K;
        }

       
        public string Encode(string sender, string receiver, string message)
        {
            int K = CalculateK(sender, receiver, "encode");
            string encodedMessage = "";

            foreach (char c in message)
            {
                int charValue = GetCharValue(c);

                if (charValue != -1) 
                {
                    int encodedValue = (charValue + K) % 52;
                    encodedMessage += GetCharFromValue(encodedValue);
                }
                else
                {
                    encodedMessage += c; 
                }
            }

            return encodedMessage;
        }

        
        public string Decode(string sender, string receiver, string encodedMessage)
        {
            int K = CalculateK(sender, receiver, "decode");
            string decodedMessage = "";

            foreach (char c in encodedMessage)
            {
                int charValue = GetCharValue(c);

                if (charValue != -1) 
                {
                    int decodedValue = (charValue - K + 52) % 52; 
                    decodedMessage += GetCharFromValue(decodedValue);
                }
                else
                {
                    decodedMessage += c; 
                }
            }

            return decodedMessage;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            
            var cipher = new SimpleCipher();

         
            Console.WriteLine("Enter the sender's name:");
            string sender = Console.ReadLine();

            Console.WriteLine("Enter the receiver's name:");
            string receiver = Console.ReadLine();

            Console.WriteLine("Enter the message to send:");
            string message = Console.ReadLine();

            string encoded = cipher.Encode(sender, receiver, message);
            Console.WriteLine("Encoded Message: " + encoded);

            string decoded = cipher.Decode(sender, receiver, encoded);
            Console.WriteLine("Decoded Message: " + decoded);
        }
    }
}
