using System;

namespace task5
{
    enum QuestionLevel { Easy = 1, Medium = 2, Hard = 3 }

    abstract class Question
    {
        public string Header;
        public int Marks;
        public QuestionLevel Level;

        public abstract void Display();
        public abstract int CheckAnswer();
    }

    class TrueFalseQuestion : Question
    {
        public bool CorrectAnswer;

        public override void Display()
        {
            Console.WriteLine("\n[True/False] " + Header + " (" + Marks + " marks)");
            Console.WriteLine("1. True");
            Console.WriteLine("2. False");
        }

        public override int CheckAnswer()
        {
            Display();
            Console.Write("Your answer: ");
            int ans = Program.ReadInt(1, 2);

            bool studentAnswer = (ans == 1);
            if (studentAnswer == CorrectAnswer)
                return Marks;
            return 0;
        }
    }

    class ChooseOneQuestion : Question
    {
        public string[] Choices = new string[4];
        public int CorrectChoice;

        public override void Display()
        {
            Console.WriteLine("\n[Choose One] " + Header + " (" + Marks + " marks)");
            for (int i = 0; i < 4; i++)
                Console.WriteLine((i + 1) + ". " + Choices[i]);
        }

        public override int CheckAnswer()
        {
            Display();
            Console.Write("Your answer: ");
            int ans = Program.ReadInt(1, 4);

            if (ans == CorrectChoice)
                return Marks;
            return 0;
        }
    }

    class MultipleChoiceQuestion : Question
    {
        public string[] Choices = new string[4];
        public int[] CorrectAnswers = new int[4];
        public int correctCount;

        public override void Display()
        {
            Console.WriteLine("\n[Multiple Choice] " + Header + " (" + Marks + " marks)");
            for (int i = 0; i < 4; i++)
                Console.WriteLine((i + 1) + ". " + Choices[i]);
        }

        public override int CheckAnswer()
        {
            Display();
            Console.Write("Your answers (example 1,3): ");
            int[] studentAnswers = Program.ParseArray(Console.ReadLine());

            if (studentAnswers.Length != correctCount)
                return 0;

            for (int i = 0; i < studentAnswers.Length; i++)
            {
                bool found = false;
                for (int j = 0; j < correctCount; j++)
                {
                    if (studentAnswers[i] == CorrectAnswers[j])
                        found = true;
                }
                if (!found)
                    return 0;
            }
            return Marks;
        }
    }

    internal class Program
    {
        static Question[] questionBank = new Question[1000];
        static int bankCount = 0;

        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("\n===== Examination System =====");
                Console.WriteLine("1. Doctor Mode");
                Console.WriteLine("2. Student Mode");
                Console.WriteLine("3. Exit");
                Console.Write("Choose: ");
                int choice = ReadInt(1, 3);

                if (choice == 1)
                    DoctorMode();
                else if (choice == 2)
                    StudentMode();
                else
                    break;
            }
        }

        static void DoctorMode()
        {
            Console.WriteLine("\n--- Doctor Mode ---");
            Console.Write("How many questions do you want to add? ");
            int count = ReadInt(1, 1000 - bankCount);

            for (int i = 1; i <= count; i++)
            {
                Console.WriteLine("\n--- Question " + i + " ---");
                Console.WriteLine("1. True/False  2. Choose One  3. Multiple Choice");
                Console.Write("Select type: ");
                int type = ReadInt(1, 3);

                Console.WriteLine("1. Easy  2. Medium  3. Hard");
                Console.Write("Select level: ");
                QuestionLevel level = (QuestionLevel)ReadInt(1, 3);

                Console.Write("Question header: ");
                string header = Console.ReadLine();

                Console.Write("Marks: ");
                int marks = ReadInt(1, 1000);

                if (type == 1)
                {
                    TrueFalseQuestion q = new TrueFalseQuestion();
                    q.Header = header;
                    q.Marks = marks;
                    q.Level = level;

                    Console.Write("Correct answer (1. True  2. False): ");
                    int c = ReadInt(1, 2);
                    q.CorrectAnswer = (c == 1);

                    questionBank[bankCount] = q;
                    bankCount++;
                }
                else if (type == 2)
                {
                    ChooseOneQuestion q = new ChooseOneQuestion();
                    q.Header = header;
                    q.Marks = marks;
                    q.Level = level;

                    for (int j = 0; j < 4; j++)
                    {
                        Console.Write("Choice " + (j + 1) + ": ");
                        q.Choices[j] = Console.ReadLine();
                    }

                    Console.Write("Correct choice number (1-4): ");
                    q.CorrectChoice = ReadInt(1, 4);

                    questionBank[bankCount] = q;
                    bankCount++;
                }
                else
                {
                    MultipleChoiceQuestion q = new MultipleChoiceQuestion();
                    q.Header = header;
                    q.Marks = marks;
                    q.Level = level;

                    for (int j = 0; j < 4; j++)
                    {
                        Console.Write("Choice " + (j + 1) + ": ");
                        q.Choices[j] = Console.ReadLine();
                    }

                    Console.Write("Correct answers (example 1,3): ");
                    int[] answers = ParseArray(Console.ReadLine());
                    q.correctCount = 0;
                    for (int j = 0; j < answers.Length && j < 4; j++)
                    {
                        q.CorrectAnswers[j] = answers[j];
                        q.correctCount++;
                    }

                    questionBank[bankCount] = q;
                    bankCount++;
                }
            }

            Console.WriteLine("\nQuestions saved. Back to main menu...");
        }

        static void StudentMode()
        {
            Console.WriteLine("\n--- Student Mode ---");
            Console.WriteLine("1. Practical  2. Final");
            Console.Write("Choose exam type: ");
            int examType = ReadInt(1, 2);

            Console.WriteLine("1. Easy  2. Medium  3. Hard");
            Console.Write("Choose level: ");
            QuestionLevel level = (QuestionLevel)ReadInt(1, 3);

            Question[] questions = new Question[bankCount];
            int levelCount = 0;
            for (int i = 0; i < bankCount; i++)
            {
                if (questionBank[i].Level == level)
                {
                    questions[levelCount] = questionBank[i];
                    levelCount++;
                }
            }

            if (levelCount == 0)
            {
                Console.WriteLine("No questions for this level.");
                return;
            }

            int numberOfQuestions = levelCount;
            if (examType == 1)
            {
                numberOfQuestions = levelCount / 2;
                if (numberOfQuestions == 0)
                    numberOfQuestions = 1;
            }

            int totalMarks = 0;
            int studentMarks = 0;

            for (int i = 0; i < numberOfQuestions; i++)
            {
                totalMarks += questions[i].Marks;
                studentMarks += questions[i].CheckAnswer();
            }

            Console.WriteLine("\n===== Your Result: " + studentMarks + " / " + totalMarks + " =====");
        }

        public static int ReadInt(int min, int max)
        {
            int value;
            while (!int.TryParse(Console.ReadLine(), out value) || value < min || value > max)
            {
                Console.Write("Invalid input, enter a number between " + min + " and " + max + ": ");
            }
            return value;
        }

        public static int[] ParseArray(string input)
        {
            string[] parts = input.Split(',');
            int[] temp = new int[parts.Length];
            int count = 0;

            for (int i = 0; i < parts.Length; i++)
            {
                int n;
                if (int.TryParse(parts[i].Trim(), out n))
                {
                    temp[count] = n;
                    count++;
                }
            }

            int[] result = new int[count];
            for (int i = 0; i < count; i++)
                result[i] = temp[i];
            return result;
        }
    }
}