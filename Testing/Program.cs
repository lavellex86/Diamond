namespace Testing;

class Program
{
    static void Main(string[] args)
    {   
        BigIntTests.TestAdd();      // working!
        BigIntTests.TestSubtract(); // working!
        BigIntTests.TestMultiply(); // working!
        BigIntTests.TestDivide();   // working!
        BigIntTests.TestMod();      // working!
        BigIntTests.TestGCD();      // working!
        BigIntTests.TestModInverse();
        BigIntTests.TestBarrett();  // working!
        BigIntTests.TestMonty();    // working!
        
        ConstantTime.Analytics.TestMontgomeryModPow();    // constant time checked
        ConstantTime.Analytics.TestGCD();               // constant time checked
        ConstantTime.Analytics.TestGCDEvenVsOdd();      // constant time failed
    }
}