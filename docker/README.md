## ImageGallery Client Docker

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

