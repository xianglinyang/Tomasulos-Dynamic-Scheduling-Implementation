namespace Tomasulo
{
    public class Speculator
    {
        private enum SpecualtionState { StrongTaken, WeakTaken, StrongNotTaken, WeakNotTaken };
        private SpecualtionState currentState = SpecualtionState.StrongTaken;

        public void RecordBranchResult(bool wasTaken)
        {
            if (wasTaken)
            {
                if (currentState == SpecualtionState.StrongTaken)
                {
                    // Don't change.
                }
                else if (currentState == SpecualtionState.WeakTaken)
                {
                    currentState = SpecualtionState.StrongTaken;
                }
                else if (currentState == SpecualtionState.StrongNotTaken)
                {
                    currentState = SpecualtionState.WeakNotTaken;
                }
                else
                {
                    currentState = SpecualtionState.StrongTaken;
                }
            }
            else
            {
                if (currentState == SpecualtionState.StrongTaken)
                {
                    currentState = SpecualtionState.WeakTaken;
                }
                else if (currentState == SpecualtionState.WeakTaken)
                {
                    currentState = SpecualtionState.StrongNotTaken;
                }
                else if (currentState == SpecualtionState.StrongNotTaken)
                {
                    // Don't change.
                }
                else
                {
                    currentState = SpecualtionState.StrongNotTaken;
                }
            }
        }

        public bool GetBranchPrediction()
        {
            if ((currentState == SpecualtionState.StrongTaken) || (currentState == SpecualtionState.WeakTaken))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
