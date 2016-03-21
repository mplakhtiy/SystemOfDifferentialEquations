using System;

namespace SystemOfDifferentialEquations.Source
{
    public  class Vector
    {
        private double[] vector { get; set; }
        public int Length { get; set; }

        public double this[int i]
        {
            get
            {
                if (Length <= 0) throw new ArgumentException("Vector not Find!");
                if ((i > -1) & (i < Length))
                {
                    return vector[i];
                }
                throw new ArgumentException("Wrong index!");
            }
            set
            {
                if (Length <= 0) throw new ArgumentException("Vector not Find!");
                if ((i > -1) & (i < Length))
                {
                    vector[i] = value;
                }
                else throw new ArgumentException("Wrong indexes!");
            }
        }

        #region Constructors

        public Vector(int size)
        {
            Length = size;
            ZeroVector();
        }

        public Vector(double[] vectorInput)
        {
            Length = vectorInput.Length;
            vector = vectorInput;
        }

        public Vector(Vector inputVector)
        {
            Length = inputVector.Length;
            vector = copyArray(inputVector.vector);
        }

        #endregion

        #region Public Methods

        public Vector IdentityVector()//This method change current object
        {
            for (int i = 0; i < Length; i++)
            {
                vector[i] = 1;
            }

            return this;
        }

        public Vector ZeroVector()//This method change current object
        {
            vector = new double[Length];

            return this;
        }

        #endregion

        #region Operators

        //This operators returns new object without changes to input parameters

        public static double operator *(Vector inputVectorA, Vector inputVectorB)
        {
            if (inputVectorA.Length != inputVectorB.Length)
                throw new ArgumentException("Diferent Vector Length");

            double resultValue = 0;

            for (int i = 0; i < inputVectorA.Length; i++)
            {
                resultValue += inputVectorA[i] * inputVectorB[i];
            }

            return resultValue;
        }

        public static Vector operator *(double inputValue, Vector inputVector)
        {
            Vector resultVector = new Vector(inputVector.Length);

            for (int i = 0; i < inputVector.Length; i++)
            {
                resultVector[i] = inputVector[i] * inputValue;
            }

            return resultVector;
        }

        public static Vector operator +(Vector inputVectorA, Vector inputVectorB)
        {
            if (inputVectorA.Length != inputVectorB.Length)
                throw new ArgumentException("Diferent Vector Length");

            Vector resultVector = new Vector(inputVectorA.Length);

            for (int i = 0; i < inputVectorA.Length; i++)
            {
                resultVector[i] = inputVectorA[i] + inputVectorB[i];
            }

            return resultVector;
        }

        public static Vector operator -(Vector inputVectorA, Vector inputVectorB)
        {
            if (inputVectorA.Length != inputVectorB.Length)
                throw new ArgumentException("Diferent Vector Length");

            Vector resultVector = new Vector(inputVectorA.Length);

            for (int i = 0; i < inputVectorA.Length; i++)
            {
                resultVector[i] = inputVectorA[i] - inputVectorB[i];
            }

            return resultVector;
        }

        #endregion

        #region Private Methods

        private double[] copyArray(double[] inputArray)
        {
            int size = inputArray.Length;
            double[] tempArray = new double[size];
            for (int i = 0; i < size; i++)
            {

                tempArray[i] = inputArray[i];

            }
            return tempArray;
        }

        #endregion

    }
}
