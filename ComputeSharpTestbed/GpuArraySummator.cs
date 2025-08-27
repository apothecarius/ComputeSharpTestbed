using ComputeSharp;
#pragma warning disable CA1416

namespace ComputeSharpTestbed;

[ThreadGroupSize(DefaultThreadGroupSizes.X)]
[GeneratedComputeShaderDescriptor]
[RequiresDoublePrecisionSupport]
public readonly partial struct GpuArraySummator(ReadWriteBuffer<int> input, ReadWriteBuffer<int> output, int NAdditionsPerThread) : IComputeShader
{
    public void Execute()
    {
        int retu = 0;
        for (int i = 0; i < NAdditionsPerThread; i++)
        {
            int srcAdr = (ThreadIds.X * NAdditionsPerThread) + i;
            if (input.Length > srcAdr)
            {
                retu += input[srcAdr];
            }
        }
        
        int outIdx = ThreadIds.X;
        output[outIdx] = retu;
        
    }
}
#pragma warning restore CA1416
