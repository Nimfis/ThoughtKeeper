using System;
using ThoughtKeeper.Service;


namespace ThoughtKeeper.Service
{
    public interface INoteCryptoService
    {
        string EncryptString(string text);
        string DecryptString(string cipherText);
        byte[] GetKey();
        byte[] GetIV();
    }
}
