using ResultSheet.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ResultSheet
{
    class Program
    {
        static IList<Student> studentList = new List<Student>();
        static IList<StudentResult> ResultList = new List<StudentResult>();
        static string studentFile;
        static string resultFile;

        static void PrepareStudentList()
        {
            try
            {
                Helper helper = new Helper();
                var contentInStudentFile = helper.ReadFile(studentFile);
                foreach (var content in contentInStudentFile)
                {
                    Student s = new Student() { Roll = Int32.Parse(content[0]), Name = content[1], Age = Int32.Parse(content[2]) };
                    studentList.Add(s);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        static void PrepareResultList()
        {
            try
            {
                Helper helper = new Helper();
                var contentInResultFile = helper.ReadFile(resultFile);
                foreach (var content in contentInResultFile)
                {
                    StudentResult r = new StudentResult() { Roll = Int32.Parse(content[0]), Subject = content[1], GP = double.Parse(content[2]) };
                    ResultList.Add(r);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        static void TakeFileNmaes()
        {
            Console.WriteLine("Enter student file name");
            studentFile = Console.ReadLine();
            Console.WriteLine("Enter result file name");
            resultFile = Console.ReadLine();
        }

        static void Main(string[] args)
        {
            TakeFileNmaes();
            PrepareStudentList();
            PrepareResultList();

            var sortedResultList = ResultList.GroupBy(s => new { s.Roll })
                                         .Select(s => new
                                         {
                                             Roll = s.Key.Roll,
                                             GPA = s.Average(z => z.GP)
                                         })
                                         .Join(studentList,
                                               s => s.Roll,
                                               r => r.Roll,
                                               (s, r) => new
                                               {
                                                   name = r.Name,
                                                   roll = s.Roll,
                                                   cg = s.GPA
                                               })
                                         .OrderByDescending(s => s.cg)
                                         .ThenBy(s => s.roll)
                                         .ThenBy(s => s.name);

            var contentsToWrite = new List<string>();
            contentsToWrite.Add("Roll,Name,GPA");
            foreach (var student in sortedResultList)
            {
                contentsToWrite.Add(student.roll + "," + student.name + "," + String.Format("{0:0.00}", student.cg));
            }

            Helper helper = new Helper();
            helper.Print(contentsToWrite);
        }
    }
}
