export interface IImage {
    id: string;
    title: string;
    fileName: string;
    category: string;
}

export interface IGalleryIndexViewModel {
    images: IImage[];
    imagesUri: string;
}

export interface IEditImageViewModel {
    id: string;
    title: string;
    category: string;
}