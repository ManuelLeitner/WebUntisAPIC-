using System;
using System.Collections.Generic;
using WebUntis.Connector.Requests;

namespace WebUntis.Connector {
    class Program {
        static void Main(string[] args) {
            WebUntisConnector connector = new WebUntisConnector(new User("", ""));
            connector.Authenticate();

            IDictionary<int, Class> classes = connector.Classes;

            HashSet<TeacherSubject> teacherSubjects = new HashSet<TeacherSubject>();


            foreach (Class c in classes.Values) {
                TimeTable ti = connector.GetClassTimeTable(c, DateTime.Now, new DateTime(2017, 2, 1));
                foreach (TimeTableField ttf in ti.Fields) {
                    foreach (ID s in ttf.Subjects) {
                        foreach (ID t in ttf.Subjects) {
                            teacherSubjects.Add(new TeacherSubject(s.Id, t.Id, c.Id));
                        }
                    }
                }
            }

            foreach (TeacherSubject s in teacherSubjects) {
                Console.Write(s.ToString());
                Console.Write("\t" + connector.Classes[s.Class]);
                Console.WriteLine("\t" + connector.Subjects[s.Subject]);

            }
            Console.ReadKey();
        }
    }

    public struct TeacherSubject {
        public int Subject, Teacher, Class;

        public TeacherSubject(int subject, int teacher, int _class) {
            this.Subject = subject;
            this.Teacher = teacher;
            this.Class = _class;
        }

        public override string ToString() {
            return $"{Subject}\t{Teacher}\t{Class}";
        }
    }
}
