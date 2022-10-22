pipeline {
    agent any

    stages {
	    stage ('Build Debug') 
        {
            steps 
            {
                sh """ 
                    cd src/Web/DatenMeister.WebServer
                    npm install 
                    cd ../../..
                """

 			    // Shell build step
                dotnetBuild project: 'datenmeister-new.sln', workDirectory: 'src'
            }
	    }

        stage ('Build Release')
        {
            steps
            {
                dotnetBuild configuration: 'Release', project: 'datenmeister-new.sln', workDirectory: 'src'
            }
        }

        stage ('Test Debug')
        {
            steps
            {
                dotnetTest logger: 'trx;LogFileName=test.trx', project: 'src/Tests/DatenMeister.Tests/DatenMeister.Tests.csproj'                
                dotnetTest logger: 'trx;LogFileName=test.web.trx', project: 'src/Tests/DatenMeister.Tests.Web/DatenMeister.Tests.Web.csproj'

                mstest()
            }
            
        }

        stage ('Test Release')
        {
            steps
            {
                dotnetTest logger: 'trx;LogFileName=test.trx', project: 'src/Tests/DatenMeister.Tests/DatenMeister.Tests.csproj', configuration: 'Release'
                dotnetTest logger: 'trx;LogFileName=test.web.trx', project: 'src/Tests/DatenMeister.Tests.Web/DatenMeister.Tests.Web.csproj', configuration: 'Release'

                mstest()
            }
            
        }
    }
}