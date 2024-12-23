using System;

namespace RQ.Unpacker
{
    class Cipher
    {
        public static Byte[] iDecryptData(Byte[] lpBuffer)
        {
            Int32 dwStartIndex = 0;
            
            UInt32 dwMagic = BitConverter.ToUInt32(lpBuffer, dwStartIndex);
            dwMagic ^= 0x7FFFFFFE;

            if (dwMagic == 0xD9C919B)
            {
                Int32 dwEndIndex = (lpBuffer.Length - 4) >> 2;
                Int32 dwBlocks = (dwEndIndex + 1) >> 1;

                dwStartIndex += 4;
                dwEndIndex *= 4;

                UInt32 dwSeed = dwSeed = (UInt32)lpBuffer.Length - 4;

                if (dwBlocks > 0)
                {
                    for (Int32 i = 0; i < dwBlocks; i++, dwStartIndex += 4, dwEndIndex -= 4)
                    {
                        UInt32 dwValueA = 0;
                        UInt32 dwValueB = 0;
                        
                        dwValueB = BitConverter.ToUInt32(lpBuffer, dwStartIndex);
                        dwSeed = 0x19660D * dwSeed + 0x3C6EF35F;

                        dwValueA = (dwSeed ^ BitConverter.ToUInt32(lpBuffer, dwEndIndex)) - dwSeed;
                        dwValueB = (dwSeed ^ dwValueB) - dwSeed;

                        lpBuffer[dwStartIndex + 0] = (Byte)dwValueA;
                        lpBuffer[dwStartIndex + 1] = (Byte)(dwValueA >> 8);
                        lpBuffer[dwStartIndex + 2] = (Byte)(dwValueA >> 16);
                        lpBuffer[dwStartIndex + 3] = (Byte)(dwValueA >> 24);

                        lpBuffer[dwEndIndex + 0] = (Byte)dwValueB;
                        lpBuffer[dwEndIndex + 1] = (Byte)(dwValueB >> 8);
                        lpBuffer[dwEndIndex + 2] = (Byte)(dwValueB >> 16);
                        lpBuffer[dwEndIndex + 3] = (Byte)(dwValueB >> 24);
                    }
                }
            }

            var lpResult = Zlib.iDecompress(lpBuffer, 14);

            return lpResult;
        }
    }
}
