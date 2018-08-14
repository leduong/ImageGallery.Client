export interface IImage {
    id: string;
    title: string;
    fileName: string;
    category: string;
}

export interface IAlbum {
    id: string;
    title: string;
    descritpion: string;
    photoUrl: string;
}

export interface IRouteTypeModel {
    type: string;
}

export interface IAlbumIndexViewModel {
    images: IImage;
    imagesUri: string;
}

export interface IGalleryIndexViewModel {
    images: IAlbum[];
    imagesUri: string;
}

export interface IEditImageViewModel {
    id: string;
    title: string;
    category: string;
}

export interface IAddImageViewModel {
    title: string;
    category: string;
    file: File;
}

export interface IAlbumViewModel {
    id: string;
    title: string;
    fileName: string;
}
