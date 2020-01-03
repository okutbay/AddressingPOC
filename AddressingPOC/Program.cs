using System;
using System.Threading.Tasks;

/// <summary>
/// Taken from https://www.geeksforgeeks.org/convert-base-decimal-vice-versa/
/// good article to read https://www.dcode.fr/base-n-convert
/// </summary>
namespace AddressingPOC
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.CursorVisible = false;

            char paddingChar = '0';
            int segmentLength = 3;
            int _base = 36;

            int valueDecimal = 1342;
            var segmentAddressCount = valueDecimal;

            string valueBased = ToBase(_base, valueDecimal);//11A
            Console.WriteLine($"Equivalent of {valueDecimal} in base {_base} is {valueBased}");

            int backToDecimal = ToDecimal(valueBased, _base);//1342
            Console.WriteLine($"Decimal equivalent of {valueBased} in base {_base} is {backToDecimal}");

            for (int index = 1; index <= segmentAddressCount; index++)
            {
                string convertedValue = ToBase(_base, index).PadLeft(segmentLength, paddingChar);
                //Console.WriteLine($"Equivalent of {index} in base {_base} is {convertedValue}");
            }

            string addressPattern = string.Empty;

            char segmentSeperatorChar = '-';

            int dim4SegmentLength = 1;
            int dim4SegmentCount = 3;

            int dim3SegmentLength = 1;
            int dim3SegmentCount = 5;

            int dim2SegmentLength = 1;
            int dim2SegmentCount = 35;

            int dim1SegmentLength = 4;
            int dim1SegmentCount = 100;

            bool isValidDim4SegmentCount = false;
            SegmentTypeEnum Dim4SegmentType;
            double MaxDim4SegmentCount = 0;
            string Dim4SegmentPattern = string.Empty;
            isValidDim4SegmentCount = checkSegmentCount(dim4SegmentCount, dim4SegmentLength, out Dim4SegmentType, out MaxDim4SegmentCount, out Dim4SegmentPattern);

            bool isValidDim3SegmentCount = false;
            SegmentTypeEnum Dim3SegmentType;
            double MaxDim3SegmentCount = 0;
            string Dim3SegmentPattern = string.Empty;
            isValidDim3SegmentCount = checkSegmentCount(dim3SegmentCount, dim3SegmentLength, out Dim3SegmentType, out MaxDim3SegmentCount, out Dim3SegmentPattern);

            bool isValidDim2SegmentCount = false;
            SegmentTypeEnum Dim2SegmentType;
            double MaxDim2SegmentCount = 0;
            string Dim2SegmentPattern = string.Empty;
            isValidDim2SegmentCount = checkSegmentCount(dim2SegmentCount, dim2SegmentLength, out Dim2SegmentType, out MaxDim2SegmentCount, out Dim2SegmentPattern);

            bool isValidDim1SegmentCount = false;
            SegmentTypeEnum Dim1SegmentType;
            double MaxDim1SegmentCount = 0;
            string Dim1SegmentPattern = string.Empty;
            isValidDim1SegmentCount = checkSegmentCount(dim1SegmentCount, dim1SegmentLength, out Dim1SegmentType, out MaxDim1SegmentCount, out Dim1SegmentPattern);

            addressPattern = $"{Dim4SegmentPattern}{segmentSeperatorChar}{Dim3SegmentPattern}{segmentSeperatorChar}{Dim2SegmentPattern}{segmentSeperatorChar}{Dim1SegmentPattern}";
            Console.SetCursorPosition(0, 3);
            Console.Write($"{addressPattern}");

            for (int indexDim4 = 1; indexDim4 <= dim4SegmentCount; indexDim4++)
            {
                string convertedDim4Value = getSegmentValue(indexDim4, dim4SegmentLength, Dim4SegmentType);

                for (int indexDim3 = 1; indexDim3 <= dim3SegmentCount; indexDim3++)
                {
                    string convertedDim3Value = getSegmentValue(indexDim3, dim3SegmentLength, Dim3SegmentType);

                    for (int indexDim2 = 1; indexDim2 <= dim2SegmentCount; indexDim2++)
                    {
                        string convertedDim2Value = getSegmentValue(indexDim2, dim2SegmentLength, Dim2SegmentType);

                        for (int indexDim1 = 1; indexDim1 <= dim1SegmentCount; indexDim1++)
                        {
                            string convertedDim1Value = getSegmentValue(indexDim1, dim1SegmentLength, Dim1SegmentType);

                            var address = $"{convertedDim4Value}{segmentSeperatorChar}{convertedDim3Value}{segmentSeperatorChar}{convertedDim2Value}{segmentSeperatorChar}{convertedDim1Value}";

                            Console.SetCursorPosition(0, 4);
                            Console.Write($"{address}");
                            //Task.Delay(20).Wait();
                        }
                    }
                }
            }

            Console.ReadLine();
            Console.CursorVisible = true;
        }

        private static string getSegmentValue(int Value, int SegmentLength, SegmentTypeEnum SegmentType)
        {
            const char paddingChar = '0';
            int _base = (int)SegmentType;
            string convertedValue = ToBase(_base, Value).PadLeft(SegmentLength, paddingChar);
            return convertedValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SegmentCount"></param>
        /// <param name="SegmentLength"></param>
        /// <param name="SegmentType"></param>
        /// <param name="MaxSegmentCount"></param>
        /// <returns></returns>
        private static bool checkSegmentCount(int SegmentCount, int SegmentLength, out SegmentTypeEnum SegmentType, out double MaxSegmentCount, out string SegmentPattern)
        {
            double numericSegmentLimit = 0;
            double alphanumericSegmentLimit = 0;

            numericSegmentLimit = getSegmentLimit(SegmentTypeEnum.Numeric, SegmentLength);
            alphanumericSegmentLimit = getSegmentLimit(SegmentTypeEnum.Alphanumeric, SegmentLength);
            MaxSegmentCount = alphanumericSegmentLimit;

            bool isValidSegmentCount = false;

            if (SegmentCount == 0)
            {
                isValidSegmentCount = true;
                SegmentType = SegmentTypeEnum.Closed;
                MaxSegmentCount = 0;
                SegmentPattern = "0";
            }
            else if (SegmentCount <= numericSegmentLimit)
            {
                isValidSegmentCount = true;
                SegmentType = SegmentTypeEnum.Numeric;
                SegmentPattern = string.Empty.PadLeft(SegmentLength, '#');
            }
            else if (SegmentCount <= alphanumericSegmentLimit)
            {
                isValidSegmentCount = true;
                SegmentType = SegmentTypeEnum.Alphanumeric;
                SegmentPattern = string.Empty.PadLeft(SegmentLength, '$');
            }
            else
            {
                isValidSegmentCount = false;
                SegmentType = SegmentTypeEnum.Overflow;
                SegmentPattern = "0";
            }

            return isValidSegmentCount;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SegmentType"></param>
        /// <param name="SegmentLength"></param>
        /// <returns></returns>
        private static double getSegmentLimit(SegmentTypeEnum SegmentType, int SegmentLength)
        {
            double resultLimit = 0;

            switch (SegmentType)
            {
                case SegmentTypeEnum.Numeric:
                    resultLimit = getNumericLimit(SegmentLength);
                    break;
                case SegmentTypeEnum.Alphanumeric:
                    resultLimit = getAlphanumericLimit(SegmentLength);
                    break;
                case SegmentTypeEnum.Closed:
                case SegmentTypeEnum.Overflow:
                default:
                    resultLimit = 0;
                    break;
            }

            return resultLimit;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SegmentLength"></param>
        /// <returns></returns>
        private static double getNumericLimit(int SegmentLength)
        {
            const int numericBase = 10;
            double resultLimit = 0;
            resultLimit = Math.Pow(numericBase, SegmentLength) - 1;
            return resultLimit;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SegmentLength"></param>
        /// <returns></returns>
        private static double getAlphanumericLimit(int SegmentLength)
        {
            const int alphanumericBase = 36;
            double resultLimit = 0;
            resultLimit = Math.Pow(alphanumericBase, SegmentLength) - 1;
            return resultLimit;
        }

        /// <summary>
        /// Closed: Segment is closed and will not be the part of the address.
        /// Numeric: Segment is numeric and will only contain numbers in the part of address.
        /// Alphanumeric: Segment is alphanumeric and will contain numbers and capital letters in the part of address.
        /// Overflow: Segment cannot supply desired segment count with this segment length. Please decrease segment count or increase segment length.
        /// </summary>
        public enum SegmentTypeEnum:int
        {
            /// <summary>
            /// Segment is closed and will not be the part of the address.
            /// </summary>
            Closed = 0,
            /// <summary>
            /// Segment is numeric and will only contain numbers in the part of address.
            /// </summary>
            Numeric = 10,
            /// <summary>
            /// Segment is alphanumeric and will contain numbers and capital letters in the part of address.
            /// </summary>
            Alphanumeric = 36,
            /// <summary>
            /// Segment cannot supply desired segment count with this segment length. Please decrease segment count or increase segment length.
            /// </summary>
            Overflow = -1
        }

        // To return value of a char.  
        // For example, 2 is returned 
        // for '2'. 10 is returned  
        // for 'A', 11 for 'B' 
        public static int Val(char c)
        {
            if (c >= '0' && c <= '9')
                return (int)c - '0';
            else
                return (int)c - 'A' + 10;
        }

        // Function to convert a  
        // number from given base  
        // 'b' to decimal 
        public static int ToDecimal(string Str, int Base)
        {
            int len = Str.Length;
            int power = 1; // Initialize power of base 
            int num = 0; // Initialize result 
            int i;

            // Decimal equivalent is  
            // str[len-1]*1 + str[len-1] * 
            // base + str[len-1]*(base^2) + ... 
            for (i = len - 1; i >= 0; i--)
            {
                // A digit in input number  
                // must be less than  
                // number's base 
                if (Val(Str[i]) >= Base)
                {
                    Console.WriteLine("Invalid Number");
                    return -1;
                }

                num += Val(Str[i]) * power;
                power = power * Base;
            }

            return num;
        }

        // To return char for a value. For  
        // example '2' is returned for 2.  
        // 'A' is returned for 10. 'B' for 11 
        public static char ReVal(int Num)
        {
            if (Num >= 0 && Num <= 9)
                return (char)(Num + 48);
            else
                return (char)(Num - 10 + 65);
        }

        // Function to convert a given decimal number 
        // to a base 'base' and 
        static string ToBase(int Base, int InputNum)
        {
            string s = "";

            // Convert input number is given  
            // base by repeatedly dividing it 
            // by base and taking remainder 
            while (InputNum > 0)
            {
                s += ReVal(InputNum % Base);
                InputNum /= Base;
            }
            char[] result = s.ToCharArray();

            // Reverse the result 
            Array.Reverse(result);
            return new String(result);
        }
    }
}
