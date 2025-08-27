using ComputeSharp;
#pragma warning disable CA1416
namespace ComputeSharpTestbed.MultiKernels;

/**
 * Writes 5 into all cells of input
 */
[ThreadGroupSize(DefaultThreadGroupSizes.X)]
[GeneratedComputeShaderDescriptor]
public readonly partial struct SetterKernel1(ReadWriteBuffer<int> target): IComputeShader
{
    public void Execute()
    {
        target[ThreadIds.X] = 5;
    }
}
#pragma warning restore CA1416