import { Component, OnInit } from '@angular/core';
import { ImageSelectorService } from './image-selector.service';
import { Observable } from 'rxjs';
import { BlogImage } from '../models/blog-Image.model';
import { environment } from '../../../../environments/environment';
import {  ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';


@Component({
  selector: 'app-image-selector',
  standalone: false,
  
  templateUrl: './image-selector.component.html',
  styleUrl: './image-selector.component.css'
})
export class ImageSelectorComponent implements OnInit {

 private  file?: File;

 images$? : Observable<BlogImage[]> 

  fileName: string = '';
  title: string = '';
    
  constructor(private imageSelectorService: ImageSelectorService){

  }
  @ViewChild('form',{static :false}) imageUploadForm?: NgForm;


  ngOnInit(): void {
   this.getImages();
  }

  

  onFileUpolad(event : Event):void{
    const element = event.currentTarget as HTMLInputElement;
    this.file = element.files?.[0];
  }

  uploadImage(): void{

    if(this.file && this.fileName !== '' && this.title !== ''){
      this,this.imageSelectorService.uploadImages(this.file,this.fileName,this.title)
      .subscribe({
            next: (data) =>{
              this.getImages();
              this.imageUploadForm?.resetForm();
            },
          })

    }

  }
  getImages() {
    this.images$ = this.imageSelectorService.getAllImages();
  }
  selectImage(image: BlogImage): void {
    this.imageSelectorService.selectImage(image);
  }

}
