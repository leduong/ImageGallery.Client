node('docker') {

    stage('Git checkout') {
        git branch: 'NG5', credentialsId: 'gihub-key', url: 'git@github.com:stuartshay/ImageGallery.Client.git'
    }

    stage('Package Diff') {
       echo "Package Diff Here  *.csproj or .json"
       echo "Fail Build or Call Base Image Build"
    }

    stage('Build & Deploy Docker') {
         sh '''mv docker/imagegallery-client-build.dockerfile/.dockerignore .dockerignore
         docker build -f docker/imagegallery-client-build.dockerfile/Dockerfile --build-arg BUILD_NUMBER=${BUILD_NUMBER} -t stuartshay/imagegallery-client:2.0-build .'''
        withCredentials([usernamePassword(credentialsId: 'docker-hub-navigatordatastore', usernameVariable: 'DOCKER_HUB_LOGIN', passwordVariable: 'DOCKER_HUB_PASSWORD')]) {
            sh "docker login -u ${DOCKER_HUB_LOGIN} -p ${DOCKER_HUB_PASSWORD}"
        }
        sh '''docker push stuartshay/imagegallery-client:2.0-build'''
    }


  //  stage ('Deploy to Rancher') {
  //      withCredentials([usernamePassword(credentialsId: 'JENKINS_ENV_KEY', passwordVariable: 'RANCHER_SECRET_KEY', usernameVariable: 'RANCHER_ACCESS_KEY')]) {
  //          sh 'rancher-compose --url https://rancher.navigatorglass.com/v2-beta/projects/1a5  -f docker/rancher/NavigatorIdentity/docker-compose.yml --project-name NavigatorIdentity up navigator-identity-api --force-upgrade -p -c -d'
  //      }
  //  }


   stage('Performance Metrics') {
       echo "apache ab docker"
       echo "See: http://imagegallery-client.informationcart.com/swagger/#!/Diagnostics/ApiDiagnosticsGet"
       echo "Build Report Get Host Name => Run 100x => Attach to Email"
    }


    stage('Mail') {
        emailext attachLog: true, body: '', subject: 'Jenkins build status - navigator-identity-api', to: 'sshay@yahoo.com'
    }

}
