using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ratchet.Code
{
    abstract class SPIRVModuleReader
    {
        System.IO.Stream _Stream;
        byte[] _Buffer = new byte[4096];
        int _BufferLength = 0;
        uint _Offset = 0;
        bool _EndOfStream = false;
        uint _Version = 0;
        uint _Generator = 0;
        uint _MaxID = 0;


        public SPIRVModuleReader(System.IO.Stream Stream)
        {
            _Stream = Stream;
            _BufferLength = (int)_Stream.Read(_Buffer, 0, _Buffer.Length);
            if (_BufferLength < 0) { _BufferLength = 0; _EndOfStream = true; }
            if (_BufferLength / 4 < 4) { throw new Exception("The stream doesn't contain a valid SPIRV Module"); }
            _Version = ReadDWord();
            _Generator = ReadDWord();
            _MaxID = ReadDWord();
            ReadDWord();

        }

        void NextDWord()
        {
            _Offset += 4;
            if (_Offset >= _BufferLength)
            {
                if (_BufferLength == _Buffer.Length)
                {
                    _BufferLength = (int)_Stream.Read(_Buffer, 0, _Buffer.Length);
                    if (_BufferLength < 0) { _BufferLength = 0; _EndOfStream = true; }
                }
                else { _EndOfStream = true; }
            }
        }

        protected UInt32 ReadUint32LE()
        {

            UInt32 result = _Buffer[_Offset] | ((uint)_Buffer[_Offset + 1] << 8) | ((uint)_Buffer[_Offset + 2] << 16) | ((uint)_Buffer[_Offset + 3] << 24);
            NextDWord();
            return result;
        }

        protected UInt32 ReadUint32BE()
        {

            UInt32 result = _Buffer[_Offset + 3] | ((uint)_Buffer[_Offset + 2] << 8) | ((uint)_Buffer[_Offset + 1] << 16) | ((uint)_Buffer[_Offset] << 24);
            NextDWord();
            return result;
        }

        public abstract UInt32 ReadDWord();

        class SPIRVModuleReader_LE : SPIRVModuleReader
        {
            public SPIRVModuleReader_LE(System.IO.Stream Stream) : base(Stream) { }
            public override UInt32 ReadDWord() { return ReadUint32LE(); }

            public override string LiteralStringFromData(uint[] data, uint offset, out uint finalOffset)
            {
                List<byte> text = new List<byte>();
                while (offset < data.Length)
                {
                    byte _4 = (byte)((data[offset] & 0xFF000000) >> 24);
                    byte _3 = (byte)((data[offset] & 0xFF0000) >> 16);
                    byte _2 = (byte)((data[offset] & 0xFF00) >> 8);
                    byte _1 = (byte)((data[offset] & 0xFF));
                    if (_1 == 0) { break; }
                    text.Add(_1);
                    if (_2 == 0) { break; }
                    text.Add(_2);
                    if (_3 == 0) { break; }
                    text.Add(_3);
                    if (_4 == 0) { break; }
                    text.Add(_4);
                    offset++;
                }
                finalOffset = offset + 1;
                return System.Text.Encoding.UTF8.GetString(text.ToArray());
            }
            public override unsafe double Float64FromData(uint[] data, uint offset)
            {
                ulong word = data[offset] | ((ulong)data[offset + 1] << 32);
                double* pDouble = (double*)&word;
                return *pDouble;
            }
        }

        class SPIRVModuleReader_BE : SPIRVModuleReader
        {
            public SPIRVModuleReader_BE(System.IO.Stream Stream) : base(Stream) { }
            public override UInt32 ReadDWord() { return ReadUint32BE(); }
            public override string LiteralStringFromData(uint[] data, uint offset, out uint finalOffset)
            {
                List<byte> text = new List<byte>();
                while (offset < data.Length)
                {
                    byte _1 = (byte)((data[offset] & 0xFF000000) >> 24);
                    byte _2 = (byte)((data[offset] & 0xFF0000) >> 16);
                    byte _3 = (byte)((data[offset] & 0xFF00) >> 8);
                    byte _4 = (byte)((data[offset] & 0xFF));
                    if (_1 == 0) { break; }
                    text.Add(_1);
                    if (_2 == 0) { break; }
                    text.Add(_2);
                    if (_3 == 0) { break; }
                    text.Add(_3);
                    if (_4 == 0) { break; }
                    text.Add(_4);
                    offset++;
                }
                finalOffset = offset + 1;
                return System.Text.Encoding.UTF8.GetString(text.ToArray());
            }
            public override unsafe double Float64FromData(uint[] data, uint offset)
            {
                ulong word = data[offset + 1] | ((ulong)data[offset] << 32);
                double* pDouble = (double*)&word;
                return *pDouble;
            }
        }

        public static SPIRVModuleReader Create(System.IO.Stream Stream)
        {
            byte[] buffer = new byte[4];
            if (Stream.Read(buffer, 0, 4) != 4) { throw new Exception("The stream doesn't contain a valid SPIRV Module"); }
            uint Magick = BitConverter.ToUInt32(buffer, 0);
            if (Magick == 0x07230203) { return new SPIRVModuleReader_LE(Stream); }
            else { return new SPIRVModuleReader_BE(Stream); }
        }

        static Dictionary<ushort, SPIRV.OpCode> opcodes_bytes = new Dictionary<ushort, SPIRV.OpCode>();


        static SPIRVModuleReader()
        {
            foreach (System.Reflection.FieldInfo _ in typeof(SPIRV.OpCodes).GetFields())
            {
                if (_.IsStatic && _.IsPublic)
                {
                    SPIRV.OpCode opcode = (SPIRV.OpCode)_.GetValue(null);
                    opcodes_bytes[(ushort)opcode.OpCodeValue] = opcode;
                }
            }
        }

        public SPIRV.Instruction ReadInstruction()
        {
            if (_EndOfStream) { return null; }
            uint firstWord = ReadDWord();
            ushort opcode = (ushort)(firstWord & 0xFFFF);
            uint wordcount = ((firstWord & 0xFFFF0000) >> 16);

            if (!opcodes_bytes.ContainsKey(opcode)) { throw new Exception("Unknwon opcode " + opcode.ToString()); }
            SPIRV.Instruction instruction = new SPIRV.Instruction() { _OpCode = opcodes_bytes[opcode] };
            if (wordcount > 0)
            {
                wordcount--;
                uint[] words = new uint[wordcount];
                for (int n = 0; n < wordcount; n++) { words[n] = ReadDWord(); }
                instruction._Data = words;

            }
            return instruction;
        }

        public abstract string LiteralStringFromData(uint[] data, uint offset, out uint finalOffset);
        public unsafe float Float32FromData(uint[] data, uint offset)
        {
            uint word = data[offset];
            float* pFloat = (float*)&word;
            return *pFloat;
        }
        public abstract double Float64FromData(uint[] data, uint offset);
        public unsafe float Float16FromData(uint[] data, uint offset)
        {
            ushort word = (ushort)data[offset];

            if (word == 0) { return 0.0f; }
            else if (word == 0x8000) { return -0.0f; }

            bool negative = (word & 0x8000) != 0;
            int exponant = (word & 0x7C00) >> 10;
            int mantissa = (word & 0x03FF);

            if (exponant == 0x6)
            {
                if (mantissa == 0)
                {
                    return negative ? float.NegativeInfinity : float.PositiveInfinity;
                }
                else
                {
                    return float.NaN;
                }
            }
            else if (exponant == 0x0)
            {
                // Subnormal numbers
                return (float)((negative ? -1.0 : 1.0) * (double)mantissa * System.Math.Pow(2.0, -24));
            }

            return (float)((negative ? -1.0 : 1.0) * (double)(mantissa) * System.Math.Pow(2.0, exponant - 25) + System.Math.Pow(2.0, exponant - 15));
        }
    }
}
