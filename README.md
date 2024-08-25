# SkillMill.NET

---

## Overview
SkillMill.NET is a powerful .NET-based project that streamlines and refines the process of developing coding skills,
much like a mill processing raw materials into polished, usable products.
Just as a traditional mill grinds grain into flour,
SkillMill.NET takes complex programming tasks, such as API development, database querying, and breaks them down into manageable,
optimized code.

This project is aimed at developers who want to improve their .NET programming techniques, embrace efficient coding practices, and create refined, production-ready applications faster and with greater precision.

---
## Commands

### Create New Migration

To create a new migration, use the following command:

```bash
dotnet ef migrations add AddRowVersionToCustomer --project SkillMill.Data.EF --startup-project SkillMill.API
```

---
## Sieve
- [Sieve Documentation](https://github.com/Sinet2000/Sieve)
- [How to work with the array?](https://github.com/Biarity/Sieve/issues/2)


### How to make a query?
```curl
GET /YOUR-ENDPOINT

?sorts=     LikeCount,CommentCount,-created         // sort by likes, then comments, then descendingly by date created 
&filters=   LikeCount>10, Title@=awesome title,     // filter to posts with more than 10 likes, and a title that contains the phrase "awesome title"
&page=      1                                       // get the first page...
&pageSize=  10                                      // ...which contains 10 posts

```

### Operators
| Operator   | Meaning                  |
|------------|--------------------------|
| `==`       | Equals                   |
| `!=`       | Not equals               |
| `>`        | Greater than             |
| `<`        | Less than                |
| `>=`       | Greater than or equal to |
| `<=`       | Less than or equal to    |
| `@=`       | Contains                 |
| `_=`       | Starts with              |
| `_-=`      | Ends with                |
| `!@=`      | Does not Contains        |
| `!_=`      | Does not Starts with     |
| `!_-=`     | Does not Ends with       |
| `@=*`      | Case-insensitive string Contains |
| `_=*`      | Case-insensitive string Starts with |
| `_-=*`     | Case-insensitive string Ends with |
| `==*`      | Case-insensitive string Equals |
| `!=*`      | Case-insensitive string Not equals |
| `!@=*`     | Case-insensitive string does not Contains |
| `!_=*`     | Case-insensitive string does not Starts with |

### Handle Sieve's exceptions

Sieve will silently fail unless `ThrowExceptions` in the configuration is set to true. 3 kinds of custom exceptions can be thrown:

* `SieveMethodNotFoundException` with a `MethodName`
* `SieveIncompatibleMethodException` with a `MethodName`, an `ExpectedType` and an `ActualType`
* `SieveException` which encapsulates any other exception types in its `InnerException`