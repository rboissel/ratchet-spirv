using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ratchet.Code
{
    /// <summary>
    /// Tools for SPIR-V manipulation.
    /// </summary>
    public static partial class SPIRV
    {
        public abstract class Item
        {
            protected uint _Id = 0;
            public uint Id { get { return _Id; } }

            protected string _Name = "";
            public string Name { get { return _Name; } set { _Name = value; } }


            protected Item() { _Id = 0; }
            protected Item(uint Id) { _Id = Id; }

            public override string ToString()
            {
                return "%" + _Id.ToString() + " " + _Name;
            }
        }

        public abstract class Type : Item
        {
            protected Type(uint Id) : base(Id) { }
            public abstract string TypeName { get; }
            public override string ToString()
            {
                return "%" + Id.ToString() + " Type<" + TypeName + ">";
            }
        }

        public class IntegerType : Type
        {
            bool _Signedness = false;
            public bool Signedness { get { return _Signedness; } }

            uint _Width = 1;
            public uint Width { get { return _Width; } }

            public IntegerType(uint Id, uint Width, bool Signedness) : base(Id) { _Signedness = Signedness; _Width = Width; }

            public override string TypeName { get { return ((!Signedness) ? "U" : "") + "Int" + _Width.ToString(); } }
        }

        public class FloatingPointType : Type
        {
            uint _Width = 1;
            public uint Width { get { return _Width; } }

            public FloatingPointType(uint Id, uint Width) : base(Id) { _Width = Width; }
            public override string TypeName { get { return "Float" + _Width.ToString(); } }
        }


        public class VoidType : Type
        {
            public VoidType(uint Id) : base(Id) { }
            public override string TypeName { get { return "Void"; } }
        }

        public class Constant<T> : Item
        {
            T _Value = default(T);
            public T Value { get { return _Value; } }

            Type _Type;
            Type Type { get { return _Type; } }

            public Constant(uint Id, T Value, Type Type) : base (Id) { _Value = Value; _Type = Type; }

            public override string ToString()
            {
                return "%" + Id + " " + _Type.TypeName + " " + _Value.ToString();
            }
        }

        public enum AddressingModel
        {
            LOGICAL = 0,
            PHYSICAL32 = 1,
            PHYSICAL64 = 2
        }

        public enum MemoryModel
        {
            SIMPLE = 0,
            GLSL450 = 1,
            OPENCL = 2
        }

        public enum Capability
        {
            MATRIX = 0,
            SHADER = 1,
            GEOMETRY = 2,
            TESSELLATION = 3,
            ADDRESS = 4,
            LINKAGE = 5,
            KERNEL = 6,
            VECTOR16 = 7,
            FLOAT16_BUFFER = 8,
            FLOAT16 = 9,
            FLOAT64 = 10,
            INT64 = 11,
            INT64_ATOMICS = 12,
            IMAGE_BASIC = 13,
            IMAGE_READ_WRITE = 14,
            IMAGE_MIPMAP = 15,
            PIPES = 17,
            GROUPS = 18,
            DEVICE_ENQUEUE = 19,
            LITERAL_SAMPLER = 20,
            ATOMIC_STORAGE = 21,
            INT16 = 22,
            TESSELLATION_POINT_SIZE = 23,
            GEOMETRY_POINT_SIZE = 24,
            IMAGE_GATHER_EXTENDED = 25,
            STORAGE_IMAGE_MULTISAMPLE = 27,
            UNIFORM_BUFFER_ARRAY_DYNAMIC_INDEXING_BLOCK = 28,
            SAMPLED_IMAGE_ARRAY_DYNAMIC_INDEXING = 29,
            STORAGE_BUFFER_ARRAY_DYNAMIC_INDEXING_BUFFER_BLOCK = 30,
            STORAGE_IMAGE_ARRAY_DYNAMIC_INDEXING = 31,
            CLIP_DISTANCE = 32,
            CULL_DISTANCE = 33,
            IMAGE_CUBE_ARRAY = 34,
            SAMPLE_RATE_SHADING = 35,
            IMAGE_RECT = 36,
            SAMPLED_RECT = 37,
            GENERIC_POINTER = 38,
            INT8 = 39,
            INPUTE_ATTACHMENT = 40,
            SPARSE_RESIDENCY = 41,
            MIN_LOD = 42,
            SAMPLED_1D = 43,
            IMAGE_1D = 44,
            SAMPLED_CUBE_ARRAY = 45,
            SAMPLED_BUFFER = 46,
            IMAGE_BUFFER = 47,
            IMAGE_MS_ARRAY = 48,
            STORAGE_IMAGE_EXTENDED_FORMATS = 49,
            IMAGE_QUERY = 50,
            DERIVATIVE_CONTROL = 51,
            INTERPOLATION_FUNCTION = 52,
            TRANSFORM_FEEDBACK = 53,
            GEOMETRY_STREAMS = 54,
            STORAGE_IMAGE_READ_WITHOUT_FORMAT = 55,
            STORAGE_IMAGE_WRITE_WITHOUT_FORMAT = 56,
            MULTI_VIEWPORT = 57,
            SUBGROUP_BALLOT_KHR = 4423,
            DRAW_PARAMETERS =4427,
            SUBGROUP_VOTE_KHR = 4431,
            STORAGE_BUFFER_16BIT_ACCESS = 4433,
            UNIFORM_AND_STORAGE_BUFFER_16BIT_ACCESS = 4434,
            STORAGE_PUSH_CONSTANT16 = 4435,
            STORAGE_INPUT_OUTPUT_16 = 4436,
            DEVICE_GROUP = 4437,
            MULTI_VIEW = 4439,
            VARIABLE_POINTERS_STORAGE_BUFFER = 4441,
            VARIABLE_POINTERS = 4442,
            ATOMIC_STORAGE_OPS = 4445,
            SAMPLE_MASK_POST_DEPTH_COVERAGE = 4447,
            IMAGE_GATHER_BIAS_LOD_AMD = 5009,
            FRAGMENT_MASK_AMD = 5010,
            STENCIL_EXPORT_EXT = 5013,
            IMAGE_READ_WRITE_LOD_AMD = 5015,
            SAMPLE_MASK_OVERRIDE_COVERAGE_NV = 5249,
            GEOMETRY_SHADER_PASSTROUGH_NV = 5251,
            SHADER_VIEWPORT_INDEX_LAYER_EXT = 5254,
            SHADER_VIEWPORT_INDEX_LAYER_NV = 5255,
            SHADER_STEREO_VIEW_NV = 5259,
            PER_VIEW_ATTRIBUTES = 5260,
            SUBGROUP_SHUFFLE_INTEL = 5568,
            SUBGROUP_BUFFER_BLOCK_IO_INTEL = 5569,
            SUBGROUP_IMAHE_BLOCK_IO_INTEL = 5570
        }

        /// <summary>
        /// Represent a SPIR-V Instruction incuding the opcode and the data associated with it.
        /// the data is stored as an object type use 'is' and 'as' to manipulate it.
        /// </summary>
        public class Instruction
        {
            internal OpCode _OpCode = OpCodes.Nop;
            internal int _Offset = -1;
            internal object _Data = null;

            /// <summary>
            /// Get the offset of the instruction in the source bytecode. If unknown it is set to -1.
            /// </summary>
            public int Offset { get { return _Offset; } }
            /// <summary>
            /// Get the OpCode associated to this instruction.
            /// </summary>
            public OpCode OpCode { get { return _OpCode; } set { _OpCode = value; } }
            /// <summary>
            /// Get the Data associated to this instruction. NULL if the opcode doesn't have any data associated with it.
            /// </summary>
            public object Data { get { return _Data; } set { _Data = value; } }

            public override string ToString() { return _OpCode.ToString(); }
        }

        /// <summary>
        /// Represent a SPIR-V Module
        /// </summary>
        public class Module
        {
            internal List<EntryPoint> _EntryPoints = new List<EntryPoint>();
            public List<EntryPoint> EntryPoints { get { return _EntryPoints; } }

            internal List<Capability> _Capabilities = new List<Capability>();
            public List<Capability> Capabilities { get { return _Capabilities; } }

            internal List<Item> _Items = new List<Item>();
            public List<Item> Items { get { return _Items; } }


            internal AddressingModel _AddressingModel = AddressingModel.LOGICAL;
            public AddressingModel AddressingModel { get { return _AddressingModel; } set { _AddressingModel = value; } }

            internal MemoryModel _MemoryModel = MemoryModel.SIMPLE;
            public MemoryModel MemoryModel { get { return _MemoryModel; } set { _MemoryModel = value; } }
        }


        class PendingEntryPoint : Item
        {
            public enum EntryPointExecutionModel
            {
                Vertex = 0,
                TessellationControl = 1,
                TessellationEvaluation = 2,
                Geometry = 3,
                Fragment = 4,
                GLCompute = 5
            }
            public EntryPointExecutionModel ExecutionModel;
            public string Name;
            public PendingEntryPoint(uint Id) : base(Id) { }
        }

        public static Module LoadFromFile(string File)
        {
            Module module = new Module();
            SPIRVModuleReader reader = SPIRVModuleReader.Create(System.IO.File.OpenRead(File));
            List<Instruction> instructions = new List<Instruction>();
            Function currentFunction = null;
            Dictionary<uint, Item> items = new Dictionary<uint, Item>();
            List<PendingEntryPoint> entryPoints = new List<PendingEntryPoint>();

            while (true)
            {
                Instruction instruction = reader.ReadInstruction();
                if (instruction == null) { break; }
                uint[] data = instruction.Data as uint[];

                switch (instruction.OpCode.OpCodeValue)
                {
                    case OpCodes.OpFunction:
                        {
                            if (data == null || data.Length != 4) { throw new Exception("Invalid Function opcode."); }
                            if (currentFunction != null) { throw new Exception("Invalid function declaration. Functions can't be neasted"); }

                            uint resultType = data[0];
                            uint id = data[1];
                            Function.FunctionControl control = (Function.FunctionControl)data[2];
                            uint functionType = data[0];
                            currentFunction = new Function(id);
                            currentFunction.Control = control;
                            if (items.ContainsKey(id)) { throw new Exception("The item " + id.ToString() + " has already been declared"); }
                            items.Add(id, currentFunction);
                        }
                        break;
                    case OpCodes.OpFunctionEnd:
                        {
                            if (currentFunction == null) { throw new Exception("Invalid FunctionEnd declaration. FunctionEnd must have a matching Function"); }
                            currentFunction = null;
                        }
                        break;
                    case OpCodes.OpEntryPoint:
                        {
                            PendingEntryPoint entryPoint = new PendingEntryPoint(data[1]);
                            entryPoint.ExecutionModel = (PendingEntryPoint.EntryPointExecutionModel)data[0];
                            uint dataOffset = 0;
                            entryPoint.Name = reader.LiteralStringFromData(data, 2, out dataOffset);
                            entryPoints.Add(entryPoint);
                        }
                        break;
                    case OpCodes.OpCapability:
                        {
                            if (data == null || data.Length != 1) { throw new Exception("Invalid Capability opcode."); }
                            module.Capabilities.Add((Ratchet.Code.SPIRV.Capability)data[0]);
                        }
                        break;
                    case OpCodes.OpMemoryModel:
                        {
                            if (data == null || data.Length != 2) { throw new Exception("Invalid MemoryModel opcode."); }
                            module._AddressingModel = (AddressingModel)data[0];
                            module._MemoryModel = (MemoryModel)data[1];
                        }
                        break;
                    case OpCodes.OpName:
                        {
                            if (data == null || data.Length < 2) { throw new Exception("Invalid Name opcode."); }
                            if (items.ContainsKey(data[0]))
                            {
                                uint offset = 0;
                                items[data[0]].Name = reader.LiteralStringFromData(data, 1, out offset);
                            }
                        }
                        break;
                    case OpCodes.OpTypeVoid:
                        {
                            if (data == null || data.Length != 1) { throw new Exception("Invalid TypeVoid opcode."); }
                            uint id = data[0];
                            if (items.ContainsKey(id)) { throw new Exception("The item " + id.ToString() + " has already been declared"); }
                            VoidType voidType = new VoidType(id);
                            items.Add(id, voidType);
                        }
                        break;
                    case OpCodes.OpTypeInt:
                        {
                            if (data == null || data.Length != 3) { throw new Exception("Invalid TypeInt opcode."); }
                            uint id = data[0];
                            uint width = data[1];
                            bool signedness = (data[2] != 0);

                            if (items.ContainsKey(id)) { throw new Exception("The item " + id.ToString() + " has already been declared"); }
                            IntegerType integerType = new IntegerType(id, width, signedness);
                            items.Add(id, integerType);
                        }
                        break;
                    case OpCodes.OpTypeFloat:
                        {
                            if (data == null || data.Length != 2) { throw new Exception("Invalid TypeInt opcode."); }
                            uint id = data[0];
                            uint width = data[1];

                            if (items.ContainsKey(id)) { throw new Exception("The item " + id.ToString() + " has already been declared"); }
                            FloatingPointType floatingPointType = new FloatingPointType(id, width);
                            items.Add(id, floatingPointType);
                        }
                        break;
                    case OpCodes.OpConstant:
                        {
                            if (data == null || data.Length < 2) { throw new Exception("Invalid TypeInt opcode."); }
                            uint id = data[1];
                            uint typeId = data[0];

                            if (items.ContainsKey(id)) { throw new Exception("The item " + id.ToString() + " has already been declared"); }
                            if (!items.ContainsKey(typeId)) { throw new Exception("The Type " + id.ToString() + " does not exist"); }
                            if (!(items[typeId] is Type)) { throw new Exception("The Item " + id.ToString() + " is not a type"); }

                            Type type = items[typeId] as Type;
                            if (type is IntegerType)
                            {
                                IntegerType integerType = (type as IntegerType);
                                
                                if (integerType.Signedness)
                                {
                                    if (integerType.Width == 8) { unchecked { items.Add(id, new Constant<sbyte>(id, (sbyte)data[2], type as IntegerType)); } }
                                    else if (integerType.Width == 16) { unchecked { items.Add(id, new Constant<short>(id, (short)data[2], type as IntegerType)); } }
                                    else if (integerType.Width == 32) { unchecked { items.Add(id, new Constant<int>(id, (int)data[2], type as IntegerType)); } }
                                    else { throw new Exception("Invalid Integer Type"); }
                                }
                                else
                                {
                                    if (integerType.Width == 8) { unchecked { items.Add(id, new Constant<byte>(id, (byte)data[2], type as IntegerType)); } }
                                    else if (integerType.Width == 16) { unchecked { items.Add(id, new Constant<ushort>(id, (ushort)data[2], type as IntegerType)); } }
                                    else if (integerType.Width == 32) { unchecked { items.Add(id, new Constant<uint>(id, (uint)data[2], type as IntegerType)); } }
                                    else { throw new Exception("Invalid Integer Type"); }
                                }
                            }
                            else if (type is FloatingPointType)
                            {
                                throw new Exception("Invalid Floating Point Type");
                            }
                        }
                        break;
                    default:
                        if (currentFunction != null)
                        {
                            currentFunction._Instructions.Add(instruction);
                        }
                        else
                        {
                            instructions.Add(instruction);
                        }
                        break;
                }
            }

            if (currentFunction != null) { throw new Exception("Missing FunctionEnd"); }

            foreach (Item item in items.Values)
            {
                module._Items.Add(item);
            }

            foreach (PendingEntryPoint pendingEntryPoint in entryPoints)
            {
                if (!items.ContainsKey(pendingEntryPoint.Id)) { throw new Exception("Invalid entry point declation. The Id " + pendingEntryPoint.Id + " is not declared"); }
                if (!(items[pendingEntryPoint.Id] is Function)) { throw new Exception("Invalid entry point declation. The Id " + pendingEntryPoint.Id + " is not a function"); }

                Function function = items[pendingEntryPoint.Id] as Function;
                if (function.Name == "") { function.Name = pendingEntryPoint.Name; }
                EntryPoint finalEntryPoint = null;
                switch (pendingEntryPoint.ExecutionModel)
                {
                    case PendingEntryPoint.EntryPointExecutionModel.Fragment:
                        finalEntryPoint = new Fragment(function, pendingEntryPoint.Name);
                        break;
                }
                module._EntryPoints.Add(finalEntryPoint);
            }

            return module;
        }

        public class Function : Item
        {
            public enum FunctionControl
            {
                NONE = 0,
                /// <summary>
                /// Strong request, to the extent possible, to inline the function.
                /// </summary>
                INLINE = 1,
                /// <summary>
                /// Strong request, to the extent possible, to not inline the function.
                /// </summary>
                DONT_INLINE = 2,
                /// <summary>
                /// Compiler can assume this function has no side effect, but might read global memory
                /// or read through dereferenced function parameters. Always computes the same
                /// result for the same argument values.
                /// </summary>
                PURE = 4,
                /// <summary>
                /// Compiler can assume this function has no side effect, and will not access global memory
                /// or dereferenced function parameters. Always computes the same
                /// result for the same argument values.
                /// </summary>
                CONST = 8
            }

            internal FunctionControl _Control = FunctionControl.NONE;
            public FunctionControl Control { get { return _Control; } set { _Control = value; } }


            internal List<Instruction> _Instructions = new List<Instruction>();
            public List<Instruction> Instructions { get { return _Instructions; } }

            internal Function(uint Id) : base(Id) { }
        }

        public class EntryPoint        {
            protected Function _Function;
            public Function Function { get { return _Function; } }
            protected string _Name;
            public string Name { get { return _Name; } }
            protected EntryPoint(Function Function, string Name) { _Function = Function; _Name = Name; }
        }

        public class Fragment : EntryPoint
        {
            public Fragment(Function Function, string Name) : base(Function, Name) { }
        }

        public class Geometry : EntryPoint
        {
            public Geometry(Function Function, string Name) : base(Function, Name) { }
        }

        public class TessellationEvaluation : EntryPoint
        {
            public TessellationEvaluation(Function Function, string Name) : base(Function, Name) { }
        }

        public class TessellationControl : EntryPoint
        {
            public TessellationControl(Function Function, string Name) : base(Function, Name) { }
        }

        public class Vertex : EntryPoint
        {
            public Vertex(Function Function, string Name) : base(Function, Name) { }
        }

        public class GLCompute : EntryPoint
        {
            public GLCompute(Function Function, string Name) : base(Function, Name) { }
        }

        public class Kernel : EntryPoint
        {
            public Kernel(Function Function, string Name) : base(Function, Name) { }
        }
    }
}
