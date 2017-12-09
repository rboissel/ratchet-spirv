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

            List<Decoration> _Decorations = new List<SPIRV.Decoration>();
            public List<Decoration> Decorations { get { return _Decorations; } }
            protected Item() { _Id = 0; }
            protected Item(uint Id) { _Id = Id; }

            public override string ToString()
            {
                return "%" + _Id.ToString() + " " + _Name;
            }
        }

        public class Variable : Item
        {
            Builtin _Builtin = Builtin.NONE;
            public Builtin Builtin { get { return _Builtin; } set { _Builtin = value; } }
            PointerType _Type = null;
            public PointerType Type { get { return _Type; } }
            Item _Initializer = null;
            public Item Initializer { get { return _Initializer; } }
            StorageClass _StorageClass = StorageClass.PRIVATE;
            public StorageClass StorageClass { get { return _StorageClass; } }

            public Variable(uint Id, StorageClass StorageClass, PointerType Type, Item Initializer) : base(Id) { _Type = Type; _Initializer = Initializer; _StorageClass = StorageClass; }

            public override string ToString()
            {
                if (_Initializer == null) { return _Type.TypeName + " %" + Id.ToString(); }
                else { return _Type.TypeName + " %" + Id.ToString() + " = " + _Initializer.ToString(); }
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

        public class VectorType : Type
        {
            Type _Type;
            public Type Type { get { return _Type; } }
            uint _ComponentCount = 2;
            public uint ComponentCount { get { return _ComponentCount; } }
            public VectorType(uint Id, Type Type, uint ComponentCount) : base(Id) { _ComponentCount = ComponentCount; _Type = Type; }
            public override string TypeName { get { return "Vector" + _ComponentCount.ToString() + "<" + _Type.TypeName + ">"; } }
        }

        public class ArrayType : Type
        {
            Type _Type;
            public Type Type { get { return _Type; } }
            Item _Length = null;
            public Item Length { get { return _Length; } }
            public ArrayType(uint Id, Type Type, Item Length) : base(Id) { _Length = Length; _Type = Type; }
            public override string TypeName { get { return _Type.TypeName  + "[" + _Length.ToString() + "]"; } }
        }

        public class MatrixType : Type
        {
            Type _Type;
            public Type Type { get { return _Type; } }
            uint _Row = 0;
            public uint RowCount { get { return _Row; } }
            uint _Col = 0;
            public uint ColCount { get { return _Col; } }

            public MatrixType(uint Id, uint RowCount, uint ColCount, Type Type) : base(Id) { _Row = RowCount; _Col = ColCount; _Type = Type; }
            public override string TypeName { get { return _Type.TypeName + "[" + _Row.ToString() + ", " + _Col.ToString() + "]"; } }
        }

        public class Parameter
        {
            Type _Type;
            public Type Type { get { return _Type; } }
            protected string _Name = "";
            public string Name { get { return _Name; } set { _Name = value; } }

            public Parameter(Type Type) { _Type = Type; }
        }

        public class FunctionType : Type
        {
            Type _ReturnType;
            public Type ReturnType { get { return _ReturnType; } }
            Parameter[] _Parameters;
            public Parameter[] Parameters { get { return _Parameters; } }
            public FunctionType(uint Id, Type ReturnType, Parameter[] Parameters) : base(Id) { _ReturnType = ReturnType; _Parameters = Parameters; }
            public override string TypeName { get { return _ReturnType.TypeName + " %" + Id.ToString() + " (...)"; } }
        }

        public class VoidType : Type
        {
            public VoidType(uint Id) : base(Id) { }
            public override string TypeName { get { return "Void"; } }
        }

        public class Field
        {
            List<Decoration> _Decorations = new List<SPIRV.Decoration>();
            public List<Decoration> Decorations { get { return _Decorations; } }
            Builtin _Builtin = Builtin.NONE;
            public Builtin Builtin { get { return _Builtin; } set { _Builtin = value; } }
            Type _Type;
            public Type Type { get { return _Type; } }
            protected string _Name = "";
            public string Name { get { return _Name; } set { _Name = value; } }

            public Field(Type Type) { _Type = Type; }
        }

        public class StructType : Type
        {
            Field[] _Fields;
            public Field[] Fields { get { return _Fields; } }
            public StructType(uint Id, Field[] Fields) : base(Id) { _Fields = Fields; }
            public override string TypeName { get { return "Struct" + ((_Name != "") ? " " + _Name : ""); } }
        }


        public class PointerType : Type
        {
            Type _Type;
            public Type Type { get { return _Type; } }
            StorageClass _StorageClass = StorageClass.UNIFORM_CONSTANT;
            public StorageClass StorageClass { get { return _StorageClass; } }

            public PointerType(uint Id, Type Type, StorageClass StorageClass) : base(Id) { _StorageClass = StorageClass; _Type = Type; }
            public override string TypeName { get { return "(" + _StorageClass.ToString() + ") (" + _Type.TypeName + ") *" ; } }
        }

        public class GenericConstant : Item
        {
            internal virtual object GetValue() { return null; }
            public GenericConstant(uint Id) : base (Id) { }

        }

        public class Constant<T> : GenericConstant
        {
            T _Value = default(T);
            public T Value { get { return _Value; } }
            internal override object GetValue() { return _Value; }


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

        public enum MemoryAccess
        {
            NONE = 0,
            VOLATILE = 1,
            ALIGNED = 2,
            NONTEMPORAL = 4
        }

        public enum Builtin
        {
            POSITION=0,
            POINT_SIZE=1,
            CLIP_DISTANCE=2,
            CULL_DISTANCE=4,
            VERTEX_ID=5,
            INSTANCE_ID=6,
            PRIMITIVE_ID=7,
            INVOCATION_ID=8,
            LAYER=9,
            VIEWPORT_INDEX=10,
            TESS_LEVEL_OUTER=11,
            TESS_LEVEL_INNER=12,
            TESS_COORD = 13,
            PATCH_VERTICES=14,
            FRAG_COORD=15,
            POINT_COORD=16,
            FRONT_FACING=17,
            SAMPLE_ID=18,
            SAMPLE_POSITION=19,
            SAMPLE_MASK=20,
            FRAG_DEPTH=22,
            HELPER_INVOCATION=23,
            NUM_WORK_GROUP=24,
            WORK_GROUP_SIZE=25,
            WORK_GROUP_ID=26,
            LOCAL_INVOCATION_ID=27,
            GLOBAL_INVOCATION_ID=28,
            LOCAL_INVOCATION_IDEX=29,
            WORK_DIM=30,
            GLOBAL_SIZE=31, 
            ENQUEUED_WORK_GROUP_SIZE=32,
            GLOBAL_OFFSET=33,
            GLOBAL_LINEAR_ID=34,
            SUBGROUP_SIZE=36,
            SUBGROUP_MAX_SIZE=37,
            NUM_SUBGROUPS=38,
            NUM_ENQUEUED_SUBGROUP=39,
            SUBGROUP_ID=40,
            SUBGROUP_LOCAL_INVOCATION_ID=41,
            VERTEX_INDEX=42,
            INSTANCE_INDEX=43,
            SUBGROUP_EQ_MASK_KHR=4416,
            SUBGROUP_GE_MASK_KHR = 4417,
            SUBGROUP_GT_MASK_KHR = 4418,
            SUBGROUP_LE_MASK_KHR = 4419,
            SUBGROUP_LT_MASK_KHR = 4420,
            BASE_VERTEX=4424,
            BASE_INSTANCE=4425,
            DRAW_INDEX=4426,
            DEVICE_INDEX=4438,
            VIEW_INDEX=4440,
            BARY_COORD_NO_PERSP_AMD=4992,
            BARY_COORD_NO_PERSP_CENTROID_AMD = 4993,
            BARY_COORD_NO_PERSP_SAMPLE_AMD = 4994,
            BARY_COORD_NO_PERSP_SMOOTH_AMD = 4995,
            BARY_COORD_NO_PERSP_SMOOTH_CENTROID_AMD = 4996,
            BARY_COORD_NO_PERSP_SMOOTH_SAMPLE_AMD = 4997,
            BARY_COORD_NO_PERSP_PULL_MODEL_AMD = 4998,
            FRAG_STENCIL_REF_EXT = 5014,
            VIEWPORT_MASK_NV = 5253,
            SECONDARY_POSITION_NV = 5257,
            SECONDARY_VIEWPORT_MASK_NV= 5258,
            POSITION_PER_VIEW_NV = 5261,
            VIEWPORT_MASK_PER_VIEW_NV = 5262,


            NONE = 0xFFFF
        }

        public enum Decoration
        {
            RELAXED_PRECISION=0,
            SPEC_ID=1,
            BLOCK = 2,
            BUFFER_BLOCK=3,
            ROW_MAJOR=4,
            COL_MAJOR=5,
            ARRAY_STRIDE=6,
            MATRIX_STRIDE=7,
            GLSL_SHARED=8,
            GLSL_PACKED=9,
            C_PACKED=10,
            BUILTIN=11,
            NO_PERSPECTIVE=13,
            FLAT=14,
            PATCH=15,
            CENTROID=16,
            SAMPLE=17,
            INVARIANT=18,
            RESTRICT=19,
            ALIASED = 20,
            VOLATILE=21,
            CONSTANT=22,
            COHERENT=23,
            NON_WRITABLE=24,
            NON_READABLE=25,
            UNIFORM=26,
            SATURATED_CONVERSION=28,
            STREAM=29,
            LOCATION=30,
            COMPONENT=31,
            INDEX=32,
            BINDING=33,
            DESCRIPTOR_SET=34,
            OFFSET=35,
            XFB_BUFFER=36,
            XFB_STRIDE=37,
            FUNC_PARAM_ATTR=38,
            FP_ROUNDING_MODE=39,
            FP_FAST_MATH_MODE=40,
            LINKAGE_ATTRIBUTES=41,
            NO_CONTRACTION=42,
            INPUT_ATTACHMENT_INDEX=43,
            ALIGNEMENT=44,
            EXPLICIT_INTERP_AMD=4999,
            OVERRIDE_COVERAGE_NV=5248,
            PASSTHROUGH_NV=5250,
            VIEWPORT_RELATIVE_NV=5252,
            SECONDARY_VIEWPORT_RELATIVE_NV=5256
        }

        public enum StorageClass
        {
            UNIFORM_CONSTANT = 0,
            INPUT = 1,
            UNIFORM = 2,
            OUTPUT = 3,
            WORKGROUP = 4,
            CROSS_WORKGROUP = 5,
            PRIVATE = 6,
            FUNCTION = 7,
            GENERIC = 8,
            PUSH_CONSTANT = 9,
            ATOMIC_COUNTER = 10,
            IMAGE = 11,
            STORAGE_BUFFER = 12
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
            return Load(System.IO.File.OpenRead(File));
        }

        public static Module LoadFromBytes(byte[] Data)
        {
            return Load(new System.IO.MemoryStream(Data));
        }

        public static Module Load(System.IO.Stream Stream)
        {
            Module module = new Module();
            SPIRVModuleReader reader = SPIRVModuleReader.Create(Stream);
            List<Instruction> instructions = new List<Instruction>();
            Dictionary<uint, string> names = new Dictionary<uint, string>();
            Dictionary<uint, Dictionary<uint, string>> memberNames = new Dictionary<uint, Dictionary<uint, string>>();
            List<Instruction> decorations = new List<Instruction>();

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

                            uint resultTypeId = data[0];
                            uint id = data[1];
                            uint functionTypeId = data[3];

                            if (!items.ContainsKey(functionTypeId)) { throw new Exception("The Type " + id.ToString() + " does not exist"); }
                            if (!(items[functionTypeId] is FunctionType)) { throw new Exception("The Item " + id.ToString() + " is not a function type"); }

                            Function.FunctionControl control = (Function.FunctionControl)data[2];
                            uint functionType = data[0];
                            currentFunction = new Function(id, control, (items[functionTypeId] as FunctionType));
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
                            uint offset = 0;

                            if (data == null || data.Length < 2) { throw new Exception("Invalid Name opcode."); }
                            if (!names.ContainsKey(data[0]))
                            {
                                names.Add(data[0], reader.LiteralStringFromData(data, 1, out offset));
                            }
                        }
                        break;
                    case OpCodes.OpMemberName:
                        {
                            if (data == null || data.Length < 3) { throw new Exception("Invalid MemberName opcode."); }

                            uint structId = data[0];
                            uint memberNameId = data[1];
                            uint offset = 0;
                            if (!memberNames.ContainsKey(structId))
                            {
                                memberNames.Add(structId, new Dictionary<uint, string>());
                            }

                            if (!memberNames[structId].ContainsKey(memberNameId))
                            {
                                memberNames[structId].Add(memberNameId, reader.LiteralStringFromData(data, 2, out offset));
                            }
                        }
                        break;
                    case OpCodes.OpTypePointer:
                        {
                            if (data == null || data.Length != 3) { throw new Exception("Invalid TypeVoid opcode."); }
                            uint id = data[0];
                            StorageClass storageClass = (StorageClass)data[1];
                            uint typeId = data[2];

                            if (items.ContainsKey(id)) { throw new Exception("The item " + id.ToString() + " has already been declared"); }
                            if (!items.ContainsKey(typeId)) { throw new Exception("The Type " + id.ToString() + " does not exist"); }
                            if (!(items[typeId] is Type)) { throw new Exception("The Item " + id.ToString() + " is not a type"); }

                            PointerType pointerType = new PointerType(id, (items[typeId] as Type), storageClass);
                            items.Add(id, pointerType);
                        }
                        break;
                    case OpCodes.OpTypeArray:
                        {
                            if (data == null || data.Length != 3) { throw new Exception("Invalid TypeVoid opcode."); }
                            uint id = data[0];
                            if (items.ContainsKey(id)) { throw new Exception("The item " + id.ToString() + " has already been declared"); }
                            uint lengthId = data[2];
                            if (!items.ContainsKey(lengthId)) { throw new Exception("The Item " + id.ToString() + " does not exist"); }
                            uint typeId = data[1];
                            if (!items.ContainsKey(typeId)) { throw new Exception("The Type " + id.ToString() + " does not exist"); }


                            ArrayType arrayType = new ArrayType(id, items[typeId] as Type, items[lengthId]);
                            items.Add(id, arrayType);
                        }
                        break;
                    case OpCodes.OpTypeMatrix:
                        {
                            if (data == null || data.Length != 3) { throw new Exception("Invalid TypeVoid opcode."); }
                            uint id = data[0];
                            if (items.ContainsKey(id)) { throw new Exception("The item " + id.ToString() + " has already been declared"); }
                            uint colTypeId = data[1];

                            if (!items.ContainsKey(colTypeId)) { throw new Exception("The Item " + id.ToString() + " does not exist"); }
                            if (!(items[colTypeId] is VectorType)) { throw new Exception("The Item " + id.ToString() + " does not represent a vector type"); }
                            VectorType colType = items[colTypeId] as VectorType;

                            uint colCount = data[2];

                            MatrixType matrixType = new MatrixType(id, colType.ComponentCount, colCount, colType.Type);
                            items.Add(id, matrixType);
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
                    case OpCodes.OpTypeVector:
                        {
                            if (data == null || data.Length != 3) { throw new Exception("Invalid TypeVoid opcode."); }
                            uint id = data[0];
                            if (items.ContainsKey(id)) { throw new Exception("The item " + id.ToString() + " has already been declared"); }
                            uint count = data[2];
                            if (count < 2) { throw new Exception("A vector must have at least 2 components"); }
                            uint typeId = data[1];
                            if (!items.ContainsKey(typeId)) { throw new Exception("The Type " + id.ToString() + " does not exist"); }

                            VectorType vectorType = new VectorType(id, (items[typeId] as Type), count);
                            items.Add(id, vectorType);
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
                    case OpCodes.OpTypeFunction:
                        {
                            if (data == null || data.Length < 2) { throw new Exception("Invalid TypeFunction opcode."); }
                            uint id = data[0];
                            uint returnTypeId = data[1];
                            if (!items.ContainsKey(returnTypeId)) { throw new Exception("The item " + returnTypeId.ToString() + " has not been declared"); }
                            if (!(items[returnTypeId] is Type)) { throw new Exception("The item " + returnTypeId.ToString() + " is not a type"); }
                            Type returnType = items[returnTypeId] as Type;

                            List<Parameter> parameters = new List<Parameter>();
                            for (int n = 2; n < data.Length; n++)
                            {
                                uint paramType = data[n];
                                if (!items.ContainsKey(paramType)) { throw new Exception("The item " + paramType.ToString() + " has not been declared"); }
                                if (!(items[paramType] is Type)) { throw new Exception("The item " + paramType.ToString() + " is not a type"); }

                                parameters.Add(new Parameter(items[paramType] as Type));
                            }

                            items.Add(id, new FunctionType(id, returnType, parameters.ToArray()));
                        }
                        break;
                    case OpCodes.OpTypeStruct:
                        {
                            if (data == null || data.Length < 2) { throw new Exception("Invalid TypeStruct opcode."); }
                            uint id = data[0];

                            List<Field> fields = new List<Field>();
                            for (int n = 1; n < data.Length; n++)
                            {
                                uint fieldType = data[n];
                                if (!items.ContainsKey(fieldType)) { throw new Exception("The item " + fieldType.ToString() + " has not been declared"); }
                                if (!(items[fieldType] is Type)) { throw new Exception("The item " + fieldType.ToString() + " is not a type"); }

                                fields.Add(new Field(items[fieldType] as Type));
                            }

                            items.Add(id, new StructType(id, fields.ToArray()));
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
                                    if (integerType.Width == 8) { unchecked { items.Add(id, new Constant<sbyte>(id, (sbyte)data[2], type)); } }
                                    else if (integerType.Width == 16) { unchecked { items.Add(id, new Constant<short>(id, (short)data[2], type)); } }
                                    else if (integerType.Width == 32) { unchecked { items.Add(id, new Constant<int>(id, (int)data[2], type)); } }
                                    else { throw new Exception("Not supported Integer Type"); }
                                }
                                else
                                {
                                    if (integerType.Width == 8) { unchecked { items.Add(id, new Constant<byte>(id, (byte)data[2], type)); } }
                                    else if (integerType.Width == 16) { unchecked { items.Add(id, new Constant<ushort>(id, (ushort)data[2], type)); } }
                                    else if (integerType.Width == 32) { unchecked { items.Add(id, new Constant<uint>(id, (uint)data[2], type)); } }
                                    else { throw new Exception("Not supported Integer Type"); }
                                }
                            }
                            else if (type is FloatingPointType)
                            {
                                FloatingPointType  floatingPointType = (type as FloatingPointType);
                                if (floatingPointType.Width == 16) { items.Add(id, new Constant<float>(id, reader.Float16FromData(data, 2), type)); }
                                else if (floatingPointType.Width == 32) { items.Add(id, new Constant<float>(id, reader.Float32FromData(data, 2), type)); }
                                else if (floatingPointType.Width == 64) { items.Add(id, new Constant<double>(id, reader.Float64FromData(data, 2), type)); }
                                else { throw new Exception("Not supported Floating Point Type"); }
                            }
                        }
                        break;
                    case OpCodes.OpConstantComposite:
                        {
                            uint id = data[1];
                            uint typeId = data[0];

                            if (items.ContainsKey(id)) { throw new Exception("The item " + id.ToString() + " has already been declared"); }
                            if (!items.ContainsKey(typeId)) { throw new Exception("The Type " + id.ToString() + " does not exist"); }
                            if (!(items[typeId] is Type)) { throw new Exception("The Item " + id.ToString() + " is not a type"); }

                            if (items[typeId] is VectorType)
                            {
                                VectorType type = items[typeId] as VectorType;
                                if (data.Length - 2 != type.ComponentCount) { throw new Exception("Missing element in the vector declatarion"); }
                                if (type.Type is FloatingPointType)
                                {
                                    uint n = 0;
                                    FloatingPointType fpointtype = (type.Type as FloatingPointType);
                                    switch (fpointtype.Width)
                                    {
                                        case 32:
                                        case 16:
                                            {
                                                float[] array = new float[type.ComponentCount];
                                                while (n < type.ComponentCount)
                                                {
                                                    if (!items.ContainsKey(data[n + 2])) { throw new Exception("Invalid id " + data[n + 2] + " the item is not yet declared"); }
                                                    if (!(items[data[n + 2]] is Constant<float>)) { throw new Exception("%" + data[n + 2] + " is not a valid constant"); }
                                                    array[n] = (items[data[n + 2]] as Constant<float>).Value;
                                                    n++;
                                                }
                                                items.Add(id, new Constant<float[]>(id, array, type));
                                                break;
                                            }
                                        case 64:
                                            {
                                                double[] array = new double[type.ComponentCount];
                                                while (n < type.ComponentCount)
                                                {
                                                    if (!items.ContainsKey(data[n + 2])) { throw new Exception("Invalid id " + data[n + 2] + " the item is not yet declared"); }
                                                    if (!(items[data[n + 2]] is Constant<double>)) { throw new Exception("%" + data[n + 2] + " is not a valid constant"); }
                                                    array[n] = (items[data[n + 2]] as Constant<double>).Value;
                                                    n++;
                                                }
                                                items.Add(id, new Constant<double[]>(id, array, type));
                                                break;
                                            }
                                    }

                                }
                            }
                            else if (items[typeId] is ArrayType)
                            {
                                ArrayType type = items[typeId] as ArrayType;
                                uint length = 0;
                                if (type.Length is Constant<UInt32>) { length = (type.Length as Constant<UInt32>).Value; }
                                else if (type.Length is Constant<Int32>) { length = (uint)(type.Length as Constant<Int32>).Value; }
                                else if (type.Length is Constant<UInt16>) { length = (uint)(type.Length as Constant<UInt16>).Value; }
                                else if (type.Length is Constant<Int16>) { length = (uint)(type.Length as Constant<Int16>).Value; }
                                else if (type.Length is Constant<UInt64>) { length = (uint)(type.Length as Constant<UInt16>).Value; }
                                else if (type.Length is Constant<Int64>) { length = (uint)(type.Length as Constant<Int16>).Value; }
                                else { throw new Exception("can't evalutate the length of the array at load time to ddeclare the constant " + id.ToString()); }


                                object[] array = new object[length];
                                for (uint n = 0; n < length; n++)
                                {
                                    if (!items.ContainsKey(data[n + 2])) { throw new Exception("Invalid id " + data[n + 2] + " the item is not yet declared"); }
                                    if (!(items[data[n + 2]] is GenericConstant)) { throw new Exception("%" + data[n + 2] + " is not a valid constant"); }

                                    array[n] = (items[data[n + 2]] as GenericConstant).GetValue();
                                }
                                items.Add(id, new Constant<object[]>(id, array, type));
                                break;
                            }
                            else if (items[typeId] is MatrixType)
                            {
                                MatrixType type = items[typeId] as MatrixType;
                                object[,] matrix = new object[type.ColCount, type.RowCount];
                                for (uint c = 0; c < type.ColCount; c++)
                                {
                                    if (!items.ContainsKey(data[c + 2])) { throw new Exception("Invalid id " + data[c + 2] + " the item is not yet declared"); }
                                    if (!(items[data[c + 2]] is GenericConstant)) { throw new Exception("%" + data[c + 2] + " is not a valid constant"); }
                                    GenericConstant genericConstant = (items[data[c + 2]] as GenericConstant);
                                    object vectorObj = genericConstant.GetValue();
                                    if (!(vectorObj is System.Array)) { throw new Exception("%" + data[c + 2] + " is not a valid vector"); }
                                    Array vectorAsArray = (vectorObj as System.Array);
                                    if (vectorAsArray.Length != type.RowCount) { throw new Exception("%" + data[c + 2] + " has not the valid dim"); }
                                    for (int r = 0; r < type.RowCount; r++)
                                    {
                                        matrix[c, r] = vectorAsArray.GetValue(r);
                                    }
                                }
                                items.Add(id, new Constant<object[,]>(id, matrix, type));
                                break;
                            }
                            else { throw new Exception("Invalid type for a a composite constant"); }
                        }
                        break;
                    case OpCodes.OpVariable:
                        {
                            if (data.Length < 3) { throw new Exception("Invalid opcode OpVariable"); }

                            uint id = data[1];
                            uint typeId = data[0];
                            StorageClass storageClass = (StorageClass)data[2];

                            if (items.ContainsKey(id)) { throw new Exception("The item " + id.ToString() + " has already been declared"); }
                            if (!items.ContainsKey(typeId)) { throw new Exception("The Type " + id.ToString() + " does not exist"); }
                            if (!(items[typeId] is PointerType)) { throw new Exception("The Item " + id.ToString() + " is not a pointer type"); }

                            if (data.Length == 3) { items.Add(id, new Variable(id, storageClass, (items[typeId] as PointerType), null)); }
                            else { items.Add(id, new Variable(id, storageClass, (items[typeId] as PointerType), items[data[4]])); }
                        }
                        break;
                    case OpCodes.OpDecorate:
                        if (data.Length < 2) { throw new Exception("Invalid opcode OpDecorate"); }
                        decorations.Add(instruction);
                        break;
                    case OpCodes.OpMemberDecorate:
                        if (data.Length < 3) { throw new Exception("Invalid opcode OpMemberDecorate"); }
                        decorations.Add(instruction);
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

            foreach (KeyValuePair<uint, string> name in names)
            {
                if (items.ContainsKey(name.Key)) { items[name.Key].Name = name.Value; }
            }


            foreach (KeyValuePair<uint, Dictionary<uint, string>> structnames in memberNames)
            {
                if (items.ContainsKey(structnames.Key))
                {
                    if (items[structnames.Key] is StructType)
                    {
                        StructType structtype = items[structnames.Key] as StructType;
                        foreach (KeyValuePair<uint, string> name in structnames.Value)
                        {
                            if (structtype.Fields.Length < name.Key) { continue; }
                            structtype.Fields[name.Key].Name = name.Value;
                        }
                    }
                }
            }

            foreach (Instruction decoration in decorations)
            {
                uint[] data = decoration.Data as uint[];
                Decoration decorationFlag;
                Item target;
                uint targetid = data[0];
                if (!items.ContainsKey(targetid)) { throw new Exception("The item " + targetid.ToString() + " has not been declared"); }
                target = items[targetid];

                if (decoration.OpCode.OpCodeValue == OpCodes.OpMemberDecorate)
                {
                    uint fieldid = data[1];

                    if (target is StructType)
                    {
                        StructType structure = (target as StructType);
                        if (fieldid >= structure.Fields.Length) { throw new Exception("The specified field in the structure does not exist"); }
                        decorationFlag = (Decoration)data[2];
                        if (decorationFlag == Decoration.BUILTIN)
                        {
                            if (data.Length < 4) { throw new Exception("Invalid builtin decoration"); }
                            Builtin buitin = (Builtin)data[3];
                            structure.Fields[fieldid].Builtin = buitin;
                        }
                        else
                        {
                            structure.Fields[fieldid].Decorations.Add(decorationFlag);
                        }
                    }
                    else { throw new Exception("Invalid target for the decoration"); }
                }
                else if (decoration.OpCode.OpCodeValue == OpCodes.OpDecorate)
                {
                    decorationFlag = (Decoration)data[1];
                    if (decorationFlag == Decoration.BUILTIN)
                    {
                        if (data.Length < 3) { throw new Exception("Invalid builtin decoration"); }
                        Builtin buitin = (Builtin)data[2];
                        if (target is Variable) { (target as Variable).Builtin = buitin; }
                        else { throw new Exception("Invalid target for the builtin decoration"); }
                    }
                    else
                    {
                        target.Decorations.Add(decorationFlag);
                    }
                }
                else { throw new Exception("Invalid decoration"); }


            }

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
                    case PendingEntryPoint.EntryPointExecutionModel.Vertex:
                        finalEntryPoint = new Vertex(function, pendingEntryPoint.Name);
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

            Type _Type;
            public Type Type { get { return _Type; } }

            internal Function(uint Id, FunctionControl Control, Type FunctionType) : base(Id) { _Control = Control; _Type = FunctionType; }
        }

        public class EntryPoint
        {
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
