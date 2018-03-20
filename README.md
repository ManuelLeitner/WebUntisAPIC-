# WebUntis_API_C_Sharp

This repo contains a the implementation of a WebUntisClient written in C#.

#WebUntis JSON-RPC API
an API for accessing WebUntis functionality based on [JSON-RPC 2.0]( http://groups.google.com/group/json-rpc/web/json-rpc-2-0 )

Service-URL: http(s)://<SERVER>/WebUntis/jsonrpc.do

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

## Authentication
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


