pipeline {
    agent any

    tools {
        // pour vérieifer que Jenkins utilise le bon SDK .NET installé 
        dotnet 'dotnet-sdk'
    }

    environment {
        CONFIGURATION = 'Release'
        API_PROJECT = 'Library3.0.API/Library3.0.API.csproj'
        UI_PROJECT = 'Library3.0.UI/Library3.0.UI.csproj'
    }

    stages {
        stage('Checkout') {
            steps {
                git 'https://github.com/Breil11/4DOT/new/main' 
            }
        }

        stage('Restore') {
            steps {
                sh 'dotnet restore'
            }
        }

        stage('Build API') {
            steps {
                sh "dotnet build ${API_PROJECT} --configuration ${CONFIGURATION}"
            }
        }

        stage('Test API') {
            steps {
                sh "dotnet test ${API_PROJECT} --configuration ${CONFIGURATION} --no-build"
            }
        }

        stage('Build UI') {
            steps {
                sh "dotnet build ${UI_PROJECT} --configuration ${CONFIGURATION}"
            }
        }

        stage('Publish') {
            steps {
                sh "dotnet publish ${API_PROJECT} --configuration ${CONFIGURATION} --output ./publish/api"
                sh "dotnet publish ${UI_PROJECT} --configuration ${CONFIGURATION} --output ./publish/ui"
            }
        }

        stage('Archive Artifacts') {
            steps {
                archiveArtifacts artifacts: 'publish/**/*.*', fingerprint: true
            }
        }
    }

    post {
        success {
            echo '✅ Build terminé avec succès !'
        }
        failure {
            echo '❌ Une erreur est survenue pendant le build.'
        }
    }
}
