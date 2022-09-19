.class public abstract sealed auto ansi beforefieldinit
  Hw2.Parser
{
    .method static public void ParseCalcArguments(string[], [out] float64&, [out] valuetype Hw2.CalculatorOperation&, [out] float64&) cil managed
    {
        ldarg.0
        call bool Hw2.Parser::IsArgLengthSupported(string[])
        brzero ArgumentException
        
        ldarg.0 //string[]
        ldc.i4.0 //string[], 0
        ldelem.ref //string[0]
        ldarg.1 //string[0], float32&
        call bool [mscorlib]System.Double::TryParse(string, float64&) //bool
        brzero ArgumentException
        
        ldarg.0
        ldc.i4.2
        ldelem.ref
        ldarg.3
        call bool [mscorlib]System.Double::TryParse(string, float64&)
        brzero ArgumentException
        
        ldarg.2
        ldarg.0
        ldc.i4.1
        ldelem.ref
        call valuetype Hw2.CalculatorOperation Hw2.Parser::ParseOperation(string)
        stind.i4
        br Result
        
        ArgumentException:
        newobj instance void [mscorlib]System.ArgumentException::.ctor()
        throw
        
        Result:
        ret
    }
    
    .method static public bool IsArgLengthSupported(string[]) cil managed
    {
        ldarg.0
        ldlen
        ldc.i4.3
        ceq
        ret
    }
    
    .method static public valuetype Hw2.CalculatorOperation ParseOperation(string) cil managed
    {
        ldarg.0
        ldstr "+"
        call bool [mscorlib]System.String::op_Equality(string, string)
        brtrue Plus
        
        ldarg.0
        ldstr "-"
        call bool [mscorlib]System.String::op_Equality(string, string)
        brtrue Minus
        
        ldarg.0
        ldstr "*"
        call bool [mscorlib]System.String::op_Equality(string, string)
        brtrue Multiply
        
        ldarg.0
        ldstr "/"
        call bool [mscorlib]System.String::op_Equality(string, string)
        brtrue Divide
        
        br Undefined
        
        Plus:
        ldc.i4.0
        ret
        
        Minus:
        ldc.i4.1
        ret
        
        Multiply:
        ldc.i4.2
        ret
        
        Divide:
        ldc.i4.3
        ret
        
        Undefined:
        newobj instance void [mscorlib]System.InvalidOperationException::.ctor()
        throw
    }
}