# twitter-api

*** Before running please place the twitter bearer token in the appsettings at line 10:
 "TwitterBearer": "",
 
 Overall it is a cool challenge. I have spent a significant amount of time (probably close to 30-40 hours) on designing a scale-able solution that will be easy to extend (which I have done when adding new handlers like author and language)
 
 Things I have considered:
 

- I took an approach of CQRS: splitting the reads and writes. I have created 2 repositories, one for reading and one for writing.
   In this solution it's a simple in-memory collections, but in real production code you can replace those.
 - I took an event driven approach that when tweets are consumed (batch size is defined as 50, but can be changed in the appsettings file); as the poller is getting tweets, an event is published. This approach allows us to easily add a new cosumers in very litte effort. currently I have 4 handlers (AllTweets, Author, HashTags and Language). This is following Eventual Consistency appraoch which is just fine (in my mind) for this kind of solution. I prefer being more flexibly and scale-able and having a ~0.5 second delay in the data consistency, than strong consistency. In my mind this is not a banking solution that needs that strong consistency.
- for adding tweetsIds for hashtags - I don't check for uniqueness of tweetIds (easy to add but lack of time on my side). I can assume we won't get duplicates but it is easy to handle.
- SOLID - I have used most or all of the principles. I think the ability to just add a new handler (just like I added the language handler) to persist more data is a good example of open for extension and single responsability
- exceptions - will bubble up on the controllers or handlers. there is a lot I can improve but again I have spent a lot of time on other tings
- tests - added some, but again lack of time
- you can use swagger to run the APIs and there is also console output that will log the poller
 
 
