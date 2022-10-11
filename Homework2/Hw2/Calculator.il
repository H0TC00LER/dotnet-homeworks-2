.class public abstract sealed auto ansi beforefieldinit
  Hw2.Calculator
{
    .method static public float32 Calculate(float32, valuetype Hw2.CalculatorOperation, float32) cil managed
    {
        ldarg.0
        ldarg.2

        ldc.i4.0
        ldarg.1
        beq Plus

        ldc.i4.1
        ldarg.1
        beq Minus

        ldc.i4.2
        ldarg.1
        beq Multiply

        ldc.i4.3
        ldarg.1
        beq Divide

        ldc.i4.4
        ldarg.1
        beq Undefined

        Plus:
        add
        ret

        Minus:
        sub
        ret

        Multiply:
        mul
        ret

        Divide:
        div
        ret

        Undefined:
        newobj instance void [mscorlib]System.ArgumentOutOfRangeException::.ctor()
        throw
    }
}