using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymApplication
{
    public enum WorkoutType
    {
        Strength,
        Cardio,
        Flexibility
    }

    public static class WorkoutFactory
    {
        public static IWorkout CreateWorkout(WorkoutType type, string name, int intensity)
        {
            IWorkout workout;
            switch (type)
            {
                case WorkoutType.Strength:
                    workout = new StrengthWorkout(name, intensity, GetStrengthExercises(intensity));
                    break;
                case WorkoutType.Cardio:
                    workout = new CardioWorkout(name, intensity, GetCardioExercises(intensity));
                    break;
                case WorkoutType.Flexibility:
                    workout = new FlexibilityWorkout(name, intensity, GetFlexibilityExercises(intensity));
                    break;
                default:
                    throw new ArgumentException("Invalid workout type");
            }
            return workout;
        }

        // Methods to add predefined excercise lists to the workouts
        private static List<Exercise> GetStrengthExercises(int intensity)
        {
            var exercises = new List<Exercise>
        {
            new Exercise { Name = "Squat", IntensityEffect = 20*intensity, IntensitySpecification = "kg", Description = "The squat is a compound exercise that targets the muscles of the lower body, including the quadriceps, hamstrings, and glutes." },
            new Exercise { Name = "Bench Press", IntensityEffect = 15*intensity, IntensitySpecification = "kg", Description = "The bench press is a fundamental upper-body strength exercise that targets the chest, shoulders, and triceps." },
            new Exercise { Name = "Deadlift", IntensityEffect = 25*intensity, IntensitySpecification = "kg", Description = "The deadlift is a compound movement that primarily targets the muscles of the posterior chain, including the lower back, glutes, and hamstrings." },
        };
            return exercises;
        }

        private static List<Exercise> GetCardioExercises(int intensity)
        {
            var exercises = new List<Exercise>
        {
            new Exercise { Name = "Running", IntensityEffect = 0.5*intensity, IntensitySpecification = "km", Description = "Running is a cardiovascular exercise that helps improve endurance, burn calories, and strengthen the lower body muscles." },
            new Exercise { Name = "Cycling", IntensityEffect = 1*intensity, IntensitySpecification = "km", Description = "Cycling is a low-impact cardiovascular exercise that targets the lower body muscles while providing a great aerobic workout." },
            new Exercise { Name = "Jump Rope", IntensityEffect = 50*intensity, IntensitySpecification = "jumps", Description = "Jump rope is a high-intensity cardiovascular exercise that improves coordination, agility, and cardiovascular fitness." },
        };
            return exercises;
        }

        private static List<Exercise> GetFlexibilityExercises(int intensity)
        {
            var exercises = new List<Exercise>
        {
            new Exercise { Name = "Stretching", IntensityEffect = 5*intensity, IntensitySpecification = "minutes", Description = "Stretching exercises help improve flexibility, range of motion, and reduce the risk of injury by lengthening tight muscles and improving joint mobility." },
            new Exercise { Name = "Yoga", IntensityEffect = 10*intensity, IntensitySpecification = "minutes", Description = "Yoga is a mind-body practice that combines physical postures, breathing exercises, and meditation to promote relaxation, stress relief, and overall well-being." },
            new Exercise { Name = "Pilates", IntensityEffect = 15*intensity, IntensitySpecification = "minutes", Description = "Pilates is a low-impact exercise method that focuses on core strength, flexibility, and overall body conditioning. It emphasizes proper alignment, breathing, and controlled movements." },
        };
            return exercises;
        }
    }
}
