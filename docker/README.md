## ImageGallery Client Docker

### Docker Compose 

#### Staging Build 
```
cd  ImageGallery.Client/
docker-compose --file docker/imagegallery-client-local.dockerfile/image-gallery-compose.yml  pull
docker-compose --file docker/imagegallery-client-local.dockerfile/image-gallery-compose.yml  up
```







### Base Image
```
docker pull stuartshay/imagegallery-client:2.0-base
```

### Build Image 
```
docker pull stuartshay/imagegallery-client:2.0-build
```

### Inspect Image 
```
docker run -i -t --entrypoint /bin/bash <IMAGEID>  
docker run -i -t --entrypoint cmd <WIN-IMAGEID> 
``` 

### Inspect Container 
```
docker inspect -f '{{ .Created }}' <CONTAINERID> 
docker logs <CONATINERID>
```

