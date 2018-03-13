using System;
using System.Linq;
using System.Net;
using System.Windows.Media;
using Newtonsoft.Json;

namespace WebUntis.Connector.Requests {
	#region Answer

	public class Class {
		[JsonProperty(PropertyName = "id")]
		public int Id { get; set; }
		[JsonProperty(PropertyName = "name")]
		public string Name { get; set; }
		[JsonProperty(PropertyName = "longName")]
		public string LongName { get; set; }
		[JsonProperty(PropertyName = "active")]
		public bool Active { get; set; }
		public override string ToString() {
			return $"{Name} ({LongName})";
		}
	}

	public class Answer<T> {
		[JsonProperty(PropertyName = "jsonrpc")]
		public string Jsonrpc { get; set; }
		[JsonProperty(PropertyName = "id")]
		public string Id { get; set; }
		[JsonProperty(PropertyName = "result")]
		public T Result { get; set; }
	}

	public class ErrorAnswer {
		[JsonProperty(PropertyName = "jsonrpc")]
		public string Jsonrpc { get; set; }
		[JsonProperty(PropertyName = "id")]
		public string Id { get; set; }
		[JsonProperty(PropertyName = "error")]
		public Error Error { get; set; }
	}

	public class Credentials {
		[JsonProperty(PropertyName = "sessionId")]
		public string SessionId { get; set; }
		[JsonProperty(PropertyName = "personType")]
		public int PersonType { get; set; }
		[JsonProperty(PropertyName = "personId")]
		public int PersonId { get; set; }
		[JsonProperty(PropertyName = "klasseId")]
		public int ClassId { get; set; }

		public override string ToString() {
			return $"SessionId: {SessionId} PersonType: {PersonType} PersonId {PersonId} KlasseId: {ClassId}";
		}
	}


	public class Error {
		public Error() { }
		[JsonProperty(PropertyName = "message")]
		public string Message { get; set; }
		[JsonProperty(PropertyName = "code")]
		public int Code { get; set; }

		public static implicit operator Exception(Error error) {
			if(error == null)
				return null;
			return new Exception(error.ToString());
		}
		public override string ToString() {
			return $"{Message} ({Code})";
		}
	}


	public class TimeTable {
		[JsonProperty(PropertyName = "jsonrpc")]
		public string Jsonrpc { get; set; }
		[JsonProperty(PropertyName = "id")]
		public string Id { get; set; }
		[JsonIgnore]
		public int Days {
			get {
				DateTime maxDate = Fields.Max((field) => field.EndTime);
				return (maxDate - MinDate).Days + 1;
			}
		}
		[JsonIgnore]
		public DateTime MinDate {
			get {
				return Fields.Min((field) => field.StartTime);
			}
		}
		[JsonIgnore]
		public int EarliestLessonMinute {
			get {
				return Fields.Min((field) => MinutesOfDay(field.StartTime));
			}
		}

		[JsonIgnore]
		public int LongestDayMinutes {
			get {
				int maxLesson = Fields.Max((field) => MinutesOfDay(field.EndTime));
				return (maxLesson - EarliestLessonMinute) + 1;
			}
		}
		[JsonProperty(PropertyName = "result")]
		public TimeTableField[] Fields { get; set; }
		public static int MinutesOfDay(DateTime date) {
			return date.Hour * 60 + date.Minute;
		}
	}


	public class Subject {
		[JsonProperty(PropertyName = "id")]
		public int Id { get; set; }
		[JsonProperty(PropertyName = "name")]
		public string Name { get; set; }
		[JsonProperty(PropertyName = "longName")]
		public string LongName { get; set; }
		[JsonProperty(PropertyName = "alternateName")]
		public string AlternateName { get; set; }
		[JsonProperty(PropertyName = "active")]
		public bool Active { get; set; }
		[JsonIgnore]
		public Color ForegroundColor { get; set; }
		[JsonIgnore]
		public Color BackgroundColor { get; set; }

		[JsonProperty(PropertyName = "foreColor")]
		public string Foreground {
			set {
				ForegroundColor = (Color) ColorConverter.ConvertFromString("#" + value);
			}
		}
		[JsonProperty(PropertyName = "backColor")]
		public string Background {
			set {
				BackgroundColor = (Color) ColorConverter.ConvertFromString("#" + value);
			}
		}

		public override string ToString() {
			return Name;
		}
	}


	public class TimeTableField {

		[JsonIgnore]
		public DateTime StartTime { get { return DateTime.ParseExact(String.Format("{0:00000000}{1:0000}", Date, Start), "yyyyMMddHHmm", null); } }
		[JsonIgnore]
		public DateTime EndTime { get { return DateTime.ParseExact(String.Format("{0:00000000}{1:0000}", Date, End), "yyyyMMddHHmm", null); } }

		[JsonProperty(PropertyName = "id")]
		public int Id { get; set; }
		[JsonProperty(PropertyName = "date")]
		public string Date { get; set; }
		[JsonProperty(PropertyName = "startTime")]
		public int Start { set; get; }
		[JsonProperty(PropertyName = "endTime")]
		public int End { set; get; }
		[JsonProperty(PropertyName = "code")]
		public string Code { get; set; }
		[JsonProperty(PropertyName = "kl")]
		public ID[] Classes { get; set; }
		[JsonProperty(PropertyName = "te")]
		public ID[] Teachers { get; set; }
		[JsonProperty(PropertyName = "su")]
		public ID[] Subjects { get; set; }
		[JsonProperty(PropertyName = "ro")]
		public ID[] Rooms { get; set; }
		[JsonProperty(PropertyName = "statflags")]
		public string Flags { get; set; }
	}

	public class ID {
		[JsonProperty(PropertyName = "id")]
		public int Id { get; set; }

		public static implicit operator int(ID id) {
			return id.Id;
		}
	}

	public class Room {
		[JsonProperty(PropertyName = "id")]
		public int Id { get; set; }
		[JsonProperty(PropertyName = "name")]
		public string Name { get; set; }
		[JsonProperty(PropertyName = "longName")]
		public string LongName { get; set; }
		[JsonProperty(PropertyName = "active")]
		public bool Active { get; set; }
		[JsonProperty(PropertyName = "did")]
		public int Did { get; set; }
		[JsonProperty(PropertyName = "building")]
		public string Building { get; set; }
	}


	public class Holiday {
		[JsonProperty(PropertyName = "id")]
		public int Id { get; set; }
		[JsonProperty(PropertyName = "name")]
		public string Name { get; set; }
		[JsonProperty(PropertyName = "longName")]
		public string LongName { get; set; }

		[JsonIgnore]
		public DateTime StartDate { get; set; }
		[JsonIgnore]
		public DateTime EndDate { get; set; }


		[JsonProperty(PropertyName = "startDate")]
		public string Start {
			set {
				StartDate = DateTime.ParseExact(value, "yyyyMMdd", null);
			}
		}
		[JsonProperty(PropertyName = "endDate")]
		public string End {
			set {
				EndDate = DateTime.ParseExact(value, "yyyyMMdd", null);
			}
		}
	}

	#endregion

	#region Requests
	public class Request {
		private RequestType method;

		public Request(RequestType method, int id) {
			this.method = method;
			Id = id;
		}
		public Request(IRequestable _params) {
			Params = _params;
		}
		public Request(IRequestable _params, int id) {
			Params = _params;
			Id = id;
		}

		public Request(RequestType method) {
			this.method = method;
		}

		[JsonProperty(PropertyName = "method")]
		public string Method {
			get {
				if(Params == null)
					return method;
				return Params.Method;
			}
		}
		[JsonProperty(PropertyName = "id")]
		public int Id { get; } = 1;
		[JsonProperty(PropertyName = "jsonrpc")]
		public string Jsonrpc { get; } = "2.0";
		[JsonProperty(PropertyName = "params")]
		public IRequestable Params { get; set; }
	}

	public interface IRequestable {
		[JsonIgnore]
		RequestType Method { get; }
	}

	[JsonObject(MemberSerialization.OptIn)]
	public class User : NetworkCredential, IRequestable {
		public User() { }

		public User(string username, string password) {
			UserName = username;
			Password = password;
		}

		public RequestType Method { get; } = RequestType.Authenticate;
		[JsonProperty(PropertyName = "password")]
		public new string Password { get { return base.Password; } set { base.Password = value; } }
		[JsonProperty(PropertyName = "user")]
		public new string UserName { get { return base.UserName; } set { base.UserName = value; } }
	}

	public enum TimeTableType : byte {
		CLASS = 1, TEACHER = 2, SUBJECT = 3, ROOM = 4, STUDENT = 5
	}


	public sealed class RequestType {
		public string Method { get; set; }
		public static implicit operator RequestType(string requestType) {
			return new RequestType() { Method = requestType };
		}
		public static implicit operator string(RequestType r) {
			return r.Method;
		}
		public static readonly RequestType Classes = "getKlassen", Authenticate = "authenticate", Timetable = "getTimetable",
			Subjects = "getSubjects", Logout = "logout", Rooms = "getRooms", Departments = "getDepartments", Holidays = "getHolidays",
			Units = "getTimegridUnits", Status = "getStatusData", Schoolyear = "getCurrentSchoolyear", PersonId = "getPersonId", Teachers = "getTeachers";
	}
	public class TimeTableSpecification : IRequestable {

		public TimeTableSpecification(TimeTableType type, DateTime? start, DateTime? end, int id) {
			StartDate = start;
			EndDate = end;
			Id = id;
			Type = type;
		}

		[JsonIgnore]
		public User User { get; set; }
		[JsonIgnore]
		public DateTime? StartDate { get; set; }
		[JsonIgnore]
		public DateTime? EndDate { get; set; }
		[JsonProperty(PropertyName = "id")]
		public int Id { get; set; }
		[JsonProperty(PropertyName = "type")]
		public TimeTableType Type { get; set; }
		[JsonProperty(PropertyName = "startDate")]
		public string Start { get { return StartDate?.ToString("yyyyMMdd"); } }
		[JsonProperty(PropertyName = "endDate")]
		public string End { get { return EndDate?.ToString("yyyyMMdd"); } }
		public RequestType Method { get; } = RequestType.Timetable;
	}



	#endregion

}
