import { HttpClient } from '@angular/common/http';
import { Injectable, ViewChild } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { BlogImage } from '../models/blog-Image.model';
import { NgForm } from '@angular/forms';

@Injectable({
  providedIn: 'root'
})
export class ImageSelectorService {
  selectedImage: BehaviorSubject<BlogImage> = new BehaviorSubject<BlogImage>({
    id: '',
    fileExtension:'',
    fileName:'',
    title:'',
    url:''
  })

  constructor(private http: HttpClient) {


   }

  uploadImages(file:File,fileName :string,title :string):Observable<BlogImage>{
    const formData = new FormData();
    formData.append('file', file);
    formData.append('fileName', fileName);
    formData.append('title', title);
    return this.http.post<BlogImage>(`${environment.apiBaseUrl}/api/image`, formData)
    
  }

  getAllImages() : Observable<BlogImage[]>{
    return this.http.get<BlogImage[]>(`${environment.apiBaseUrl}/api/image`)

  }

  selectImage(image: BlogImage): void{
    this.selectedImage.next(image)
  }
  onSelectImage(): Observable<BlogImage>{
    return this.selectedImage.asObservable()
  }

}
