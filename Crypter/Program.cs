using dnlib.DotNet;
using dnlib.DotNet.Emit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Crypter
{
    class Program
    {
        static void Main(string[] args)
        {
            Random rnd = new Random();
            var source = File.ReadAllBytes(args[0]).ToList<byte>();
            List<string> listBase = new List<string>();

           while(source.Count > 0)
            {
                var dead = rnd.Next(1, source.Count);
              
                var baseStr =source.GetRange(0, dead);
                for (int i = 0; i < baseStr.Count; i++)
                {
                    baseStr[i] = (byte)((uint)baseStr[i] ^ 2);
                }
                listBase.Add(Convert.ToBase64String(baseStr.ToArray()));
                source.RemoveRange(0, dead);
            }

            byte[] data = System.IO.File.ReadAllBytes("stub.exe");
            ModuleDefMD module = ModuleDefMD.Load(data);
            module.EntryPoint.Body.Instructions[1] =  OpCodes.Ldc_I4_S.ToInstruction((sbyte)listBase.Count);
            module.EntryPoint.Body.Instructions.RemoveAt(3);
            module.EntryPoint.Body.Instructions.RemoveAt(3);
            module.EntryPoint.Body.Instructions.RemoveAt(3);
            module.EntryPoint.Body.Instructions.RemoveAt(3);
            var instructions = new List<Instruction>();
            for (int i = 0, b = 5; i < listBase.Count; i++, b += 5)
            {
                instructions.Add( OpCodes.Dup.ToInstruction());
                if (i == 0)
                {
                    instructions.Add(OpCodes.Ldc_I4_0.ToInstruction()); 
                }
                else if (i == 1)
                {
                    instructions.Add(OpCodes.Ldc_I4_1.ToInstruction()); 
                }
                else if (i == 2)
                {
                    instructions.Add(OpCodes.Ldc_I4_2 .ToInstruction()); 
                }
                else if (i == 3)
                {
                    instructions.Add(OpCodes.Ldc_I4_3 .ToInstruction()); 
                }
                else if (i == 4)
                {
                    instructions.Add(OpCodes.Ldc_I4_4 .ToInstruction()); 
                }
                else if (i == 5)
                {
                    instructions.Add(OpCodes.Ldc_I4_5 .ToInstruction()); 
                }
                else if (i == 6)
                {
                    instructions.Add(OpCodes.Ldc_I4_6 .ToInstruction()); 
                }
                else if (i == 7)
                {
                    instructions.Add(OpCodes.Ldc_I4_7 .ToInstruction());
                }
                else if (i == 8)
                {
                    instructions.Add(OpCodes.Ldc_I4_8.ToInstruction()); 
                }
                else
                {
                    instructions.Add(OpCodes.Ldc_I4_S.ToInstruction((sbyte)i)); 
                }
                instructions.Add(OpCodes.Ldstr.ToInstruction(listBase[i]));
                instructions.Add(OpCodes.Stelem_Ref.ToInstruction());


            }
            for (int i = 3; i < instructions.Count + 3; i++)
            {
                module.EntryPoint.Body.Instructions.Insert(i, instructions[i - 3]);
            }
            module.EntryPoint.Body.KeepOldMaxStack = true;
            module.Write("Crypted.exe");
        }

    }
}

