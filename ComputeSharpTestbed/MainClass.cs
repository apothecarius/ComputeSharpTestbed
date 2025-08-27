using ComputeSharp;
using ComputeSharpTestbed.MultiKernels;

#pragma warning disable CA1416

namespace ComputeSharpTestbed;

public partial class MainClass
{
    private const int nNums = 250000000;
    private static int nIntermSums = (int)Math.Ceiling(Math.Sqrt(nNums));
    static void Main(String[] args)
    {
        multiKernelTest();
        return;
        /*Console.WriteLine("nIntermSums: "+nIntermSums+" , MachineName:"+GraphicsDevice.GetDefault().Name);
        int[] inputData = new int[nNums];
        Random randy = new Random();
        for (int i = 0; i < nNums; i++)
        {
            inputData[i] = randy.Next(10);
        }
        var gpuInputBuffer = GraphicsDevice.GetDefault().AllocateReadWriteBuffer<int>(inputData);
        var gpuIntermBuffer = GraphicsDevice.GetDefault().AllocateReadWriteBuffer<int>(nIntermSums,AllocationMode.Clear);
        var gpuResultBuffer = GraphicsDevice.GetDefault().AllocateReadWriteBuffer<int>(1,AllocationMode.Clear);
        
        DateTime t0 = DateTime.Now;
        int cpuResult = 0;
        for (int i = 0; i < nNums; i++)
        {
            cpuResult += inputData[i];
        }
        long cpuTime = (DateTime.Now - t0).Milliseconds;
        Console.WriteLine("Cpu msecs: "+cpuTime+" \t("+cpuResult+")");

        t0 = DateTime.Now;
        int gpuSum;
        
        using (ComputeContext ctx = GraphicsDevice.GetDefault().CreateComputeContext())
        {
            GraphicsDevice.GetDefault().For<GpuArraySummator>(nIntermSums,new (gpuInputBuffer,gpuIntermBuffer,nIntermSums));
            //ctx.Barrier(gpuIntermBuffer);
            GraphicsDevice.GetDefault().For<GpuArraySummator>(1,new (gpuIntermBuffer,gpuResultBuffer,nIntermSums));
            //GraphicsDevice.GetDefault().For(12,new GpuArraySummator(gpuInputBuffer,gpuResultBuffer));
            //ctx.Barrier(gpuResultBuffer);
            
            int[] gpuSumArr = new int[1];
            int[] intermSumArray = new int[nIntermSums];
            gpuResultBuffer.CopyTo(gpuSumArr);
            gpuSum = gpuSumArr[0];
            
        }
        long gpuTime = (DateTime.Now - t0).Milliseconds;
        Console.WriteLine("Gpu msecs: "+gpuTime+" \t("+gpuSum+")");
        */
    }

    public static void multiKernelTest()
    {
        DateTime t0 = DateTime.Now;
        int quarterOfNums = 25000;
        int trues = 0;

        ReadWriteBuffer<int> bufferA;
        ReadWriteBuffer<int> bufferB;
        ReadWriteBuffer<bool> bufferC;
        using (ComputeContext ctx = GraphicsDevice.GetDefault().CreateComputeContext())
        {
            bufferA = ctx.GraphicsDevice.AllocateReadWriteBuffer<int>(quarterOfNums*4);
            bufferB = ctx.GraphicsDevice.AllocateReadWriteBuffer<int>(quarterOfNums);
            bufferC = ctx.GraphicsDevice.AllocateReadWriteBuffer<bool>(quarterOfNums);
            ctx.For(quarterOfNums*4,new SetterKernel1(bufferA));
            ctx.Barrier(bufferA);
            ctx.For(quarterOfNums*4,new MultiplierKernel2(bufferA));
            ctx.Barrier(bufferA);
            ctx.For(quarterOfNums*4,new AddThreadIdKernel3(bufferA));
            ctx.Barrier(bufferA);
            ctx.For(quarterOfNums,new CollectFourKernel4(bufferA,bufferB));
            ctx.Barrier(bufferA);
            ctx.Barrier(bufferB);
            ctx.For(quarterOfNums,new VerificationKernel5(bufferB,bufferC));            
            ctx.Barrier(bufferC);
        }
        
        bool[] downloadedResults = new bool[quarterOfNums];
        bufferC.CopyTo(downloadedResults);
        trues = downloadedResults.Count(x => x);

        
        Console.WriteLine("Duration: "+(DateTime.Now-t0).Milliseconds);
        Console.WriteLine("Done: valid are "+trues);
    }

    /*public static void MinimalTestcase1()
    {
        int[] resAr = new int[1]{42};
        
        ReadWriteBuffer<int> testBufer = GraphicsDevice.GetDefault().AllocateReadWriteBuffer<int>(resAr);
        
        GraphicsDevice.GetDefault().For(1,new MinimalKernel(testBufer));
        testBufer.CopyTo(resAr);

        Console.WriteLine(resAr[0]);
        
    }*/
    public static void Main12()
    {
        using (var ctx = GraphicsDevice.GetDefault().CreateComputeContext())
        {
            int[] resAr = new int[1] { 1337 };
            ReadWriteBuffer<int> testBufer = ctx.GraphicsDevice.AllocateReadWriteBuffer<int>(resAr);
        
            //iwie passiert nix wenn man den Kernel mit ctx ausführt
            GraphicsDevice.GetDefault().For(1,new MinimalKernel(testBufer));
            GraphicsDevice.GetDefault().For(1,new DoublingKernel(testBufer));
            //ctx.Barrier(testBufer);
            testBufer.CopyTo(resAr);

            Console.WriteLine(resAr[0]);
        }
    }
    
    
    
    [ThreadGroupSize(DefaultThreadGroupSizes.X)]
    [GeneratedComputeShaderDescriptor]
    public readonly partial struct MinimalKernel (ReadWriteBuffer<int> target): IComputeShader
    {
        public void Execute()
        {
            target[0] = 42;
        }
    }
    [ThreadGroupSize(DefaultThreadGroupSizes.X)]
    [GeneratedComputeShaderDescriptor]
    public readonly partial struct DoublingKernel (ReadWriteBuffer<int> target): IComputeShader
    {
        public void Execute()
        {
            target[0] *= 2;
        }
    }
    
}
#pragma warning restore CA1416
