using System;

namespace Demo
{
    static class Assert
    {
        public static void AreEqual<T, U>(T expect, U actual)
        {
            if (!actual.Equals(expect))
                throw new AssertException(string.Format("Expect \"{0}\", but got \"{1}\".", expect, actual));
        }

        public static void AreEqual(int expect, double actual)
        {
            if (!DoubleEqual(expect, actual))
                throw new AssertException(string.Format("Expect \"{0}\", but got \"{1}\".", expect, actual));
        }

        private static bool DoubleEqual(double expect, double actual)
        {
            return Math.Abs(expect - actual) < double.Epsilon;
        }
    }
}