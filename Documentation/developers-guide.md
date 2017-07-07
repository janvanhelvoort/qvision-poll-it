# Developers Guide

## Contents

* [Introduction](#introduction)
* [Get the value](#get-the-value)
* [Service](#service)

---

### Introduction

**Qvision Poll-it** is an backoffice extension for umbraco, its at the functionality to umbraco for create, edit en delete polls inside a custom section. The polls can be picked inside the content with af custom propery editor wich is included.

The poll can be acced eather way strongly typed with a custom property value converter or with the service.

With the service, a vote can registered to the poll, therefore you need the `question id` and `answer id`.

---

### Get the value
The value stored in the property is the `id` of the question wich is selected in the content editor.

#### Value Conveter
```csharp
@Model.GetPropertValue<Qvision.Umbraco.PollIt.Models.Question>("poll");
```

#### Dynamic

```csharp
var question = PollItService.Current.GetQuestion(Model.content.poll)
```

##### Result
```javascript
{
   "Id":3,
   "Name":"What version of umbraco do u use",
   "StartDate":null,
   "EndDate":null,
   "CreatedDate":"\/Date(1499371861200)\/",
   "Responses":9,
   "Answers":[
      {
         "Id":7,
         "Value":"6.2.6",
         "Index":1,
		 "Percentage":11,
         "Responses":[
            {
               "Id":25,
               "ResponseDate":"\/Date(1499299200000)\/"
            }
         ]
      },
      {
         "Id":8,
         "Value":"7.1.0",
         "Index":2,
		 "Percentage":22,
         "Responses":[
            {
               "Id":22,
               "ResponseDate":"\/Date(1499299200000)\/"
            },
            ...
         ]
      },
      ...
   ]
}
```

### Service
The service has 2 methods, one for getting the value and one for voting.

#### Get the value
```csharp
var question = PollItService.Current.GetQuestion(Model.content.poll)
```
You receive the same model as the example above

#### Vote
```csharp
var question = PollItService.Current.Vote(questionId, answerId);
```
You receive the same model as if you call the `getQuestion` method