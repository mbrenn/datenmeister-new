pipeline {
    agent any

    stages {
        
        stage('NPM install')
        {
            steps
            {                    
                sh """                 
                    cd src/DatenMeister.Reports.Forms
                    npm install
                    cd ../..

                    cd src/Web/DatenMeister.WebServer
                    npm install 
                    cd ../../..
                """
            }
        }

        
        stage ('Typescript')
        {   
            steps{
                sh """ 
                    cd src/DatenMeister.Reports.Forms
                    tsc
                    cd ../..
                    cd src/Web/DatenMeister.WebServer
                    tsc
                    cd ../../..
                """
            }
        }


        stage('Cake Install')
        {
            steps
            {                    
                sh """                 
                    cd src/DatenMeister.Reports.Forms
                    dotnet new tool-manifest --force
                    dotnet tool install Cake.Tool --version 5.0.0
                    cd ../..

                    cd src/Web/DatenMeister.WebServer
                    dotnet new tool-manifest --force
                    dotnet tool install Cake.Tool --version 5.0.0
                    dotnet tool install BS_Remove_File_Attribute_From_JUnit
                    cd ../../..
                """
            }
        }

        stage ('Build Debug') 
        {
            steps 
            {
                // Shell build step
                dotnetBuild project: 'datenmeister-new.slnx', workDirectory: 'src'
            }
        }

        stage ('Build Release')
        {
            steps
            {

                dotnetBuild configuration: 'Release', project: 'datenmeister-new.slnx', workDirectory: 'src'
            }
        }

        stage ('Test Debug')
        {
            steps
            {
                dotnetTest logger: 'trx;LogFileName=test.trx', project: 'src/Tests/DatenMeister.Tests/DatenMeister.Tests.csproj', continueOnError: true
                dotnetTest logger: 'trx;LogFileName=test.core.typeindexstorage.trx', project: 'src/Tests/DatenMeister.Core.TypeIndexStorage.Tests/DatenMeister.Core.TypeIndexStorage.Tests.csproj', continueOnError: true
                dotnetTest logger: 'trx;LogFileName=test.web.trx', project: 'src/Tests/DatenMeister.Tests.Web/DatenMeister.Tests.Web.csproj', continueOnError: true
                dotnetTest logger: 'trx;LogFileName=test.provider.json.trx', project: 'src/Tests/DatenMeister.Provider.Json.Test/DatenMeister.Provider.Json.Test.csproj', continueOnError: true
                dotnetTest logger: 'trx;LogFileName=test.domains.trx', project: 'src/Tests/DatenMeister.Domains.Tests/DatenMeister.Domains.Tests.csproj', continueOnError: true
                dotnetTest logger: 'trx;LogFileName=test.issuemeister.trx', project: 'src/Tests/IssueMeisterLib.Tests/IssueMeisterLib.Tests.csproj', continueOnError: true
                dotnetTest logger: 'trx;LogFileName=test.timecacheddictionary.trx', project: 'src/BurnSystems.TimeCachedDictionary.Tests/BurnSystems.TimeCachedDictionary.Tests.csproj', continueOnError: true

                mstest()
            }
        }

        stage ('Test Release')
        {
            steps
            {
                dotnetTest logger: 'trx;LogFileName=test.trx', project: 'src/Tests/DatenMeister.Tests/DatenMeister.Tests.csproj', configuration: 'Release', continueOnError: true
                dotnetTest logger: 'trx;LogFileName=test.core.typeindexstorage.trx', project: 'src/Tests/DatenMeister.Core.TypeIndexStorage.Tests/DatenMeister.Core.TypeIndexStorage.Tests.csproj', configuration: 'Release', continueOnError: true
                dotnetTest logger: 'trx;LogFileName=test.web.trx', project: 'src/Tests/DatenMeister.Tests.Web/DatenMeister.Tests.Web.csproj', configuration: 'Release', continueOnError: true
                dotnetTest logger: 'trx;LogFileName=test.provider.json.trx', project: 'src/Tests/DatenMeister.Provider.Json.Test/DatenMeister.Provider.Json.Test.csproj', configuration: 'Release', continueOnError: true
                dotnetTest logger: 'trx;LogFileName=test.domains.trx', project: 'src/Tests/DatenMeister.Domains.Tests/DatenMeister.Domains.Tests.csproj', configuration: 'Release', continueOnError: true
                dotnetTest logger: 'trx;LogFileName=test.issuemeister.trx', project: 'src/Tests/IssueMeisterLib.Tests/IssueMeisterLib.Tests.csproj', configuration: 'Release', continueOnError: true
                dotnetTest logger: 'trx;LogFileName=test.timecacheddictionary.trx', project: 'src/BurnSystems.TimeCachedDictionary.Tests/BurnSystems.TimeCachedDictionary.Tests.csproj', configuration: 'Release', continueOnError: true

                mstest()
            }            
        }

        stage ('JS Tests')
        {
            steps
            {
                sh """
                    cd src/Web/DatenMeister.WebServer
                    npm test
                    dotnet tool run bs-remove-file-attribute-from-junit ./jstests.xml 
                    cd ../../..
                """

                xunit(
                    tools: [JUnit(pattern: 'src/Web/DatenMeister.WebServer/jstests.xml')]
                )
            }
        }
    }
}