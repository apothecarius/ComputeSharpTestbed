using ComputeSharp;
#pragma warning disable CA1416
namespace ComputeSharpTestbed.MultiKernels;

/**
 * Collects four values from input and writes the sum into output.
 * Size of input must be 4 times that of output
 */
[ThreadGroupSize(DefaultThreadGroupSizes.X)]
[GeneratedComputeShaderDescriptor]
public readonly partial struct CollectFourKernel4(ReadWriteBuffer<int> input,ReadWriteBuffer<int> output): IComputeShader
{
    public void Execute()
    {
        int sum = 0;
        int inputIdxBase = ThreadIds.X*4;
        for (int i = 0; i < 4; i++)
        {
            sum += input[inputIdxBase + i];
        }
        output[ThreadIds.X] = sum;

    }
}
#pragma warning restore CA1416