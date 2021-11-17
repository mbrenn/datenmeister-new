pipeline {
    agent any

    stages {
	    stage ('Build') 
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

        stage ('Test')
        {
            steps
            {
                dotnetTest logger: 'trx;LogFileName=test.trx', project: 'src/Tests/DatenMeister.Tests/DatenMeister.Tests.csproj'                
                dotnetTest logger: 'trx;LogFileName=test.trx', project: 'src/Tests/DatenMeister.Tests.Web/DatenMeister.Tests.Web.csproj'

                mstest()
            }
            
        }
    }
}