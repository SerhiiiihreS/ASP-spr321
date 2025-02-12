using Microsoft.AspNetCore.Components.Forms;
using System;

namespace ASP_spr321.Services.OTP
{
    public class OtpRand : OTPservice
    {
        
        string[] AllChar = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" };
         public int Input(int _OTPLength1)
        {
            int OTPLength1 = _OTPLength1;
            //int OTPLength1 = Convert.ToInt32(Console.ReadLine());
            return OTPLength1;
        }
        private string GenrtOTP(int _OTPLength, String[] AllChar) 
        {
            string sOTP=String.Empty;
            string sIndOTP=String.Empty;
            Random rand=new Random();
            for (int i = 0; i < _OTPLength; i++)
            {
                sIndOTP= AllChar[rand.Next(0, AllChar.Length)];
                sOTP += sIndOTP;
            }
            return sOTP;
        }
        public int OTPLength => Input(10);
        public string OTP =>GenrtOTP(OTPLength, AllChar);
        
    }
}
