pipeline {
    agent any

    stages {
	    stage ('Build') 
        {
            steps 
            {
 			    // Shell build step
                dotnetBuild project: 'datenmeister-new.sln', workDirectory: 'src'
            }
	    }

        stage ('Test')
        {
            steps
            {
                dotnetTest logger: 'trx;LogFileName=test.trx', project: 'src/Tests/DatenMeister.Tests/DatenMeister.Tests.csproj'

                mstest()
            }
            
        }
    }
}