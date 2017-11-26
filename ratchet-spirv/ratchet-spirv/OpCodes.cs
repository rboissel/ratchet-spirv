using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ratchet.Code
{
    public static partial class SPIRV
    {
        public struct OpCode
        {
            public int WordCount;
            public int OpCodeValue;
            public string Name;
            public override string ToString() { return Name; }
        }

        public static class OpCodes
        {
            internal const int OpName = 5;
            internal const int OpMemberName = 6;
            internal const int OpMemoryModel = 14;
            internal const int OpEntryPoint = 15;
            internal const int OpCapability = 17;
            internal const int OpTypeVoid = 19;
            internal const int OpTypeInt = 21;
            internal const int OpTypeFloat = 22;
            internal const int OpTypeVector = 23;
            internal const int OpTypeArray = 28;
            internal const int OpTypeStruct = 30;
            internal const int OpTypePointer = 32;
            internal const int OpTypeFunction = 33;
            internal const int OpConstant = 43;
            internal const int OpConstantComposite = 44;
            internal const int OpFunction = 54;
            internal const int OpFunctionEnd = 56;
            internal const int OpVariable = 59;
            internal const int OpDecorate = 71;
            internal const int OpMemberDecorate = 72;


            public static OpCode Nop = new OpCode { WordCount = 1, OpCodeValue = 0, Name = "Nop" };
            public static OpCode Undef = new OpCode { WordCount = 3, OpCodeValue = 1, Name = "Undef" };
            public static OpCode SourceContinued = new OpCode { WordCount = 2, OpCodeValue = 2, Name = "SourceContinued" };
            public static OpCode Source = new OpCode { WordCount = 3, OpCodeValue = 3, Name = "Source" };
            public static OpCode SourceExtension = new OpCode { WordCount = 2, OpCodeValue = 4, Name = "SourceExtension" };
            public static OpCode Name = new OpCode { WordCount = 3, OpCodeValue = OpName, Name = "Name" };
            public static OpCode MemberName = new OpCode { WordCount = 4, OpCodeValue = OpMemberName, Name = "MemberName" };
            public static OpCode String = new OpCode { WordCount = 3, OpCodeValue = 7, Name = "String" };
            public static OpCode Line = new OpCode { WordCount = 4, OpCodeValue = 8, Name = "Line" };
            public static OpCode NoLine = new OpCode { WordCount = 1, OpCodeValue = 317, Name = "NoLine" };
            public static OpCode Decorate = new OpCode { WordCount = 3, OpCodeValue = OpDecorate, Name = "Decorate" };
            public static OpCode MemberDecorate = new OpCode { WordCount = 4, OpCodeValue = OpMemberDecorate, Name = "MemberDecorate" };
            public static OpCode DecorationGroup = new OpCode { WordCount = 2, OpCodeValue = 73, Name = "DecorationGroup" };
            public static OpCode GroupDecorate = new OpCode { WordCount = 2, OpCodeValue = 74, Name = "GroupDecorate" };
            public static OpCode GroupMemberDecorate = new OpCode { WordCount = 2, OpCodeValue = 75, Name = "GroupMemberDecorate" };
            public static OpCode Extension = new OpCode { WordCount = 2, OpCodeValue = 10, Name = "Extension" };
            public static OpCode ExtInstImport = new OpCode { WordCount = 3, OpCodeValue = 11, Name = "ExtInstImport" };
            public static OpCode ExtInst = new OpCode { WordCount = 5, OpCodeValue = 12, Name = "ExtInst" };
            public static OpCode MemoryModel = new OpCode { WordCount = 3, OpCodeValue = OpMemoryModel, Name = "MemoryModel" };
            public static OpCode EntryPoint = new OpCode { WordCount = 4, OpCodeValue = OpEntryPoint, Name = "EntryPoint" };
            public static OpCode ExecutionMode = new OpCode { WordCount = 3, OpCodeValue = 16, Name = "ExecutionMode" };
            public static OpCode Capability = new OpCode { WordCount = 2, OpCodeValue = OpCapability, Name = "Capability" };
            public static OpCode TypeVoid = new OpCode { WordCount = 2, OpCodeValue = OpTypeVoid, Name = "TypeVoid" };
            public static OpCode TypeBool = new OpCode { WordCount = 2, OpCodeValue = 20, Name = "TypeBool" };
            public static OpCode TypeInt = new OpCode { WordCount = 4, OpCodeValue = OpTypeInt, Name = "TypeInt" };
            public static OpCode TypeFloat = new OpCode { WordCount = 3, OpCodeValue = OpTypeFloat, Name = "TypeFloat" };
            public static OpCode TypeVector = new OpCode { WordCount = 4, OpCodeValue = OpTypeVector, Name = "TypeVector" };
            public static OpCode TypeMatrix = new OpCode { WordCount = 4, OpCodeValue = 24, Name = "TypeMatrix" };
            public static OpCode TypeImage = new OpCode { WordCount = 9, OpCodeValue = 25, Name = "TypeImage" };
            public static OpCode TypeSampler = new OpCode { WordCount = 2, OpCodeValue = 26, Name = "TypeSampler" };
            public static OpCode TypeSampledImage = new OpCode { WordCount = 3, OpCodeValue = 27, Name = "TypeSampledImage" };
            public static OpCode TypeArray = new OpCode { WordCount = 4, OpCodeValue = OpTypeArray, Name = "TypeArray" };
            public static OpCode TypeRuntimeArray = new OpCode { WordCount = 3, OpCodeValue = 29, Name = "TypeRuntimeArray" };
            public static OpCode TypeStruct = new OpCode { WordCount = 2, OpCodeValue = OpTypeStruct, Name = "TypeStruct" };
            public static OpCode TypeOpaque = new OpCode { WordCount = 3, OpCodeValue = 31, Name = "TypeOpaque" };
            public static OpCode TypePointer = new OpCode { WordCount = 4, OpCodeValue = OpTypePointer, Name = "TypePointer" };
            public static OpCode TypeFunction = new OpCode { WordCount = 3, OpCodeValue = OpTypeFunction, Name = "TypeFunction" };
            public static OpCode TypeEvent = new OpCode { WordCount = 2, OpCodeValue = 34, Name = "TypeEvent" };
            public static OpCode TypeDeviceEvent = new OpCode { WordCount = 2, OpCodeValue = 35, Name = "TypeDeviceEvent" };
            public static OpCode TypeReserveId = new OpCode { WordCount = 2, OpCodeValue = 36, Name = "TypeReserveId" };
            public static OpCode TypeQueue = new OpCode { WordCount = 2, OpCodeValue = 37, Name = "TypeQueue" };
            public static OpCode TypePipe = new OpCode { WordCount = 3, OpCodeValue = 38, Name = "TypePipe" };
            public static OpCode TypeForwardPointer = new OpCode { WordCount = 3, OpCodeValue = 39, Name = "TypeForwardPointer" };
            public static OpCode ConstantTrue = new OpCode { WordCount = 3, OpCodeValue = 41, Name = "ConstantTrue" };
            public static OpCode ConstantFalse = new OpCode { WordCount = 3, OpCodeValue = 42, Name = "ConstantFalse" };
            public static OpCode Constant = new OpCode { WordCount = 3, OpCodeValue = OpConstant, Name = "Constant" };
            public static OpCode ConstantComposite = new OpCode { WordCount = 3, OpCodeValue = OpConstantComposite, Name = "ConstantComposite" };
            public static OpCode ConstantSampler = new OpCode { WordCount = 6, OpCodeValue = 45, Name = "ConstantSampler" };
            public static OpCode ConstantNull = new OpCode { WordCount = 3, OpCodeValue = 46, Name = "ConstantNull" };
            public static OpCode SpecConstantTrue = new OpCode { WordCount = 3, OpCodeValue = 48, Name = "SpecConstantTrue" };
            public static OpCode SpecConstantFalse = new OpCode { WordCount = 3, OpCodeValue = 49, Name = "SpecConstantFalse" };
            public static OpCode SpecConstant = new OpCode { WordCount = 3, OpCodeValue = 50, Name = "SpecConstant" };
            public static OpCode SpecConstantComposite = new OpCode { WordCount = 3, OpCodeValue = 51, Name = "SpecConstantComposite" };
            public static OpCode SpecConstantOp = new OpCode { WordCount = 4, OpCodeValue = 52, Name = "SpecConstantOp" };
            public static OpCode Variable = new OpCode { WordCount = 4, OpCodeValue = OpVariable, Name = "Variable" };
            public static OpCode ImageTexelPointer = new OpCode { WordCount = 4, OpCodeValue = 60, Name = "ImageTexelPointer" };
            public static OpCode Load = new OpCode { WordCount = 4, OpCodeValue = 61, Name = "Load" };
            public static OpCode Store = new OpCode { WordCount = 4, OpCodeValue = 62, Name = "Store" };
            public static OpCode CopyMemory = new OpCode { WordCount = 3, OpCodeValue = 63, Name = "CopyMemory" };
            public static OpCode CopyMemorySized = new OpCode { WordCount = 4, OpCodeValue = 64, Name = "CopyMemorySized" };
            public static OpCode AccessChain = new OpCode { WordCount = 4, OpCodeValue = 65, Name = "AccessChain" };
            public static OpCode InBoundsAccessChain = new OpCode { WordCount = 4, OpCodeValue = 66, Name = "InBoundsAccessChain" };
            public static OpCode PtrAccessChain = new OpCode { WordCount = 5, OpCodeValue = 67, Name = "PtrAccessChain" };
            public static OpCode ArrayLength = new OpCode { WordCount = 5, OpCodeValue = 68, Name = "ArrayLength" };
            public static OpCode GenericPtrMemSemantics = new OpCode { WordCount = 4, OpCodeValue = 69, Name = "GenericPtrMemSemantics" };
            public static OpCode InBoundsPtrAccessChain = new OpCode { WordCount = 5, OpCodeValue = 70, Name = "InBoundsPtrAccessChain" };
            public static OpCode Function = new OpCode { WordCount = 5, OpCodeValue = OpFunction, Name = "Function" };
            public static OpCode FunctionParameter = new OpCode { WordCount = 3, OpCodeValue = 55, Name = "FunctionParameter" };
            public static OpCode FunctionEnd = new OpCode { WordCount = 1, OpCodeValue = OpFunctionEnd, Name = "FunctionEnd" };
            public static OpCode FunctionCall = new OpCode { WordCount = 4, OpCodeValue = 57, Name = "FunctionCall" };
            public static OpCode SampledImage = new OpCode { WordCount = 5, OpCodeValue = 86, Name = "SampledImage" };
            public static OpCode ImageSampleImplicitLod = new OpCode { WordCount = 5, OpCodeValue = 87, Name = "ImageSampleImplicitLod" };
            public static OpCode ImageSampleExplicitLod = new OpCode { WordCount = 7, OpCodeValue = 88, Name = "ImageSampleExplicitLod" };
            public static OpCode ImageSampleDrefImplicitLod = new OpCode { WordCount = 6, OpCodeValue = 89, Name = "ImageSampleDrefImplicitLod" };
            public static OpCode ImageSampleDrefExplicitLod = new OpCode { WordCount = 8, OpCodeValue = 90, Name = "ImageSampleDrefExplicitLod" };
            public static OpCode ImageSampleProjImplicitLod = new OpCode { WordCount = 5, OpCodeValue = 91, Name = "ImageSampleProjImplicitLod" };
            public static OpCode ImageSampleProjExplicitLod = new OpCode { WordCount = 7, OpCodeValue = 92, Name = "ImageSampleProjExplicitLod" };
            public static OpCode ImageSampleProjDrefImplicitLod = new OpCode { WordCount = 6, OpCodeValue = 93, Name = "ImageSampleProjDrefImplicitLod" };
            public static OpCode ImageSampleProjDrefExplicitLod = new OpCode { WordCount = 8, OpCodeValue = 94, Name = "ImageSampleProjDrefExplicitLod" };
            public static OpCode ImageGather = new OpCode { WordCount = 6, OpCodeValue = 96, Name = "ImageGather" };
            public static OpCode ImageDrefGather = new OpCode { WordCount = 6, OpCodeValue = 97, Name = "ImageDrefGather" };
            public static OpCode ImageRead = new OpCode { WordCount = 5, OpCodeValue = 98, Name = "ImageRead" };
            public static OpCode ImageWrite = new OpCode { WordCount = 4, OpCodeValue = 99, Name = "ImageWrite" };
            public static OpCode Image = new OpCode { WordCount = 4, OpCodeValue = 100, Name = "Image" };
            public static OpCode ImageQueryFormat = new OpCode { WordCount = 4, OpCodeValue = 101, Name = "ImageQueryFormat" };
            public static OpCode ImageQueryOrder = new OpCode { WordCount = 4, OpCodeValue = 102, Name = "ImageQueryOrder" };
            public static OpCode ImageQuerySizeLod = new OpCode { WordCount = 5, OpCodeValue = 103, Name = "ImageQuerySizeLod" };
            public static OpCode ImageQuerySize = new OpCode { WordCount = 4, OpCodeValue = 104, Name = "ImageQuerySize" };
            public static OpCode ImageQueryLod = new OpCode { WordCount = 5, OpCodeValue = 105, Name = "ImageQueryLod" };
            public static OpCode ImageQueryLevels = new OpCode { WordCount = 4, OpCodeValue = 106, Name = "ImageQueryLevels" };
            public static OpCode ImageQuerySamples = new OpCode { WordCount = 4, OpCodeValue = 107, Name = "ImageQuerySamples" };
            public static OpCode ImageSparseSampleImplicitLod = new OpCode { WordCount = 5, OpCodeValue = 305, Name = "ImageSparseSampleImplicitLod" };
            public static OpCode ImageSparseSampleExplicitLod = new OpCode { WordCount = 7, OpCodeValue = 306, Name = "ImageSparseSampleExplicitLod" };
            public static OpCode ImageSparseSampleDrefImplicitLod = new OpCode { WordCount = 6, OpCodeValue = 307, Name = "ImageSparseSampleDrefImplicitLod" };
            public static OpCode ImageSparseSampleDrefExplicitLod = new OpCode { WordCount = 8, OpCodeValue = 308, Name = "ImageSparseSampleDrefExplicitLod" };
            public static OpCode ImageSparseSampleProjImplicitLod = new OpCode { WordCount = 5, OpCodeValue = 309, Name = "ImageSparseSampleProjImplicitLod" };
            public static OpCode ImageSparseSampleProjExplicitLod = new OpCode { WordCount = 7, OpCodeValue = 310, Name = "ImageSparseSampleProjExplicitLod" };
            public static OpCode ImageSparseSampleProjDrefImplicitLod = new OpCode { WordCount = 6, OpCodeValue = 311, Name = "ImageSparseSampleProjDrefImplicitLod" };
            public static OpCode ImageSparseSampleProjDrefExplicitLod = new OpCode { WordCount = 8, OpCodeValue = 312, Name = "ImageSparseSampleProjDrefExplicitLod" };
            public static OpCode ImageSparseFetch = new OpCode { WordCount = 5, OpCodeValue = 313, Name = "ImageSparseFetch" };
            public static OpCode ImageSparseGather = new OpCode { WordCount = 6, OpCodeValue = 314, Name = "ImageSparseGather" };
            public static OpCode ImageSparseDrefGather = new OpCode { WordCount = 6, OpCodeValue = 315, Name = "ImageSparseDrefGather" };
            public static OpCode ImageSparseTexelsResident = new OpCode { WordCount = 4, OpCodeValue = 316, Name = "ImageSparseTexelsResident" };
            public static OpCode ImageSparseRead = new OpCode { WordCount = 5, OpCodeValue = 320, Name = "ImageSparseRead" };
            public static OpCode ConvertFToU = new OpCode { WordCount = 4, OpCodeValue = 109, Name = "ConvertFToU" };
            public static OpCode ConvertFToS = new OpCode { WordCount = 4, OpCodeValue = 110, Name = "ConvertFToS" };
            public static OpCode ConvertSToF = new OpCode { WordCount = 4, OpCodeValue = 111, Name = "ConvertSToF" };
            public static OpCode ConvertUToF = new OpCode { WordCount = 4, OpCodeValue = 112, Name = "ConvertUToF" };
            public static OpCode UConvert = new OpCode { WordCount = 4, OpCodeValue = 113, Name = "UConvert" };
            public static OpCode SConvert = new OpCode { WordCount = 4, OpCodeValue = 114, Name = "SConvert" };
            public static OpCode FConvert = new OpCode { WordCount = 4, OpCodeValue = 115, Name = "FConvert" };
            public static OpCode QuantizeToF16 = new OpCode { WordCount = 4, OpCodeValue = 116, Name = "QuantizeToF16" };
            public static OpCode ConvertPtrToU = new OpCode { WordCount = 4, OpCodeValue = 117, Name = "ConvertPtrToU" };
            public static OpCode SatConvertSToU = new OpCode { WordCount = 4, OpCodeValue = 118, Name = "SatConvertSToU" };
            public static OpCode SatConvertUToS = new OpCode { WordCount = 4, OpCodeValue = 119, Name = "SatConvertUToS" };
            public static OpCode ConvertUToPtr = new OpCode { WordCount = 4, OpCodeValue = 120, Name = "ConvertUToPtr" };
            public static OpCode PtrCastToGeneric = new OpCode { WordCount = 4, OpCodeValue = 121, Name = "PtrCastToGeneric" };
            public static OpCode GenericCastToPtr = new OpCode { WordCount = 4, OpCodeValue = 122, Name = "GenericCastToPtr" };
            public static OpCode GenericCastToPtrExplicit = new OpCode { WordCount = 5, OpCodeValue = 123, Name = "GenericCastToPtrExplicit" };
            public static OpCode Bitcast = new OpCode { WordCount = 4, OpCodeValue = 124, Name = "Bitcast" };
            public static OpCode VectorExtractDynamic = new OpCode { WordCount = 5, OpCodeValue = 77, Name = "VectorExtractDynamic" };
            public static OpCode VectorInsertDynamic = new OpCode { WordCount = 6, OpCodeValue = 78, Name = "VectorInsertDynamic" };
            public static OpCode VectorShuffle = new OpCode { WordCount = 5, OpCodeValue = 79, Name = "VectorShuffle" };
            public static OpCode CompositeConstruct = new OpCode { WordCount = 3, OpCodeValue = 80, Name = "CompositeConstruct" };
            public static OpCode CompositeExtract = new OpCode { WordCount = 4, OpCodeValue = 81, Name = "CompositeExtract" };
            public static OpCode CompositeInsert = new OpCode { WordCount = 5, OpCodeValue = 82, Name = "CompositeInsert" };
            public static OpCode CopyObject = new OpCode { WordCount = 4, OpCodeValue = 83, Name = "CopyObject" };
            public static OpCode Transpose = new OpCode { WordCount = 4, OpCodeValue = 84, Name = "Transpose" };
            public static OpCode SNegate = new OpCode { WordCount = 4, OpCodeValue = 126, Name = "SNegate" };
            public static OpCode FNegate = new OpCode { WordCount = 4, OpCodeValue = 127, Name = "FNegate" };
            public static OpCode IAdd = new OpCode { WordCount = 5, OpCodeValue = 128, Name = "IAdd" };
            public static OpCode FAdd = new OpCode { WordCount = 5, OpCodeValue = 129, Name = "FAdd" };
            public static OpCode ISub = new OpCode { WordCount = 5, OpCodeValue = 130, Name = "ISub" };
            public static OpCode FSub = new OpCode { WordCount = 5, OpCodeValue = 131, Name = "FSub" };
            public static OpCode IMul = new OpCode { WordCount = 5, OpCodeValue = 132, Name = "IMul" };
            public static OpCode FMul = new OpCode { WordCount = 5, OpCodeValue = 133, Name = "FMul" };
            public static OpCode UDiv = new OpCode { WordCount = 5, OpCodeValue = 134, Name = "UDiv" };
            public static OpCode SDiv = new OpCode { WordCount = 5, OpCodeValue = 135, Name = "SDiv" };
            public static OpCode FDiv = new OpCode { WordCount = 5, OpCodeValue = 136, Name = "FDiv" };
            public static OpCode UMod = new OpCode { WordCount = 5, OpCodeValue = 137, Name = "UMod" };
            public static OpCode SRem = new OpCode { WordCount = 5, OpCodeValue = 138, Name = "SRem" };
            public static OpCode SMod = new OpCode { WordCount = 5, OpCodeValue = 139, Name = "SMod" };
            public static OpCode FRem = new OpCode { WordCount = 5, OpCodeValue = 140, Name = "FRem" };
            public static OpCode FMod = new OpCode { WordCount = 5, OpCodeValue = 141, Name = "FMod" };
            public static OpCode VectorTimesScalar = new OpCode { WordCount = 5, OpCodeValue = 142, Name = "VectorTimesScalar" };
            public static OpCode MatrixTimesScalar = new OpCode { WordCount = 5, OpCodeValue = 143, Name = "MatrixTimesScalar" };
            public static OpCode VectorTimesMatrix = new OpCode { WordCount = 5, OpCodeValue = 144, Name = "VectorTimesMatrix" };
            public static OpCode MatrixTimesVector = new OpCode { WordCount = 5, OpCodeValue = 145, Name = "MatrixTimesVector" };
            public static OpCode MatrixTimesMatrix = new OpCode { WordCount = 5, OpCodeValue = 146, Name = "MatrixTimesMatrix" };
            public static OpCode OuterProduct = new OpCode { WordCount = 5, OpCodeValue = 147, Name = "OuterProduct" };
            public static OpCode Dot = new OpCode { WordCount = 5, OpCodeValue = 148, Name = "Dot" };
            public static OpCode IAddCarry = new OpCode { WordCount = 5, OpCodeValue = 149, Name = "IAddCarry" };
            public static OpCode ISubBorrow = new OpCode { WordCount = 5, OpCodeValue = 150, Name = "ISubBorrow" };
            public static OpCode UMulExtended = new OpCode { WordCount = 5, OpCodeValue = 151, Name = "UMulExtended" };
            public static OpCode SMulExtended = new OpCode { WordCount = 5, OpCodeValue = 152, Name = "SMulExtended" };
            public static OpCode ShiftRightLogical = new OpCode { WordCount = 5, OpCodeValue = 194, Name = "ShiftRightLogical" };
            public static OpCode ShiftRightArithmetic = new OpCode { WordCount = 5, OpCodeValue = 195, Name = "ShiftRightArithmetic" };
            public static OpCode ShiftLeftLogical = new OpCode { WordCount = 5, OpCodeValue = 196, Name = "ShiftLeftLogical" };
            public static OpCode BitwiseOr = new OpCode { WordCount = 5, OpCodeValue = 197, Name = "BitwiseOr" };
            public static OpCode BitwiseXor = new OpCode { WordCount = 5, OpCodeValue = 198, Name = "BitwiseXor" };
            public static OpCode BitwiseAnd = new OpCode { WordCount = 5, OpCodeValue = 199, Name = "BitwiseAnd" };
            public static OpCode Not = new OpCode { WordCount = 4, OpCodeValue = 200, Name = "Not" };
            public static OpCode BitFieldInsert = new OpCode { WordCount = 7, OpCodeValue = 201, Name = "BitFieldInsert" };
            public static OpCode BitFieldSExtract = new OpCode { WordCount = 6, OpCodeValue = 202, Name = "BitFieldSExtract" };
            public static OpCode BitFieldUExtract = new OpCode { WordCount = 6, OpCodeValue = 203, Name = "BitFieldUExtract" };
            public static OpCode BitReverse = new OpCode { WordCount = 4, OpCodeValue = 204, Name = "BitReverse" };
            public static OpCode BitCount = new OpCode { WordCount = 4, OpCodeValue = 205, Name = "BitCount" };
            public static OpCode Any = new OpCode { WordCount = 4, OpCodeValue = 154, Name = "Any" };
            public static OpCode All = new OpCode { WordCount = 4, OpCodeValue = 155, Name = "All" };
            public static OpCode IsNan = new OpCode { WordCount = 4, OpCodeValue = 156, Name = "IsNan" };
            public static OpCode IsInf = new OpCode { WordCount = 4, OpCodeValue = 157, Name = "IsInf" };
            public static OpCode IsFinite = new OpCode { WordCount = 4, OpCodeValue = 158, Name = "IsFinite" };
            public static OpCode IsNormal = new OpCode { WordCount = 4, OpCodeValue = 159, Name = "IsNormal" };
            public static OpCode SignBitSet = new OpCode { WordCount = 4, OpCodeValue = 160, Name = "SignBitSet" };
            public static OpCode LessOrGreater = new OpCode { WordCount = 4, OpCodeValue = 161, Name = "LessOrGreater" };
            public static OpCode Ordered = new OpCode { WordCount = 5, OpCodeValue = 162, Name = "Ordered" };
            public static OpCode Unordered = new OpCode { WordCount = 5, OpCodeValue = 163, Name = "Unordered" };
            public static OpCode LogicalEqual = new OpCode { WordCount = 5, OpCodeValue = 164, Name = "LogicalEqual" };
            public static OpCode LogicalNotEqual = new OpCode { WordCount = 5, OpCodeValue = 165, Name = "LogicalNotEqual" };
            public static OpCode LogicalOr = new OpCode { WordCount = 5, OpCodeValue = 166, Name = "LogicalOr" };
            public static OpCode LogicalAnd = new OpCode { WordCount = 5, OpCodeValue = 167, Name = "LogicalAnd" };
            public static OpCode LogicalNot = new OpCode { WordCount = 5, OpCodeValue = 168, Name = "LogicalNot" };
            public static OpCode Select = new OpCode { WordCount = 5, OpCodeValue = 169, Name = "Select" };
            public static OpCode IEqual = new OpCode { WordCount = 5, OpCodeValue = 170, Name = "IEqual" };
            public static OpCode INotEqual = new OpCode { WordCount = 5, OpCodeValue = 171, Name = "INotEqual" };
            public static OpCode UGreaterThan = new OpCode { WordCount = 5, OpCodeValue = 172, Name = "UGreaterThan" };
            public static OpCode SGreaterThan = new OpCode { WordCount = 5, OpCodeValue = 173, Name = "SGreaterThan" };
            public static OpCode UGreaterThanEqual = new OpCode { WordCount = 5, OpCodeValue = 174, Name = "UGreaterThanEqual" };
            public static OpCode SGreaterThanEqual = new OpCode { WordCount = 5, OpCodeValue = 175, Name = "SGreaterThanEqual" };
            public static OpCode ULessThan = new OpCode { WordCount = 5, OpCodeValue = 176, Name = "ULessThan" };
            public static OpCode SLessThan = new OpCode { WordCount = 5, OpCodeValue = 177, Name = "SLessThan" };
            public static OpCode ULessThanEqual = new OpCode { WordCount = 5, OpCodeValue = 178, Name = "ULessThanEqual" };
            public static OpCode SLessThanEqual = new OpCode { WordCount = 5, OpCodeValue = 179, Name = "SLessThanEqual" };
            public static OpCode FOrdEqual = new OpCode { WordCount = 5, OpCodeValue = 180, Name = "FOrdEqual" };
            public static OpCode FUnordEqual = new OpCode { WordCount = 5, OpCodeValue = 181, Name = "FUnordEqual" };
            public static OpCode FOrdNotEqual = new OpCode { WordCount = 5, OpCodeValue = 182, Name = "FOrdNotEqual" };
            public static OpCode FUnordNotEqual = new OpCode { WordCount = 5, OpCodeValue = 183, Name = "FUnordNotEqual" };
            public static OpCode FOrdLessThan = new OpCode { WordCount = 5, OpCodeValue = 184, Name = "FOrdLessThan" };
            public static OpCode FUnordLessThan = new OpCode { WordCount = 5, OpCodeValue = 185, Name = "FUnordLessThan" };
            public static OpCode FOrdGreaterThan = new OpCode { WordCount = 5, OpCodeValue = 186, Name = "FOrdGreaterThan" };
            public static OpCode FUnordGreaterThan = new OpCode { WordCount = 5, OpCodeValue = 187, Name = "FUnordGreaterThan" };
            public static OpCode FOrdLessThanEqual = new OpCode { WordCount = 5, OpCodeValue = 188, Name = "FOrdLessThanEqual" };
            public static OpCode FUnordLessThanEqual = new OpCode { WordCount = 5, OpCodeValue = 189, Name = "FUnordLessThanEqual" };
            public static OpCode FOrdGreaterThanEqual = new OpCode { WordCount = 5, OpCodeValue = 190, Name = "FOrdGreaterThanEqual" };
            public static OpCode FUnordGreaterThanEqual = new OpCode { WordCount = 5, OpCodeValue = 191, Name = "FUnordGreaterThanEqual" };
            public static OpCode DPdx = new OpCode { WordCount = 4, OpCodeValue = 207, Name = "DPdx" };
            public static OpCode DPdy = new OpCode { WordCount = 4, OpCodeValue = 208, Name = "DPdy" };
            public static OpCode Fwidth = new OpCode { WordCount = 4, OpCodeValue = 209, Name = "Fwidth" };
            public static OpCode DPdxFine = new OpCode { WordCount = 4, OpCodeValue = 210, Name = "DPdxFine" };
            public static OpCode DPdyFine = new OpCode { WordCount = 4, OpCodeValue = 211, Name = "DPdyFine" };
            public static OpCode FwidthFine = new OpCode { WordCount = 4, OpCodeValue = 212, Name = "FwidthFine" };
            public static OpCode DPdxCoarse = new OpCode { WordCount = 4, OpCodeValue = 213, Name = "DPdxCoarse" };
            public static OpCode DPdyCoarse = new OpCode { WordCount = 4, OpCodeValue = 214, Name = "DPdyCoarse" };
            public static OpCode FwidthCoarse = new OpCode { WordCount = 4, OpCodeValue = 215, Name = "FwidthCoarse" };
            public static OpCode Phi = new OpCode { WordCount = 3, OpCodeValue = 245, Name = "Phi" };
            public static OpCode LoopMerge = new OpCode { WordCount = 4, OpCodeValue = 246, Name = "LoopMerge" };
            public static OpCode SelectionMerge = new OpCode { WordCount = 3, OpCodeValue = 247, Name = "SelectionMerge" };
            public static OpCode Label = new OpCode { WordCount = 2, OpCodeValue = 248, Name = "Label" };
            public static OpCode Branch = new OpCode { WordCount = 2, OpCodeValue = 249, Name = "Branch" };
            public static OpCode BranchConditional = new OpCode { WordCount = 4, OpCodeValue = 250, Name = "BranchConditional" };
            public static OpCode Switch = new OpCode { WordCount = 3, OpCodeValue = 251, Name = "Switch" };
            public static OpCode Kill = new OpCode { WordCount = 1, OpCodeValue = 252, Name = "Kill" };
            public static OpCode Return = new OpCode { WordCount = 1, OpCodeValue = 253, Name = "Return" };
            public static OpCode ReturnValue = new OpCode { WordCount = 2, OpCodeValue = 254, Name = "ReturnValue" };
            public static OpCode Unreachable = new OpCode { WordCount = 1, OpCodeValue = 255, Name = "Unreachable" };














        }
    }
}
