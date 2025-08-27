using ComputeSharp;
#pragma warning disable CA1416
namespace ComputeSharpTestbed.MultiKernels;

/**
 * Multiplies values in input array by 4
 */
[ThreadGroupSize(DefaultThreadGroupSizes.X)]
[GeneratedComputeShaderDescriptor]
public readonly partial struct MultiplierKernel2(ReadWriteBuffer<int> target): IComputeShader
{
    public void Execute()
    {
        target[ThreadIds.X] *= 4;
    }
}
#pragma warning restore CA1416