using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spirv_dump
{
    class Program
    {
        static void Main(string[] args)
        {
            Ratchet.Code.SPIRV.LoadFromFile(@"C:\Users\rboissel\Source\Repos\ratchet-vulkan\ratchet-vulkan\Samples\Triangle\Shaders\fragmentShader.spv");

        }
    }
}
