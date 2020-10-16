Simple command line exe.
Run the exe and you are asked for a band or artist name, type in your query and press return. The tool then calculates the results.

Requirements

Newtonsoft.Json 12.0.3 - this should be automatically installed via Nuget, but have only tested it on my local machine as that is all I have access to.
(https://www.newtonsoft.com/json)

Dot Net Core - https://dotnet.microsoft.com/download/dotnet-core/?utm_source=getdotnetcore&utm_medium=referral


TODO

Improved error handling - if you don't enter a name or enter one that is too long (256 is the maximum length) then the program will exit, albeit with an error message.

More info - at the moment, just does a simple mean average calculation.  Can easily extend (data is available) to use median or modal averages, variance & standard dev, 
min & max or produce counts over ranges.  For extra bonus points, could export this into a format for reading in a tool such as R and write a simple script to draw some graphical interpretations.

More rigorous testing of different bands - all the artists I tried worked (a couple gave errors due to missing data, which I've handled) but for a full release, 
I would want a proper QA run through

Have only tried the exe on my local machine.  Ideally, would test it on a clean machine to see if there are any runtimes etc needed.  
At the moment, I don't have ready access to one.

Testing to destruction - I have tested it with a band like Queen (258 songs, avg 237) and Bob Dylan (330 songs, avg 270) who have a lot of songs and albums and it seemed to work

Speed improvements - 
	add support for storing songs & artists we have already parsed.  Probably best to check count on initial artist query to see if any new songs have been added.
	report partial results - sort songs by popularity (frequency on albums?) and only check the top 20 for example?

Release testing
	https://github.com/dotnet/core/blob/master/Documentation/prereqs.md
	Have only tested it on my local machine (windows 10 PC) - obviously for a full release, this isn't enough!

