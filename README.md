# WebUntis_API_C_Sharp

This repo contains the implementation of a WebUntisClient written in C#.

## WebUntis JSON-RPC API

an API for accessing WebUntis functionality based on [JSON-RPC 2.0]( http://groups.google.com/group/json-rpc/web/json-rpc-2-0 )

Service-URL: http(s)://\<SERVER\>/WebUntis/jsonrpc.do

requests must use POST

An API request has the general format

``` {"id":"ID","method":"METHOD NAME","params":{PARAMS},"jsonrpc":"2.0"} ```

| Field         | Description                                                   |
|:--------------|:--------------------------------------------------------------|
| ID            | identifies the request, is repeated in the response           |
| METHOD NAME   | name of the method                                            |
| PARAMS        | method parameters as described in the section of the method for simple functions PARAMS can be empty  |

A response has the general format
```{"jsonrpc":"2.0","id":"ID","result":RESULT}```


| Field         | Description                                                   |
|:--------------|:--------------------------------------------------------------|
| ID            | request identifier                                            |
| Result        | result of the method as described in the section of the method |

remarks:
- character set: utf-8
- date format: YYYYMMDD 
- time format: HHMM
- color format: RRGGBB
- fields in the result may be omitted if empty (e.g. foreColor and  backColor will be omitted if 
not set)

Each section defines a method (function) of the API and describes
- purpose 
- method name
- needed right for the user 
- parameters
- result

Additionally there are often given examples and sometimes special remarks.

### Authentication
Authenticate the given user and start a session

| Field         | Value        |
|:--------------|:-------------|
| method name   | authenticate |
| right         | -            |

mandatory parameter: ?school=SCHULNAME

```
{"id":"ID","method":"authenticate","params":{"user":"ANDROID", 
"password":"PASSWORD", "client":"CLIENT"},"jsonrpc":"2.0"}
```

The parameter CLIENT is a unique identifier for the client app. The parameter client will be 
mandatory in the future.
result: 
```
{"jsonrpc":"2.0","id":"ID","result":
{"sessionId":"644AFBF2C1B592B68C6B04938BD26965","personType"=2,"personId"=17}
```



| Field         | Description                                                   |
|:--------------|:--------------------------------------------------------------|
| personType | type of person 2 = teacher, 5 = student |
| personId | ID of person |
| jsessionid | All other methods require the result from authentication ( = sessionId), either per pathparameter or per cookie (RequestHeader)|

### Logout

End the session

| Field         | Value        |
|:--------------|:-------------|
| method name   | logout       |
| right         | -            |

```
{"id":"ID","method":"logout","params":{},"jsonrpc":"2.0"}
```

An application should always logout as soon as possible to free system resources on the server.

### Request teachers

Get list of teachers


| Field         | Value        |
|:--------------|:-------------|
| method name   | getTeachers  |
| right         | -            |
| parameters    | -            |

```
{"id":"ID","method":"getTeachers","params":{},"jsonrpc":"2.0"}
```

```
{"jsonrpc":"2.0","id":"ID","result":[ 
{"id":1,"name":"Bach","foreName":"Ingeborg","longName":"Bachmann","foreColor":"0
00000","backColor":"000000"}, 
{"id":2,"name":"Foss","foreName":"Dian","longName":"Fossey","foreColor":"000000"
,"backColor":"000000"}, 
...]}
```

### Request base classes


| Field         | Value        |
|:--------------|:-------------|
| method name   | getKlassen   |
| right         | -            |
| parameters    | schoolyearId: default current year |

```
{"id":"ID","method":"getKlassen","params": 
{"schoolyearId":"SCHOOLYEARID"},"jsonrpc":"2.0"}
```

```
{"jsonrpc":"2.0","id":"ID","result":[ 
{"id":71,"name":"1A","longName":"Klasse
1A","foreColor":"000000","backColor":"000000",did:2}, 
{"id":72,"name":"1B","longName":"Klasse
1B","foreColor":"000000","backColor":"000000"}, 
...]}
```

### Request subjects

Get list of subjects

| Field         | Value        |
|:--------------|:-------------|
| method name   | getSubjects  |
| right         | -            |
| parameters    | -            |


```
{"jsonrpc":"2.0","id":"ID","result":[ 
{"id":1,"name":"RK","longName":"Kath.Religion","foreColor":"000000","backColor":
"000000"}, 
{"id":2,"name":"RE","longName":"Evang.
Religion","foreColor":"000000","backColor":"000000"}, 
...]}
```


### Request holidays

Get list of holidays

| Field         | Value        |
|:--------------|:-------------|
| method name   | getHolidays  |
| right         | -            |
| parameters    | -            |

```
{"jsonrpc":"2.0","id":"ID","result":[ 
{"id":44,"name":"Natio","longName":"Nationalfeiertag","startDate":20101026,"endD
ate":20101026}, 
{"id":42,"name":"Allerheiligen","longName":"Allerheiligen","startDate":20101101,
"endDate":20101101}, 
...]}
```

### Request timegrid

Get timegrid

| Field         | Value        |
|:--------------|:-------------|
| method name   | getTimegridUnits |
| right         | -            |
| parameters    | -            |

```
{"jsonrpc":"2.0","id":"ID","result":[ 
{"day":0,"timeUnits":[{"startTime":800,"endTime":850},
{"startTime":855,"endTime":945},{"startTime":1000,"endTime":1050}, 
...],
{"day":1,"timeUnits":[{"startTime":800,"endTime":850}, 
...],
...]}
```

1 = sunday, 2 = monday, ..., 7 = saturday
