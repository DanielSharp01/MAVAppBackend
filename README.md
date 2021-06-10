# MAVAppBackend

ASP.net core server for a train app based on the Hungarian Railway Company's data

## State of the application

At first I moved away from the C# implementation because I wanted to try my hands at implementing this in Node.js. Then I have moved into the city and now fortunately not require the services of MÁV or this app. Nevertheless I will share the motivation of the application and also the concepts of some of the code.

## Motivation for the application

There was no app on the marker that allowed for knowing how much of the train journey you already passed through. I wanted to show a percentage of the train journey maybe some estimated times based on the real speed data and previous data. I unfortunately instead of creating a simple POC created a very general application that would be hard to develop and I got stuck in processing the data from the company instead of implementing any endpoints to request it.

## Parsing logic

Because of the unreliable nature of the data the idea of my architecture was to move the unreliable APIs into a couple of "Statements" that state very basic facts. Like train number A moves through station B at time C. These more simple data points are then aggregated into the database by an aggragating logic. This is the logic that I worked on the most and is mostly finished with only a couple missing features.
