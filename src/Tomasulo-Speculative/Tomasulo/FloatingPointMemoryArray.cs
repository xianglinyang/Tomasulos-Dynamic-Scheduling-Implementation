using System.Collections.Generic;

namespace Tomasulo
{
    public class FloatingPointMemoryArrary
    {
        private List<float> fpMem = new List<float>();
        public FloatingPointMemoryArrary(int numToCreate)
        {
            for (int i = 0; i < numToCreate; i++)
            {
                fpMem.Add(0.0f);
            }
        }

        public void Set(float val, int address)
        {
            fpMem[address] = val;
        }

        public float Get(int address)
        {
            return fpMem[address];
        }

    }
}