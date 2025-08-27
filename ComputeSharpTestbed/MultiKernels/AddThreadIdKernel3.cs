using ComputeSharp;
#pragma warning disable CA1416
namespace ComputeSharpTestbed.MultiKernels;

/**
 * Add 1 to the values to the buffer, if this thread has an even threadid(x)
 */
[ThreadGroupSize(DefaultThreadGroupSizes.X)]
[GeneratedComputeShaderDescriptor]
public readonly partial struct AddThreadIdKernel3(ReadWriteBuffer<int> target): IComputeShader
{
    public void Execute()
    {
        int bonus = ThreadIds.X % 2;
        target[ThreadIds.X] += bonus;
    }
}
#pragma warning restore CA1416