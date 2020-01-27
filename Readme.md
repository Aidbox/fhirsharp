## Aidbox.Generator 

generates classes for working with the Aidbox API through Aidbox.RestClient.

Compile the project and run it binary with the required connection parameters, for example: 
address=http://127.0.0.1:8888 username=admin password=secret clientid="dev_api" clientsecret="secret"
The generator obtain metadata for the actual Aidbox entitites including custom properties 
and create .cs files for them in the GeneratedResources and GeneratedTypes subfolders.
These classes can be used to work with Aidbox through Aidbox.RestClient.


## Aidbox.RestClient

a simple rest client for working with Aidbox server entities.


## Aidbox.SampleApp 

a sample appliction for testing Aidbox.RestClient work.

First generate api-classes by Aidbox.Generator.
Then uncomment the code in the testRun method or write own code for work with AidBox entities.
