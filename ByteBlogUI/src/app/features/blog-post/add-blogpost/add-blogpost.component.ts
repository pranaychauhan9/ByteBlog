import { Component, OnDestroy, OnInit } from '@angular/core';
import { AddBlogPostRequest } from '../models/add-blogpost-request.model';
import { BlogpostsService } from '../services/blogposts.service';
import { Router } from '@angular/router';
import { Category } from '../../category/models/category.model';
import { CategoryService } from '../../category/services/category.service';
import { Observable, Subscription } from 'rxjs';
import { ImageSelectorService } from '../../../shared/components/image-selector/image-selector.service';

@Component({
  selector: 'app-add-blogpost',
  standalone: false,

  
  templateUrl: './add-blogpost.component.html',
  styleUrl: './add-blogpost.component.css'
})
export class AddBlogpostComponent implements OnInit ,OnDestroy {
  model:AddBlogPostRequest;
  isImageSelectorVisible : boolean =false

  categories$! : Observable<Category[]>;

  imageSelectorSubscription?: Subscription

  constructor(private blogPostsrvice: BlogpostsService,
    private router: Router,private categorySerive : CategoryService,
    private imageService : ImageSelectorService) 
  {
    this.model = {
      title: '',
      shortDescription: '',
      urlHandle: '',
      content: '',
      featuredImageUrl: '',
      author: '',
      isVisible: true,
      publishedDate: new Date(), // Use Date type for datetime values
      categories: []


    };
  }
  ngOnDestroy(): void {
    this.imageSelectorSubscription?.unsubscribe();  }
  ngOnInit(): void {
      this.categories$ = this.categorySerive.getAllCategory();
      this.imageSelectorSubscription =this.imageService.onSelectImage()
      .subscribe({
        next: (image) => {

          this.model.featuredImageUrl = image.url;
          this.closeImageSelector();
        }
      })
    
  }

  // private getImages(){
  //   this.imageService.getAllImages();
  // }
  OnFormSubmit(): void{
    console.log(this.model)

    this.blogPostsrvice.addBlogPosts(this.model)
    .subscribe({
   next:(resonse) =>{

    this.router.navigateByUrl('admin/blogposts')

   }
    })

  }

  openImageSelector():void{
    
    this.isImageSelectorVisible = true;
  }
  closeImageSelector():void{
    this.isImageSelectorVisible = false;
  }

}
