using ComputeSharp;
#pragma warning disable CA1416
namespace ComputeSharpTestbed.MultiKernels;

[ThreadGroupSize(DefaultThreadGroupSizes.X)]
[GeneratedComputeShaderDescriptor]
public readonly partial struct VerificationKernel5(ReadWriteBuffer<int> toCheck,ReadWriteBuffer<bool> result): IComputeShader
{
    public void Execute()
    {
        int compareVal = toCheck[ThreadIds.X];

        int expectedVal = 82; //4*( 4*5 + 1/2)

        result[ThreadIds.X] = compareVal == expectedVal;
    }
}
#pragma warning restore CA1416