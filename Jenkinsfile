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

        stage ('Build Debug') 
        {
            steps 
            {
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
                dotnetTest logger: 'trx;LogFileName=test.trx', project: 'src/Tests/DatenMeister.Tests/DatenMeister.Tests.csproj', continueOnError: true
                dotnetTest logger: 'trx;LogFileName=test.web.trx', project: 'src/Tests/DatenMeister.Tests.Web/DatenMeister.Tests.Web.csproj', continueOnError: true

                mstest()
            }
            
        }

        stage ('Test Release')
        {
            steps
            {
                dotnetTest logger: 'trx;LogFileName=test.trx', project: 'src/Tests/DatenMeister.Tests/DatenMeister.Tests.csproj', configuration: 'Release', continueOnError: true
                dotnetTest logger: 'trx;LogFileName=test.web.trx', project: 'src/Tests/DatenMeister.Tests.Web/DatenMeister.Tests.Web.csproj', configuration: 'Release', continueOnError: true

                mstest()
            }
            
        }
    }
}