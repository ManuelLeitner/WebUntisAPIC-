using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using WebUntis.Connector.Requests;

namespace WebUntis.Connector {

    public class WebUntisConnector {
        public User User { get; set; }
        public Credentials Credentials { get; private set; }
        private IDictionary<int, Subject> subjects;
        public IDictionary<int, Subject> Subjects {
            get {
                if (subjects == null)
                    RefreshSubjects();
                return subjects;
            }
            set { subjects = value; }
        }
        private IDictionary<int, Class> classes;
        public IDictionary<int, Class> Classes {
            get {
                if (classes == null)
                    RefreshClasses();
                return classes;
            }
            set { classes = value; }
        }
        private IDictionary<int, Room> rooms;
        public IDictionary<int, Room> Rooms {
            get {
                if (rooms == null)
                    RefreshRooms();
                return rooms;
            }
            set { rooms = value; }
        }
        private IDictionary<int, Holiday> holidays;
        public IDictionary<int, Holiday> Holidays {
            get {
                if (holidays == null)
                    RefreshHolidays();
                return holidays;
            }
            set { holidays = value; }
        }

        public WebUntisConnector(User user) {
            User = user;
        }

        private string CommunicateJson(object o) {
            return CommunicateJson(JsonConvert.SerializeObject(o));
        }
        private string CommunicateJson(string json) {
            return Communicate(json, "application /json; charset=utf-8");
        }

        public void Authenticate() {
            string json = JsonConvert.SerializeObject(new Request(User));
            string answer = CommunicateJson(json);
            Credentials = JsonConvert.DeserializeObject<Answer<Credentials>>(answer).Result;
        }
        public IDictionary<int, Class> RefreshClasses() {
            if (Credentials == null) {
                Authenticate();
            }
            string answer = CommunicateJson(JsonConvert.SerializeObject(new Request(RequestType.Classes, Credentials.ClassId)));
            Class[] classes = JsonConvert.DeserializeObject<Answer<Class[]>>(answer).Result;
            this.classes = new Dictionary<int, Class>();
            foreach (Class c in classes) {
                this.classes[c.Id] = c;
            }
            return this.classes;
        }

        private string Communicate(string post, string contentType) {
            HttpWebRequest request = HttpWebRequest.CreateHttp(Settings.Default.WebUntisUrl);
            request.UserAgent = ".NET Framework Test Client";
            request.ContentType = contentType;
            request.Headers.Add("Accept-Language", Settings.Default.Language);
            if (Credentials != null)
                request.Headers.Add("Cookie", $"JSESSIONID={Credentials.SessionId}");

            request.ContentLength = post.Length;
            request.Method = WebRequestMethods.Http.Post;

            using (var streamWriter = new StreamWriter(request.GetRequestStream())) {
                streamWriter.Write(post);
                streamWriter.Flush();
                streamWriter.Close();
            }

            var httpResponse = (HttpWebResponse)request.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream())) {
                var result = streamReader.ReadToEnd();
                streamReader.Close();
                Error e = null;
                e = JsonConvert.DeserializeObject<ErrorAnswer>(result).Error;
                if (e != null)
                    throw new Exception(e.ToString());
                return result;
            }
        }

        public TimeTable GetClassTimeTable(Class cl, DateTime? start, DateTime? end) {
            TimeTableSpecification specification = new TimeTableSpecification(TimeTableType.CLASS, start, end, cl.Id);
            var request = new Request(specification);
            string answer = CommunicateJson(request);
            return JsonConvert.DeserializeObject<TimeTable>(answer);
        }
        public IDictionary<int, Subject> RefreshSubjects() {
            string answer = CommunicateJson(new Request(RequestType.Subjects));
            Subject[] subjects = JsonConvert.DeserializeObject<Answer<Subject[]>>(answer).Result;
            this.subjects = new Dictionary<int, Subject>();
            foreach (Subject subject in subjects)
                this.subjects[subject.Id] = subject;
            return this.subjects;
        }

        public IDictionary<int, Room> RefreshRooms() {
            string answer = CommunicateJson(new Request(RequestType.Rooms));
            Room[] rooms = JsonConvert.DeserializeObject<Answer<Room[]>>(answer).Result;
            this.rooms = new Dictionary<int, Room>();
            foreach (Room room in rooms)
                this.rooms[room.Id] = room;
            return this.rooms;
        }

        public IDictionary<int, Holiday> RefreshHolidays() {
            string answer = CommunicateJson(new Request(RequestType.Holidays));
            Holiday[] holidays = JsonConvert.DeserializeObject<Answer<Holiday[]>>(answer).Result;
            this.holidays = new Dictionary<int, Holiday>();

            foreach (Holiday holiday in holidays)
                this.holidays[holiday.Id] = holiday;
            return this.holidays;
        }
        public void RefreshTeachers() {
            string answer = CommunicateJson(new Request(RequestType.Teachers));
            Console.WriteLine(answer);
        }

        public void Logout() {
            CommunicateJson(new Request(RequestType.Logout));
        }
    }
}
