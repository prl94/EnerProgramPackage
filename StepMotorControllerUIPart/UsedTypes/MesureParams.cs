namespace StepMotorControllerUIPart.UsedTypes
{
    public class MesureParams
    {
        public readonly int StepsCount;
        public readonly int MesuresCount;
        public readonly int DelayBeforeStep;

        public MesureParams(int stepsCount, int mesuresCount, int delayBeforeStep)
        {
            StepsCount = stepsCount;
            MesuresCount = mesuresCount;
            DelayBeforeStep = delayBeforeStep * 1000;
        }

    }
}